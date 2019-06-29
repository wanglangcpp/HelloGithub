using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using System;
using GameFramework;

namespace Genesis.GameClient
{
    public class ChargeForm : NGUIForm
    {
        /// <summary>
        /// //Vip等级
        /// </summary>
        [SerializeField]
        private UILabel m_VipLv = null;
        /// <summary>
        /// //升级所需钻石
        /// </summary>
        [SerializeField]
        private UILabel m_VipUpText = null;
        /// <summary>
        /// //Vip经验
        /// </summary>
        [SerializeField]
        private UILabel m_VipExpText = null;
        /// <summary>
        /// //经验条
        /// </summary>
        [SerializeField]
        private UISlider m_ExpPrograssBg = null;
        /// <summary>
        /// //提示信息
        /// </summary>
        [SerializeField]
        private UILabel m_TipsText = null;
        /// <summary>
        /// //特权信息
        /// </summary>
        [SerializeField]
        private UIButton m_VipPrivilege = null;
        /// <summary>
        /// //滑动提示
        /// </summary>
        [SerializeField]
        private UISprite m_DropDown = null;

        [SerializeField]
        private UIGrid m_ChargeItemGridView = null;

        [SerializeField]
        private GameObject ChargeItemTemplate = null;
        [SerializeField]
        private GameObject VipIntroduce = null;
        [SerializeField]
        private GameObject VipIntroduceMask = null;

        private Dictionary<int, ChargeItem> m_MonthCardItems = new Dictionary<int, ChargeItem>();
        private Dictionary<int, ChargeItem> m_DiamondItems = new Dictionary<int, ChargeItem>();

        private ChargeInfo[] chargeItems = null;

        public GameFrameworkAction<object> onPaySuccess = null;

        private string Itemdata = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            chargeItems = GameEntry.Data.ChargeTable.ToArray();
            //  chargeItems=GameEntry.Data
        }

        //private void RequestRefresh()
        //{
        //    CLGetMonthCard request = new CLGetMonthCard();
        //    GameEntry.Network.Send(request);
        //}

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RefreshLabel();
            StartCoroutine(CreateItemsData());
            UIEventListener.Get(VipIntroduceMask).onClick = (go) => { VipIntroduce.SetActive(false); };
            //RequestRefresh();//请求月卡剩余时间

