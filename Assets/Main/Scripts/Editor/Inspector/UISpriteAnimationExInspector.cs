using UnityEditor;

namespace Genesis.GameClient.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UISpriteAnimationEx))]
    public class UISpriteAnimationExInspector : UISpriteAnimationInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            NGUIEditorTools.DrawProperty("Play Mode", serializedObject, "m_PlayMode");
            serializedObject.ApplyModifiedProperties();
        }
    }
}
