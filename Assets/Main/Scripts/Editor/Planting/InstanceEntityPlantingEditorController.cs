using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Genesis.GameClient.Editor
{
    [Serializable]
    internal class InstanceEntityPlantingEditorController
    {
        private readonly Dictionary<string, PlanterInfo> m_PlanterInfos = new Dictionary<string, PlanterInfo>();

        internal IDictionary<string, PlanterInfo> GetPlanterInfos()
        {
            return m_PlanterInfos;
        }

        private const string ClassName = "GameFramework.DataTable.a+a"; // GameFramework.DataTable.DataTableManager+DataTable`1
        private const string MethodName = "f"; // AddDataRow
        public const string DataTableAssetPath = "Assets/Main/DataTables/";
        private readonly static Encoding SimplifiedChineseEncoding = Encoding.GetEncoding(Constant.SimplifiedChineseCodePage);

        public List<BaseEntityPlanter> SelectedPlanters { get; private set; }

        public Vector2 LookAtPoint { get; set; }

        public InstanceEntityPlantingEditorController()
        {
            var npcPlanterInfo = new PlanterInfo
            (
                key: "NPC",
                instanceDTPrefix: "InstanceNpcs_",
                planterPrefabAssetPath: "Assets/Main/Prefabs/Others/NpcPlanter.prefab",
                defaultNewTableName: "InstanceNpcs_New",
                drType: typeof(DRInstanceNpcs),
                planterType: typeof(NpcPlanter)
            );

            var buildingPlanterInfo = new PlanterInfo
            (
                key: "Building",
                instanceDTPrefix: "InstanceBuildings_",
                planterPrefabAssetPath: "Assets/Main/Prefabs/Others/BuildingPlanter.prefab",
                defaultNewTableName: "InstanceBuildings_New",
                drType: typeof(DRInstanceBuildings),
                planterType: typeof(BuildingPlanter)
            );

            m_PlanterInfos.Add(npcPlanterInfo.Key, npcPlanterInfo);
            m_PlanterInfos.Add(buildingPlanterInfo.Key, buildingPlanterInfo);
            SelectedPlanters = new List<BaseEntityPlanter>();
        }

        public void Reset()
        {
            var scripts = Object.FindObjectsOfType<BaseEntityPlanter>();
            foreach (var script in scripts)
            {
                Object.DestroyImmediate(script.gameObject);
            }

            foreach (var kv in m_PlanterInfos)
            {
                kv.Value.CurrentDTFilePath = string.Empty;
            }

            SelectedPlanters.Clear();
        }

        public void Load(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            PlanterInfo firstPlanterInfo = null;
            foreach (var kv in m_PlanterInfos)
            {
                if (fileName.StartsWith(kv.Value.InstanceDTPrefix))
                {
                    firstPlanterInfo = kv.Value;
                    break;
                }
            }

            if (firstPlanterInfo == null)
            {
                Debug.LogErrorFormat("Invalid data table name '{0}'.", fileName);
                return;
            }

            var methodInfo = GetType().GetMethod("ReadInstanceEntitiesDataTable", BindingFlags.Static | BindingFlags.NonPublic);
            var firstDataTable = methodInfo.MakeGenericMethod(firstPlanterInfo.DRType).Invoke(this, new object[] { filePath });

            if (firstDataTable == null)
            {
                Debug.LogError("Data table cannot be read.");
                return;
            }

            firstPlanterInfo.CurrentDTFilePath = filePath;

            var dataTables = new Dictionary<string, object>();
            dataTables.Add(firstPlanterInfo.Key, firstDataTable);

            foreach (var kv in m_PlanterInfos)
            {
                if (kv.Key == firstPlanterInfo.Key)
                {
                    continue;
                }

                var currentPlantInfo = m_PlanterInfos[kv.Key];
                var otherFileName = Regex.Replace(fileName, string.Format("{0}", firstPlanterInfo.InstanceDTPrefix), currentPlantInfo.InstanceDTPrefix);
                var otherFilePath = Regex.Replace(filePath, string.Format("{0}", fileName), otherFileName);
                var dt = methodInfo.MakeGenericMethod(kv.Value.DRType).Invoke(this, new object[] { otherFilePath });
                if (dt == null)
                {
                    currentPlantInfo.CurrentDTFilePath = string.Empty;
                    continue;
                }

                currentPlantInfo.CurrentDTFilePath = otherFilePath;
                dataTables.Add(kv.Key, dt);
            }

            foreach (var kv in dataTables)
            {
                var key = kv.Key;
                var dt = kv.Value;
                var currentPlantInfo = m_PlanterInfos[key];
                var prefab = AssetDatabase.LoadAssetAtPath(currentPlantInfo.PlanterPrefabAssetPath, typeof(GameObject)) as GameObject;

                foreach (var dr in dt as System.Collections.IEnumerable)
                {
                    var go = Object.Instantiate(prefab);
                    var script = go.GetComponent(currentPlantInfo.PlanterType) as BaseEntityPlanter;
                    script.GetType().GetMethod("Init", BindingFlags.Instance | BindingFlags.Public).Invoke(script, new object[] { dr });
                }
            }
        }

        public void Save<TDR, TPlanter>(string filePath, string planterInfoKey)
            where TDR : DRInstanceEntities, new()
            where TPlanter : AbstractEntityPlanter<TDR>
        {
            List<string> lines = new List<string>();
            if (File.Exists(filePath))
            {
                lines = File.ReadAllLines(filePath, SimplifiedChineseEncoding)
                    .Where(line => line.StartsWith("#"))
                    .ToList();
            }

            IList<TPlanter> planters = new List<TPlanter>(Object.FindObjectsOfType<TPlanter>());

            List<TDR> dataRows = new List<TDR>();
            foreach (var planter in planters)
            {
                dataRows.Add(planter.SaveDataRow());
            }

            dataRows.Sort((a, b) => (a.Id.CompareTo(b.Id)));

            HashSet<int> existingIds = new HashSet<int>();
            foreach (var dataRow in dataRows)
            {
                if (existingIds.Contains(dataRow.Id))
                {
                    throw new Exception(string.Format("Instance {0} ID (index) '{1}' has already been used.", planterInfoKey, dataRow.Id));
                }

                existingIds.Add(dataRow.Id);
                lines.Add(string.Join("\t", dataRow.WriteDataRow()));
            }
            File.WriteAllLines(filePath, lines.ToArray(), SimplifiedChineseEncoding);
            m_PlanterInfos[planterInfoKey].CurrentDTFilePath = filePath;
            AssetDatabase.ImportAsset(filePath.Replace(Application.dataPath, "Assets"), ImportAssetOptions.ForceUpdate);
        }

        private static IDataTable<TDR> ReadInstanceEntitiesDataTable<TDR>(string filePath) where TDR : DRInstanceEntities, new()
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            string[] text = File.ReadAllText(filePath, SimplifiedChineseEncoding).Split('\n');
            Assembly assembly = Assembly.Load("GameFramework");
            Type dataTableGenericType = assembly.GetType(ClassName);
            Type dataTableType = dataTableGenericType.MakeGenericType(typeof(TDR));
            IDataTable<TDR> dataTable = Activator.CreateInstance(dataTableType, new object[] { string.Empty }) as IDataTable<TDR>;
            MethodInfo methodInfo = dataTable.GetType().GetMethod(MethodName, BindingFlags.Public | BindingFlags.Instance);
            foreach (string i in text)
            {
                if (i.Length <= 0 || i[0] == '#')
                {
                    continue;
                }

                methodInfo.Invoke(dataTable, BindingFlags.InvokeMethod, null, new object[] { i }, null);
            }

            return dataTable;
        }

        public void PerformLookAt()
        {
            foreach (var planter in SelectedPlanters)
            {
                planter.CachedTransform.LookAt2D(LookAtPoint);
            }
        }

        internal class PlanterInfo
        {
            public string Key { get; private set; }
            public string InstanceDTPrefix { get; private set; }
            public string PlanterPrefabAssetPath { get; private set; }
            public string DefaultNewTableName { get; private set; }
            public Type DRType { get; private set; }
            public Type PlanterType { get; private set; }
            public string CurrentDTFilePath { get; set; }

            public string CurrentDTAssetPath
            {
                get
                {
                    if (string.IsNullOrEmpty(CurrentDTFilePath))
                    {
                        return string.Empty;
                    }

                    return CurrentDTFilePath.Replace(Application.dataPath, "Assets");
                }
            }

            public PlanterInfo(string key, string instanceDTPrefix, string planterPrefabAssetPath, string defaultNewTableName, Type drType, Type planterType)
            {
                Key = key;
                InstanceDTPrefix = instanceDTPrefix;
                PlanterPrefabAssetPath = planterPrefabAssetPath;
                DefaultNewTableName = defaultNewTableName;
                DRType = drType;
                PlanterType = planterType;

                CurrentDTFilePath = string.Empty;
            }
        }

    }
}
