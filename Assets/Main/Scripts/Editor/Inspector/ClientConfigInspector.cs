using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(ClientConfig))]
    public class ClientConfigInspector : UnityEditor.Editor
    {
        private const string GameClientPath = "Assets/Main/Configs/GameClientConfig.asset";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var t = target as ClientConfig;
            SerializedProperty sp;

            sp = serializedObject.FindProperty("m_StringReplacementLabelColorConfigs");
            EditorGUILayout.PropertyField(sp, true);

            sp = serializedObject.FindProperty("m_LuaScriptsToPreload");
            EditorGUILayout.PropertyField(sp, true);

            sp = serializedObject.FindProperty("m_GenericSkillBadgeSlotColors");
            EditorGUILayout.PropertyField(sp, true);

            sp = serializedObject.FindProperty("m_ColorConfig");
            EditorGUILayout.PropertyField(sp, true);

            sp = serializedObject.FindProperty("m_ShaderPathsForWarmUp");
            EditorGUILayout.PropertyField(sp, true);

            sp = serializedObject.FindProperty("m_BuildAccount");
            EditorGUILayout.PropertyField(sp, true);

            if (GUILayout.Button("Edit Device Models"))
            {
                ClientConfigEditorWindow.OpenWindow(t);
            }

            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem("Game Framework/Edit Client Config", priority = 3000)]
        public static void EditGameClientConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath<ClientConfig>(GameClientPath);
            ClientConfigEditorWindow.OpenWindow(config);
        }
    }
}
