using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class HeroesHPRatio : Conditional
    {
        [SerializeField]
        private float m_TargetRatio = 0f;

        [SerializeField]
        private OrderRelationType m_OrderRelation = OrderRelationType.EqualTo;

        [SerializeField]
        private NumericalCalcStrategy m_CalcStrategy = NumericalCalcStrategy.Min;

        public override TaskStatus OnUpdate()
        {
            var heroes = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData.GetHeroes();
            var ratioData = new float[heroes.Length];

            for (int i = 0; i < heroes.Length; ++i)
            {
                ratioData[i] = heroes[i].HPRatio;
            }

            var currentVal = NumericalCalcUtility.CalcFloat(ratioData, m_CalcStrategy);
            return OrderRelationUtility.AreSatisfying<float>(currentVal, m_TargetRatio, m_OrderRelation) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
