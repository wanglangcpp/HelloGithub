using UnityEditor;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(Character))]
    internal class CharacterInspector : EntityInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
