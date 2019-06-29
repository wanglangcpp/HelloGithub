using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor;

namespace Genesis.GameClient.Editor
{
    internal class TextureModificationProcessor : UnityEditor.AssetModificationProcessor
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

                var asset = AssetDatabase.LoadAssetAtPath(pathWithoutMeta, typeof(Texture)) as Texture;
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
