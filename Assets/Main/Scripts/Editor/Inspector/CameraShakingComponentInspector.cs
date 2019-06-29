using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(CameraShakingComponent))]
    internal class CameraShakingComponentInspector : UnityEditor.Editor
    {
        private int m_IndexToPerform = 0;

        public override void OnInspectorGUI()
        {
            var cameraShakingComponent = target as CameraShakingComponent;
            EditorGUILayout.BeginHorizontal("box");
            {
                GUILayout.Label("Test with index: ");
                m_IndexToPerform = EditorGUILayout.IntField(m_IndexToPerform);
                GUILayout.Space(6f);
                if (GUILayout.Button("Perform Shaking"))
                {
                    cameraShakingComponent.PerformShaking(m_IndexToPerform);
                }
            }
            EditorGUILayout.EndHorizontal();

            base.OnInspectorGUI();
        }
    }
}
