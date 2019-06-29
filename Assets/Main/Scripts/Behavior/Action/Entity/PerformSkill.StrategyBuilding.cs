using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    public partial class PerformSkill
    {
        private class StrategyBuilding : StrategyBase
        {
            private Building m_Building = null;
            private PerformSkillOperation m_PerformSkillOperation = null;

            public StrategyBuilding(PerformSkill action, Building building)
                : base(action)
            {
                m_Building = building;
            }

            public override TaskStatus PerformSkill()
            {
                var shooter = m_Building.ShooterTransform;
                if (shooter == null)
                {
                    return TaskStatus.Failure;
                }

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
                        case PerformSkillOperationState.Waiting:
                        case PerformSkillOperationState.Performing:
                        default:
                            return TaskStatus.Running;
                    }
                }

                CheckFaceTarget(shooter);

                var op = m_Building.Motion.PerformSkill(m_Action.m_SkillId.Value, -1, false, false, false, false, PerformSkillType.Click);
                if (op == null || op.State == PerformSkillOperationState.PerformFailure)
                {
                    return TaskStatus.Failure;
                }

                m_PerformSkillOperation = op;
                return TaskStatus.Running;
            }

            private void CheckFaceTarget(UnityEngine.Transform shooter)
            {
                if (m_Action.m_FaceTarget && m_Building.HasTarget)
                {
                    shooter.LookAt2D((m_Building.Target as Entity).CachedTransform.position.ToVector2());
                }
            }
        }
    }
}
