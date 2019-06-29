using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using System;

namespace Genesis.GameClient
{
    public class ChargeFirstForm : NGUIForm
    {
        [SerializeField]
        private List<GeneralItemView> m_RewardList = null;

        private ChargeItemsDisPlayData displayData = null;

        [SerializeField]
        private Transform m_HeroPrent = null;

        [SerializeField]
        private UISprite[] m_HeroMight = null;

        [SerializeField]
        private UIButton m_Button = null;
        [SerializeField]
        private UILabel m_ButtonName = null;

        private int giftId;

        private const string Charge = "UI_TEXT_CHARGE";
        private const string GetReward = "UI_WELFARE_CARD_EASY";
        private const string RewardInFinish = "UI_TEXT_BENEFITS";

        private int enityId;

        /// <summary>
        /// 要展示的英雄模型ID
        /// </summary>
        [SerializeField]
        private int m_HeroModleID;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            displayData = userData as ChargeItemsDisPlayData;
            giftId = displayData.GiftId;

            if (displayData == null)
            {
                CloseSelf(true);
                return;
            }

            ShowHero();
            StartCoroutine(RefreshItem());

        }

        private void Start()
        {
            Transform trans = transform.FindChild("Bg Mask");
            UIButton btn = null;
            if (null != trans)
            {
                btn = trans.GetComponent<UIButton>();
            }
            if(null != btn)
            {
                btn.defaultColor = new Color(0,0,0,120f/255);
                btn.UpdateColor(true);
            }
        }

        public void OnEnable()
        {
            var data = GameEntry.Data.ChargeStatusData;
            if (data.StatusData != null)
            {
                UpDataStatus(data.StatusData);
            }
            SubscribeEvents();
        }
        public void OnDisable()
        {
            UnsubscribeEvents();
        }
        private void SubscribeEvents()
        {
            GameEntry.Event.Subscribe(EventId.GetItemStatus, GetGiftState);
            GameEntry.Event.Subscribe(EventId.ReceiveBuyItem, OnReceiveBuyItem);
        }

        private void UnsubscribeEvents()
        {
            GameEntry.Event.Unsubscribe(EventId.GetItemStatus, GetGiftState);
            GameEntry.Event.Unsubscribe(EventId.ReceiveBuyItem, OnReceiveBuyItem);
        }

        private void OnReceiveBuyItem(object sender, GameEventArgs e)
        {
            ReceivePayItemEventArgs ne = e as ReceivePayItemEventArgs;
            GameEntry.RewardViewer.RequestShowRewards(ne.CompoundItemInfo, false);
        }

        /// <summary>
        /// 获取礼包状态回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetGiftState(object sender, GameEventArgs e)
        {
            ChargeStatus status = GameEntry.Data.ChargeStatusData.StatusData;
            if (status != null)
            {
                UpDataStatus(status);
            }
        }

        private void UpDataStatus(ChargeStatus ne)
        {
            #region 原有协议
            //if (ne.FirstChargeItems.Count == 0)
            //{
            //    m_Button.onClick.Clear();
            //    m_Button.isEnabled = true;
            //    m_Button.onClick.Add(new EventDelegate(() => OnClickPenMentButton()));
            //    m_ButtonName.text = GameEntry.Localization.GetString(Charge);
            //}
            //else
            //{
            //    if (ne.NoGetGifts.Contains(giftId))
            //    {
            //        m_Button.onClick.Clear();
            //        m_Button.isEnabled = true;
            //        m_Button.onClick.Add(new EventDelegate(() => OnClickReceiveReward()));
            //        m_ButtonName.text = GameEntry.Localization.GetString(GetReward);
            //    }
            //    else
            //    {
            //        m_Button.onClick.Clear();
            //        m_Button.isEnabled = false;
            //        m_ButtonName.text = "福利已领取";
            //    }
            //}
            #endregion
            if (ne.FirstChargeItems.Count != 0)
            {
                if (ne.GiftStatus.ContainsKey(giftId))
                {
                    if (ne.GiftStatus[giftId] == 1)//领取结束
                    {
                        m_Button.onClick.Clear();
                        m_Button.isEnabled = false;
                        m_ButtonName.text = GameEntry.Localization.GetString(RewardInFinish);
                    }
                    else if (ne.GiftStatus[giftId] == 0)//可以领取
                    {
                        m_Button.onClick.Clear();
                        m_Button.isEnabled = true;
                        m_Button.onClick.Add(new EventDelegate(() => OnClickReceiveReward()));
                        m_ButtonName.text = GameEntry.Localization.GetString(GetReward);
                    }
                }
            }
            else//未购买
            {
                m_Button.onClick.Clear();
                m_Button.isEnabled = true;
                m_Button.onClick.Add(new EventDelegate(() => OnClickPenMentButton()));
                m_ButtonName.text = GameEntry.Localization.GetString(Charge);
            }
        }

        private IEnumerator RefreshItem()
        {
            for (int i = 0; i < m_RewardList.Count; i++)
            {
                if (i < displayData.Rewards.Count)
                {
                    m_RewardList[i].InitGeneralItem(displayData.Rewards[i].Id, displayData.Rewards[i].Count);
                    m_RewardList[i].gameObject.SetActive(true);

                }
                else
                {
                    m_RewardList[i].gameObject.SetActive(false);
                }
            }
            var data = GameEntry.Data.ChargeStatusData;
            if (data.StatusData != null)
            {
                UpDataStatus(data.StatusData);
            }
            yield return null;
            //加载奖励物品
        }
        //英雄展示
        private void ShowHero()
        {
            DRHero dr = GameEntry.DataTable.GetDataTable<DRHero>().GetDataRow(m_HeroModleID);

            enityId = FakeCharacter.Show(dr.CharacterId, dr.Id, m_HeroPrent, null, 0f, FakeCharacterData.ActionOnShow.Debut);
            char[] might = dr.DefaultMight.ToString().ToCharArray();
            for (int i = 0; i < m_HeroMight.Length; i++)
            {
                if (i < might.Length)
                {
                    m_HeroMight[i].spriteName = might[i].ToString();
                    m_HeroMight[i].gameObject.SetActive(true);
                }
                else
                {
                    m_HeroMight[i].gameObject.SetActive(false);
                }
            }
        }

        public void OnClickPenMentButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChargeForm);
        }

        public void OnClickReceiveReward()
        {
            CLGetGift request = new CLGetGift();
            request.giftId = giftId;
            GameEntry.Network.Send(request);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            var enity = GameEntry.Entity.GetEntity(enityId);
            if (enity == null)
                return;
            GameEntry.Entity.HideEntity(enityId);
        }

    }
}

