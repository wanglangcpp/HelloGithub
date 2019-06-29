using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ContainHeroesWithIds : Conditional
    {
        [SerializeField]
        private int m_MinCount = int.MinValue;

        [SerializeField]
        private int m_MaxCount = int.MaxValue;

        [SerializeField]
        private int[] m_CandidateIds = null;

        public override void OnStart()
        {
            if (m_CandidateIds == null)
            {
                m_CandidateIds = new int[0];
            }
        }

        public override TaskStatus OnUpdate()
        {
            var meHeroes = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData.GetHeroes();
            var candidateIdSet = new HashSet<int>();

            for (int i = 0; i < m_CandidateIds.Length; ++i)
            {
                candidateIdSet.Add(m_CandidateIds[i]);
            }

            int count = 0;
            for (int i = 0; i < meHeroes.Length; ++i)
            {
                if (candidateIdSet.Contains(meHeroes[i].HeroId))
                {
                    ++count;

                    if (count > m_MaxCount)
                    {
                        return TaskStatus.Failure;
                    }
                }
            }

            return count < m_MinCount ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
