using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    internal class ModelPostprocessor : AssetPostprocessor
    {
        private const string AnimReduceKeyframeLabel = "AnimReduceKeyframe";
        private const string AnimCompressOffLabel = "AnimCompressOff";

        private readonly static HashSet<string> AnimCompressionLabels = new HashSet<string>
        {
            AnimReduceKeyframeLabel,
            AnimCompressOffLabel,
        };

        public static bool IsAnimCompressionLabel(string assetLabel)
        {
            if (string.IsNullOrEmpty(assetLabel))
            {
                return false;
            }

            return AnimCompressionLabels.Contains(assetLabel);
        }

        private void OnPreprocessModel()
        {
            var modelImporter = assetImporter as ModelImporter;
            modelImporter.importMaterials = false;

            if (assetPath.Contains("Main/Models/CollisionMeshes"))
            {
                modelImporter.isReadable = true;
            }
            else
            {
                modelImporter.isReadable = false;
            }

            modelImporter.meshCompression = ModelImporterMeshCompression.Medium;

            if (assetPath.Contains("Main/Models/Characters")
                || assetPath.Contains("Main/Models/Building")
                || assetPath.Contains("Main/Models/SceneObjects/AnimatedObjects")
                || assetPath.Contains("Main/Models/Others"))
            {
                TackleAnimatedModels(modelImporter);
            }
            else
            {
                TackleOtherModels(modelImporter);
            }
        }

        private void TackleAnimatedModels(ModelImporter modelImporter)
        {
            modelImporter.animationType = ModelImporterAnimationType.Legacy;
            UpdateAnimCompressionOption(modelImporter);
        }

        private void TackleOtherModels(ModelImporter modelImporter)
        {
            modelImporter.animationType = ModelImporterAnimationType.None;
        }

        private void UpdateAnimCompressionOption(ModelImporter modelImporter)
        {
            var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

            if (asset == null)
            {
                return;
            }

            var labels = AssetDatabase.GetLabels(asset);

            if (labels.Contains(AnimReduceKeyframeLabel))
            {
                modelImporter.animationCompression = ModelImporterAnimationCompression.KeyframeReduction;
                var newLabels = new List<string>(labels);
                newLabels.RemoveAll(l => l == AnimCompressOffLabel);
                AssetDatabase.SetLabels(asset, newLabels.ToArray());
            }
            else if (labels.Contains(AnimCompressOffLabel))
            {
                modelImporter.animationCompression = ModelImporterAnimationCompression.Off;
            }
            else
            {
                modelImporter.animationCompression = ModelImporterAnimationCompression.KeyframeReductionAndCompression;
            }
        }
    }
}
