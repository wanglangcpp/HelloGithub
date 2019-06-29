using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Genesis.GameClient.Editor
{
    public class ShaderVariantCollectionExtension
    {
        public delegate void GetShaderVariantEntriesDelegate(Shader shader, ShaderVariantCollection skipAlreadyInCollection, out int[] types, out string[] keywords);

        private static GetShaderVariantEntriesDelegate s_GetShaderVariantEntries;

        public static GetShaderVariantEntriesDelegate GetShaderVariantEntries
        {
            get
            {
                if (null == s_GetShaderVariantEntries)
                {
                    var method = typeof(ShaderUtil).GetMethod("GetShaderVariantEntries", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                    if (null == method)
                    {
                        throw new Exception("Method not found.");
                    }

                    s_GetShaderVariantEntries = (GetShaderVariantEntriesDelegate)Delegate.CreateDelegate(typeof(GetShaderVariantEntriesDelegate), method, true);
                }
                return s_GetShaderVariantEntries;
            }
        }

        private const string UniversalCollectionPath = "Assets/Main/Shaders/VariantsCollection/UniversalShaderVariantsCollection.shadervariants";

        private static readonly HashSet<string> ExcludedShaders = new HashSet<string>
        {
            "Standard"
        };

        private enum FeatureKey
        {
            LIGHTTYPE = 0,
            FOG,
            SOFTPARTICLES,
            DIRLIGHTMAP,
            DYNAMICLIGHTMAP,
            LIGHTMAP,
            SHADOW,
            SHADOWCAST,

            NUM_FEATURE_KEY
        }

        private static readonly string[][] FeatureSets =
        {
            new string[] { "DIRECTIONAL" },
            new string[] { "", "FOG_LINEAR" },
            new string[] { "SOFTPARTICLES_OFF", "SOFTPARTICLES_ON" },
            new string[] { "DIRLIGHTMAP_OFF" },
            new string[] { "DYNAMICLIGHTMAP_OFF" },
            new string[] { "LIGHTMAP_OFF", "LIGHTMAP_ON" },
            new string[] { "SHADOWS_OFF", "SHADOWS_SCREEN" },
            new string[] { "SHADOWS_DEPTH" },
        };

        private class ShaderVariantInfo
        {
            public int[] RawTypes { get; private set; }
            public string[] RawKeywords { get; private set; }

            public Dictionary<PassType, HashSet<FeatureKey>> Features { get; private set; }

            public ShaderVariantInfo(int[] rawTypes, string[] rawKeywords)
            {
                RawTypes = rawTypes;
                RawKeywords = rawKeywords;

                if (null == RawTypes || null == RawKeywords || RawTypes.Length != RawKeywords.Length)
                    return;

                Features = new Dictionary<PassType, HashSet<FeatureKey>>();

                for (int i = 0; i < RawTypes.Length; i++)
                {
                    PassType type = (PassType)Enum.ToObject(typeof(PassType), RawTypes[i]);
                    string keywords = RawKeywords[i];
                    string[] keywordsArr = keywords.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    HashSet<string> keywordsSet = new HashSet<string>();
                    foreach (var keyword in keywordsArr)
                    {
                        keywordsSet.Add(keyword);
                    }

                    switch (type)
                    {
                        case PassType.Normal:
                            AddPassType(type);
                            if (keywordsSet.Contains("FOG_LINEAR"))
                            {
                                AddFeature(type, FeatureKey.FOG);
                            }
                            if (keywordsSet.Contains("SOFTPARTICLES_OFF"))
                            {
                                AddFeature(type, FeatureKey.SOFTPARTICLES);
                            }
                            break;
                        case PassType.ForwardBase:
                            AddFeature(type, FeatureKey.LIGHTTYPE);
                            if (keywordsSet.Contains("FOG_LINEAR"))
                            {
                                AddFeature(type, FeatureKey.FOG);
                            }
                            if (keywordsSet.Contains("DIRLIGHTMAP_OFF"))
                            {
                                AddFeature(type, FeatureKey.DIRLIGHTMAP);
                            }
                            if (keywordsSet.Contains("DYNAMICLIGHTMAP_OFF"))
                            {
                                AddFeature(type, FeatureKey.DYNAMICLIGHTMAP);
                            }
                            if (keywordsSet.Contains("LIGHTMAP_OFF"))
                            {
                                AddFeature(type, FeatureKey.LIGHTMAP);
                            }
                            if (keywordsSet.Contains("SHADOWS_OFF"))
                            {
                                AddFeature(type, FeatureKey.SHADOW);
                            }
                            break;
                        case PassType.ForwardAdd:
                            AddFeature(type, FeatureKey.LIGHTTYPE);
                            if (keywordsSet.Contains("FOG_LINEAR"))
                            {
                                AddFeature(type, FeatureKey.FOG);
                            }
                            break;
                        case PassType.ShadowCaster:
                            AddFeature(type, FeatureKey.SHADOWCAST);
                            break;
                    }
                }
            }

            private void AddPassType(PassType type)
            {
                if (!Features.ContainsKey(type))
                    Features.Add(type, new HashSet<FeatureKey>());
            }

            private void AddFeature(PassType type, FeatureKey feature)
            {
                if (!Features.ContainsKey(type))
                    Features.Add(type, new HashSet<FeatureKey>());

                if (!Features[type].Contains(feature))
                    Features[type].Add(feature);
            }
        }

        private delegate void AddCombinationDelegate(HashSet<string> combination);

        private static void EnumerateFeatureSets(HashSet<string> combination, List<FeatureKey> featureList, int depth, AddCombinationDelegate callback)
        {
            if (depth >= featureList.Count)
            {
                callback(combination);
                return;
            }

            int feature = (int)featureList[depth];
            foreach (var keyword in FeatureSets[feature])
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    combination.Add(keyword);
                }

                EnumerateFeatureSets(combination, featureList, depth + 1, callback);

                if (!string.IsNullOrEmpty(keyword))
                {
                    combination.Remove(keyword);
                }
            }
        }

        private static string[] GetKeywordsList(PassType passType, HashSet<string> combination, int[] rawTypes, string[] rawKeywords)
        {
            for (int i = 0; i < rawTypes.Length; i++)
            {
                if (rawTypes[i] != (int)passType)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(rawKeywords[i]) && combination.Count == 0)
                {
                    Debug.Log(passType.ToString() + " <no keyword>");
                    return new string[] { };
                }

                string[] arr = rawKeywords[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (arr.Length != combination.Count)
                {
                    continue;
                }

                bool match = true;
                foreach (var keyword in arr)
                {
                    if (!combination.Contains(keyword))
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    Debug.Log(passType.ToString() + " " + rawKeywords[i]);
                    return arr;
                }
            }
            throw new Exception("Combination doesn't exist.");
        }

        private static void AddUniversalVariantsToCollection(Shader shader, ShaderVariantInfo info, ShaderVariantCollection collection)
        {
            List<FeatureKey> featureList = new List<FeatureKey>();

            foreach (var passType in info.Features.Keys)
            {
                featureList.Clear();
                foreach (var feature in info.Features[passType])
                {
                    featureList.Add(feature);
                }

                EnumerateFeatureSets(new HashSet<string>(), featureList, 0, delegate (HashSet<string> combination)
                {
                    Debug.Log(shader.name);
                    try
                    {
                        collection.Add(new ShaderVariantCollection.ShaderVariant(shader, passType,
                            GetKeywordsList(passType, combination, info.RawTypes, info.RawKeywords)));
                    }
                    catch (Exception e)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(shader.name);
                        sb.Append(passType.ToString());

                        foreach (var keyword in combination)
                        {
                            sb.Append(' ');
                            sb.Append(keyword);
                        }
                        sb.AppendLine();

                        int[] types = new int[12];
                        foreach (int type in info.RawTypes)
                        {
                            types[type] = 1;
                        }
                        foreach (int type in types)
                        {
                            sb.Append(' ');
                            sb.Append(type);
                        }
                        sb.AppendLine();
                        sb.AppendLine(e.ToString());
                        Debug.LogWarning(sb.ToString());
                    }
                });
            }
        }

        [MenuItem("Game Framework/Shader Tools/Generate Universal Shader Variant Collection", priority = 5003)]
        public static void GenerateUniversalVariantCollection()
        {
            var shaders = UsedShadersFinder.FindAllUsedShaders();

            var emptyCollection = new ShaderVariantCollection();

            var universalCollection = new ShaderVariantCollection();
            if (File.Exists(UniversalCollectionPath))
            {
                AssetDatabase.DeleteAsset(UniversalCollectionPath);
            }

            foreach (var instanceId in shaders.Keys)
            {
                var shaderInfo = shaders[instanceId];
                if (UsedShadersFinder.BuiltInShaderPath == shaderInfo.path)
                {
                    continue;
                }

                var shader = Shader.Find(shaderInfo.name);
                if (null == shader)
                {
                    Debug.LogError("Invalid shader name");
                    continue;
                }

                if (ExcludedShaders.Contains(shader.name))
                {
                    continue;
                }

                int[] types = null;
                string[] keywords = null;

                GetShaderVariantEntries(shader, emptyCollection, out types, out keywords);

                if (null == types || null == keywords || types.Length != keywords.Length)
                {
                    throw new Exception("Failed to get variants info from shader: " + shader.name);
                }

                AddUniversalVariantsToCollection(shader, new ShaderVariantInfo(types, keywords), universalCollection);
            }

            AssetDatabase.CreateAsset(universalCollection, UniversalCollectionPath);
            AssetDatabase.SaveAssets();
        }
    }
}
