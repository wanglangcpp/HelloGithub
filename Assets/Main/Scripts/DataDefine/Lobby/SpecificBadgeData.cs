using UnityEngine;
using System.Collections;
using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class SpecificBadgeData : BaseSkillBadgeData
    {
        public void UpdateData(PBHeroSkillBadgesInfo data)
        {
            if (data.HasSpecificBadgeId)
            {
                BadgeId = data.SpecificBadgeId;
            }
        }
    }
}