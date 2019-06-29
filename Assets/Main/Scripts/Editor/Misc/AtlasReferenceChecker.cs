using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    /// <summary>
    /// 检查 UI 图集引用的工具类。
    /// </summary>
    public static class AtlasReferenceChecker
    {
        private const string ProgressBarTitleFormat = "Checking NGUI Atlas Reference ({0} / {1})";

        [MenuItem("Assets/Check NGUI Atlas Reference", validate = true)]
        public static bool Validate()
        {
            var selectedPrefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets).ToList().ConvertAll(o => o as GameObject);
            return selectedPrefabs.Count > 0;
        }

        [MenuItem("Assets/Check NGUI Atlas Reference")]
        public static void Run()
        {
            var selectedPrefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets).ToList().ConvertAll(o => o as GameObject);

            var allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");

            int count = 0;
            foreach (var prefab in selectedPrefabs)
            {
                ++count;
                var progressBarTitle = string.Format(ProgressBarTitleFormat, count, selectedPrefabs.Count);
                CheckPrefab(prefab, allPrefabGuids, progressBarTitle);
            }

            EditorUtility.ClearProgressBar();
        }

        private static void CheckPrefab(GameObject prefab, IList<string> allPrefabGuids, string progressBarTitle)
        {
            var atlasPrefabAssetPath = AssetDatabase.GetAssetPath(prefab);
            var atlas = prefab.GetComponent<UIAtlas>();
            if (atlas == null)
            {
                Debug.LogFormat("Prefab {0} is not an NGUI atlas.", atlasPrefabAssetPath);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendFormat("Atlas: {0}, referenced by {{0}} prefabs.\n\n", atlasPrefabAssetPath);
            var atlasPrefabGuid = AssetDatabase.AssetPathToGUID(atlasPrefabAssetPath);

            int guidCount = 0;
            int prefabCount = 0;
            foreach (var guid in allPrefabGuids)
            {
                guidCount++;

                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                EditorUtility.DisplayProgressBar(progressBarTitle,
                    string.Format("Checking references of '{0}' in '{1}'", Path.GetFileName(atlasPrefabAssetPath), Path.GetFileName(assetPath)),
                    (float)(guidCount) / allPrefabGuids.Count);

                var modifiedPath = Regex.Replace(assetPath, @"^Assets[\/]", string.Empty);
                var fullPath = Utility.Path.GetCombinePath(Application.dataPath, modifiedPath);
                var lines = File.ReadAllLines(fullPath);
                int refCount = 0;

                foreach (var line in lines)
                {
                    if (line.Contains(atlasPrefabGuid))
                    {
                        refCount++;
                    }
                }

                if (refCount > 0)
                {
                    sb.AppendFormat("  '{0}': {1} times\n", assetPath, refCount);
                    prefabCount++;
                }
            }

            Debug.LogFormat(sb.ToString(), prefabCount.ToString());
        }
    }
}
