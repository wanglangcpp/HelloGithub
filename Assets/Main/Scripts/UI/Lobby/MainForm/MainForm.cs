using GameFramework;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Genesis.GameClient
{
    public class MainForm : NGUIForm
    {
        private enum ReminderType
        {
            ShopReminder = 0,
            ChanceReminder = 1,
            DailyQuestReminder = 2,
            InstanceReminder = 3,
            ActivityReminder = 4,
            InventoryReminder = 5,
            HeroInfoReminder = 6,
            TeamReminder = 7,
            DepotReminder = 8,
            MeridianReminder = 9,
            ChatReminder = 10,
        }

        [SerializeField]
        private UILabel m_PlayerName = null;

#pragma warning disable 0414

        [SerializeField]
        private UILabel m_PlayerLevel = null;

#pragma warning restore 0414

        [SerializeField]
        private UIProgressBar m_PlayerExp = null;

        [SerializeField]
        private UISprite m_PlayerPortrait = null;

        [SerializeField]
        private UILabel m_VipLevel = null;

        [SerializeField]
        private UILabel m_Money = null;

        [SerializeField]
        private UILabel m_Coin = null;

        [SerializeField]
        private UILabel m_Energy = null;

        [SerializeField]
        private GameObject m_Chat = null;
        [SerializeField]
        private UILabel m_ChatMsg = null;

        [SerializeField]
        private UISprite m_FriendReminder = null;

        [SerializeField]
        private UISprite m_MailReminder = null;

        [SerializeField]
        private GameObject[] ReminderIcons = null;

        [SerializeField]
        private UISprite m_OtherPlayerHeroIcon = null;

        [SerializeField]
        private GameObject m_OtherPlayerHeroRoot = null;

        [SerializeField]
        private Animation m_OtherPlayerHeroAnimation = null;

        [SerializeField]
        private UILabel m_MightLabel = null;

        [SerializeField]
        private GameObject m_SwitchButton = null;

        [SerializeField]
        private Animation m_SwitchAnimation = null;

        [SerializeField]
        private Animation m_TopSwitchAnimation = null;

        [SerializeField]
        private GameObject m_SystemChat = null;

        [SerializeField]
        private GameObject m_DailyLoginReminderObject = null;

        [SerializeField]
        private Collider m_SwitchBtn = null;

        [SerializeField]
        private Collider m_InstanceBtn = null;

        [SerializeField]
        private Collider m_RankBtn = null;

        [SerializeField]
        private GameObject m_BtnFirshCharge = null;

        [SerializeField]
        private GameObject m_TaskTraceForm = null;
        [SerializeField]
        private BoxCollider m_TaskListCillider = null;
        [SerializeField]
        private TweenPosition m_TaskListAnimation = null;
        [SerializeField]
        private TweenPosition m_BtnShrinkAnimation = null;
        [SerializeField]
        private BoxCollider m_BtnShrinkCollider = null;
        [SerializeField]
        private TweenRotation m_IconRotationAnimation = null;
        private const string TaskListIsOpen = "TaskListIsOpen";
        [SerializeField]
        private GameObject[] m_EffectTaskFinish = null;
        [Serializable]
        private class TaskItem
        {
            public DRTask Task = null;
            public UILabel TaskType = null;
            public UILabel TaskName = null;
            public UILabel TaskDesc = null;
            public UIButton TaskBtn = null;
            public bool IsFinish = false;

            public void OnClickTaskItem()
            {
                if (IsFinish)
                    GameEntry.TaskComponent.FinishTask(Task.Id);
                else
                {
                    if (GameEntry.TaskComponent.isGoTo(Task.Id))
                    {
                        GameEntry.TaskComponent.GoToStartTask(Task);
                    }
                }
            }
        }
        [SerializeField]
        private TaskItem[] m_TaskList = null;
        [SerializeField]
        private UISprite m_NewworkSignal = null;
        [SerializeField]
        private UILabel m_CurrentTime = null;
        [SerializeField]
        private UIProgressBar m_Battery = null;

        private Color MainTaskColor = new Color(225 / 225f, 198 / 225f, 96 / 225f);
        private Color OtherTaskColor = new Color(0 / 225f, 225 / 225f, 180 / 225f);
        private Color FinishTaskColor = new Color(0 / 225f, 225 / 225f, 0 / 225f);
        private HashSet<UIFormId> m_UIFormsToOpen = new HashSet<UIFormId>();

        private float m_CurrentUpdateTime = 0.0f;

        private float m_DurationUpdateTime = 1.0f;

        private int m_OtherPlayerIdForInteraction = 0;

        private const string BottomListIsOpen = "BottomListIsOpen";

        private const float HalfRoundAngle = -180;

        protected override bool BackButtonIsAvailable
        {
            get
            {
                return false;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GameEntry.OpenFunction.ShowLobbyOpenFunction(this, GameEntry.OpenFunction.GetLobbyOpenFunction());
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_UIFormsToOpen.Clear();
            RefreshPlayerPortrait(true);
            RefreshSocialReminder();
            GameEntry.Event.Subscribe(EventId.PlayerDataChanged, OnPlayerDataChanged);
            GameEntry.Event.Subscribe(EventId.GetChat, OnGetChatDataChanged);
            GameEntry.Event.Subscribe(EventId.SendChat, OnSendChatDataReturn);
            GameEntry.Event.Subscribe(EventId.ReminderUpdated, OnPendingFriendRequestsDataChanged);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.OpenUIFormFailure, OnOpenUIFormFailure);
            GameEntry.Event.Subscribe(EventId.AddBlackList, OnPendingFriendRequestsDataChanged);
            GameEntry.Event.Subscribe(EventId.ReminderUpdated, OnReminderUpdated);
            GameEntry.Event.Subscribe(EventId.GetItemStatus, GetItemSuatus);
            //GameEntry.Event.Subscribe(EventId.ReceiveAndShowItems, ReceiveAndShowItems);
            GameEntry.Event.Subscribe(EventId.TaskListChanged, OnTaskListChanged);
            GameEntry.Event.Subscribe(EventId.GetSystemMsg, OnGetSystemMsg);
            if (GameEntry.Data.ChargeStatusData.StatusData != null)
            {
                GetItemSuatus(GameEntry.Data.ChargeStatusData.StatusData);
            }
            RefreshTaskTraceList();
            RefreshPlayerData();
            StartCoroutine(CheckTempData());
            //m_Chat.SetActive(false);
            if (GameEntry.Data.Chat.WorldChatList == null || GameEntry.Data.Chat.WorldChatList.Count == 0)
                m_ChatMsg.text = GameEntry.Localization.GetString("UI_TEXT_WORLD_SPEAK");
            else
            {
                var chatMsg = GameEntry.Data.Chat.WorldChatList[GameEntry.Data.Chat.WorldChatList.Count - 1];
                string msg = chatMsg.Message;
                string name;
                if (chatMsg.Sender == null)
                    name = GameEntry.Data.Player.Name;
                else
                    name = chatMsg.Sender.PlayerName;
                OnPlayerChat(msg, name);
            }
            if (GameEntry.Data.Chat.ChatSystemBroadCastMsg == null)
            {
                m_SystemChat.SetActive(false);
            }
            m_OtherPlayerHeroRoot.SetActive(false);
            //下面代码离线模式不执行
#if UNITY_EDITOR
            if (GameEntry.OfflineMode.OfflineModeEnabled)
                return;
#endif
            GameEntry.Network.Send(new CLPlayerBattleStatus());
            GameEntry.Network.Send(new CLGetSinglePvpRank());
            //             if (!GameEntry.OfflineMode.OfflineModeEnabled)
            //             {
            //                 if (GameEntry.Data.GetTempData<bool>(Constant.TempData.NeedCreatePlayer))
            //                 {
            //                     PlayBottomListAnim(false, false);
            //                     PlayTopListAnim(false, false);
            //                 }
            //                 m_SwitchBtn.enabled = !GameEntry.Data.GetTempData<bool>(Constant.TempData.NeedCreatePlayer);
            //                 m_InstanceBtn.enabled = !GameEntry.Data.GetTempData<bool>(Constant.TempData.NeedCreatePlayer);
            //                 m_RankBtn.enabled = !GameEntry.Data.GetTempData<bool>(Constant.TempData.NeedCreatePlayer);
            //             }
            m_BtnShrinkAnimation.onFinished.Add(new EventDelegate(OnTaskTweenFinished));
        }

        private void OnReminderUpdated(object sender, GameEventArgs e)
        {
            RefreshSocialReminder();
        }

        protected override void OnResume()
        {
            base.OnResume();
            ShowGuideEffect();
            GameEntry.Event.Subscribe(EventId.TouchUp, OnTouchUp);
            RefreshMightData();
        }

        protected override void OnPostOpen(object data)
        {
            base.OnPostOpen(data);
            ShowGuideEffect();
        }

        protected override void OnPause()
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.TouchUp, OnTouchUp);
            }

            base.OnPause();
        }

        protected override void OnClose(object userData)
        {
            m_UIFormsToOpen.Clear();
            base.OnClose(userData);

            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.PlayerDataChanged, OnPlayerDataChanged);
                GameEntry.Event.Unsubscribe(EventId.GetChat, OnGetChatDataChanged);
                GameEntry.Event.Unsubscribe(EventId.SendChat, OnSendChatDataReturn);
                GameEntry.Event.Unsubscribe(EventId.ReminderUpdated, OnPendingFriendRequestsDataChanged);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.OpenUIFormFailure, OnOpenUIFormFailure);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
                GameEntry.Event.Unsubscribe(EventId.AddBlackList, OnPendingFriendRequestsDataChanged);
                GameEntry.Event.Unsubscribe(EventId.ReminderUpdated, OnReminderUpdated);
                GameEntry.Event.Unsubscribe(EventId.GetItemStatus, GetItemSuatus);
                //GameEntry.Event.Unsubscribe(EventId.ReceiveAndShowItems, ReceiveAndShowItems);
                GameEntry.Event.Unsubscribe(EventId.TaskListChanged, OnTaskListChanged);
                GameEntry.Event.Unsubscribe(EventId.GetSystemMsg, OnGetSystemMsg);
            }

            CheckHideOtherPlayerDisplay();
        }

        private void OnGetSystemMsg(object sender, GameEventArgs e)
        {
            var request = e as GetSystemMsgArgs;
            if (request.Sender.contextType != (int)SystemChatType.NOMAL)
            {
                m_SystemChat.SetActive(true);
            }
        }

        //private void ReceiveAndShowItems(object sender, GameEventArgs e)
        //{
        //    ShowGetItemsInfoEventArgs ne = e as ShowGetItemsInfoEventArgs;
        //    GameEntry.RewardViewer.RequestShowRewards(ne.Rewards.ReceiveGoodsData, false);
        //}

        private void GetItemSuatus(object sender, GameEventArgs e)
        {
            GetItemSuatus(GameEntry.Data.ChargeStatusData.StatusData);
        }
        private void GetItemSuatus(ChargeStatus ne)
        {
            int GiftId = UIUtility.GetGiftType(GiftType.ChargeFirst)[0].Id;
            bool status = true;
            if (ne.FirstChargeItems.Count != 0)
            {
                if (ne.GiftStatus.ContainsKey(GiftId))
                {
                    if (ne.GiftStatus[GiftId] == 1)
                    {
                        status = false;
                    }
                    else if (ne.GiftStatus[GiftId] == 0)
                    {
                        status = true;
                        //显示小红点
                    }
                    else
                    {
                        status = true;
                    }
                }
            }
            GameEntry.OpenFunction.SetIsFirstCharge(status);
            m_BtnFirshCharge.SetActive(status);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (GameEntry.Time.LobbyServerTimeState != TimeStateType.Set)
            {
                return;
            }

            m_CurrentUpdateTime += realElapseSeconds;
            if (m_CurrentUpdateTime >= m_DurationUpdateTime)
            {
                m_CurrentUpdateTime = 0.0f;
                UpdateChanceFreeReminderIcon();
            }
            UpdataPhoneDisplayMsg();
        }
        private void UpdataPhoneDisplayMsg()
        {
            m_CurrentTime.text = DateTime.Now.ToString("HH:mm");
        }

        private void UpdateChanceFreeReminderIcon()
        {
            //TimeSpan timeCoinSpan = GameEntry.Data.Chances.GetChanceData(ChanceType.Coin).NextFreeTime;
            //TimeSpan timeMoneySpan = GameEntry.Data.Chances.GetChanceData(ChanceType.Money).NextFreeTime;
            //             ReminderIcons[(int)ReminderType.ChanceReminder].SetActive(((timeCoinSpan <= TimeSpan.Zero) &&
            //                 (GameEntry.Data.Chances.GetChanceData(ChanceType.Coin).FreeChanceTimes < GameEntry.ServerConfig.GetInt(Constant.ServerConfig.MaxFreeCountForCoinChance, 5)))
            //                 || (timeMoneySpan <= TimeSpan.Zero));
        }

        public void OnClickAddEnergy()
        {
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.CostConfirmDialog, new CostConfirmDialogDisplayData { Mode = CostConfirmDialogType.Energy });
            //GameEntry.OpenFunction.CheckTaskList(new List<int>());
        }

        public void OnClickAddCoin()
        {
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.CostConfirmDialog, new CostConfirmDialogDisplayData { Mode = CostConfirmDialogType.Coin });
            //GameEntry.NoviceGuide.ShowNovGuide();
        }

        public void OnClickAddMoney()
        {
            CheckHideOtherPlayerDisplay();
        }

        public void OnClickPvp()
        {
            //GameEntry.UI.OpenUIForm(UIFormId.PvpSelectForm);
            EnterMimicMeleeInstance();
        }

        private void EnterMimicMeleeInstance()
        {
            var allMimicMeleeInstanceDataRows = GameEntry.DataTable.GetDataTable<DRMimicMeleeInstance>().GetAllDataRows();
            for (int i = 0; i < allMimicMeleeInstanceDataRows.Length; i++)
            {
                var drMimicMeleeInstance = allMimicMeleeInstanceDataRows[i];
                int levelRangeId = drMimicMeleeInstance.LevelRangeId;
                var drLevelRange = GameEntry.DataTable.GetDataTable<DRLevelRange>().GetDataRow(levelRangeId);
                if (drLevelRange == null)
                {
                    Log.Warning("Level range '{0}' not found in data table.", levelRangeId.ToString());
                    continue;
                }

                if (GameEntry.Data.Player.Level >= drLevelRange.MinLevel && GameEntry.Data.Player.Level <= drLevelRange.MaxLevel)
                {
                    GameEntry.LobbyLogic.EnterMimicMeleeInstance(drMimicMeleeInstance.Id);
                    return;
                }
            }

            Log.Error("No mimic melee instance matches the player level '{0}'.", GameEntry.Data.Player.Level.ToString());
        }

        // Called by NGUI via reflection
        public void OnClickInstanceButton()
        {
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.InstanceSelectForm);
        }

        // Called by NGUI via reflection
        public void OnClickHeroTeamButton()
        {
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.HeroTeamForm, new HeroTeamDisplayData { Scenario = HeroTeamDisplayScenario.Lobby, });
        }

        // Called by NGUI via reflection
        public void OnClickHeroesButton()
        {
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.HeroAlbumForm);
        }

        // Called by NGUI via reflection
        public void OnClickDepotButton()
        {
            CheckHideOtherPlayerDisplay();
        }

        // Called by NGUI via reflection
        public void OnClickActivityButton()
        {
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.ActivitySelectForm);
        }
        // Called by NGUI via reflection
        /// <summary>
        /// pvp的活动窗口
        /// </summary>
        public void OnClickPvpActivityButton()
        {
            CheckHideOtherPlayerDisplay();
            //GameEntry.UI.OpenUIForm(UIFormId.PvpSelectForm);
            GameEntry.UI.OpenUIForm(UIFormId.ActivitySinglePvpMainForm);
        }
        // Called by NGUI via reflection
        public void OnClickMoreButton()
        {
            CheckHideOtherPlayerDisplay();
        }

        // Called by NGUI via reflection
        public void OnClickMeridianButton()
        {
            //if (GameEntry.Data.Player.Level < GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Meridian.UnlockLevel, 3))
            //{
            //    GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FUNCTION_ON_CONDITION", 3) });
            //    return;
            //}
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.MeridianForm);
        }

        // Called by NGUI via reflection
        public void OnClickChatButton()
        {
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.ChatForm);
        }

        // Called by NGUI via reflection
        public void OnClickChanceButton()
        {
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.ChanceSelectForm);
        }

        // Called by NGUI via reflection
        public void OnClickInventoryButton()
        {
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.InventoryForm);
        }

        // Called by NGUI via reflection
        public void OnClickShopButton()
        {
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.ShopForm, new ShopDisplayData { Scenario = ShopScenario.Common });
        }

        // Called by NGUI via reflection
        public void OnClickPlayerPortrait()
        {
            CheckHideOtherPlayerDisplay();
            GameEntry.UI.OpenUIForm(UIFormId.PlayerInfoForm, new PlayerInfoDisplayData { PlayerId = GameEntry.Data.Player.Id });
        }

        // Called by NGUI via reflection
        public void OnClickOtherPlayerHeroPortrait()
        {
            var player = GameEntry.Data.NearbyPlayers.GetData(m_OtherPlayerIdForInteraction);
            CheckHideOtherPlayerDisplay();
            if (player == null)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_PLAYERS_ARE_OFFLINE"));
                return;
            }
            GameEntry.UI.OpenUIForm(UIFormId.PlayerSummaryForm, new PlayerSummaryFormDisplayData { ShowPlayerData = player.Player });
        }

        // Called by NGUI via reflection
        public void OnClickDailyLoginButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.OperationActivityForm, new LuaFormDisplayData { });
        }
        //首冲
        public void OnClickChargeFirstButton()
        {
            //读取首充数据表
            var vr = UIUtility.GetGiftType(GiftType.ChargeFirst);

            List<ChargeItemsDisPlayData.Reward> m_RewardList = new List<ChargeItemsDisPlayData.Reward>();
            for (int i = 0; i < vr[0].Items.Count; i++)
            {
                ChargeItemsDisPlayData.Reward reward = new ChargeItemsDisPlayData.Reward(vr[0].Items[i].Icon, vr[0].Items[i].Count);
                m_RewardList.Add(reward);
            }
            GameEntry.UI.OpenUIForm(UIFormId.ChargeFirstForm, new ChargeItemsDisPlayData(m_RewardList, vr[0].Id));
        }
        //充值
        public void OnClickChargeButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChargeForm);
        }
        //福利
        public void OnClickWelfareCenterButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.WelfareListForm, new WelfareCenterDisplayData() { Scenario = WelfareType.EveryDayGift });
        }

        // Called by NGUI via reflection
        public void OnClickUnopenedButton()
        {
            UIUtility.ShowToast(GameEntry.Localization.GetString("UI_UNOPENED_FUNCTION"));
        }

        protected override void OnChangeSceneStart(object sender, GameEventArgs e)
        {
            // Empty. SceneLogicComponent will close this page.
        }

        private void OnPlayerDataChanged(object sender, GameEventArgs e)
        {
            if (GameEntry.Data.Player.IsLevelUp)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PlayerLevelUpForm, new PlayerLevelUpDisplayData() { PlayerLevelUpReturn = OnPlayerLevelUpReturn });
            }
            RefreshPlayerData();
            RefreshPlayerPortrait();
        }
        private void OnPlayerLevelUpReturn()
        {
            GameEntry.Event.Fire(this, new PlayerLevelUpAnimationEventArgs());
            GameEntry.Data.Player.IsLevelUp = false;
            Log.Debug("level up success....");
        }
        private void RefreshPlayerData()
        {
            m_PlayerName.text = GameEntry.Data.Player.Name;// GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", GameEntry.Data.Player.Name, GameEntry.Data.Player.Level);
            m_PlayerLevel.text = GameEntry.Data.Player.Level.ToString();
            m_PlayerExp.value = GameEntry.Data.Player.ExpRatio;
            m_VipLevel.text = GameEntry.Localization.GetString("UI_TEXT_VIPNUMBER", GameEntry.Data.Player.VipLevel.ToString());
            m_Money.text = GameEntry.Data.Player.Money.ToString();
            m_Coin.text = GameEntry.Data.Player.Coin.ToString();
            m_Energy.text = GameEntry.Localization.GetString("UI_TEXT_SLASH", GameEntry.Data.Player.Energy.ToString(), Constant.PlayerMaxEnergy.ToString());
            RefreshMightData();
        }

        private void RefreshMightData()
        {
            m_MightLabel.text = GameEntry.Data.Player.TeamMight.ToString();
        }

        private void OnTaskListChanged(object sender, GameEventArgs e)
        {
            RefreshTaskTraceList();
        }
        private void RefreshTaskTraceList()
        {
            Color currentColor = new Color();
            List<TaskStep> tasklist = GameEntry.TaskComponent.SortTaskList();
            if (tasklist == null || tasklist.Count == 0)
            {
                m_TaskTraceForm.SetActive(false);
                return;
            }
            for (int i = 0; i < m_TaskList.Length; i++)
            {
                m_TaskList[i].TaskName.transform.parent.gameObject.SetActive(true);
                if (i >= tasklist.Count)
                {
                    m_TaskList[i].TaskName.transform.parent.gameObject.SetActive(false);
                    continue;
                }
                m_TaskList[i].IsFinish = false;
                m_TaskList[i].Task = tasklist[i].Task;
                m_TaskList[i].TaskDesc.color = Color.white;
                m_TaskList[i].TaskDesc.text = GameEntry.Localization.GetString(tasklist[i].Task.Desc);
                m_TaskList[i].TaskName.text = GameEntry.Localization.GetString(tasklist[i].Task.Name);
                if (tasklist[i].Task.TaskType == 1)
                {
                    currentColor = MainTaskColor;
                    m_TaskList[i].TaskType.text = GameEntry.Localization.GetString("UI_TEXT_MAIN_LINE");
                }
                else
                {
                    currentColor = OtherTaskColor;
                    m_TaskList[i].TaskType.text = GameEntry.Localization.GetString("UI_TEXT_REGIONAL");
                }
                m_TaskList[i].TaskType.color = currentColor;
                m_TaskList[i].TaskName.color = currentColor;
                if (tasklist[i].IsFinish)
                {
                    m_TaskList[i].TaskDesc.color = FinishTaskColor;
                    m_TaskList[i].IsFinish = true;
                    m_EffectTaskFinish[i].SetActive(true);
                }
                else
                {
                    m_EffectTaskFinish[i].SetActive(false);
                }
                m_TaskList[i].TaskBtn.onClick.Clear();
                m_TaskList[i].TaskBtn.onClick.Add(new EventDelegate(m_TaskList[i].OnClickTaskItem));
            }
            //GameEntry.OpenFunction.CheckTaskList(GameEntry.Data.TaskStepData.TasksItemData.CurrentTaskListId);
        }

        void OnTaskTweenFinished()
        {
            m_BtnShrinkCollider.enabled = true;
            m_TaskListCillider.enabled = true;
        }
        public void OnClickShrinkTaskList()
        {
            m_BtnShrinkCollider.enabled = false;
            m_TaskListCillider.enabled = false;
            if (!GameEntry.Setting.HasKey(TaskListIsOpen))
            {
                GameEntry.Setting.SetBool(TaskListIsOpen, true);
                GameEntry.Setting.Save();
            }
            bool taskListIsOpen = GameEntry.Setting.GetBool(TaskListIsOpen, true);
            if (taskListIsOpen)
            {
                m_TaskListAnimation.PlayForward();
                m_BtnShrinkAnimation.PlayForward();
                m_IconRotationAnimation.PlayForward();
            }
            else
            {
                m_TaskListAnimation.PlayReverse();
                m_BtnShrinkAnimation.PlayReverse();
                m_IconRotationAnimation.PlayReverse();
            }
            GameEntry.Setting.SetBool(TaskListIsOpen, !taskListIsOpen);
            GameEntry.Setting.Save();
        }

        private void RefreshPlayerPortrait(bool clearAtlas = false)
        {
            if (clearAtlas)
            {
                m_PlayerPortrait.atlas = null;
            }

            m_PlayerPortrait.LoadAsync(UIUtility.GetPlayerPortraitIconId());
        }

        //TODO 主城聊天窗口
        private void OnGetChatDataChanged(object sender, GameEventArgs e)
        {
            ReminderIcons[(int)ReminderType.ChatReminder].SetActive(GameEntry.Reminder.PrivateChatRequestsReminder);
            OnShowChatMsg();
            LCReceiveChat msg = (e as GetChatEventArgs).Msg;
            OnShowChatMsg(msg);
        }
        private void OnSendChatDataReturn(object sender, GameEventArgs e)
        {
            List<BaseChatData> selfChatData = GameEntry.Data.Chat.WorldChatList;
            BaseChatData myChat = selfChatData.FindLast(x => x.Sender == null);
            if (myChat == null)
                return;
            OnPlayerChat(myChat.Message, GameEntry.Data.Player.Name);
        }
        private void OnPlayerChat(string msg, string playerName = null)
        {
            if (playerName == null)
                playerName = GameEntry.Data.Player.Name;
            StringBuilder chatMsg = new StringBuilder();
            chatMsg.Append(GameEntry.Localization.GetString("UI_TEXT_WORLD_TALK"))
                .Append("[00ff00]").Append(playerName).Append("[-]").Append(":").Append(msg);
            m_ChatMsg.text = chatMsg.ToString();
        }
        public void OnShowChatMsg(LCReceiveChat msg)
        {
            StringBuilder chatMsg = new StringBuilder();
            chatMsg.Append(GameEntry.Localization.GetString("UI_TEXT_WORLD_TALK")).Append(msg.Sender.Name).Append(":").Append(msg.Message);
            if (msg.Channel == (int)ChatType.Private)
            {
                Log.Debug("私人聊天{0}", msg.Message);
            }
            else if (msg.Channel == (int)ChatType.World)
            {
                Log.Debug("世界聊天{0}", msg.Message);
            }
            m_ChatMsg.text = chatMsg.ToString();
        }
        public void OnShowChatMsg()
        {
            if (GameEntry.Data.Chat.HasSystemBroadCastMsg)
            {
                m_SystemChat.SetActive(true);
            }
        }

        private void OnPendingFriendRequestsDataChanged(object sender, GameEventArgs e)
        {
            RefreshSocialReminder();
        }

        private void RefreshSocialReminder()
        {
            bool showFriendReminder = GameEntry.Data.FriendRequests.RequestList.Count > 0;
            //&& GameEntry.Data.Player.Level >= GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.UnlockLevel, 5);
            m_FriendReminder.gameObject.SetActive(showFriendReminder);
            m_MailReminder.gameObject.SetActive(GameEntry.Data.Mails.HasUnreadMail());
            ReminderIcons[(int)ReminderType.ChatReminder].SetActive(GameEntry.Reminder.PrivateChatRequestsReminder);
            ReminderIcons[(int)ReminderType.MeridianReminder].SetActive(GameEntry.Reminder.HasMeridianEnergyReminder);
            ReminderIcons[(int)ReminderType.HeroInfoReminder].SetActive(GameEntry.Reminder.HasCanSummonHero || GameEntry.Reminder.HasCanHeroStarUp());
            ReminderIcons[(int)ReminderType.ChanceReminder].SetActive(GameEntry.Reminder.HasFreeChancedTimes);

            ReminderIcons[(int)ReminderType.DailyQuestReminder].SetActive(GameEntry.Reminder.HasUnclaimedQuest);
            ReminderIcons[(int)ReminderType.ActivityReminder].SetActive(GameEntry.Reminder.HasUnfinishedActivity);
            m_DailyLoginReminderObject.SetActive(GameEntry.Reminder.EnableDailyLogin);
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            if (m_UIFormsToOpen.Count <= 0)
            {
                return;
            }

            var ne = e as UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
            var uiFormId = (UIFormId)ne.UIForm.TypeId;
            if (m_UIFormsToOpen.Contains(uiFormId))
            {
                m_UIFormsToOpen.Remove(uiFormId);
            }

            if (m_UIFormsToOpen.Count <= 0)
            {
                GameEntry.Loading.Hide();
            }
        }

        private void OnOpenUIFormFailure(object sender, GameEventArgs e)
        {

        }

        private IEnumerator CheckTempData()
        {
            yield return null;

            CheckBooleanTempDataAndOpenUIForm(Constant.TempData.AutoOpenChessBoard, UIFormId.ActivityChessmanForm);
            CheckBooleanTempDataAndOpenUIForm(Constant.TempData.AutoOpenOfflineArena, UIFormId.ActivityOfflineArenaForm);
            CheckBooleanTempDataAndOpenUIForm(Constant.TempData.AutoOpenCosmosCrack, UIFormId.ActivityCosmosForm);
            CheckBooleanTempDataAndOpenUIForm(Constant.TempData.AutoOpenInstanceSelectForm, UIFormId.InstanceSelectForm);
            CheckBooleanTempDataAndOpenUIForm(Constant.TempData.AutoOpenActivitySinglePvpMainForm, UIFormId.ActivitySinglePvpMainForm);
            CheckObjectTempDataAndOpenUIForm(Constant.TempData.AutoOpenHeroInfoForm, UIFormId.HeroInfoForm_Possessed);
            CheckBooleanTempDataAndOpenUIForm(Constant.TempData.AutoOpenMeridianForm, UIFormId.MeridianForm);
            CheckBooleanTempDataAndOpenUIForm(Constant.TempData.AutoOpenActivitySelectForm, UIFormId.ActivitySelectForm);
            CheckBooleanTempDataAndOpenUIForm(Constant.TempData.AutoOpenActivityBossForm, UIFormId.ChallengeBossForm);
            CheckBooleanTempDataAndOpenUIForm(Constant.TempData.AutoOpenActivityTowerForm, UIFormId.StormTowerForm);
        }

        private void CheckBooleanTempDataAndOpenUIForm(string key, UIFormId uiFormId)
        {
            if (!GameEntry.Data.HasTempData(key))
            {
                return;
            }

            bool val = GameEntry.Data.GetTempData<bool>(key);
            if (val)
            {
                m_UIFormsToOpen.Add(uiFormId);
                GameEntry.UI.OpenUIForm(uiFormId, new UIFormSimpleUserData { ShouldOpenImmediately = true });
            }

            GameEntry.Data.RemoveTempData(key);
        }

        private void CheckObjectTempDataAndOpenUIForm(string key, UIFormId uiFormId)
        {
            if (!GameEntry.Data.HasTempData(key))
            {
                return;
            }

            var userData = GameEntry.Data.GetTempData<object>(key) as UIFormBaseUserData;
            if (userData != null)
            {
                m_UIFormsToOpen.Add(uiFormId);
                GameEntry.UI.OpenUIForm(uiFormId, userData);
            }

            GameEntry.Data.RemoveTempData(key);
        }

        private void OnTouchUp(object sender, GameEventArgs e)
        {
            var ne = e as TouchUpEventArgs;

            if (!GameEntry.IsAvailable || !GameEntry.Input.JoystickEnabled)
            {
                return;
            }

            if (!TryInteractWithOtherPlayer(ne.Position))
            {
                CheckHideOtherPlayerDisplay();
            }
        }

        private bool CheckHideOtherPlayerDisplay()
        {
            if (m_OtherPlayerIdForInteraction <= 0)
            {
                return false;
            }

            m_OtherPlayerIdForInteraction = 0;
            m_OtherPlayerHeroRoot.SetActive(false);
            return true;
        }

        private void ShowOtherPlayerDisplay(int playerId, int heroIconId)
        {
            if (playerId <= 0)
            {
                CheckHideOtherPlayerDisplay();
                return;
            }

            if (!m_OtherPlayerHeroRoot.activeSelf)
            {
                m_OtherPlayerHeroRoot.SetActive(true);
            }
            m_OtherPlayerHeroAnimation.Rewind();
            m_OtherPlayerHeroAnimation.Play();
            m_OtherPlayerIdForInteraction = playerId;
            m_OtherPlayerHeroIcon.LoadAsync(heroIconId);
        }

        private bool TryInteractWithOtherPlayer(Vector2 screenPos)
        {
            if (!GameEntry.IsAvailable)
            {
                return false;
            }

            var mainCamera = GameEntry.Scene.MainCamera;
            if (mainCamera == null)
            {
                return false;
            }

            Ray ray = mainCamera.ScreenPointToRay(new Vector3(screenPos.x, screenPos.y, 0f));
            var hits = Physics.RaycastAll(ray, mainCamera.farClipPlane, 1 << Constant.Layer.TargetableObjectLayerId) as IList<RaycastHit>;
            int playerId = -1;
            int heroId = -1;
            for (int i = 0; i < hits.Count; ++i)
            {
                var trans = hits[i].transform;
                var heroCharacter = trans.GetComponent<HeroCharacter>();
                if (heroCharacter == null || heroCharacter is MeHeroCharacter)
                {
                    continue;
                }

                // Relying on the assumption that other players' hero characters in the lobby has the entity ID equal to their player ID.
                playerId = heroCharacter.Id;
                heroId = heroCharacter.Data.HeroId;
                break;
            }

            if (playerId <= 0 || heroId <= 0)
            {
                return false;
            }

            var dt = GameEntry.DataTable.GetDataTable<DRHero>();
            DRHero dr = dt.GetDataRow(heroId);
            if (dr == null)
            {
                Log.Warning("Hero '{0}' not found.", heroId);
                return false;
            }

            var nearbyPlayerDatas = GameEntry.Data.NearbyPlayers.Data;
            for (int i = 0; i < nearbyPlayerDatas.Count; ++i)
            {
                var playerData = nearbyPlayerDatas[i];
                if (playerData.Key == playerId)
                {
                    ShowOtherPlayerDisplay(playerId, dr.IconId);
                    return true;
                }
            }

            return false;
        }

        public void OnClickEmail()
        {
            GameEntry.UI.OpenUIForm(UIFormId.EmailForm);
        }

        public void OnClickSkill()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SkillStrengthenForm, new SkillStrengthenDisplayData { SelectHeroType = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).MainHeroType, SkillIndex = 0, });
        }

        public void OnClickFriend()
        {
            //if (GameEntry.Data.Player.Level < GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.UnlockLevel, 5))
            //{
            //    GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_OPEN_LEVEL") });
            //    return;
            //}
            GameEntry.UI.OpenUIForm(UIFormId.FriendListForm);
        }

        public void OnClickRank()
        {
            GameEntry.UI.OpenUIForm(UIFormId.RankListForm, new RankListDisplayData { Scenario = RankListType.TotalMight });
        }

        public void OnClickAchievements()
        {
            //if (GameEntry.Data.Player.Level < GameEntry.ServerConfig.GetInt(Constant.ServerConfig.DailyQuest.UnlockLevel, 10))
            //{
            //    GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_DAILY_QUEST_OPEN_LEVEL") });
            //    return;
            //}

            GameEntry.UI.OpenUIForm(UIFormId.TasksForm);
        }

        public void OnClickSwitchButton()
        {
            bool bottomListIsOpen = GameEntry.Setting.GetBool(BottomListIsOpen, true);
            GameEntry.Setting.SetBool(BottomListIsOpen, !bottomListIsOpen);
            GameEntry.Setting.Save();
            SwitchBottomList(false);
        }

        private void SwitchBottomList(bool isInit)
        {
            if (!GameEntry.Setting.HasKey(BottomListIsOpen))
            {
                GameEntry.Setting.SetBool(BottomListIsOpen, true);
                GameEntry.Setting.Save();
            }
            bool bottomListIsOpen = GameEntry.Setting.GetBool(BottomListIsOpen, true);
            PlayBottomListAnim(bottomListIsOpen, isInit);
            //PlayTopListAnim(bottomListIsOpen, isInit);
        }

        private void PlayBottomListAnim(bool isOpen, bool isInit)
        {
            float speed = 1.0f;
            float time = 0.0f;
            float startRot = HalfRoundAngle;
            float endRot = 0;
            if (isOpen)
            {
                speed = -1.0f;
                time = m_SwitchAnimation.clip.length;
                startRot = HalfRoundAngle;
                endRot = 0;
            }
            else
            {
                speed = 1.0f;
                time = 0;
                startRot = 0;
                endRot = HalfRoundAngle;
            }

            TweenRotation tweenRo = m_SwitchButton.GetComponent<TweenRotation>();
            if (tweenRo == null)
            {
                tweenRo = m_SwitchButton.gameObject.AddComponent<TweenRotation>();
            }
            tweenRo.from = new Vector3(0, 0, startRot);
            tweenRo.to = new Vector3(0, 0, endRot);
            tweenRo.duration = 0.3f;
            tweenRo.ResetToBeginning();
            tweenRo.PlayForward();

            if (isInit)
            {
                m_SwitchAnimation.Stop();
                m_SwitchAnimation.clip.SampleAnimation(m_SwitchAnimation.gameObject, isOpen ? 0 : m_SwitchAnimation.clip.length);
            }
            else
            {
                m_SwitchAnimation.Rewind();
                m_SwitchAnimation[m_SwitchAnimation.clip.name].time = time;
                m_SwitchAnimation[m_SwitchAnimation.clip.name].speed = speed;
                m_SwitchAnimation.Play();
            }
        }

        private void PlayTopListAnim(bool isOpen, bool isInit)
        {
            float speed = 1.0f;
            float time = 0.0f;
            if (isOpen)
            {
                speed = -1.0f;
                time = m_TopSwitchAnimation.clip.length;
            }
            else
            {
                speed = 1.0f;
                time = 0;
            }

            if (isInit)
            {
                m_TopSwitchAnimation.Stop();
                m_TopSwitchAnimation.clip.SampleAnimation(m_TopSwitchAnimation.gameObject, isOpen ? 0 : m_TopSwitchAnimation.clip.length);
            }
            else
            {
                m_TopSwitchAnimation.Rewind();
                m_TopSwitchAnimation[m_TopSwitchAnimation.clip.name].time = time;
                m_TopSwitchAnimation[m_TopSwitchAnimation.clip.name].speed = speed;
                m_TopSwitchAnimation.Play();
            }
        }

        private void ShowGuideEffect()
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                //                 if (GameEntry.Data.GetTempData<bool>(Constant.TempData.NeedCreatePlayer))
                //                 {
                //                     m_EffectsController.ShowEffect("EffectPVPClick");
                //                 }
                //                 else
                {
                    m_EffectsController.ShowEffect("EffectPVP");
                }
            }
        }
    }
}
