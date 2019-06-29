using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    [TaskDescription("Switch the opponent's hero based on conditions. This task will complete immediately.")]
    public class OppSwitchHeroIfNeeded : Action
    {
        [SerializeField]
        private float m_CurrentHeroMaxHPRatio = .15f;

        [SerializeField]
        private float m_BgHeroMinHpRatio = .3f;

        public override TaskStatus OnUpdate()
        {
            if (!(GameEntry.SceneLogic.BaseInstanceLogic is BasePvpaiInstanceLogic))
            {
                Log.Warning("Not in a player v.s. player AI instance.");
                return TaskStatus.Failure;
            }

            var instanceLogic = GameEntry.SceneLogic.BasePvpaiInstanceLogic;
            var heroesData = instanceLogic.OppHeroesData;

            if (heroesData.CurrentHeroData.BaseHPRatio >= m_CurrentHeroMaxHPRatio)
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

                if (heroes[i].BaseHPRatio > m_BgHeroMinHpRatio)
                {
                    newHeroIndex = i;
                    break;
                }
            }

            if (newHeroIndex < 0 || instanceLogic.OppHeroIsCoolingDown(newHeroIndex))
            {
                return TaskStatus.Failure;
            }

            instanceLogic.OppSwitchHero(newHeroIndex);
            return TaskStatus.Success;
        }
    }
}
