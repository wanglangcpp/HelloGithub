using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class PlayerInAreaRectangle : Conditional
    {
        [SerializeField]
        private Vector2 m_AreaCenter = Vector2.zero;

        [SerializeField]
        private float m_Rotation = 0f;

        [SerializeField]
        private float m_Width = 0f;

        [SerializeField]
        private float m_Height = 0f;

        private Vector2 m_RectForward;
        private Vector2 m_RectRight;

        public override void OnAwake()
        {
#if UNITY_EDITOR
            InitGizmos();
#endif

            var rotInRadians = Mathf.Deg2Rad * m_Rotation;
            var cos = Mathf.Cos(rotInRadians);
            var sin = Mathf.Sin(rotInRadians);

            m_RectRight = new Vector2(cos, -sin);
            m_RectForward = new Vector2(sin, cos);
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.IsAvailable || GameEntry.SceneLogic == null)
            {
                Log.Warning("GameEntry is invalid.");
                return TaskStatus.Failure;
            }

            var myHeroCharacter = GameEntry.SceneLogic.MeHeroCharacter;
            if (myHeroCharacter == null)
            {
                // 正在换英雄
                return TaskStatus.Failure;
            }

            var myDisplacement = myHeroCharacter.CachedTransform.localPosition.ToVector2() - m_AreaCenter;

            bool inRect = (Mathf.Abs(Vector2.Dot(myDisplacement, m_RectForward)) < m_Height / 2f) && (Mathf.Abs(Vector2.Dot(myDisplacement, m_RectRight)) < m_Width / 2f);
            return inRect ? TaskStatus.Success : TaskStatus.Failure;
        }

#if UNITY_EDITOR
        private static readonly Color WireGizmoColor = Color.green;
        private static readonly Color SolidGizmoColor = Color.green * new Color(1f, 1f, 1f, 0.05f);
        private static readonly Color ArrowColor = Color.blue;

        private Vector3 m_Center3D;

        private void InitGizmos()
        {
            m_Center3D = AIUtility.SamplePosition(m_AreaCenter);
        }

        public override void OnDrawGizmos()
        {
            var forward = m_RectForward.ToVector3();
            var right = m_RectRight.ToVector3();

            var verts = new Vector3[]
            {
                m_Center3D + forward * m_Height / 2f + right * m_Width / 2f,
                m_Center3D + forward * m_Height / 2f - right * m_Width / 2f,
                m_Center3D - forward * m_Height / 2f - right * m_Width / 2f,
                m_Center3D - forward * m_Height / 2f + right * m_Width / 2f,
            };

            Handles.DrawSolidRectangleWithOutline(verts, SolidGizmoColor, WireGizmoColor);

            var cachedColor = Handles.color;
            Handles.color = ArrowColor;
            var up = Vector3.Cross(forward, right);
            Handles.ArrowCap(0, m_Center3D, Quaternion.LookRotation(forward, up), 1f);
            Handles.DotCap(0, m_Center3D, Quaternion.LookRotation(forward, up), 0.05f);
            Handles.color = cachedColor;
        }

#endif
    }
}
