using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ContainHeroesWithProfessions : Conditional
    {
        [SerializeField]
        private int m_MinCount = int.MinValue;

        [SerializeField]
        private int m_MaxCount = int.MaxValue;

        [SerializeField]
        private int[] m_Professions = null;

        public override void OnStart()
        {
            if (m_Professions == null)
            {
                m_Professions = new int[0];
            }
        }

        public override TaskStatus OnUpdate()
        {
            var meHeroes = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.MyHeroesData.GetHeroes();
            var candidateProfessionSet = new HashSet<int>();

            for (int i = 0; i < m_Professions.Length; ++i)
            {
                candidateProfessionSet.Add(m_Professions[i]);
            }

            int count = 0;
            for (int i = 0; i < meHeroes.Length; ++i)
            {
                if (candidateProfessionSet.Contains(meHeroes[i].Profession))
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
