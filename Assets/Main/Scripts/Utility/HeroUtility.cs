using GameFramework;
using System;

namespace Genesis.GameClient
{
    public static class HeroUtility
    {
        public static void ForEachGenericSkillBadge(SkillBadgesData skillBadgesData, GameFrameworkAction<GenericBadgeData, DRGenericSkillBadge, object> forEachOp, object userData = null)
        {
            if (skillBadgesData == null)
            {
                return;
            }

            var genericBadges = skillBadgesData.GenericBadges;
            if (genericBadges == null)
            {
                return;
            }

            for (int i = 0; i < genericBadges.Count; ++i)
            {
                if (genericBadges[i] == null || genericBadges[i].BadgeId <= 0)
                {
                    continue;
                }

                var drGenericBadge = GameEntry.DataTable.GetDataTable<DRGenericSkillBadge>().GetDataRow(genericBadges[i].BadgeId);
                if (drGenericBadge == null)
                {
                    continue;
                }

                if (forEachOp != null)
                {
                    forEachOp(genericBadges[i], drGenericBadge, userData);
                }
            }
        }
    }
}
