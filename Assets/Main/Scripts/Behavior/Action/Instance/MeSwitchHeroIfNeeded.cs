using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    [TaskDescription("Switch the current player's hero based on conditions. This task will complete immediately.")]
    public class MeSwitchHeroIfNeeded : Action
    {
        [SerializeField]
        private float m_CurrentHeroMaxHPRatio = .15f;

        [SerializeField]
        private float m_BgHeroMinHPRatio = .3f;

        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.NonInstance)
            {
                Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            var heroesData = instanceLogic.MyHeroesData;

            if (heroesData.CurrentHeroData.BaseHPRatio > m_CurrentHeroMaxHPRatio)
            {
                return TaskStatus.Failure;
            }

            var heroes = heroesData.GetHeroes();

            int newHeroIndex = -1;
            for (int i = 0; i < heroes.Length; ++i)
            {
                if (i == heroesData.CurrentHeroIndex)
                {
                    continue;
                }

                if (heroes[i].BaseHPRatio > m_BgHeroMinHPRatio)
                {
                    newHeroIndex = i;
                    break;
                }
            }

            if (newHeroIndex < 0 || instanceLogic.HeroIsCoolingDown(newHeroIndex))
            {
                return TaskStatus.Failure;
            }

            instanceLogic.RequestSwitchHero(newHeroIndex);
            return TaskStatus.Success;
        }
    }
}
