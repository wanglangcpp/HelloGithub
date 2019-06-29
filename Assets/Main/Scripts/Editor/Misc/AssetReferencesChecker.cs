using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public static class AssetReferencesChecker
    {
        private const string SceneObjectsRootPath = "Assets/Main/Prefabs/SceneObjects";
        private static readonly string[] s_SearchInFolders = new string[] { "Assets/Main" };

        private static string SceneObjectRefsOutputPath
        {
            get
            {
                return Utility.Path.GetCombinePath(Application.temporaryCachePath, "scene_object_refs.txt");
            }
        }

        private static string OtherAssetRefsOutputPath
        {
            get
            {
                return Utility.Path.GetCombinePath(Application.temporaryCachePath, "other_asset_refs.txt");
            }
        }

        [MenuItem("Game Framework/Check Asset References/Scene Objects", priority = 4004)]
        public static void CheckSceneObjects()
        {
            var referencers = new HashSet<string>(AssetDatabase.FindAssets("t:scene", s_SearchInFolders));
            var referencees = new HashSet<string>(AssetDatabase.FindAssets("t:prefab", s_SearchInFolders)).Where(guid => AssetDatabase.GUIDToAssetPath(guid).StartsWith(SceneObjectsRootPath));

            var cachedDependencies = new Dictionary<string, HashSet<string>>();
            CacheDependencies(cachedDependencies, referencers);
            var refs = new Dictionary<string, int>();
            CheckReferences(cachedDependencies, referencers, referencees, refs);
            OutputResult(refs, SceneObjectRefsOutputPath);
            EditorUtility.ClearProgressBar();
        }

        [MenuItem("Game Framework/Check Asset References/Other Assets", priority = 4005)]
        public static void CheckOtherAssets()
        {
            var referencers = new HashSet<string>(AssetDatabase.FindAssets("t:prefab t:material", s_SearchInFolders));
            var referencees = new HashSet<string>();
            referencees.UnionWith(AssetDatabase.FindAssets("t:material", s_SearchInFolders));
            referencees.UnionWith(AssetDatabase.FindAssets("t:texture", s_SearchInFolders).Where(guid =>
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                return !Path.GetFileName(assetPath).StartsWith("Lightmap")
                    && !assetPath.StartsWith("Assets/Main/UI")
                    && !assetPath.StartsWith("Assets/Main/AppIcons")
                    && !assetPath.StartsWith("Assets/Main/AppSplashImages");
            }));
            referencees.UnionWith(AssetDatabase.FindAssets("t:model", s_SearchInFolders).Where(guid => !AssetDatabase.GUIDToAssetPath(guid).Contains("@")));
            referencees.UnionWith(AssetDatabase.FindAssets("t:animation t:AnimatorController", s_SearchInFolders));

            var cachedDependencies = new Dictionary<string, HashSet<string>>();
            CacheDependencies(cachedDependencies, referencers);
            var refs = new Dictionary<string, int>();
            CheckReferences(cachedDependencies, referencers, referencees, refs);
            OutputResult(refs, OtherAssetRefsOutputPath);
            EditorUtility.ClearProgressBar();
        }

        private static void CheckReferences(IDictionary<string, HashSet<string>> cachedDependencies, HashSet<string> referencers, IEnumerable<string> referencees, IDictionary<string, int> refs)
        {
            var title = "Checking Asset References";
            int referenceeCount = referencees.Count();
            int referenceeIndex = 0;
            foreach (var referencee in referencees)
            {
                var referenceePath = AssetDatabase.GUIDToAssetPath(referencee);
                refs.Add(referenceePath, 0);

                foreach (var referencer in referencers)
                {
                    var referencerPath = AssetDatabase.GUIDToAssetPath(referencer);
                    var deps = GetDependencies(cachedDependencies, referencerPath);

                    if (deps.Contains(referenceePath))
                    {
                        refs[referenceePath]++;
                    }
                }

                EditorUtility.DisplayProgressBar(title, string.Format("{0}/{1}", ++referenceeIndex, referenceeCount), (float)referenceeIndex / referenceeCount);
            }
        }

        private static void CacheDependencies(Dictionary<string, HashSet<string>> cachedDependencies, HashSet<string> referencers)
        {
            string title = "Caching dependencies";
            int referencerCount = referencers.Count;
            int referencerIndex = 0;

            EditorUtility.DisplayProgressBar(title, string.Empty, 0f);
            foreach (var referencer in referencers)
            {
                GetDependencies(cachedDependencies, AssetDatabase.GUIDToAssetPath(referencer));
                EditorUtility.DisplayProgressBar(title, string.Format("{0}/{1}", ++referencerIndex, referencerCount), (float)referencerIndex / referencerCount);
            }
        }

        private static HashSet<string> GetDependencies(IDictionary<string, HashSet<string>> cachedDependencies, string path)
        {
            if (cachedDependencies.ContainsKey(path))
            {
                return cachedDependencies[path];
            }

            var depSet = new HashSet<string>(AssetDatabase.GetDependencies(path, true));
            cachedDependencies.Add(path, depSet);
            return depSet;
        }

        private static void OutputResult(IDictionary<string, int> refs, string outputPath)
        {
            var kvs = new List<KeyValuePair<string, int>>();
            foreach (var kv in refs)
            {
                kvs.Add(kv);
            }

            kvs.Sort((a, b) => { return a.Value == b.Value ? a.Key.CompareTo(b.Key) : a.Value.CompareTo(b.Value); });

            var lines = new List<string>();
            foreach (var kv in kvs)
            {
                lines.Add(string.Format("{0}\t{1}", kv.Key, kv.Value));
            }

            File.WriteAllLines(outputPath, lines.ToArray());
        }

    }
}
