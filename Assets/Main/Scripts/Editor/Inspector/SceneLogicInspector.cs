using UnityEditor;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(SceneLogicComponent))]
    internal class SceneLogicComponentInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SceneLogicComponent t = target as SceneLogicComponent;
            EditorGUILayout.LabelField("State", t.State.ToString());
            if (t.State != SceneLogicState.Ready)
            {
                return;
            }

            EditorGUILayout.LabelField("Scene Id", t.SceneId.ToString());
            EditorGUILayout.LabelField("Scene Name", t.SceneName.ToString());
            EditorGUILayout.LabelField("Instance Logic Type", t.InstanceLogicType.ToString());

            SerializedProperty instanceLogicProperty = serializedObject.FindProperty(string.Format("m_{0}", t.BaseInstanceLogic.GetType().Name));
            if (instanceLogicProperty != null)
            {
                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUILayout.PropertyField(instanceLogicProperty, true);
                }
                EditorGUI.EndDisabledGroup();
            }
        }
    }
}
