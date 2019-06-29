using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable]
    public class SkillBadgesData
    {
        [SerializeField]
        private SpecificBadgeData m_SpecificBadge = new SpecificBadgeData();

        public SpecificBadgeData SpecificBadge
        {
            get { return m_SpecificBadge; }
        }

        [SerializeField]
        private List<GenericBadgeData> m_GenericBadges = new List<GenericBadgeData>();

        public List<GenericBadgeData> GenericBadges
        {
            get { return m_GenericBadges; }
        }

        public void UpdateData(PBHeroSkillBadgesInfo data)
        {
            m_SpecificBadge.UpdateData(data);

            if (!data.HasGenericBadgeIds)
            {
                return;
            }

            m_GenericBadges.Clear();

            for (int i = 0; i < Constant.Hero.MaxBadgeSlotCountPerSkill; i++)
            {
                GenericBadgeData genericBadge = new GenericBadgeData();
                m_GenericBadges.Add(genericBadge);
                m_GenericBadges[i].UpdateData(data, i);
            }
        }

        public List<int> GetCanEquipSkillBadgeId()
        {
            List<int> badgeIds = new List<int>();
            for (int i = 0; i < m_GenericBadges.Count; i++)
            {
                if (m_GenericBadges[i].BadgeId == 0)
                {
                    badgeIds.Add(m_GenericBadges[i].BadgeId);
                }
            }
            return badgeIds;
        }
    }
}