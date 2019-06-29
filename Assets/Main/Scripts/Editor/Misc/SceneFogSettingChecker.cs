using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    public class SceneFogSettingChecker : EditorWindow
    {
        [MenuItem("Assets/Check Fog Settings of All Scene")]
        public static void CheckFogSettingOfAllScene()
        {
            var guids = AssetDatabase.FindAssets("t:scene");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(path))
                {
                    throw new System.Exception("Invalid scene guid or path");
                }

                var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                Debug.Log(scene.name + " [" + (RenderSettings.fog ? RenderSettings.fogMode.ToString() : "fog disabled") + "]");
            }
        }

        private void OnGUI()
        {

        }
    }
}
