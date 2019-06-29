using UnityEditor;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(Entity))]
    internal class EntityInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Entity t = target as Entity;
            serializedObject.Update();
            EditorGUILayout.PropertyField(GetDataProperty(t), true);
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual SerializedProperty GetDataProperty(Entity t)
        {
            return serializedObject.FindProperty(string.Format("m_{0}Data", t.GetType().Name));
        }
    }
}
