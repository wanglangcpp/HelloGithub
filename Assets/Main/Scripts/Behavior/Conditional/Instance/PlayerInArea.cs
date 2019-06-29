using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class PlayerInArea : Conditional
    {
        [SerializeField]
        private SharedVector2 m_AreaCenter = null;

        [SerializeField]
        private SharedFloat m_AreaRadius = null;

        public override TaskStatus OnUpdate()
        {
            return Vector2.Distance(GameEntry.SceneLogic.MeHeroCharacter.CachedTransform.localPosition.ToVector2(), m_AreaCenter.Value) <= m_AreaRadius.Value ? TaskStatus.Success : TaskStatus.Failure;
        }

#if UNITY_EDITOR
        private static readonly Color WireGizmoColor = Color.green;
        private static readonly Color SolidGizmoColor = Color.green * new Color(1f, 1f, 1f, 0.05f);

        private Vector3 m_Center3D;

        public override void OnAwake()
        {
            m_Center3D = AIUtility.SamplePosition(m_AreaCenter.Value);
        }

        public override void OnDrawGizmos()
        {
            var cachedColor = Handles.color;
            Handles.color = WireGizmoColor;
            Handles.DrawWireDisc(m_Center3D, Vector3.up, m_AreaRadius.Value);
            Handles.color = SolidGizmoColor;
            Handles.DrawSolidDisc(m_Center3D, Vector3.up, m_AreaRadius.Value);
            Handles.color = cachedColor;
        }

#endif
    }
}
