using UnityEditor;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(Bullet))]
    internal class BulletInspector : EntityInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Bullet t = target as Bullet;
            EditorGUILayout.ObjectField("Owner", t.Owner, typeof(Character), true);
            EditorGUILayout.LabelField("Left Time", t.LeftTime == null ? "Forever" : t.LeftTime.Value.ToString("F2"));
            Repaint();
        }
    }
}