            var data = GameEntry.Data.ChargeStatusData;
            if (data.StatusData != null)
            {
                GetItemSuatus(data);
            }
            SubscribeEvents();
        }
        private void OnEnable()
        {
            //var data = GameEntry.Data.ChargeStatusData;
            //if (data.StatusData != null)
            //{
            //    GetItemSuatus(data);
            //}
            //SubscribeEvents();
        }
        private void OnDisable()
        {
            //UnsubscribeEvents();
        }
        private void SubscribeEvents()
        {
            //GameEntry.Event.Subscribe(EventId.RefreshMonthCard, OnRefreshMonthCard);
            GameEntry.Event.Subscribe(EventId.GetItemStatus, GetItemSuatus);
            GameEntry.Event.Subscribe(EventId.ReceiveBuyItem, OnReceiveBuyItem);
            GameEntry.Event.Subscribe(EventId.PlayerDataChanged, OnPlayerDataChanged);
            GameEntry.Event.Subscribe(EventId.LoadTitleSuccess, OnLoadTitleSuccess);
        }

        private void OnLoadTitleSuccess(object sender, GameEventArgs e)
        {
            Title title = GetComponentInChildren<Title>();
            UIButton button = title.transform.Find("Title/Money").GetComponentInChildren<UIButton>();
            if (button != null)
                button.gameObject.SetActive(false);
        }

        private void UnsubscribeEvents()
        {
            //GameEntry.Event.Unsubscribe(EventId.RefreshMonthCard, OnRefreshMonthCard);
            GameEntry.Event.Unsubscribe(EventId.GetItemStatus, GetItemSuatus);
            GameEntry.Event.Unsubscribe(EventId.ReceiveBuyItem, OnReceiveBuyItem);
            GameEntry.Event.Unsubscribe(EventId.PlayerDataChanged, OnPlayerDataChanged);
            GameEntry.Event.Unsubscribe(EventId.LoadTitleSuccess, OnLoadTitleSuccess);
        }
        private void OnPlayerDataChanged(object sender, GameEventArgs e)
        {
            RefreshLabel();
        }
        /// <summary>
        /// 领取礼包显示回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReceiveBuyItem(object sender, GameEventArgs e)
        {
            ReceivePayItemEventArgs ne = e as ReceivePayItemEventArgs;
            GameEntry.RewardViewer.RequestShowRewards(ne.CompoundItemInfo, false, onPaySuccess, Itemdata);
            for (int i = 0; i < ne.CompoundItemInfo.Items.Count; i++)
            {
                Log.Info("Receive Buy Item:Type{0}:Count{1}", ne.CompoundItemInfo.Items[i].Type, ne.CompoundItemInfo.Items[i].Count);
            }
        }

        private void OnPaySuccess(object obj)
        {
            Itemdata = null;
            onPaySuccess = null;
            SDKManager.Instance.helper.Record("Pay", obj.ToString());
        }
        /// <summary>
        /// 物品购买状态的回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetItemSuatus(object sender, GameEventArgs e)
        {
            var data = GameEntry.Data.ChargeStatusData;
            if (data.StatusData != null)
            {
                GetItemSuatus(data);
            }
        }
        private void GetItemSuatus(ChargeStatusData ne)
        {
            List<int> firstCharge = ne.StatusData.FirstChargeItems;
            if (firstCharge.Count != 0)
            {
                for (int i = 0; i < firstCharge.Count; i++)
                {
                    if (m_DiamondItems.ContainsKey(firstCharge[i]))
                    {
                        m_DiamondItems[firstCharge[i]].UpdateRewardsLabel(false);
                    }
                }
            }
            Dictionary<int, int> monthCardTime = ne.StatusData.MonthCardTime;

            foreach (var item in m_MonthCardItems)
            {
                if (monthCardTime.ContainsKey(item.Key))
                {
                    var script = item.Value;
                    script.PayMent.gameObject.SetActive(false);
                    script.RemainingTime.gameObject.SetActive(true);
                    script.RemainingTime.text = GameEntry.Localization.GetString("UI_WELFARE_CARD_GET", monthCardTime[item.Key]);
                    script.ButtonIcon.onClick.Clear();
                    script.ButtonIcon.onClick.Add(new EventDelegate(() => OnCickGetButton()));
                }
            }
        }

        ///// <summary>
        ///// 刷新月卡剩余时间
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void OnRefreshMonthCard(object sender, GameEventArgs e)
        //{
        //    GetMonthCardTimeEventArgs ne = e as GetMonthCardTimeEventArgs;
        //    if (ne == null)
        //        return;

        //    for (int i = 0; i < ne.MonthCard.Count; i++)
        //    {
        //        long time = ne.MonthCard[i].BuyTime;
        //        int m_RemainingTime = (int)Computationtime(time);
        //        if (m_RemainingTime < 30)
        //        {
        //            var script = m_MonthCardItems[ne.MonthCard[i].CardId];
        //            script.PayMent.gameObject.SetActive(false);
        //            script.RemainingTime.gameObject.SetActive(true);
        //            script.RemainingTime.text = GameEntry.Localization.GetString("UI_WELFARE_CARD_GET", m_RemainingTime);
        //            script.ButtonIcon.onClick.Clear();
        //            script.ButtonIcon.onClick.Add(new EventDelegate(() => OnCickGetButton()));
        //        }
        //    }
        //}
        ///// <summary>
        ///// 计算剩余时间
        ///// </summary>
        ///// <param name="time"></param>
        ///// <returns></returns>
        //private double Computationtime(long time)
        //{
        //    System.TimeSpan gapTime = System.DateTime.Now - new System.DateTime(time, System.DateTimeKind.Utc);
        //    double RemainingTime = gapTime.TotalDays;

        //    //System.DateTime m_time = new System.DateTime(time, System.DateTimeKind.Utc);
        //    //System.TimeSpan gapTime = System.DateTime.UtcNow - m_time;
        //    //double RemainingTime = gapTime.TotalDays;
        //    return RemainingTime;
        //}

        /// <summary>
        /// 更新用户VIP信息
        /// </summary>
        private void RefreshLabel()
        {
            var playerData = GameEntry.Data.Player;//获取当前玩家数据
            var drVipData = GameEntry.DataTable.GetDataTable<DRVip>().GetDataRow(playerData.VipLevel);//获取VIP配置信息

            m_VipLv.text = playerData.VipLevel.ToString();
            int LvUpNeedExp = drVipData.LevelUpExp - playerData.VipExp;
            m_VipUpText.text = GameEntry.Localization.GetString("UI_TEXT_CHARGE_1", LvUpNeedExp);

            m_VipExpText.text = GameEntry.Localization.GetString("UI_TEXT_SLASH", playerData.VipExp, drVipData.LevelUpExp);
            m_ExpPrograssBg.value = drVipData.LevelUpExp > 0 ?
                (float)playerData.VipExp / drVipData.LevelUpExp : 0f;
            m_TipsText.text = GameEntry.Localization.GetString("UI_TEXT_CHARGE_3");
        }
        /// <summary>
        /// 生成商品
        /// </summary>
        private IEnumerator CreateItemsData()
        {
            //生成数据对应的Item并更新Item信息
            for (int i = 0; i < chargeItems.Length; i++)
            {
                ChargeItem script;

                var go = NGUITools.AddChild(m_ChargeItemGridView.gameObject, ChargeItemTemplate);
                go.name = string.Format("Charge Item {0}", i.ToString());
                script = go.GetComponent<ChargeItem>();
                if (chargeItems[i].Type == 1)
                {
                    m_MonthCardItems.Add(chargeItems[i].Id, script);
                }
                else
                {
                    m_DiamondItems.Add(chargeItems[i].Id, script);
                }
                script.RefreshData(this, chargeItems[i], true);
            }
            m_ChargeItemGridView.enabled = true;

            //初始化完成之后更新钻石状态
            var data = GameEntry.Data.ChargeStatusData;
            if (data.StatusData != null)
            {
                GetItemSuatus(data);
            }
            yield return null;
        }

        /// <summary>
        /// 查看物品简介
        /// </summary>
        /// <param name="m_ItemId"></param>
        internal void OnClickHelpButton(Transform itemTransForm, int m_ItemId)
        {
            GameEntry.UI.OpenUIForm(UIFormId.TipsForm, new TipsFormDisplayData { RefTransform = itemTransForm, GeneralItemId = m_ItemId });
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="m_ItemId"></param>
        internal void OnClickBuyButton(int m_ItemId)
        {
            onPaySuccess = OnPaySuccess;
            //进入支付
            ChargeInfo drc = GameEntry.Data.ChargeTable.Find(x => x.Id == m_ItemId);
            string id = drc.Id.ToString();
            Itemdata = drc.Id.ToString() + "," + drc.Price.ToString();
            string name = GameEntry.Localization.GetString(drc.Name);
            string desc = "";
            if (!string.IsNullOrEmpty(drc.Description) && !drc.Description.Equals("null"))
            {
                desc = GameEntry.Localization.GetString(drc.Description);
            }
            else desc = name;
            int price = drc.Price;
            PayInfos payInfos = new PayInfos(id, name, desc, price);
            SDKManager.Instance.helper.Pay(payInfos);
            Log.Debug("Enter the recharge interface.....");
        }
        /// <summary>
        /// 领取月卡福利
        /// </summary>
        private void OnCickGetButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.WelfareListForm, new WelfareCenterDisplayData() { Scenario = WelfareType.MonthCard });
        }
        /// <summary>
        /// 查看Vip特权
        /// </summary>
        public void OnClickVipPrivilegeButton()
        {
            //UIUtility.ShowToast(GameEntry.Localization.GetString("UI_UNOPENED_FUNCTION"));
            VipIntroduce.SetActive(true);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            UnsubscribeEvents();
        }
    }
}

