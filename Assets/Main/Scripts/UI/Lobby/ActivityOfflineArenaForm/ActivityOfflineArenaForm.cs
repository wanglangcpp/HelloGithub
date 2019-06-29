using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技场主界面。
    /// </summary>
    public partial class ActivityOfflineArenaForm : NGUIForm
    {
        [SerializeField]
        private UILabel m_DefendMightLabel = null;

        [SerializeField]
        private UILabel m_CurrentRankLabel = null;

        [SerializeField]
        private UILabel m_TodayChallengeTimeLabel = null;

        [SerializeField]
        private UIButton m_PickRewardButton = null;

        [SerializeField]
        private UIButton m_CheckRewardButton = null;

        [SerializeField]
        private GameObject m_RefreshRootObject = null;

        [SerializeField]
        private UILabel m_RefreshCostCountLabel = null;

        [SerializeField]
        private UILabel m_FreeRefreshLabel = null;

#pragma warning disable 0414
        [SerializeField]
        private UIButton m_IncreaseTimeButton = null;
#pragma warning restore 0414

        [SerializeField]
        private EnemyPlayerScrollView m_EnemyScrollView = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(EventId.OfflineArenaPlayerListChanged, OnEnermyChanged);
            GameEntry.Event.Subscribe(EventId.OfflineArenaLivenessRewardClaimed, OnClaimedReward);
            GameEntry.Event.Subscribe(EventId.OfflineArenaDataChanged, OnDataChanged);

            SetDefendMightLabel();

            SetChallangeTimeLabel();

            // 现在一直显示+按钮
            // m_IncreaseTimeButton.gameObject.SetActive(enableChallengeTime <= 0); 

            SetRefreshButton();

            RefreshMyRank();
            RefreshRewardButton();
            RefreshEnermyList();

            GameEntry.LobbyLogic.RefreshOfflineArena(false);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);

            GameEntry.Event.Unsubscribe(EventId.OfflineArenaPlayerListChanged, OnEnermyChanged);
            GameEntry.Event.Unsubscribe(EventId.OfflineArenaLivenessRewardClaimed, OnClaimedReward);
            GameEntry.Event.Unsubscribe(EventId.OfflineArenaDataChanged, OnDataChanged);
        }

        private void SetChallangeTimeLabel()
        {
            int remainingPlayCount, freePlayCount;
            UIUtility.GetPlayCount_OfflineArena(out remainingPlayCount, out freePlayCount);
            UIUtility.SetPlayCountLabel(m_TodayChallengeTimeLabel, remainingPlayCount, freePlayCount, "UI_TEXT_CHALLENGE_THE_NUMBER_OF_THE_DAY");
        }

        private void SetDefendMightLabel()
        {
            var myTeam = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Arena);
            int defendMight = 0;
            if (myTeam != null)
            {
                for (int i = 0; i < myTeam.HeroType.Count; i++)
                {
                    var hero = GameEntry.Data.LobbyHeros.GetData(myTeam.HeroType[i]);
                    if (hero != null)
                        defendMight += hero.Might;
                }
            }
            m_DefendMightLabel.text = defendMight.ToString();
        }

        private void RefreshMyRank()
        {
            m_CurrentRankLabel.text = GameEntry.Data.OfflineArena.CurrentRank.ToString();
        }

        private void RefreshRewardButton()
        {
            bool hasReward = GameEntry.Data.OfflineArena.HasReward;
            m_PickRewardButton.gameObject.SetActive(hasReward);
            m_CheckRewardButton.gameObject.SetActive(!hasReward);
        }

        private void RefreshEnermyList()
        {
            for (int i = 0; i < GameEntry.Data.OfflineArena.Enermies.Data.Count; ++i)
            {
                var itemScript = m_EnemyScrollView.GetOrCreateItem(i, (go) => { go.name = go.name+Convert.ToString(i + 1); }, (go) => { go.name = go.name + Convert.ToString(i + 1); });
                itemScript.SetEnemyData(GameEntry.Data.OfflineArena.Enermies.Data[i]);
            }

            m_EnemyScrollView.RecycleItemsAtAndAfter(GameEntry.Data.OfflineArena.Enermies.Data.Count);
            m_EnemyScrollView.ResetPosition();
        }

        private void SetRefreshButton()
        {
            var refreshCost = GameEntry.Data.OfflineArena.GetRefreshCostCoin();

            if (refreshCost <= 0)
            {
                m_RefreshRootObject.SetActive(false);
                int maxFreeTime = GameEntry.Data.OfflineArena.FreeRefreshTime;
                int refreshedTime = GameEntry.Data.OfflineArena.TodayRefreshedCount;
                m_FreeRefreshLabel.text = GameEntry.Localization.GetString("UI_TEXT_FREE_REFRESH", maxFreeTime - refreshedTime, maxFreeTime);
                m_FreeRefreshLabel.gameObject.SetActive(true);
            }
            else
            {
                m_RefreshCostCountLabel.text = refreshCost.ToString("N0");
                m_RefreshRootObject.SetActive(true);
                m_FreeRefreshLabel.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 刷新对手列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEnermyChanged(object sender, GameEventArgs e)
        {
            SetRefreshButton();
            RefreshEnermyList();
        }

        /// <summary>
        /// 领奖成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClaimedReward(object sender, GameEventArgs e)
        {
            var showData = e as OfflineArenaLivenessRewardClaimedEventArgs;
            if (showData == null)
                return;

            GameEntry.RewardViewer.RequestShowRewards(showData.Rewards.ReceiveGoodsData, true);
        }

        private void OnDataChanged(object sender, GameEventArgs e)
        {
            RefreshMyRank();
            RefreshRewardButton();
            SetChallangeTimeLabel();
            SetDefendMightLabel();
            RefreshEnermyList();
        }

        /// <summary>
        /// 加号的点击事件
        /// </summary>
        public void OnIncreaseTimeButtonClicked()
        {
            GameEntry.UI.OpenUIForm(UIFormId.CostConfirmDialog, new CostConfirmDialogDisplayData { Mode = CostConfirmDialogType.ArenaBattleCount });
        }

        /// <summary>
        /// 查看奖励按钮事件
        /// </summary>
        public void OnCheckRewardButtonClicked()
        {
            string title = GameEntry.Localization.GetString("UI_TEXT_SETTLEMENT_REWARD");
            string instruction = GameEntry.Localization.GetString("UI_TEXT_AWARD_TIME");

            var arenaRewardConfig = GameEntry.DataTable.GetDataTable<DRArenaReward>();
            var drRewards = arenaRewardConfig.GetAllDataRows();

            DRArenaReward reward = null;
            int myRank = GameEntry.Data.OfflineArena.CurrentRank;

            List<DRArenaReward> rewardListConfig = new List<DRArenaReward>(drRewards);
            rewardListConfig.Sort((r1, r2) => { return r1.StartRank.CompareTo(r2.StartRank); });

            if (myRank > 0)
            {
                for (int i = 0; i < rewardListConfig.Count; i++)
                {
                    if (rewardListConfig[i].StartRank == myRank)
                    {
                        reward = rewardListConfig[i];
                        break;
                    }

                    if (rewardListConfig[i].StartRank < myRank)
                        continue;

                    if (rewardListConfig[i].StartRank > myRank)
                    {
                        if (i > 1)
                            reward = rewardListConfig[i - 1];

                        break;
                    }
                }
            }

            List<ChestRewardDisplayData.Reward> rewards = new List<ChestRewardDisplayData.Reward>();
            if (reward != null)
            {
                for (int i = 0; i < reward.Rewards.Count; i++)
                    rewards.Add(new ChestRewardDisplayData.Reward(reward.Rewards[i].Id, reward.Rewards[i].Count));
            }
            GameEntry.UI.OpenUIForm(UIFormId.ChestRewardSubForm, new ChestRewardDisplayData(rewards, title, instruction));
        }

        /// <summary>
        /// 领取奖励按钮事件
        /// </summary>
        public void OnPickRewardButtonClicked()
        {
            GameEntry.LobbyLogic.PickOfflineArenaReward();
        }

        /// <summary>
        /// 查看战绩按钮事件
        /// </summary>
        public void OnRecordButtonClicked()
        {
            GameEntry.UI.OpenUIForm(UIFormId.OfflineArenaRecordForm);
        }

        /// <summary>
        /// 规则按钮事件
        /// </summary>
        public void OnRuleButtonClicked()
        {

        }

        /// <summary>
        /// 刷新按钮事件
        /// </summary>
        public void OnRefreshButtonClicked()
        {
            var cost = GameEntry.Data.OfflineArena.GetRefreshCostCoin();
            if (!UIUtility.CheckCurrency(CurrencyType.Coin, cost))
                return;

            GameEntry.LobbyLogic.RefreshOfflineArena(true);
        }

        /// <summary>
        /// 奖励兑换按钮事件。（进入商店）
        /// </summary>
        public void OnExchangeButtonClicked()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ShopForm, new ShopDisplayData { Scenario = ShopScenario.OfflineArena });
        }

        /// <summary>
        /// 排行榜按钮事件
        /// </summary>
        public void OnRankButtonClicked()
        {
            GameEntry.UI.OpenUIForm(UIFormId.RankListForm, new RankListDisplayData { Scenario = RankListType.OfflineArena });
        }

        /// <summary>
        /// 防守阵容按钮事件
        /// </summary>
        public void OnDefendLineUpButtonClicked()
        {
            GameEntry.UI.OpenUIForm(UIFormId.HeroTeamForm, new HeroTeamDisplayData { Scenario = HeroTeamDisplayScenario.ArenaBattle });
        }

        [Serializable]
        private class EnemyPlayerScrollView : UIScrollViewCache<ActivityOfflineArenaEnemyPlayer>
        {

        }
    }
}
