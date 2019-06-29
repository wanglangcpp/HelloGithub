//------------------------------------------------------------
// Game Framework v2.x
// Copyright © 2014-2016 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEditor;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(SpawnPoint))]
    internal sealed class SpawnPointInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty property = serializedObject.FindProperty("m_Color");
            property.colorValue = EditorGUILayout.ColorField("Color", property.colorValue);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
