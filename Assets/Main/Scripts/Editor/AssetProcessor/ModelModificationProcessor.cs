using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor;

namespace Genesis.GameClient.Editor
{
    internal class ModelModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static string[] OnWillSaveAssets(string[] assetPaths)
        {
            foreach (string path in assetPaths)
            {
                var pathWithoutMeta = path;

                if (path.EndsWith(".meta"))
                {
                    pathWithoutMeta = Regex.Replace(path, @"\.meta$", string.Empty);
                }

                if (!pathWithoutMeta.ToUpper().EndsWith(".FBX"))
                {
                    continue;
                }

                var asset = AssetDatabase.LoadAssetAtPath(pathWithoutMeta, typeof(GameObject)) as GameObject;
                if (asset == null)
                {
                    continue;
                }

                ProjectSaver.AddAssetPathToReimport(pathWithoutMeta);
            }

            return assetPaths;
        }
    }
}
