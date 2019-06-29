using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class AchievementsForm
    {
        private void RefreshAchievement()
        {
            m_AchievementsTab[(int)AchievementType.Achievement].Content.gameObject.SetActive(true);
            SetItemsData(GameEntry.Data.Achievements.GetShowAchievements());
        }

        private void SetItemsData(List<AchievementData> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                var item = m_AchievementScrollViewCache.GetOrCreateItem(i);
                item.RefreshData(datas[i]);
            }

            m_AchievementScrollViewCache.RecycleItemsAtAndAfter(datas.Count);
            m_AchievementScrollViewCache.ResetPosition();
        }
    }
}
