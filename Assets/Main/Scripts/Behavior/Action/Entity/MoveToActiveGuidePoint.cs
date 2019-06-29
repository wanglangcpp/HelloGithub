using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Common")]
    [TaskDescription("Move the owner hero character towards the target. ")]
    public class MoveToActiveGuidePoint : Action
    {
        [SerializeField]
        private float m_MaxMoveDuration = 1f;

        [SerializeField]
        private float m_ArrivalDistance = 1f;

        private float m_OriginalStoppingDistance = 0f;
        private HeroCharacter m_Self = null;
        private float m_StartTime = -1f;
        private Vector3? m_TargetPosition = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<HeroCharacter>();
            m_OriginalStoppingDistance = m_Self.NavAgent.stoppingDistance;
            m_Self.NavAgent.stoppingDistance = 0f;
            m_StartTime = Time.time;
        }

        public override void OnEnd()
        {
            ResetTime();
            m_Self.Motion.StopMove();
            m_Self.NavAgent.stoppingDistance = m_OriginalStoppingDistance;
            m_TargetPosition = null;
        }

        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.NonInstance)
            {
                Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            var guidePoints = instanceLogic.GuidePointSet;
            if (guidePoints.ActiveGuidePoint == null)
            {
                return TaskStatus.Failure;
            }

            var activeGuidePoint = guidePoints.ActiveGuidePoint.Value;

            if (Vector2.SqrMagnitude(activeGuidePoint - m_Self.CachedTransform.position.ToVector2()) <= m_ArrivalDistance * m_ArrivalDistance)
            {
                guidePoints.InvalidateActiveGuidePoint();
                return TaskStatus.Success;
            }

            var executionTime = Time.time - m_StartTime;
            if (executionTime > m_MaxMoveDuration)
            {
                return TaskStatus.Success;
            }

            if (!m_TargetPosition.HasValue)
            {
                m_TargetPosition = AIUtility.SamplePosition(activeGuidePoint);
            }

            m_Self.Motion.StartMove(m_TargetPosition.Value);
            return TaskStatus.Running;
        }

        private void ResetTime()
        {
            m_StartTime = -1f;
        }
    }
}
