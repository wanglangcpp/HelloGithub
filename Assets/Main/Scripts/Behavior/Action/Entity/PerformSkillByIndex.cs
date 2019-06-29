using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    [TaskDescription("Used for hero AI.")]
    public partial class PerformSkillByIndex : Action
    {
        [SerializeField]
        private int m_SkillIndex = 0;

        [SerializeField]
        private bool m_FaceTarget = true;

        [SerializeField]
        private bool m_WaitForFinish = true;

        [SerializeField]
        private float m_SelectTargetDistance = 0f;

        [SerializeField]
        private float m_SelectTargetInterval = 0f;

        private float m_LastSelectTargetTime;
        private HeroCharacter m_Self = null;
        private PerformSkillOperation m_PerformSkillOperation = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<HeroCharacter>();
            m_LastSelectTargetTime = Time.time;
        }

        public override void OnEnd()
        {
            m_PerformSkillOperation = null;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Self == null)
            {
                Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            var characterMotion = m_Self.Motion;

            if (m_PerformSkillOperation != null)
            {
                switch (m_PerformSkillOperation.State)
                {
                    case PerformSkillOperationState.PerformEnd:
                        m_PerformSkillOperation = null;
                        return TaskStatus.Success;
                    case PerformSkillOperationState.PerformFailure:
                        m_PerformSkillOperation = null;
                        return TaskStatus.Failure;

                    case PerformSkillOperationState.Performing:
                        if (!m_WaitForFinish)
                        {
                            m_PerformSkillOperation = null;
                            return TaskStatus.Success;
                        }

                        if (characterMotion.Skilling && m_SelectTargetDistance > 0f && m_SelectTargetInterval > 0f && Time.time - m_LastSelectTargetTime > m_SelectTargetInterval)
                        {
                            ReselectTarget();
                            CheckFaceTarget();
                        }
                        return TaskStatus.Running;
                    case PerformSkillOperationState.Waiting:
                    default:
                        return TaskStatus.Running;
                }
            }

            if (!m_Self.HasTarget)
            {
                return TaskStatus.Failure;
            }

            var op = m_Self.PerformSkillOnIndex(m_SkillIndex, PerformSkillType.Click);
            if (op == null || op.State == PerformSkillOperationState.PerformFailure)
            {
                return TaskStatus.Failure;
            }

            CheckFaceTarget();
            m_PerformSkillOperation = op;
            return TaskStatus.Running;
        }

        private void CheckFaceTarget()
        {
            if (!m_FaceTarget || !m_Self.HasTarget)
            {
                return;
            }

            var targetTransform = (m_Self.Target as Entity).CachedTransform;
            m_Self.CachedTransform.LookAt2D(targetTransform.position.ToVector2());
        }

        private void ReselectTarget()
        {
            if (m_Self.HasTarget)
            {
                return;
            }

            m_Self.Target = AIUtility.GetNearestTarget(m_Self, RelationType.Hostile, m_SelectTargetDistance);
        }
    }
}
