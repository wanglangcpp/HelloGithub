using GameFramework;
using System;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public static class DataTableExtension
    {
        private static readonly string[] ColumnSplit = new string[] { "\t" };

        public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName, object userData = null)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string[] splitNames = dataTableName.Split('_');
            if (splitNames.Length > 2)
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string dataTableClassName = string.Format("Genesis.GameClient.DR{0}", splitNames[0]);

            Type dataTableType = Type.GetType(dataTableClassName);
            if (dataTableType == null)
            {
                Log.Warning("Can not get data table type with class name '{0}'.", dataTableClassName);
                return;
            }

            string dataTableNameInType = splitNames.Length > 1 ? splitNames[1] : null;
            GameEntry.DataTable.LoadDataTable(dataTableName, dataTableType, dataTableNameInType, AssetUtility.GetDataTableAsset(dataTableName), userData);
        }

        /// <summary>
        /// 获取数据表对象。
        /// </summary>
        /// <param name="drTypeName">数据行的完整类型名。</param>
        /// <returns>要获取的数据表对象。</returns>
        public static object GetDataTableObject(string drTypeName)
        {
            Type drType = Type.GetType(drTypeName);
            if (drType == null)
            {
                Log.Warning("Cannot get data table type with class name '{0}'.", drTypeName);
                return null;
            }

            return GameEntry.DataTable.GetDataTable(drType);
        }

        private static float[,] s_ElementMatrix = null;

        /// <summary>
        /// 初始化克制元素关系矩阵。
        /// </summary>
        /// <param name="dataTableComponent">数据表组件。</param>
        public static void InitElementMatrix(this DataTableComponent dataTableComponent)
        {
            var dt = GameEntry.DataTable.GetDataTable<DRElement>();
            var drs = dt.GetAllDataRows();
            int maxElementId = -1;
            for (int i = 0; i < drs.Length; ++i)
            {
                var dr = drs[i];
                if (dr.Id > maxElementId)
                {
                    maxElementId = dr.Id;
                }
            }

            s_ElementMatrix = new float[maxElementId + 1, maxElementId + 1];
            for (int i = 0; i < drs.Length; ++i)
            {
                var dr = drs[i];
                for (int j = 0; j < dr.RestrainedElementIds.Length; ++j)
                {
                    if (dr.RestrainedElementIds[j] < 0)
                    {
                        continue;
                    }

                    s_ElementMatrix[j, dr.RestrainedElementIds[j]] = dr.RestrainingValues[j];
                }
            }
        }

        /// <summary>
        /// 获取克制类属性元素关系值。
        /// </summary>
        public static float GetElementValue(this DataTableComponent dataTableComponent, int originElementId, int targetElementId)
        {
            return s_ElementMatrix[originElementId, targetElementId];
        }

        public static string[] SplitDataRow(string dataRowText)
        {
            return dataRowText.Split(ColumnSplit, StringSplitOptions.None);
        }
    }
}
