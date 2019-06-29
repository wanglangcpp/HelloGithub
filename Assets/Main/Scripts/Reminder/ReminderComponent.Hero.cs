using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ReminderComponent : MonoBehaviour
    {
        private List<BadgeReminderData> m_BadgeRemindersData = new List<BadgeReminderData>();

        /// <summary>
        /// 是否有可以合成的英雄(MainForm)。
        /// </summary>
        public bool HasCanSummonHero
        {
            get
            {
                var allHeroesUnpossessed = UIUtility.GetLobbyHeroesUnpossessed();
                for (int i = 0; i < allHeroesUnpossessed.Count; i++)
                {
                    if (allHeroesUnpossessed[i].ComposedItemCount >= allHeroesUnpossessed[i].ComposeItemNeed)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 是否有可以升星的英雄。
        /// </summary>
        /// <returns></returns>
        public bool HasCanHeroStarUp()
        {
            var drHero = GameEntry.DataTable.GetDataTable<DRHero>();
            var lobbyHeros = GameEntry.Data.LobbyHeros.Data;
            for (int i = 0; i < lobbyHeros.Count; i++)
            {
                if (lobbyHeros[i].StarLevel < Constant.HeroStarLevelCount)
                {
                    var heroDataRow = drHero.GetDataRow(lobbyHeros[i].Type);
                    var item = GameEntry.Data.Items.GetData(heroDataRow.StarLevelUpItemId);
                    int count = item == null ? 0 : item.Count;
                    int starLevelUpItemCount = heroDataRow.StarLevelUpItemCounts[lobbyHeros[i].StarLevel - 1];
                    if (count >= starLevelUpItemCount)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 是否有可以穿戴的徽章。
        /// </summary>
        /// <returns></returns>
        public bool HasCanEquipSkillBadge()
        {
            var lobbyHeros = GameEntry.Data.LobbyHeros.Data;
            for (int i = 0; i < lobbyHeros.Count; i++)
            {
                var allSkillBadges = lobbyHeros[i].GetAllSkillBadges();
                for (int j = 0; j < allSkillBadges.Count; j++)
                {
                    if (j > 4 && j < 9)
                    {
                        continue;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取已获得英雄的徽章数据。
        /// </summary>
        public void GetAllHeroSkillBadgesData()
        {
            m_BadgeRemindersData.Clear();
            var lobbyHeros = GameEntry.Data.LobbyHeros.Data;
            var drSkillGroup = GameEntry.DataTable.GetDataTable<DRSkillGroup>();
            for (int i = 0; i < lobbyHeros.Count; i++)
            {
                var allSkillBadges = lobbyHeros[i].GetAllSkillBadges();

                for (int j = 0; j < allSkillBadges.Count; j++)
                {
                    if (j > 4 && j < 9)
                    {
                        continue;
                    }
                    var genericBadges = allSkillBadges[j].GenericBadges;

                    for (int k = 0; k < genericBadges.Count; k++)
                    {
                        int skillIndex = lobbyHeros[i].SkillGroupIds[k];
                        var skillGroup = drSkillGroup.GetDataRow(skillIndex);
                        m_BadgeRemindersData.Add(new BadgeReminderData { BadgeId = genericBadges[k].BadgeId, SkillBadgeColor = (GenericSkillBadgeColor)skillGroup.GenericSkillBadgeSlotColors[k] });
                    }
                }
            }
        }

        private class BadgeReminderData
        {
            public int BadgeId;
            public GenericSkillBadgeColor SkillBadgeColor;
        }
    }
}
