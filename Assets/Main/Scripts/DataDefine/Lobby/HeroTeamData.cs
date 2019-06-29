using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class HeroTeamData : IGenericData<HeroTeamData, PBHeroTeamInfo>
    {
        [SerializeField]
        private List<int> m_HeroType;

        [SerializeField]
        private HeroTeamType m_Type;

        public int Key { get { return (int)m_Type; } }

        public List<int> HeroType
        {
            get
            {
                if (m_HeroType == null)
                {
                    m_HeroType = new List<int>();
                }
                if (m_HeroType.Count == 0 || m_HeroType[0] <= 0 && m_Type != HeroTeamType.Default)
                {
                    m_HeroType.Clear();
                    m_HeroType.AddRange(GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType);
                }
                return m_HeroType;
            }
        }

        public HeroTeamType TeamType { get { return m_Type; } }

        public int MainHeroType
        {
            get
            {
                if (m_HeroType[0] <= 0)
                {
                    return GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).MainHeroType;
                }
                return m_HeroType[0];
            }
        }

        public void UpdateData(PBHeroTeamInfo data)
        {
            m_HeroType = data.HeroType;
            m_Type = (HeroTeamType)data.Type;
        }
    }
}
