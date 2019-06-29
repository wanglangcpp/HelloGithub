using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class AchievementItem : MonoBehaviour
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

        private AchievementData m_AchievementData = null;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
        }

        public void RefreshData(AchievementData data)
        {
            m_AchievementData = data;
            var dtAchievement = GameEntry.DataTable.GetDataTable<DRAchievement>().GetDataRow(data.Key);
            if (dtAchievement == null)
            {
                Log.Warning("Can't get dtAchievement,Id is {0}.", data.Key);
                return;
            }

            m_NameLabel.text = GameEntry.Localization.GetString(dtAchievement.AchievementName, data.ProgressCount < dtAchievement.TargetProgressCount ? data.ProgressCount : dtAchievement.TargetProgressCount, dtAchievement.TargetProgressCount);
            m_AchievementDesc.text = GameEntry.Localization.GetString(dtAchievement.AchievementDesc, dtAchievement.TargetProgressCount,
                dtAchievement.Params[0], dtAchievement.Params[1], dtAchievement.Params[2], dtAchievement.Params[3], dtAchievement.Params[4]);
            m_AchievementRewardDesc.text = GameEntry.Localization.GetString(dtAchievement.AchievementRewardDesc,
                dtAchievement.RewardItemCounts[0], dtAchievement.RewardItemCounts[1], dtAchievement.RewardItemCounts[2], dtAchievement.RewardItemCounts[3], dtAchievement.RewardItemCounts[4]);
            m_Icon.LoadAsync(dtAchievement.AchievementIconId);

            m_Goto.SetActive(!data.IsCompleted);
            m_Receive.SetActive(data.IsCompleted);
        }

        // Called via reflection by NGUI.
        public void OnClickReceiveButton()
        {
            if (m_AchievementData == null)
            {
                return;
            }
            GameEntry.LobbyLogic.AchivementClaimReward(m_AchievementData.Key);
        }

        public void OnClickGotoButton()
        {
            if (m_AchievementData == null)
            {
                return;
            }
            var dtAchievement = GameEntry.DataTable.GetDataTable<DRAchievement>().GetDataRow(m_AchievementData.Key);
            if (dtAchievement == null)
            {
                Log.Warning("Can't get dtAchievement,Id is {0}.", m_AchievementData.Key);
                return;
            }
            var whereToGetLogic = GameEntry.WhereToGet.GetLogic(dtAchievement.WhereToGoId);
            whereToGetLogic.OnClick();
        }
    }
}
