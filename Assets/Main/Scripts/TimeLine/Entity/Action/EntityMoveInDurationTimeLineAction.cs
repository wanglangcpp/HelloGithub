using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract class EntityMoveInDurationTimeLineAction : EntityAbstractTimeLineAction
    {
        // 子类中初始化
        protected float m_Delay = 0f;

        protected bool m_FaceTargetPositionOnMove = false;

        private Vector3 m_StartPosition = Vector3.zero;
        private Vector3 m_TargetPosition = Vector3.zero;
        private float m_CurrentTime = 0f;
        private bool m_HasStartedMoving = false;
        private NpcCharacter m_NpcOwner = null;

        public EntityMoveInDurationTimeLineAction(TimeLineActionData data)
            : base(data)
        {

        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            m_NpcOwner = timeLineInstance.Owner as NpcCharacter;

            if (m_NpcOwner == null)
            {
                Log.Error("This time line action is only intended for an NpcCharacter, rather than a '{0}'", timeLineInstance.Owner.GetType());
                return;
            }

            m_StartPosition = m_NpcOwner.CachedTransform.localPosition;
            m_TargetPosition = GetTargetPosition(m_NpcOwner);
        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {
            m_CurrentTime += elapseSeconds;
            UpdateMoving(timeLineInstance);
        }

        private void UpdateMoving(ITimeLineInstance<Entity> timeLineInstance)
        {
            if (m_CurrentTime <= m_Delay)
            {
                return;
            }

            var startPos = m_StartPosition;
            var targetPos = m_TargetPosition;
            float proportion = m_Delay >= Duration ? 1f : Mathf.Clamp01(NumericalCalcUtility.CalcProportion(m_Delay, Duration, m_CurrentTime));
            (timeLineInstance.Owner as Character).NavAgent.nextPosition = startPos + proportion * (targetPos - startPos);

            if (m_HasStartedMoving)
            {
                return;
            }

            m_HasStartedMoving = true;

            if (m_FaceTargetPositionOnMove)
            {
                m_NpcOwner.CachedTransform.LookAt2D(targetPos.ToVector2());
            }
        }

        protected abstract Vector3 GetTargetPosition(NpcCharacter npcOwner);
    }
}
