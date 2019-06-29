using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    public partial class PerformSkill
    {
        private class StrategyCharacter : StrategyBase
        {
            private Character m_Character;
            private PerformSkillOperation m_PerformSkillOperation = null;

            public StrategyCharacter(PerformSkill action, Character character)
                : base(action)
            {
                m_Character = character;
            }

            public override TaskStatus PerformSkill()
            {
                CharacterMotion characterMotion = m_Character.Motion;
                if (characterMotion == null)
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

                var op = characterMotion.PerformSkill(m_Action.m_SkillId.Value, -1, false, false, false, false, PerformSkillType.Click);
                if (op == null || op.State == PerformSkillOperationState.PerformFailure)
                {
                    return TaskStatus.Failure;
                }

                m_PerformSkillOperation = op;
                CheckFaceTarget();
                return TaskStatus.Running;
            }

            public override void Reset()
            {
                base.Reset();
                m_PerformSkillOperation = null;
            }

            private void CheckFaceTarget()
            {
                var npc = m_Character as NpcCharacter;
                if (npc == null || !npc.HasTarget)
                {
                    return;
                }

                var targetTransform = (npc.Target as Entity).CachedTransform;

                if (m_Action.m_FaceTarget)
                {
                    npc.CachedTransform.LookAt2D(targetTransform.position.ToVector2());
                }
            }
        }
    }
}
