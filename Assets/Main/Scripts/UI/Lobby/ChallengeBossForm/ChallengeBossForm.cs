using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Event;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// Boss挑战副本
    /// </summary>
    public class ChallengeBossForm : NGUIForm
    {
        [SerializeField]
        private UILabel m_BossName = null;
        [SerializeField]
        private UILabel[] m_FinishConditotion = null;
        [SerializeField]
        private UILabel m_ExpNumber = null;
        [SerializeField]
        private UILabel m_GoldNumber = null;
        [SerializeField]
        private GeneralItemView[] m_Rewards = null;
        [SerializeField]
        private UILabel m_ChallengeCount = null;
        [SerializeField]
        private UILabel m_RecommendMight = null;
        [SerializeField]
        private UILabel m_ConsumeCount = null;

        [SerializeField]
        private UILabel m_ChapterName = null;
        [SerializeField]
        private UIToggle[] m_Difficulty = null;
        [SerializeField]
        private UIProgressBar m_CurrentChapterProgress = null;
        [SerializeField]
        private UILabel m_CurrentStarCount = null;
        [SerializeField]
        private RewardChest m_FirstBox = null;
        [SerializeField]
        private RewardChest m_SecondBox = null;
        [SerializeField]
        private RewardChest m_ThirdBox = null;
        [SerializeField]
        private UIButton m_LeftPageButton = null;
        [SerializeField]
        private UIButton m_RightPageButton = null;

        [SerializeField]
        private Animation m_TitleAnimation = null;
        [SerializeField]
        private Animation m_ChestAnimation = null;

        [SerializeField]
        private BossChapterInstance m_ChapterInstance = null;
        [SerializeField]
        private BossItem m_CurrentLevel = null;
        [SerializeField]
        private GameObject m_EnterSceneBtn = null;

        private DRInstanceGroupForBoss currentChapter = null;
        private DRInstance currentInstance = null;
        /// <summary>
        /// 当前的副本类型2=boss   3=boss精英
        /// </summary>
        private int instanceType = 2;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            UIEventListener.Get(m_LeftPageButton.gameObject).onClick = Onclick_LastBtn;
            UIEventListener.Get(m_RightPageButton.gameObject).onClick = OnClick_NextBtn;
            UIEventListener.Get(m_EnterSceneBtn).onClick = OnClick_EnterSceneBtn;

            if (m_ChapterInstance != null)
            {
                m_ChapterInstance.selectedBossChanged = (item) =>
                {
                    m_CurrentLevel = item;
                    currentInstance = item.CurrentData;
                    RefreshLevelData();
                };
            }

            GameEntry.Data.InstanceForBossData.InitData();
            currentChapter = GameEntry.Data.InstanceForBossData.DataTable.MinIdDataRow;
            currentInstance = GameEntry.Data.InstanceForBossData.GetDatas(currentChapter.Id, instanceType)[0];
            //GetComponent<UITitle>().SetTitle(row.Name);
            RefreshLevelData();
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            GameEntry.Event.Subscribe(EventId.ChallengeBossDataChange, ChallengeBossDataChange);
        }
        private void UnsubscribeEvents()
        {
            GameEntry.Event.Unsubscribe(EventId.ChallengeBossDataChange, ChallengeBossDataChange);
        }
        private void ChallengeBossDataChange(object sender, GameEventArgs e)
        {
            RefreshLevelData();
        }

        private void RefreshLevelData()
        {
            SetChapterInfo();
            SetBossItems();
        }

        void SetChapterInfo()
        {
            m_BossName.text = GameEntry.Localization.GetString(currentInstance.Name);// "Boss副本 虚空魔王";
            m_FinishConditotion[0].text = GameEntry.Localization.GetString(currentInstance.GetRequestDescription(0));// "5分钟内通关";
            m_FinishConditotion[1].text = GameEntry.Localization.GetString(currentInstance.GetRequestDescription(1));//"5分钟内通关，复活次数少于2次";
            m_FinishConditotion[2].text = GameEntry.Localization.GetString(currentInstance.GetRequestDescription(2));//"通关是剩余血量在60%以上";
            m_ExpNumber.text = currentInstance.PlayerExp.ToString();// "200";
            m_GoldNumber.text = currentInstance.Coin.ToString();// "1000";
            m_ChallengeCount.text = GameEntry.Localization.GetString("UI_TEXT_CHALLENGE_NUMBER",
                GameEntry.Data.InstanceForBossData.GetInstanceChallengeCount(currentInstance.Id));
            var possibleDrops = currentInstance.PossibleDrops;
            for (int i = 0; i < m_Rewards.Length; ++i)
            {
                if (i < possibleDrops.Length)
                {
                    m_Rewards[i].gameObject.SetActive(true);
                    m_Rewards[i].InitItem(possibleDrops[i].ItemId);
                }
                else
                {
                    m_Rewards[i].gameObject.SetActive(false);
                }
            }

            string[] drops = currentInstance.DropIds.Split(';');
            //var tableDrop=GameEntry.DataTable.GetDataTable<Drop>
            //for (int i = 0; i < drops.Length; i++)
            //{
            //    string[] drop = drops[i].Split('=');
            //    int itemId = int.Parse(drop[0]);
            //    int count = int.Parse(drop[1]);
            //    if (i < 3)
            //    {
            //        m_Rewards[0].InitItem(itemId, count);
            //    }
            //}
            //m_Rewards[0].InitItem(currentInstance.DropIds);
            //m_Rewards[1].InitItem(203201, 1);
            //m_Rewards[2].InitItem(203202, 1);
            m_RecommendMight.text = currentInstance.RecommendMight.ToString();// "15000";
            m_ConsumeCount.text = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Instance.CostEnergy, 6).ToString();
            m_ChapterName.text = GameEntry.Localization.GetString(currentChapter.Name);
            //"第一章 永恒遗迹";
            //m_CurrentLevel.m_BossName.text = "虚空魔王";
            //m_CurrentLevel.m_LockIcon.gameObject.SetActive(false);
            m_EnterSceneBtn.SetActive(currentInstance.PrerequisitePlayerLevel <= GameEntry.Data.Player.Level);
        }
        void SetBossItems()
        {
            //m_CurrentLevel.m_BossName.text = "虚空魔王";
            //m_CurrentLevel.m_LockIcon.gameObject.SetActive(false);
            m_ChapterInstance.SetData(currentChapter, instanceType);
        }

        void Onclick_LastBtn(GameObject go)
        {
            if (currentChapter == GameEntry.Data.InstanceForBossData.DataTable.MinIdDataRow)
            {
                return;
            }
            GameEntry.Data.InstanceForBossData.InitData();
            currentChapter = GameEntry.Data.InstanceForBossData.DataTable.GetDataRow(currentChapter.Id - 1);
            currentInstance = GameEntry.Data.InstanceForBossData.GetDatas(currentChapter.Id, instanceType)[0];
            //GetComponent<UITitle>().SetTitle(row.Name);
            RefreshLevelData();
        }
        void OnClick_NextBtn(GameObject go)
        {
            if (currentChapter == GameEntry.Data.InstanceForBossData.DataTable.MaxIdDataRow)
            {
                return;
            }
            GameEntry.Data.InstanceForBossData.InitData();
            currentChapter = GameEntry.Data.InstanceForBossData.DataTable.GetDataRow(currentChapter.Id + 1);
            currentInstance = GameEntry.Data.InstanceForBossData.GetDatas(currentChapter.Id, instanceType)[0];
            //GetComponent<UITitle>().SetTitle(row.Name);
            RefreshLevelData();
        }
        void OnClick_EnterSceneBtn(GameObject go)
        {
            //GameEntry.LobbyLogic.EnterInstance(currentInstance.Id);
            int usePower = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Instance.CostEnergy, 6);

            if (!UIUtility.CheckEnergy(usePower))
            {
                return;
            }
            if (GameEntry.Data.InstanceForBossData.GetInstanceChallengeCount(currentInstance.Id) < 1)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_CHALLENGE_RUN_OUT"));
                return;
            }

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLEnterInstanceForGroupBoss request = new CLEnterInstanceForGroupBoss();
                request.InstanceType = currentInstance.Id;
                GameEntry.Network.Send(request);
            }
            else
            {
                if (GameEntry.Procedure.CurrentProcedure is ProcedureMain)
                {
                    //GameEntry.Event.Fire(this, new WillChangeSceneEventArgs(InstanceLogicType.SinglePlayer, instanceTypeId, true));
                }
            }
        }
        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            UnsubscribeEvents();
        }
    }


}

