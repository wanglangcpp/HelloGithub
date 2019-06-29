using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using System;

namespace Genesis.GameClient
{
    public class TaskListTabContent_Daily : TaskListBaseTabContent
    {
        private List<TaskItem_Default> listItem = new List<TaskItem_Default>();

        [SerializeField]
        private UILabel m_ActiveNumber = null;
        [SerializeField]
        private UISlider m_Progerss = null;
        [SerializeField]
        private UISprite m_ProgressArea = null;
        [SerializeField]
        private DailyQuestRewardChestItem[] m_Box = null;
        [SerializeField]
        private UILabel m_TaskIsNullLabel = null;
        [SerializeField]
        private UIParticle[] m_EffectIds = null;
        private int CurrentActiveness = 0;//当前活跃度

        private int BoxStatus = 0;//宝箱状态

        protected override TaskListType TaskType
        {
            get
            {
                return TaskListType.Daily;
            }
        }

        protected override void OnInit()
        {
            base.OnInit();
            listItem.Clear();

            //更新活跃度
            RefreshActiveness();

        }

        private void OnClaimDailyQuestReward(object sender, GameEventArgs e)
        {
            RefreshActiveness();
        }

        protected override IEnumerator PopulateStart(List<TaskStep> data)
        {
            yield return null;
            if (data.Count == 0)
            {
                //没有任务给个提示
                m_TaskIsNullLabel.gameObject.SetActive(true);
            }
            foreach (var item in data)
            {
                m_TaskIsNullLabel.gameObject.SetActive(false);
                DRTask task = item.Task;
                GameObject go = NGUITools.AddChild(m_ListView.gameObject, m_ItemTemplate);
                TaskItem_Default script = go.GetComponent<TaskItem_Default>();
                script.RefreshItemData(item);
                listItem.Add(script);
            }
            yield return null;

            m_ScrollView.panel.UpdateAnchors();
            m_ListView.Reposition();
            m_ScrollView.ResetPosition();
            for (int i = 0; i < listItem.Count; i++)
            {
                listItem[i].UpdateAnchors();
            }
        }

        private void RefreshActiveness()
        {
            BoxStatus = GameEntry.Data.DailyQuests.ClaimActivenessChestStatus;
            //CurrentActiveness = GameEntry.Data.Player.ActivenessToken;
            CurrentActiveness = GameEntry.Data.TaskStepData.Activeness;

            DRDailyQuestActiveness[] dailyQuestActiveness = GameEntry.DataTable.GetDataTable<DRDailyQuestActiveness>().GetAllDataRows();
            int count = dailyQuestActiveness.Length;//宝箱总数量
            int maxActiveness = dailyQuestActiveness[count - 1].Activeness;

            m_ActiveNumber.text = CurrentActiveness.ToString();

            float scale = (float)CurrentActiveness / maxActiveness;
            scale = Mathf.Clamp(scale, 0, 1);
            m_Progerss.value = scale;

            for (int i = 0; i < m_Box.Length; i++)
            {
                if (i >= count)
                {
                    m_Box[i].gameObject.SetActive(false);
                }
                else
                {
                    bool isopen = (BoxStatus & (1 << (i + 1))) != 0;
                    SetBox(m_Box[i], maxActiveness, dailyQuestActiveness[i].Activeness, isopen);
                    if (!isopen && dailyQuestActiveness[i].Activeness <= CurrentActiveness)
                    {
                        m_EffectIds[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        m_EffectIds[i].gameObject.SetActive(false);
                    }
                }
            }
        }
        private void SetBox(DailyQuestRewardChestItem chestItem, int maxActiveness, int activeness, bool isOpen)
        {
            m_ProgressArea.ResetAndUpdateAnchors();
            chestItem.Refresh(maxActiveness, activeness, isOpen);
            var width = chestItem.GetComponent<UIWidget>().width;
            float progressValue = (float)activeness / maxActiveness;
            chestItem.transform.localPosition = new Vector3(m_ProgressArea.width * progressValue - width / 2, chestItem.transform.localPosition.y, chestItem.transform.localPosition.z);
        }

        public void OnClaimChest(int index)
        {
            //活跃度小于领取所需的活跃度
            //(BoxStatus & (1 << (index + 1))) != 0 这里不等于0意味着这个奖励宝箱已经领取
            if (CurrentActiveness < m_Box[index].Activeness || (BoxStatus & (1 << (index + 1))) != 0)
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
                GameEntry.UI.OpenUIForm(UIFormId.ChestRewardSubForm, new ChestRewardDisplayData(rewards, GameEntry.Localization.GetString("UI_TEXT_PROMPT"), GameEntry.Localization.GetString("UI_TEXT_DAYBOX_ON_CONDITION", m_Box[index].Activeness)));
                return;
            }

            GameEntry.LobbyLogic.ClaimActivenessChest(index + 1);
        }

        protected override void OnRefreshTaskData(object sender, GameEventArgs e)
        {
            base.OnRefreshTaskData(sender, e);
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            GameEntry.Event.Subscribe(EventId.ClaimActivenessChest, OnClaimActivessChest);
        }

        private void OnClaimActivessChest(object sender, GameEventArgs e)
        {
            RefreshActiveness();
            var data = e as ClaimActivenessChestEventArgs;
            GameEntry.RewardViewer.RequestShowRewards(data.ReceivedItemsView, false);
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            GameEntry.Event.Unsubscribe(EventId.ClaimActivenessChest, OnClaimActivessChest);
        }
    }

}

