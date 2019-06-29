using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    public partial class PerformSkill
    {
        private class StrategyBullet : StrategyBase
        {
            private Bullet m_Bullet;

            public StrategyBullet(PerformSkill action, Bullet bullet)
                : base(action)
            {
                m_Bullet = bullet;
            }

            public override TaskStatus PerformSkill()
            {
                m_Bullet.PerformSkill(m_Action.m_SkillId.Value);
                return TaskStatus.Success;
            }
        }
    }
}
