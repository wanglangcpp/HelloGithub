using UnityEditor;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(HeroCharacter))]
    internal class HeroCharacterInspector : CharacterInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Weapons"), true);
        }

        protected override SerializedProperty GetDataProperty(Entity t)
        {
            return serializedObject.FindProperty("m_HeroData");
        }
    }
}
