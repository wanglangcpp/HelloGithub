using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class AssetBundleOptimizer : EditorWindow
    {
        private const string AssetBundleCollectionConfigurationName = "GameFramework/Editor/AssetBundleCollection.xml";
        private const string OptimizedAssetBundleCollectionConfigurationName = "GameFramework/Editor/AssetBundleCollection.Optimized.xml";

        private struct AssetBundle
        {
            public string Name;
            public string Variant;
        }

        private static Dictionary<string, AssetBundle> s_AssetMap = null;
        private static Dictionary<string, HashSet<string>> s_RefTable = null;
        private static Dictionary<string, HashSet<string>> s_SceneDeps = null;
        private static Dictionary<AssetBundle, HashSet<string>> s_NewBundles = null;

        private Vector2 m_ScrollPosition = Vector2.zero;

        // [MenuItem("Game Framework/Asset Bundle/Asset Bundle Optimizer")]
        public static void BundleOptimizer()
        {
            if (!AnalyzeDependencies())
                return;

            GetWindow<AssetBundleOptimizer>();
        }

        // [MenuItem("Game Framework/Asset Bundle/Optimize Asset Bundle")]
        public static void OptimizeAndGenerateNewConfiguration()
        {
            if (!AnalyzeDependencies())
            {
                Debug.LogError("Failed to analyze configuration");
                return;
            }

            if (!Apply())
            {
                Debug.LogError("Failed to generate new configuration");
                return;
            }

            Debug.Log("Asset Bundles Optimized Successfully.");
        }

        private static bool Apply()
        {
            string originalPath = Utility.Path.GetCombinePath(Application.dataPath, AssetBundleCollectionConfigurationName);
            if (!File.Exists(originalPath))
            {
                Debug.LogError("Can not found asset bundle editor configuration.");
                return false;
            }

            string optimizedPath = Utility.Path.GetCombinePath(Application.dataPath, OptimizedAssetBundleCollectionConfigurationName);

            var config = new AssetBundleConfiguration();
            if (!config.Load(originalPath))
            {
                return false;
            }

            foreach (var item in s_NewBundles)
            {
                foreach (var asset in item.Value)
                {
                    config.ReassignAssetBundle(asset, item.Key.Name, item.Key.Variant, false);
                }
            }

            config.Clean();

            if (!config.Save(optimizedPath))
            {
                return false;
            }

            return true;
        }

        private static bool AnalyzeDependencies()
        {
            s_AssetMap = new Dictionary<string, AssetBundle>();
            s_RefTable = new Dictionary<string, HashSet<string>>();
            s_SceneDeps = new Dictionary<string, HashSet<string>>();
            s_NewBundles = new Dictionary<AssetBundle, HashSet<string>>();

            if (!ParseXML())
                return false;

            OptimizeSingleRefAssets();
            OptimizeSharedAssets();

            if (!CheckNewBundleList())
            {
                return false;
            }

            return true;
        }

        private static bool ParseXML()
        {
            string configurationPath = Utility.Path.GetCombinePath(Application.dataPath, AssetBundleCollectionConfigurationName);
            if (!File.Exists(configurationPath))
            {
                Debug.LogError("Can not found asset bundle editor configuration.");
                return false;
            }

            s_AssetMap = new Dictionary<string, AssetBundle>();

            AssetBundleConfiguration config = new AssetBundleConfiguration();
            if (!config.Load(configurationPath))
            {
                return false;
            }

            foreach (var asset in config.Assets)
            {
                string assetGuid = asset.Guid;
                string assetName = AssetDatabase.GUIDToAssetPath(assetGuid);
                if (string.IsNullOrEmpty(assetName))
                {
                    Debug.LogError("Can not find asset by guid '" + assetGuid + "'.");
                    return false;
                }

                string assetBundleName = asset.AssetBundleName;
                string assetBundleVariant = asset.AssetBundleVariant;

                s_AssetMap.Add(assetGuid, new AssetBundle()
                {
                    Name = assetBundleName,
                    Variant = assetBundleVariant
                });
            }

            var sceneGuids = AssetDatabase.FindAssets("t:scene");
            foreach (string sceneGuid in sceneGuids)
            {
                if (!s_AssetMap.ContainsKey(sceneGuid))
                    continue;

                string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                var deps = AssetDatabase.GetDependencies(scenePath);
                s_SceneDeps[sceneGuid] = new HashSet<string>();

                foreach (string assetPath in deps)
                {
                    string guid = AssetDatabase.AssetPathToGUID(assetPath);
                    if (string.IsNullOrEmpty(guid))
                    {
                        Debug.LogError("Can not find asset by guid '" + guid + "'.");
                        return false;
                    }

                    if (guid == sceneGuid)
                        continue;

                    if (!s_AssetMap.ContainsKey(guid))
                        continue;

                    s_SceneDeps[sceneGuid].Add(guid);

                    if (!s_RefTable.ContainsKey(guid))
                        s_RefTable[guid] = new HashSet<string>();

                    s_RefTable[guid].Add(sceneGuid);
                }
            }

            return true;
        }

        private static bool CheckNewBundleList()
        {
            var bundleMap = new Dictionary<string, AssetBundle>();

            foreach (var bundle in s_NewBundles.Keys)
            {
                foreach (var guid in s_NewBundles[bundle])
                {
                    if (bundleMap.ContainsKey(guid))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Asset '{0}' belongs to both '{1}' and '{2}'\n",
                            AssetDatabase.GUIDToAssetPath(guid),
                            bundleMap[guid],
                            bundle);

                        sb.AppendLine(bundleMap[guid].Name + "#" + bundleMap[guid].Variant);
                        foreach (var item in s_NewBundles[bundleMap[guid]])
                        {
                            sb.AppendLine("    " + AssetDatabase.GUIDToAssetPath(item));
                        }

                        sb.AppendLine(bundle.Name + "#" + bundle.Variant);
                        foreach (var item in s_NewBundles[bundle])
                        {
                            sb.AppendLine("    " + AssetDatabase.GUIDToAssetPath(item));
                        }

                        Debug.LogError(sb.ToString());

                        return false;
                    }

                    bundleMap[guid] = bundle;
                }
            }

            return true;
        }

        private static void OptimizeSingleRefAssets()
        {
            var guids = new List<string>(s_RefTable.Keys);
            var newBundles = new Dictionary<AssetBundle, HashSet<string>>();

            foreach (string guid in guids)
            {
                if (s_RefTable[guid].Count == 1)
                {
                    string sceneGuid = null;
                    foreach (string p in s_RefTable[guid])
                    {
                        sceneGuid = p;
                        break;
                    }

                    var bundle = s_AssetMap[sceneGuid];
                    bundle.Name += "_Combined_Assets";

                    if (!newBundles.ContainsKey(bundle))
                        newBundles[bundle] = new HashSet<string>();

                    newBundles[bundle].Add(guid);
                }
            }

            {
                var list = newBundles.ToList();
                foreach (var item in list)
                {
                    if (item.Value.Count <= 1)
                    {
                        newBundles.Remove(item.Key);
                    }
                }
            }

            AddToNewBundleList(newBundles);
        }

        private static void AddToNewBundleList(Dictionary<AssetBundle, HashSet<string>> newBundles)
        {
            foreach (var bundle in newBundles.Keys)
            {
                if (!s_NewBundles.ContainsKey(bundle))
                {
                    s_NewBundles[bundle] = newBundles[bundle];
                }
                else
                {
                    foreach (var item in newBundles[bundle])
                    {
                        s_NewBundles[bundle].Add(item);
                    }
                }
            }
        }

        private static HashSet<string> Intersection(HashSet<string> a, HashSet<string> b)
        {
            var retVal = new HashSet<string>();

            foreach (var item in a)
            {
                if (b.Contains(item))
                {
                    retVal.Add(item);
                }
            }

            return retVal;
        }

        private static bool Contains(HashSet<string> a, HashSet<string> b)
        {
            foreach (var item in b)
            {
                if (!a.Contains(item))
                    return false;
            }
            return true;
        }

        private static HashSet<string> Subtract(HashSet<string> a, HashSet<string> b)
        {
            var retVal = new HashSet<string>();

            foreach (var item in a)
            {
                if (!b.Contains(item))
                    retVal.Add(item);
            }

            return retVal;
        }

