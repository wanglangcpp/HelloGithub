using UnityEditor;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(NpcCharacter))]
    internal class NpcCharacterInspector : CharacterInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            NpcCharacter t = target as NpcCharacter;
            EditorGUILayout.ObjectField("Target", t.Target as Entity, typeof(Entity), true);
        }
    }
}
