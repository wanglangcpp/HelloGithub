using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    internal class LuaScriptPostprocessor : AssetPostprocessor
    {
        private const string LuaScriptLabel = "LuaScript";

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                if (!assetPath.ToLower().EndsWith(".lua"))
                {
                    continue;
                }

                var filePath = Regex.Replace(assetPath, @"^Assets", Application.dataPath);
                if (!File.Exists(filePath) || Directory.Exists(filePath))
                {
                    continue;
                }

                var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                var labels = new HashSet<string>(AssetDatabase.GetLabels(asset));
                labels.Add(LuaScriptLabel);
                AssetDatabase.SetLabels(asset, new List<string>(labels).ToArray());
            }

            AssetDatabase.SaveAssets();
        }
    }
}
