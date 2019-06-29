using System.Text;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(CharacterMotion))]
    internal class CharacterMotionInspector : UnityEditor.Editor
    {
        private static StringBuilder s_SharedSb = new StringBuilder();

        public override void OnInspectorGUI()
        {
            CharacterMotion t = target as CharacterMotion;
            EditorGUILayout.LabelField("State", t.CurrentStateName);
            DrawCurrentlyPlayingAnimationClips(t);
        }

        private void DrawCurrentlyPlayingAnimationClips(CharacterMotion t)
        {
            var anim = t.GetComponent<Animation>();
            if (anim == null)
            {
                return;
            }

            s_SharedSb.Length = 0;
            bool first = true;
            foreach (AnimationState state in anim)
            {
                if (anim.IsPlaying(state.clip.name))
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        s_SharedSb.Append(", ");
                    }
                    s_SharedSb.Append(state.clip.name);
                }
            }
            EditorGUILayout.LabelField("Playing Animation Clips", s_SharedSb.ToString());
        }
    }
}
