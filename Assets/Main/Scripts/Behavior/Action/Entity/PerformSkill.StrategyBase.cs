using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    public partial class PerformSkill
    {
        private abstract class StrategyBase
        {
            protected PerformSkill m_Action = null;

            public StrategyBase(PerformSkill action)
            {
                m_Action = action;
            }

            public abstract TaskStatus PerformSkill();

            public virtual void Reset()
            {

            }
        }
    }
}
