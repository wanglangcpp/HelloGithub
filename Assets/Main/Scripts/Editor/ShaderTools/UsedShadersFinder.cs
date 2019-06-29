using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class UsedShadersFinder : EditorWindow
    {
        public const string BuiltInShaderPath = "Resources/unity_builtin_extra";

        public class ShaderInfo
        {
            public string name;
            public string path;
            public List<Material> references;
        }

        public static Dictionary<int, ShaderInfo> FindAllUsedShaders()
        {
            var shaders = new Dictionary<int, ShaderInfo>();

            var mats = AssetDatabase.FindAssets("t:Material");
            foreach (var guid in mats)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                var instanceId = mat.shader.GetInstanceID();

                ShaderInfo info = null;
                shaders.TryGetValue(instanceId, out info);
                if (null == info)
                {
                    info = new ShaderInfo();
                    info.name = mat.shader.name;
                    info.path = AssetDatabase.GetAssetPath(mat.shader);
                    info.references = new List<Material>();

                    shaders.Add(instanceId, info);
                }

                info.references.Add(mat);
            }

            return shaders;
        }

        [MenuItem("Game Framework/Shader Tools/Find All Used Shaders", priority = 5001)]
        private static void OpenWindow()
        {
            var shaders = FindAllUsedShaders();
            var finder = GetWindow<UsedShadersFinder>();

            finder.shaders = shaders;
            finder.folded = new Dictionary<int, bool>();
        }

        private Vector2 scrollPosition = Vector2.zero;
        private Dictionary<int, ShaderInfo> shaders;
        private Dictionary<int, bool> folded;
        private bool showShaders = true;

        private void OnGUI()
        {
            if (null == shaders)
                return;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            List<int> keys = new List<int>(shaders.Keys);
            keys.Sort(delegate (int a, int b)
            {
                return shaders[a].name.CompareTo(shaders[b].name);
            });

            showShaders = EditorGUILayout.Foldout(showShaders, keys.Count + " Shaders");

            if (showShaders)
            {
                foreach (var key in keys)
                {
                    var info = shaders[key];
                    var f = folded.ContainsKey(key) ? folded[key] : false;

                    Color origColor = GUI.color;
                    if (info.path == BuiltInShaderPath)
                        GUI.color = Color.red;

                    if (f = EditorGUILayout.Foldout(f, info.name + "    [" + info.path + "]     [references: " + info.references.Count + "]"))
                    {
                        foreach (var mat in info.references)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            EditorGUILayout.ObjectField(mat, typeof(Material), false, GUILayout.Width(200));
                            EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(mat));
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    folded[key] = f;

                    GUI.color = origColor;
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
