using UnityEditor;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(Effect))]
    internal class EffectInspector : EntityInspector
    {
        public override void OnInspectorGUI()
        {
            Entity t = target as Entity;
            serializedObject.Update();
            EditorGUILayout.PropertyField(GetDataProperty(t), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
