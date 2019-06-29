using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class AchievementsForm : NGUIForm
    {
        private enum AchievementType
        {
            DailyQuest = 0,
            Achievement = 1,
        }

        [Serializable]
        private class AchievementTab
        {
            public GameObject Content = null;
            public UIToggle TabToggle = null;
        }

        [SerializeField]
        private AchievementCache m_AchievementScrollViewCache = null;

        [SerializeField]
        private DailyQuestCache m_DailyQuestScrollViewCache = null;

        [SerializeField]
        private AchievementTab[] m_AchievementsTab = null;

        [SerializeField]
        private GameObject m_QuestReminderObject = null;

        [SerializeField]
        private GameObject m_AchievementReminderObject = null;

        private AchievementType m_CurrentTab = AchievementType.DailyQuest;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.ClaimAchievementReward, OnClaimAchievementReward);
            GameEntry.Event.Subscribe(EventId.UpdateAchievement, OnUpdateAchievements);
            GameEntry.Event.Subscribe(EventId.ClaimDailyQuestReward, OnClaimDailyQuestReward);
            GameEntry.Event.Subscribe(EventId.UpdateDailyQuest, OnUpdateDailyQuest);
            GameEntry.Event.Subscribe(EventId.DailyQuestsRefreshed, OnDailyQuestsRefreshed);
            GameEntry.Event.Subscribe(EventId.ClaimActivenessChest, OnClaimActivenessChest);            
        }

        protected override void OnPostOpen(object data)
        {
            base.OnPostOpen(data);
            InitAchievementsTab();
            OnToggleChanged(true);
            RefreshReminder();
        }

        protected override void OnClose(object userData)
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.ClaimAchievementReward, OnClaimAchievementReward);
                GameEntry.Event.Unsubscribe(EventId.UpdateAchievement, OnUpdateAchievements);
                GameEntry.Event.Unsubscribe(EventId.ClaimDailyQuestReward, OnClaimDailyQuestReward);
                GameEntry.Event.Unsubscribe(EventId.UpdateDailyQuest, OnUpdateDailyQuest);
                GameEntry.Event.Unsubscribe(EventId.DailyQuestsRefreshed, OnDailyQuestsRefreshed);
                GameEntry.Event.Unsubscribe(EventId.ClaimActivenessChest, OnClaimActivenessChest);
            }
            base.OnClose(userData);
        }

        private void RefreshReminder()
        {
            m_QuestReminderObject.SetActive(GameEntry.Data.DailyQuests.HasUnclaimedChest || GameEntry.Data.DailyQuests.HasUnclaimedQuest);
            m_AchievementReminderObject.SetActive(GameEntry.Data.Achievements.HasUnclaimedAchievement);
        }

        private void InitAchievementsTab()
        {
            SetTab(AchievementType.DailyQuest);
        }

        private void SetTab(AchievementType tab)
        {
            for (int i = 0; i < m_AchievementsTab.Length; i++)
            {
                m_AchievementsTab[i].TabToggle.value = i == (int)tab;
            }
        }

        private void OnUpdateAchievements(object sender, GameEventArgs e)
        {
            if (m_CurrentTab == AchievementType.Achievement)
            {
                RefreshAchievement();
            }
            RefreshReminder();
        }

        private void OnClaimAchievementReward(object sender, GameEventArgs e)
        {
            if (m_CurrentTab == AchievementType.Achievement)
            {
                RefreshAchievement();
            }

            var data = e as ClaimAchievementRewardEventArgs;
            GameEntry.RewardViewer.RequestShowRewards(data.ReceivedItemsView, false);
            RefreshReminder();
        }

        private void OnClaimDailyQuestReward(object sender, GameEventArgs e)
        {
            if (m_CurrentTab == AchievementType.DailyQuest)
            {
                RefreshDailyQuest();
            }

            var data = e as ClaimDailyQuestRewardEventArgs;
            GameEntry.RewardViewer.RequestShowRewards(data.ReceivedItemsView, false);
            RefreshReminder();
        }

        private void OnUpdateDailyQuest(object sender, GameEventArgs e)
        {
            if (m_CurrentTab == AchievementType.DailyQuest)
            {
                RefreshDailyQuest();
            }
            RefreshReminder();
        }

        private void OnDailyQuestsRefreshed(object sender, GameEventArgs e)
        {
            if (m_CurrentTab == AchievementType.DailyQuest)
            {
                RefreshDailyQuest();
            }
            RefreshReminder();
        }

        private void OnClaimActivenessChest(object sender, GameEventArgs e)
        {
            if (m_CurrentTab == AchievementType.DailyQuest)
            {
                RefreshDailyQuest();
            }
            var data = e as ClaimActivenessChestEventArgs;
            m_EffectsController.ShowEffect(ChestOpenedEffectName + (data.ChestId - 1).ToString());
            GameEntry.RewardViewer.RequestShowRewards(data.ReceivedItemsView, false);
            RefreshReminder();
        }

        public void OnToggleChanged(bool value)
        {
            if (!value)
            {
                return;
            }

            for (int i = 0; i < m_AchievementsTab.Length; i++)
            {
                if (m_AchievementsTab[i].TabToggle.value)
                {
                    m_CurrentTab = (AchievementType)i;
                    switch (m_CurrentTab)
                    {
                        case AchievementType.Achievement:
                            RefreshAchievement();
                            GetComponent<UITitle>().SetTitle("UI_BUTTON_ACHIEVEMENT");
                            continue;
                        case AchievementType.DailyQuest:
                        default:
                            RefreshDailyQuest();
                            GetComponent<UITitle>().SetTitle("UI_BUTTON_DAILYQUEST");
                            continue;
                    }
                }

                m_AchievementsTab[i].Content.gameObject.SetActive(false);
            }
        }

        [Serializable]
        private class AchievementCache : UIScrollViewCache<AchievementItem>
        {

        }

        [Serializable]
        private class DailyQuestCache : UIScrollViewCache<DailyQuestItem>
        {

        }
    }
}
