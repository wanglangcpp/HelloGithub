using UnityEngine;
using System.Collections;
using GameFramework.Event;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class MonthCardForm : WelfareCenterBaseTabContent
    {
        [Serializable]
        private class CardTemplate
        {
            public int CardId;//月卡的编号
            public GameObject m_HoldCardTag = null;//持续标签
            public UISprite m_CardIcon = null;//月卡图标
            public UILabel m_DesText = null;//赠送钻石提示
            public List<GeneralItemView> m_RewardList = null;//赠送的物品
            public UILabel m_RemainingText = null;//剩余天数
            public UIButton m_BtnCharge = null;//充值按钮
            public UIButton m_BtnGet = null;//领取奖励
            public UISprite m_RemindIcon = null;//小红点
        }
        [SerializeField]
        private CardTemplate[] CardTemplates = null;

        private List<int> CardCount = new List<int>();//拥有的月卡Id

        private List<int> cardCount = new List<int>();
        private List<int> canGet = new List<int>();
        private List<int> notGet = new List<int>();
        private DRGfitBag[] m_GiftData = null;

        protected override void OnOpen(GameObject obj)
        {
            m_GiftData = UIUtility.GetGiftType(GiftType.MonthCard).ToArray();
            base.OnOpen(this.gameObject);
        }
        protected override void OnClose()
        {
            base.OnClose();
            //GameEntry.Event.Unsubscribe(EventId.RefreshMonthCard, OnRefreshMonthCardTime);
        }
        /// <summary>
        /// 初始化更新
        /// </summary>
        protected override IEnumerator RefreshData()
        {
            for (int i = 0; i < CardTemplates.Length; i++)
            {
                CardTemplates[i].m_BtnCharge.onClick.Clear();
                CardTemplates[i].m_BtnCharge.name = i.ToString();
                UIEventListener.Get(CardTemplates[i].m_BtnCharge.gameObject).onClick = OnClickBuyButton;
                //new UIEventListener.VoidDelegate(OnClickBuyButton);

                CardTemplates[i].m_BtnGet.onClick.Clear();
                CardTemplates[i].m_BtnGet.name = i.ToString();
                UIEventListener.Get(CardTemplates[i].m_BtnGet.gameObject).onClick = OnClickGetButton;
                //new UIEventListener.VoidDelegate(OnClickGetButton);

                CardTemplates[i].m_HoldCardTag.SetActive(false);
                CardTemplates[i].m_DesText.text = GameEntry.Localization.GetString("UI_WELFARE_CARD_PRESENT", m_GiftData[i].Diamond);
                CardTemplates[i].m_BtnCharge.gameObject.SetActive(true);
                CardTemplates[i].m_BtnCharge.GetComponentInChildren<UILabel>().text =
                    GameEntry.Localization.GetString("UI_WELFARE_CARD_ACTIVATION", m_GiftData[i].Price);
                CardTemplates[i].m_BtnGet.gameObject.SetActive(false);
                if (!CardCount.Contains(CardTemplates[i].CardId))
                {
                    CardTemplates[i].m_RemainingText.text = string.Empty;
                }
                for (int j = 0; j < CardTemplates[i].m_RewardList.Count; j++)
                {
                    CardTemplates[i].m_RewardList[j].InitGeneralItem(m_GiftData[i].Items[j].Icon, m_GiftData[i].Items[j].Count);
                }
            }
            yield return null;
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

        }
        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }

        ///// <summary>
        ///// 刷新月卡剩余时间
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void OnRefreshMonthCardTime(int index, long buyTime)
        //{
        //    GetMonthCardTimeEventArgs ne = e as GetMonthCardTimeEventArgs;
        //    if (ne == null)
        //        return;
        //    if (ne. == 0)
        //        return;
        //    for (int i = 0; i < ne.MonthCard.Count; i++)
        //    {
        //        long time = buyTime;
        //        int m_RemainingTime = (int)Computationtime(time);
        //        if (m_RemainingTime < 30)
        //        {
        //            CardCount.Add(ne.MonthCard[i].CardId);//添加正在使用的月卡Id
        //            for (int j = 0; j < CardTemplates.Length; j++)
        //            {
        //                if (CardTemplates[j].CardId == ne.MonthCard[i].CardId)
        //                {
        //                    CardTemplates[index].m_RemainingText.text = GameEntry.Localization.GetString("UI_WELFARE_CARD_GET", m_RemainingTime);
        //                    break;
        //                }
        //            }
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
        //    return RemainingTime;
        //}

        /// <summary>
        /// 物品状态回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void GetItemSuatus(object sender, GameEventArgs e)
        {
            ChargeStatusData ne = GameEntry.Data.ChargeStatusData;
            if (ne.StatusData != null)
            {
                UpdataItemStatus(ne.StatusData);
            }

        }
        protected override void UpdataItemStatus(ChargeStatus ne)
        {
            //cardCount = CardCount;
            //canGet = ne.NoGetGifts;
            //notGet = ne.HasGetGifts;
            for (int i = 0; i < CardTemplates.Length; i++)
            {
                int cardId = CardTemplates[i].CardId;
                if (ne.GiftStatus.ContainsKey(cardId))
                {
                    ItemStatus(i, ne.GiftStatus[cardId]);
                }
                if (ne.MonthCardTime.ContainsKey(cardId))
                {
                    CardTemplates[i].m_RemainingText.text = GameEntry.Localization.GetString("UI_WELFARE_CARD_GET", ne.MonthCardTime[cardId]);
                }
            }
        }

        private void ItemStatus(int index, int status)
        {
            if (status == 0)
            {
                CardTemplates[index].m_HoldCardTag.SetActive(true);
                CardTemplates[index].m_BtnCharge.gameObject.SetActive(false);
                CardTemplates[index].m_BtnGet.gameObject.SetActive(true);
                CardTemplates[index].m_BtnGet.isEnabled = true;
            }
            else if (status == 1)
            {
                CardTemplates[index].m_HoldCardTag.SetActive(true);
                CardTemplates[index].m_BtnCharge.gameObject.SetActive(false);
                CardTemplates[index].m_BtnGet.gameObject.SetActive(true);
                var label = CardTemplates[index].m_BtnGet.gameObject.GetComponentInChildren<UILabel>();
                label.text = GameEntry.Localization.GetString("UI_BUTTON_RECEIVED");
                CardTemplates[index].m_BtnGet.isEnabled = false;
            }
            else
            {
                CardTemplates[index].m_HoldCardTag.SetActive(false);
                CardTemplates[index].m_BtnCharge.gameObject.SetActive(true);
                CardTemplates[index].m_BtnGet.gameObject.SetActive(false);
                CardTemplates[index].m_BtnGet.isEnabled = false;
            }
        }

        public void OnClickGetButton(GameObject Id)
        {
            if (!Id.GetComponent<UIButton>().enabled)
            {
                return;
            }
            int GiftId = CardTemplates[int.Parse(Id.name)].CardId;
            if (GiftId != 0)
            {
                CLGetGift cLGetGift = new CLGetGift() { giftId = GiftId };
                GameEntry.Network.Send(cLGetGift);
            }
        }

        public void OnClickBuyButton(GameObject Id)
        {
            WelfareCenterBaseTabContent.onPaySuccess = WelfareCenterBaseTabContent.OnPaySuccess;

            //int giftId = CardTemplates[int.Parse(Id.name)].CardId;
            //GameEntry.UI.OpenUIForm(UIFormId.ChargeForm);
            int cardId = CardTemplates[int.Parse(Id.name)].CardId;
            ChargeInfo drc = GameEntry.Data.ChargeTable.Find(x => x.Id == cardId);
            string id = drc.Id.ToString();
            WelfareCenterBaseTabContent.Itemdata = drc.Id.ToString() + "," + drc.Price.ToString();
            string name = GameEntry.Localization.GetString(drc.Name);
            string desc = "";
            if (!string.IsNullOrEmpty(drc.Description) && !drc.Description.Equals("null"))
            {
                desc = GameEntry.Localization.GetString(drc.Description);
            }
            else desc = name;
            int price = drc.Price;
            PayInfos infos = new PayInfos(id, name, desc, price);
            SDKManager.Instance.helper.Pay(infos);
        }
    }

}
