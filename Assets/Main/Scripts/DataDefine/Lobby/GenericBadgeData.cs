using UnityEngine;
using System.Collections;
using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class GenericBadgeData : BaseSkillBadgeData
    {
        public void UpdateData(PBHeroSkillBadgesInfo data, int index)
        {
            if (!data.HasGenericBadgeIds)
            {
                return;
            }

            BadgeId = data.GenericBadgeIds[index];
        }
    }
}
