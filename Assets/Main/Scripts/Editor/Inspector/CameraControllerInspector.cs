using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(CameraController))]
    internal class CameraControllerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var t = target as CameraController;

            EditorGUILayout.LabelField("State", t.CurrentStateId.ToString());
            EditorGUI.BeginDisabledGroup(t.CurrentStateId != CameraController.StateId.Following);
            {
                base.OnInspectorGUI();
                EditorGUILayout.BeginHorizontal("box");
                {
                    if (GUILayout.Button("Update Look At"))
                    {
                        var mi = t.GetType().GetMethod("UpdateLookAt", BindingFlags.NonPublic | BindingFlags.Instance);
                        mi.Invoke(t, null);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
