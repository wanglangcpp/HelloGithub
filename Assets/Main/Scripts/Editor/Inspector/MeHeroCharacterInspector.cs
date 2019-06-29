using UnityEditor;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(MeHeroCharacter))]
    internal class MeHeroCharacterInspector : HeroCharacterInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.HelpBox("This is your main player!", MessageType.Info);
        }
    }
}
