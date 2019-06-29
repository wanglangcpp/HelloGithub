using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class AchievementsForm
    {
        [SerializeField]
        private UILabel m_ActivenessDegreeNumber = null;

        [SerializeField]
        private UIProgressBar m_DailyQuestDegreeProgress = null;

        [SerializeField]
        private UISprite ProgressArea = null;

        [SerializeField]
        private List<DailyQuestRewardChestItem> m_RewardChests = null;

        private const string ChestCanOpenEffectName = "EffectBox";
        private const string ChestOpenedEffectName = "EffectBoxOpen";

        private List<int> m_EffectIds = new List<int>();

        private void RefreshDailyQuest()
        {
            for (int i = 0; i < m_EffectIds.Count; i++)
            {
                m_EffectsController.DestroyEffect(m_EffectIds[i]);
            }
            m_EffectIds.Clear();
            RereshewardChests();
            m_AchievementsTab[(int)AchievementType.DailyQuest].Content.gameObject.SetActive(true);
            var datas = GameEntry.Data.DailyQuests.GetShowDailyQuests();
            for (int i = 0; i < datas.Count; ++i)
            {
                var item = m_DailyQuestScrollViewCache.GetOrCreateItem(i);
                item.RefreshData(datas[i]);
            }

            m_DailyQuestScrollViewCache.RecycleItemsAtAndAfter(datas.Count);
            m_DailyQuestScrollViewCache.ResetPosition();
        }

        private void RereshewardChests()
        {
            m_ActivenessDegreeNumber.text = GameEntry.Data.Player.ActivenessToken.ToString();
            var dailyQuestActiveness = GameEntry.DataTable.GetDataTable<DRDailyQuestActiveness>().GetAllDataRows();
            int count = dailyQuestActiveness.Length;
            int maxActiveness = dailyQuestActiveness[count - 1].Activeness;
            for (int i = 0; i < count; i++)
            {
                if (i >= m_RewardChests.Count)
                {
                    break;
                }
                SetChest(m_RewardChests[i], maxActiveness, dailyQuestActiveness[i].Activeness, (GameEntry.Data.DailyQuests.ClaimActivenessChestStatus & (1 << (i + 1))) != 0);

                if (!((GameEntry.Data.DailyQuests.ClaimActivenessChestStatus & (1 << (i + 1))) != 0) && GameEntry.Data.Player.ActivenessToken >= dailyQuestActiveness[i].Activeness)
                {
                    m_EffectIds.Add(m_EffectsController.ShowEffect(ChestCanOpenEffectName + i.ToString()));
                }
            }

            m_DailyQuestDegreeProgress.value = (float)GameEntry.Data.Player.ActivenessToken / (float)maxActiveness;
        }

        private void SetChest(DailyQuestRewardChestItem chestItem, int maxActiveness, int activeness, bool isOpen)
        {
            ProgressArea.ResetAndUpdateAnchors();
            chestItem.Refresh(maxActiveness, activeness, isOpen);
            var width = chestItem.GetComponent<UIWidget>().width;
            float progressValue = (float)activeness / (float)maxActiveness;
            chestItem.transform.localPosition = new Vector3(ProgressArea.width * progressValue - width / 2, chestItem.transform.localPosition.y, chestItem.transform.localPosition.z);
        }

        public void OnClaimChest(int index)
        {
            if (GameEntry.Data.Player.ActivenessToken < m_RewardChests[index].Activeness || (GameEntry.Data.DailyQuests.ClaimActivenessChestStatus & (1 << (index + 1))) != 0)
            {
                var dailyQuestActiveness = GameEntry.DataTable.GetDataTable<DRDailyQuestActiveness>().GetDataRow(index + 1);
                List<ChestRewardDisplayData.Reward> rewards = new List<ChestRewardDisplayData.Reward>();
                var rewardTypes = dailyQuestActiveness.GetRewardTypes();
                var rewardCounts = dailyQuestActiveness.GetRewardCounts();
                if (rewardTypes.Count > 0 && rewardCounts.Count > 0)
                {
                    for (int i = 0; i < dailyQuestActiveness.GetRewardTypes().Count; i++)
                        rewards.Add(new ChestRewardDisplayData.Reward(rewardTypes[i], rewardCounts[i]));
                }
                GameEntry.UI.OpenUIForm(UIFormId.ChestRewardSubForm, new ChestRewardDisplayData(rewards, GameEntry.Localization.GetString("UI_TEXT_PROMPT"), GameEntry.Localization.GetString("UI_TEXT_DAYBOX_ON_CONDITION_" + (index + 1).ToString())));
                return;
            }

            GameEntry.LobbyLogic.ClaimActivenessChest(index + 1);
        }
    }
}
