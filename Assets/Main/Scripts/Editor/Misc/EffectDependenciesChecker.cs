using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public static class EffectDependenciesChecker
    {
        private const string EffectsRootPath = "Assets/Main/Prefabs/Effects";

        private static string EffectDependenciesOutputPath
        {
            get
            {
                return Utility.Path.GetCombinePath(Application.temporaryCachePath, "effect_dependencies.txt");
            }
        }

        [MenuItem("Game Framework/Check Asset References/Effect Dependencies", priority = 4006)]
        public static void CheckEffectDependencies()
        {
            var prefabs = AssetDatabase.FindAssets("t:prefab", new string[] { EffectsRootPath });
            var dependencies = new Dictionary<string, HashSet<string>>();

            var title = "Checking effect dependencies";
            EditorUtility.DisplayProgressBar(title, string.Empty, 0f);

            int prefabCount = prefabs.Count();
            int prefabIndex = 0;

            foreach (var prefab in prefabs)
            {
                var prefabPath = AssetDatabase.GUIDToAssetPath(prefab);
                dependencies.Add(prefabPath, new HashSet<string>());
                var currentDeps = dependencies[prefabPath];
                currentDeps.UnionWith(AssetDatabase.GetDependencies(prefabPath, true));
                EditorUtility.DisplayProgressBar(title, string.Format("Collecting dependency data: {0}/{1}", ++prefabIndex, prefabCount), (float)prefabIndex / prefabCount);
            }

            EditorUtility.DisplayProgressBar(title, string.Empty, 0f);

            int dependencyDataCount = 0;
            foreach (var kv in dependencies)
            {
                dependencyDataCount += kv.Value.Count;
            }

            List<EffectDependency> dataList = new List<EffectDependency>(dependencyDataCount);

            int dependencyDataIndex = 0;
            foreach (var kv in dependencies)
            {
                foreach (var depPath in kv.Value)
                {
                    var assetType = AssetDatabase.LoadAssetAtPath(depPath, typeof(Object)).GetType().Name;
                    var data = new EffectDependency { PrefabPath = Regex.Replace(kv.Key, string.Format("^{0}/", EffectsRootPath), string.Empty), DepAssetPath = Regex.Replace(depPath, "^Assets/Main/", string.Empty), DepAssetType = assetType };
                    dataList.Add(data);
                    EditorUtility.DisplayProgressBar(title, "Listing dependency data...", (float)(++dependencyDataIndex) / dependencyDataCount);
                }
            }

            EditorUtility.DisplayProgressBar(title, "Sorting dependency data...", 1f);
            dataList.Sort((a, b) =>
            {
                return a.PrefabPath != b.PrefabPath ? a.PrefabPath.CompareTo(b.PrefabPath) : a.DepAssetType != b.DepAssetType ? a.DepAssetType.CompareTo(b.DepAssetType) : a.DepAssetPath.CompareTo(b.DepAssetPath);
            });

            List<string> fileContent = new List<string>();

            int lineIndex = 0;
            foreach (var data in dataList)
            {
                fileContent.Add(string.Format("{0}\t{1}\t{2}", data.PrefabPath, data.DepAssetType, data.DepAssetPath));
                EditorUtility.DisplayProgressBar(title, "Preparing contents to write to file...", (float)(++lineIndex) / dataList.Count);
            }

            EditorUtility.DisplayProgressBar(title, "Writing to file...", 1f);
            File.WriteAllLines(EffectDependenciesOutputPath, fileContent.ToArray());
            EditorUtility.ClearProgressBar();
        }

        private class EffectDependency
        {
            public string PrefabPath { get; set; }
            public string DepAssetType { get; set; }
            public string DepAssetPath { get; set; }
        }
    }
}