#if DEBUG_ASSET_BUNDLE_COMBINER
        private static void DumpBundleList(List<HashSet<string>> bundleList, string title, string path, bool append = true)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(title);
            sb.AppendLine();

            for (int i = 0; i < bundleList.Count; i++)
            {
                if (bundleList[i].Count == 0)
                    continue;

                foreach (var item in bundleList[i])
                {
                    sb.Append(AssetDatabase.GUIDToAssetPath(item));
                    sb.Append("\t");

                    foreach (var refScene in s_RefTable[item])
                    {
                        sb.Append(refScene);
                        sb.Append("\t");
                    }

                    sb.AppendLine();
                }

                sb.AppendLine();
            }

            if (!append)
            {
                File.WriteAllText(path, sb.ToString());
            }
            else
            {
                File.AppendAllText(path, sb.ToString());
            }
        }
#endif

        private static void OptimizeSharedAssets()
        {
            var keys = new List<string>(s_SceneDeps.Keys);
            var bundleList = new List<HashSet<string>>();

            for (int i = 0; i < keys.Count - 1; i++)
            {
                for (int j = i + 1; j < keys.Count; j++)
                {
                    var intersection = Intersection(s_SceneDeps[keys[i]], s_SceneDeps[keys[j]]);
                    if (intersection.Count > 0)
                    {
                        bundleList.Add(intersection);
                    }
                }
            }

            var newList = new List<HashSet<string>>();
            do
            {
                newList.Clear();

                for (int i = 0; i < bundleList.Count - 1; i++)
                {
                    for (int j = i + 1; j < bundleList.Count; j++)
                    {
                        if (Contains(bundleList[i], bundleList[j]))
                        {
                            bundleList[i] = Subtract(bundleList[i], bundleList[j]);
                        }
                        else if (Contains(bundleList[j], bundleList[i]))
                        {
                            bundleList[j] = Subtract(bundleList[j], bundleList[i]);
                        }
                        else
                        {
                            var intersection = Intersection(bundleList[i], bundleList[j]);

                            if (intersection.Count > 0)
                            {
                                bundleList[i] = Subtract(bundleList[i], intersection);
                                bundleList[j] = Subtract(bundleList[j], intersection);
                            }

                            if (intersection.Count > 1)
                            {
                                newList.Add(intersection);
                            }
                        }
                    }
                }

                bundleList.AddRange(newList);

            } while (newList.Count > 0);

            for (int i = bundleList.Count - 1; i >= 0; i--)
            {
                if (bundleList[i].Count <= 1)
                {
                    bundleList.RemoveAt(i);
                    continue;
                }

                string first = null;
                foreach (var guid in bundleList[i])
                {
                    if (null == first)
                    {
                        first = guid;
                        continue;
                    }
                    if (!Contains(s_RefTable[first], s_RefTable[guid]) || !Contains(s_RefTable[guid], s_RefTable[first]))
                    {
                        bundleList.RemoveAt(i);
                        break;
                    }
                }
            }

#if DEBUG_ASSET_BUNDLE_COMBINER
            DumpBundleList(bundleList, "null", "dump.txt", false);
#endif

            var newBundles = new Dictionary<AssetBundle, HashSet<string>>();
            for (int i = 0; i < bundleList.Count; i++)
            {
                AssetBundle bundle = new AssetBundle()
                {
                    Name = "OptimizedBundles/Optimized_" + i,
                    Variant = null
                };
                newBundles[bundle] = bundleList[i];
            }

            AddToNewBundleList(newBundles);
        }

        private void OnGUI()
        {
            if (null == s_NewBundles || s_NewBundles.Count == 0)
                return;

            StringBuilder sb = new StringBuilder();
            float windowWidth = position.width;
            float arrowWidth = 20;
            float fieldWidth = (windowWidth - arrowWidth) * 0.5f;

            if (GUILayout.Button("Apply"))
            {
                Apply();
            }

            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);

            foreach (var bundle in s_NewBundles.Keys)
            {
                sb.Length = 0;
                foreach (var guid in s_NewBundles[bundle])
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    sb.AppendLine(path);
                }
                string list = sb.ToString();
                string bundleName = bundle.Name;
                if (!string.IsNullOrEmpty(bundle.Variant))
                {
                    bundleName += "[" + bundle.Variant + "]";
                }

                GUILayout.BeginHorizontal();

                GUILayout.TextArea(list, GUILayout.Width(fieldWidth));
                GUILayout.Label("=>", GUILayout.Width(arrowWidth));
                GUILayout.TextArea(bundleName, GUILayout.Width(fieldWidth));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }

        private void OnDestroy()
        {
            s_AssetMap = null;
            s_RefTable = null;
            s_SceneDeps = null;
            s_NewBundles = null;
        }
    }
}
