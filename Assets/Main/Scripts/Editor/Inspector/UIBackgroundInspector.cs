using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(UIBackground))]
    internal class UIBackgroundInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty maskBackgroundProperty = serializedObject.FindProperty("m_MaskBackground");
            SerializedProperty backgroundTemplateProperty = serializedObject.FindProperty("m_BackgroundTemplate");
            SerializedProperty backgroundDepth = serializedObject.FindProperty("m_Depth");

            string message = "This UI has NO background.";
            if (maskBackgroundProperty.boolValue)
            {
                message = "This UI has MASK background.";
            }
            else if (backgroundTemplateProperty.objectReferenceValue != null)
            {
                message = "This UI has PREFAB background.";
            }

            EditorGUILayout.HelpBox(message, MessageType.Info);

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUI.BeginDisabledGroup(backgroundTemplateProperty.objectReferenceValue != null);
                {
                    EditorGUILayout.PropertyField(maskBackgroundProperty, true);
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(maskBackgroundProperty.boolValue);
                {
                    EditorGUILayout.PropertyField(backgroundTemplateProperty, true);
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(backgroundDepth.intValue < 0);
                {
                    EditorGUILayout.PropertyField(backgroundDepth, true);
                }
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Remove Background"))
                {
                    maskBackgroundProperty.boolValue = false;
                    backgroundTemplateProperty.objectReferenceValue = null;
                }
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
