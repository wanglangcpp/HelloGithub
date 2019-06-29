using UnityEngine;
using System;
using System.Collections.Generic;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class MailForm : NGUIForm
    {
        [SerializeField]
        private MailScrollView m_MailScrollView = null;

        [SerializeField]
        private UILabel m_MailCountLabel = null;

        [SerializeField]
        private UILabel m_EmptyTipsLabel = null;

        [SerializeField]
        private MailDetailInfo m_MailDetailInfo = null;

        [SerializeField]
        private UIPanel m_MessagePanel = null;

        private List<MailItem> m_CachedMailItems = new List<MailItem>();

        private int m_CurrentShowMailId = int.MinValue;

        private MailItem m_CurrentShowingMail = null;

        public void OnOneKeyPickRewardClick()
        {
            List<int> pickedIds = new List<int>();
            for (int i = 0; i < m_CachedMailItems.Count; i++)
            {
                var item = m_CachedMailItems[i].MailItemData;
                if (!item.IsAlreadRead && item.Rewards != null && item.Rewards.Data.Count > 0)
                {
                    pickedIds.Add(item.Id);
                }
            }

            if (pickedIds.Count > 0)
                GameEntry.LobbyLogic.MarkMailsAsRead(pickedIds);
            else
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_EMAIL_A_KEY_TO_RECEIVE_NO_ATTACHMENTS") });
        }

        public void OnOneKeyDeleteMailClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Mode = 2,
                Message = GameEntry.Localization.GetString("UI_TEXT_EMAIL_DELETE_ALL_EMAIL"),
                OnClickConfirm = (o) =>
                {
                    List<int> deleteIds = new List<int>();
                    for (int i = 0; i < m_CachedMailItems.Count; i++)
                        if (m_CachedMailItems[i].MailItemData.IsAlreadRead)
                            deleteIds.Add(m_CachedMailItems[i].MailItemData.Id);

                    if (deleteIds.Count > 0)
                        GameEntry.LobbyLogic.DeleteMails(deleteIds);
                    else
                        GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_EMAIL_A_KEY_TO_DELETE_NO_MAIL") });
                },
                OnClickCancel = (o) => { },
            });
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.MailDataChanged, OnMailDataChanged);
            GameEntry.Event.Subscribe(EventId.PickMailAttachment, OnPickAttachment);

            m_MessagePanel.depth = GetComponent<UIPanel>().depth + 1;

            RefreshList(true);
        }

        private void OnPickAttachment(object sender, GameEventArgs e)
        {
            var showData = e as PickMailAttachmentEventArgs;
            if (showData == null)
                return;

            GameEntry.RewardViewer.RequestShowRewards(showData.ShowRewards.ReceiveGoodsData, true);
        }

        private void RefreshList(bool reposition = false)
        {
            var mails = GameEntry.Data.Mails.Data;
            mails.Sort(SortMail);

            m_CachedMailItems.Clear();
            MailItem showingMail = null;
            for (int i = 0; i < mails.Count; i++)
            {
                var mail = m_MailScrollView.GetOrCreateItem(i);
                mail.SetMailData(mails[i], OnMailClick);
                m_CachedMailItems.Add(mail);

                if (m_CurrentShowMailId == mails[i].Id)
                    showingMail = mail;
            }

            m_MailScrollView.RecycleItemsAtAndAfter(mails.Count);

            if (reposition)
            {
                m_MailScrollView.Reposition();
            }
            else if (m_MailScrollView.NeedReposition)
            {
                m_MailScrollView.RepositionList();
            }

            // 每次刷新列表，如果之前打开了某一个还在列表里，默认打开，否则默认打开第一个
            if (showingMail == null)
            {
                if (mails.Count > 0)
                    OnMailClick(m_CachedMailItems[0]);
                else
                    m_MailDetailInfo.SetMailDetailInfo(null);
            }
            else
            {
                OnMailClick(showingMail);
            }

            m_EmptyTipsLabel.gameObject.SetActive(mails.Count <= 0);

            m_MailCountLabel.text = GameEntry.Localization.GetString(
                "UI_TEXT_EMAIL_NUMBER",
                mails.Count,
                GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Mail.MaxCount, 100));
        }

        private void OnMailClick(MailItem mail)
        {
            if (mail == null || mail.MailItemData == null)
            {
                m_CurrentShowMailId = int.MinValue;
                m_MailDetailInfo.SetMailDetailInfo(null);
            }
            else
            {
                if (m_CurrentShowingMail == null)
                {
                    m_CurrentShowingMail = mail;
                    m_CurrentShowingMail.SetItemSelectStatus(true);
                }
                else if (mail.MailItemData.Id != m_CurrentShowingMail.MailItemData.Id)
                {
                    m_CurrentShowingMail.SetItemSelectStatus(false);
                    mail.SetItemSelectStatus(true);
                    m_CurrentShowingMail = mail;
                }

                m_MailDetailInfo.SetMailDetailInfo(mail.MailItemData);
                m_CurrentShowMailId = mail.MailItemData.Id;
            }
        }

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable)
                return;

            base.OnClose(userData);
            GameEntry.Event.Unsubscribe(EventId.MailDataChanged, OnMailDataChanged);
            GameEntry.Event.Unsubscribe(EventId.PickMailAttachment, OnPickAttachment);
            m_CurrentShowMailId = int.MinValue;
        }

        /// <summary>
        /// 邮件排序。特殊邮件排在前面，其他的就按时间排序，最新的邮件在最上面
        /// </summary>
        private int SortMail(MailData m1, MailData m2)
        {
            if (m1.Priority != m2.Priority)
                return m2.Priority.CompareTo(m1.Priority);
            else
                return m2.SendTime.Ticks.CompareTo(m1.SendTime.Ticks);
        }

        private void OnMailDataChanged(object sender, GameEventArgs e)
        {
            var eventData = e as MailDataChangedEventArgs;

            RefreshList(eventData.NeedRepositionList);
        }

        [Serializable]
        private class MailScrollView : UIScrollViewCache<MailItem>
        {
            public void RepositionList()
            {
                Reposition();
                m_ScrollView.SetDragAmount(m_ScrollView.panel.finalClipRegion.x - m_ScrollView.panel.baseClipRegion.x, 1f, false);
            }

            private bool m_RepositionFlag = false;

            /// <summary>
            /// 这个是为了解决下面的问题：
            /// 当邮件列表很长的时候，滑到下面某一个位置，这时候把邮件删除，策划要求不能滚动列表，只把选中的删除。
            /// 这时候，当滑到最底下的时候，从下往上删除邮件，底下就会空出来。
            /// 这个变量就是判断是否需要“Reposition”，这个重新置，不是滑到顶部的Reposition，具体我也说不清楚了，进游戏里体验吧。
            /// </summary>
            public bool NeedReposition
            {
                get
                {
                    float yButtom = m_ScrollView.panel.baseClipRegion.w;
                    float yOffset = Math.Abs(m_ScrollView.panel.clipOffset.y) + m_ItemParent.transform.localPosition.y;
                    float perHeight = m_ItemParent.GetComponent<UIGrid>().cellHeight;
                    int shownItemCount = (int)Math.Floor((yButtom + yOffset) / perHeight);

                    int activeItemCount = 0;

                    for (int i = 0; i < m_ItemParent.transform.childCount; i++)
                    {
                        if (m_ItemParent.transform.GetChild(i).gameObject.activeSelf)
                            activeItemCount++;
                    }

                    // 小于一屏幕的时候，刷到初始位置
                    if (activeItemCount <= (int)Math.Floor(yButtom / perHeight))
                    {
                        if (m_RepositionFlag == false)
                        {
                            Reposition();
                            m_RepositionFlag = true;
                        }
                        return false;
                    }
                    else
                    {
                        m_RepositionFlag = false;
                    }

                    return shownItemCount >= activeItemCount;
                }
            }

        }
    }
}