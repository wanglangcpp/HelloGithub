using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class FindAllShaderNamesInFolder : EditorWindow
    {

        [MenuItem("Game Framework/Shader Tools/Find All Shaders' Names", priority = 5004)]
        public static void ScanShaders()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if ((File.GetAttributes(path) & FileAttributes.Directory) != FileAttributes.Directory)
            {
                return;
            }

            string[] shaderPaths = Directory.GetFiles(path, "*.shader", SearchOption.AllDirectories);

            Regex shaderNameRegex = new Regex("Shader\\s+\"([^\"]+)\"");
            StringBuilder sb = new StringBuilder();

            foreach (var shaderPath in shaderPaths)
            {
                string content = File.ReadAllText(shaderPath);
                var collection = shaderNameRegex.Matches(content);
                if (collection[0].Groups[1].Value.StartsWith("Hidden/"))
                    continue;

                sb.AppendLine(collection[0].Groups[1].Value);
            }

            var window = GetWindow<FindAllShaderNamesInFolder>();
            window.shaderLists = sb.ToString();
        }

        private string shaderLists = string.Empty;
        private Vector2 pos = Vector2.zero;

        private void OnGUI()
        {
            pos = EditorGUILayout.BeginScrollView(pos);
            if (!string.IsNullOrEmpty(shaderLists))
            {
                EditorGUILayout.TextArea(shaderLists);
            }
            EditorGUILayout.EndScrollView();
        }

    }
}
