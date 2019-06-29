using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    internal class TexturePostprocessor : AssetPostprocessor
    {
        private const string Force512Label = "Force512";
        private const string Force1024Label = "Force1024";
        private const string Force2048Label = "Force2048";

        private readonly static HashSet<string> ForceSizeLabels = new HashSet<string>
        {
            Force512Label,
            Force1024Label,
        };

        public static bool IsForceSizeLabel(string assetLabel)
        {
            if (string.IsNullOrEmpty(assetLabel))
            {
                return false;
            }

            return ForceSizeLabels.Contains(assetLabel);
        }

        private void OnPreprocessTexture()
        {
            var textureImporter = assetImporter as TextureImporter;

            if (assetPath.Contains("Main/UI/Textures/"))
            {
                ProcessUIRawTexture(textureImporter);
            }
            else if (assetPath.Contains("Main/UI/Atlases/") || assetPath.Contains("Main/UI/UITextures/"))
            {
                ProcessUIAtlas(textureImporter);
            }
            else if(assetPath.Contains("Main/Models/SceneObjects")||assetPath.Contains("Main/Scenes"))
            {
                return;
            }

            CommonlyProcessTexture(textureImporter);
        }

        private void ProcessUIRawTexture(TextureImporter textureImporter)
        {
            textureImporter.textureType = TextureImporterType.Advanced;
            textureImporter.mipmapEnabled = false;
            textureImporter.isReadable = true;
            textureImporter.npotScale = TextureImporterNPOTScale.None;
        }

        private void ProcessUIAtlas(TextureImporter textureImporter)
        {
            textureImporter.textureType = TextureImporterType.Advanced;
            textureImporter.mipmapEnabled = false;
            textureImporter.npotScale = TextureImporterNPOTScale.None;
        }

        private void CommonlyProcessTexture(TextureImporter textureImporter)
        {
            var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture)) as Texture;
            if (asset == null)
            {
                return;
            }

            var labels = AssetDatabase.GetLabels(asset);

            if (labels.Contains(Force512Label))
            {
                textureImporter.maxTextureSize = Mathf.Min(512, textureImporter.maxTextureSize);
                var newLabels = new List<string>(labels);
                newLabels.RemoveAll(l => l == Force1024Label);
                AssetDatabase.SetLabels(asset, newLabels.ToArray());
            }
            else if (labels.Contains(Force1024Label))
            {
                textureImporter.maxTextureSize = Mathf.Min(1024, textureImporter.maxTextureSize);
            }
            else if(labels.Contains(Force2048Label))
            {
                textureImporter.maxTextureSize = Mathf.Min(2048, textureImporter.maxTextureSize);
            }
            else
            {
                textureImporter.maxTextureSize = Mathf.Min(256, textureImporter.maxTextureSize);
            }
        }
    }
}
