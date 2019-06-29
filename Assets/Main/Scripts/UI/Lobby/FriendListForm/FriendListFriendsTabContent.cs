using GameFramework;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 好友列表标签页。用于 <see cref="Genesis.GameClient.FriendListForm"/>
    /// </summary>
    public class FriendListFriendsTabContent : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_EmptyText = null;

        [SerializeField]
        private FriendListScrollView m_FriendScrollView = null;

        [SerializeField]
        private UILabel m_FriendNumberLabel = null;

        private List<FriendListFriendItem> m_CachedItems = new List<FriendListFriendItem>();

        public void OnOneKeyPickButtonClick()
        {
            // 可领取的次数 = 最大领取数量 - 已经领取的次数
            int maxCount = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.DailyEnergyClaimTimes, 10) - GameEntry.Data.Friends.TodayClaimCount;

            if (maxCount <= 0)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_UPPER_LIMIT_OF_PHYSICAL_STRENGTH") });
                return;
            }

            //可以领取的次数
            int enableCount = 0;
            for (int i = 0; i < GameEntry.Data.Friends.Data.Count; i++)
                if (GameEntry.Data.Friends.Data[i].CanReceiveEnergy)
                    enableCount++;

            if (enableCount <= 0)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_NO_PHYSICAL_STRENGTH") });
                return;
            }

            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Mode = 2,
                Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_GET_ALL_YOUR_GOOD_FRIENDS"),
                OnClickConfirm = (o) =>
                {
                    int counter = 0;
                    for (int i = 0; i < m_CachedItems.Count; i++)
                    {
                        if (counter >= maxCount)
                        {
                            GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_UPPER_LIMIT_OF_PHYSICAL_STRENGTH") });
                            return;
                        }

                        if (m_CachedItems[i].EnableClaimEnergy)
                        {
                            m_CachedItems[i].PickEnergyFromFriend();
                            counter++;
                        }
                    }
                    GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_RECEIVE_PHYSICAL_SUCCESS") });
                },
                OnClickCancel = (o) => { },
            });
        }

        public void OnOneKeySendButtonClick()
        {
            int maxCount = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.DailyEnergyGiveTimes, 10) - GameEntry.Data.Friends.TodayGiveCount;

            if (maxCount <= 0)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_GIVE_PHYSICAL_STRENGTH_LIMIT") });
                return;
            }

            int enableReceive = 0;
            for (int i = 0; i < GameEntry.Data.Friends.Data.Count; i++)
                if (GameEntry.Data.Friends.Data[i].CanGiveEnergy)
                    enableReceive++;

            if(enableReceive <= 0)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_NOBODY_RECEIVE") });
                return;
            }

            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Mode = 2,
                Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_GIVE_ALL_YOUR_FRIENDS_PHYSICAL_STRENGTH"),
                OnClickConfirm = (o) =>
                    {
                        int counter = 0;
                        for (int i = 0; i < m_CachedItems.Count; i++)
                        {
                            if (counter >= maxCount)
                            {
                                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_GIVE_PHYSICAL_STRENGTH_LIMIT") });
                                return;
                            }

                            if (m_CachedItems[i].EnableSendEnergy)
                            {
                                m_CachedItems[i].GiveEnergyToFriend();
                                counter++;
                            }
                        }
                        GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_PHYSICAL_GIFT_SUCCESS") });
                    },
                OnClickCancel = (o) => { },
            });
        }

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(EventId.EnergyGivenToFriend, OnEnergyGivenToFriend);
            GameEntry.Event.Subscribe(EventId.EnergyGivenFromFriend, OnEnergyGivenFromFriend);
            GameEntry.Event.Subscribe(EventId.EnergyReceivedFromFriend, OnEnergyReceivedFromFriend);
            GameEntry.Event.Subscribe(EventId.FriendDeleted, OnFriendRemoved);
            GameEntry.Event.Subscribe(EventId.FriendAdded, OnPlayerAgreed);
            GameEntry.Event.Subscribe(EventId.FriendDataChanged, OnFriendDataChanged);

            StartCoroutine(InitScrollViewCo());
        }

        private void OnFriendDataChanged(object sender, GameEventArgs e)
        {
            var arg = e as FriendDataChangedEventArgs;
            if (arg.ItsChangeMode == FriendDataChangedEventArgs.ChangeMode.ListRefreshed)
                for (int i = 0; i < m_CachedItems.Count; i++)
                    m_CachedItems[i].RefreshFromEntityData();
        }

        private void OnDisable()
        {
            if (!GameEntry.IsAvailable)
                return;

            GameEntry.Event.Unsubscribe(EventId.EnergyGivenToFriend, OnEnergyGivenToFriend);
            GameEntry.Event.Unsubscribe(EventId.EnergyGivenFromFriend, OnEnergyGivenFromFriend);
            GameEntry.Event.Unsubscribe(EventId.EnergyReceivedFromFriend, OnEnergyReceivedFromFriend);
            GameEntry.Event.Unsubscribe(EventId.FriendDeleted, OnFriendRemoved);
            GameEntry.Event.Unsubscribe(EventId.FriendAdded, OnPlayerAgreed);
            GameEntry.Event.Unsubscribe(EventId.FriendDataChanged, OnFriendDataChanged);

            DeinitScrollView();
            m_EmptyText.gameObject.SetActive(false);
        }

        private IEnumerator InitScrollViewCo()
        {
            yield return null;
            InitScrollView();
        }

        private void InitScrollView()
        {
            int upperBound = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.MaxFriendCount, 50);
            m_FriendNumberLabel.text = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_ONLIEN_NUMBER", GameEntry.Data.Friends.Data.Count, upperBound);
            m_EmptyText.gameObject.SetActive(GameEntry.Data.Friends.Data.Count <= 0);

            var tempFriendList = new List<FriendData>(GameEntry.Data.Friends.Data);
            tempFriendList.Sort(CompareFriends);

            m_CachedItems.Clear();
            for (int i = 0; i < tempFriendList.Count; ++i)
            {
                var item = m_FriendScrollView.GetOrCreateItem(i);
                item.Refresh(tempFriendList[i]);
                m_CachedItems.Add(item);
            }

            m_FriendScrollView.RecycleItemsAtAndAfter(tempFriendList.Count);
            m_FriendScrollView.ResetPosition();
        }

        private void DeinitScrollView()
        {
            m_FriendScrollView.RecycleItemsAtAndAfter(0);
            m_CachedItems.Clear();
        }

        private int CompareFriends(FriendData f1, FriendData f2)
        {
            // 在线的优先
            if (f1.Player.IsOnline != f2.Player.IsOnline)
            {
                if (f1.Player.IsOnline)
                    return -1;
                else
                    return 1;
            }

            // 最近登录的优先
            if (f1.LastLogoutTime != f2.LastLogoutTime)
            {
                if (f1.LastLogoutTime < f2.LastLogoutTime)
                    return -1;
                else
                    return 1;
            }

            // 等级大的优先
            if (f1.Player.Level != f2.Player.Level)
            {
                if (f1.Player.Level > f2.Player.Level)
                    return -1;
                else
                    return 1;
            }

            // VIP等级大的优先
            if (f1.Player.VipLevel > f2.Player.VipLevel)
                return -1;
            else
                return 1;
        }

        private void OnEnergyGivenFromFriend(object sender, GameEventArgs e)
        {
            var ne = e as EnergyGivenFromFriendEventArgs;

            for (int i = 0; i < m_CachedItems.Count; ++i)
            {
                if (m_CachedItems[i].FriendPlayerData.Id == ne.FriendPlayerId)
                {
                    m_CachedItems[i].RefreshCanReceiveEnergy(true);
                    return;
                }
            }

            Log.Warning("Friend player '{0}' not found.", ne.FriendPlayerId);
        }

        private void OnEnergyGivenToFriend(object sender, GameEventArgs e)
        {
            var ne = e as EnergyGivenToFriendEventArgs;

            for (int i = 0; i < m_CachedItems.Count; ++i)
            {
                if (m_CachedItems[i].FriendPlayerData.Id == ne.FriendPlayerId)
                {
                    m_CachedItems[i].RefreshCanGiveEnergy(false);
                    return;
                }
            }

            Log.Warning("Friend player '{0}' not found.", ne.FriendPlayerId);
        }

        private void OnEnergyReceivedFromFriend(object sender, GameEventArgs e)
        {
            var ne = e as EnergyReceivedFromFriendEventArgs;

            for (int i = 0; i < m_CachedItems.Count; ++i)
            {
                if (m_CachedItems[i].FriendPlayerData.Id == ne.FriendPlayerId)
                {
                    m_CachedItems[i].RefreshCanReceiveEnergy(false);
                    return;
                }
            }

            Log.Warning("Friend player '{0}' not found.", ne.FriendPlayerId);
        }

        private void OnFriendRemoved(object sender, GameEventArgs e)
        {
            InitScrollView();
        }

        private void OnPlayerAgreed(object sender, GameEventArgs e)
        {
            InitScrollView();
        }

        [Serializable]
        private class FriendListScrollView : UIScrollViewCache<FriendListFriendItem>
        {

        }
    }
}
