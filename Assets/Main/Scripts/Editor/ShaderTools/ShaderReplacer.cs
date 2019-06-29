using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class ShaderReplacer
    {
        private const string ProgressTitle = "Shader Replacer";
        private const string ProgressMessageFormat = "Scanning materials... {0}/{1}";
        private const string CompletingMessage = "Completing...";

        [MenuItem("Game Framework/Shader Tools/Replace Shaders", priority = 5000)]
        public static void Run()
        {
            var materialGuids = AssetDatabase.FindAssets("t:Material");

            EditorUtility.DisplayProgressBar(ProgressTitle, ProgressMessageFormat, 0f);

            int progressCount = 0;
            foreach (var guid in materialGuids)
            {
                ++progressCount;
                EditorUtility.DisplayProgressBar(ProgressTitle, string.Format(ProgressMessageFormat, progressCount, materialGuids.Length), (float)(progressCount) / materialGuids.Length);
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }

                var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (material == null)
                {
                    continue;
                }

                var shader = material.shader;
                if (shader == null)
                {
                    continue;
                }

                var shaderPath = AssetDatabase.GetAssetPath(shader);

                //Debug.LogFormat("progressCount: {0}, shaderPath: '{1}', shaderName: {2}", progressCount, shaderPath, shader.name);

                if (!shaderPath.Contains("unity_builtin"))
                {
                    continue;
                }

                var newShader = Shader.Find(shader.name);
                if (newShader == null)
                {
                    continue;
                }

                var newShaderPath = AssetDatabase.GetAssetPath(newShader);
                if (newShaderPath == shaderPath)
                {
                    continue;
                }

                Debug.LogFormat("Setting shader '{0}' for material '{1}'", AssetDatabase.GetAssetPath(newShader), path);
                material.shader = newShader;
            }

            EditorUtility.DisplayProgressBar(ProgressTitle, CompletingMessage, 1f);
            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }
    }
}
