using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class DailyQuestItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_NameLabel = null;

        [SerializeField]
        private UILabel m_AchievementDesc = null;

        [SerializeField]
        private UILabel m_AchievementRewardDesc = null;

        [SerializeField]
        private UISprite m_Icon = null;

        [SerializeField]
        private GameObject m_Goto = null;

        [SerializeField]
        private GameObject m_Receive = null;

        [SerializeField]
        private GameObject m_CompleteReceive = null;

        private DailyQuestData m_CachedData = null;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
        }

        public void RefreshData(DailyQuestData data)
        {
            m_CachedData = data;

            var dtDailyQuest = GameEntry.DataTable.GetDataTable<DRDailyQuest>().GetDataRow(data.Key);
            if (dtDailyQuest == null)
            {
                Log.Warning("Can't get dtDailyQuest,Id is {0}.", data.Key);
                return;
            }

            m_NameLabel.text = GameEntry.Localization.GetString(dtDailyQuest.QuestName, data.ProgressCount < dtDailyQuest.TargetProgressCount ? data.ProgressCount : dtDailyQuest.TargetProgressCount, dtDailyQuest.TargetProgressCount);
            m_AchievementDesc.text = GameEntry.Localization.GetString(dtDailyQuest.QuestDesc);
            m_AchievementRewardDesc.text = GameEntry.Localization.GetString(dtDailyQuest.DailyQuestRewardDesc);
            m_Icon.LoadAsync(dtDailyQuest.DailyQuestIconId);

            m_Goto.SetActive(!data.IsCompleted && !data.IsClaimed);
            m_Receive.SetActive(data.IsCompleted && !data.IsClaimed);
            m_CompleteReceive.SetActive(data.IsClaimed);
        }

        public void OnClickReceive()
        {
            if (m_CachedData == null)
            {
                return;
            }
            GameEntry.LobbyLogic.DailyQuestClaimReward(m_CachedData.Key);
        }

        public void OnClickGoto()
        {
            if (m_CachedData == null)
            {
                return;
            }
            var dtDailyQuest = GameEntry.DataTable.GetDataTable<DRDailyQuest>().GetDataRow(m_CachedData.Key);
            if (dtDailyQuest == null)
            {
                Log.Warning("Can't get dtDailyQuest,Id is {0}.", m_CachedData.Key);
                return;
            }
            var whereToGetLogic = GameEntry.WhereToGet.GetLogic(dtDailyQuest.WhereToGoId);
            whereToGetLogic.OnClick();
        }
    }
}
