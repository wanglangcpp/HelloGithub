using GameFramework.DataTable;
using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ChanceSelectForm : NGUIForm
    {
        [SerializeField]
        private UILabel[] m_NextFreeTime = null;

        [SerializeField]
        private UILabel m_MoneyFreeTimes = null;

        [SerializeField]
        private GameObject[] m_NoticeObj = null;

        private IDataTable<DRChanceRefresh> m_ChanceRefresh = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ChanceRefresh = GameEntry.DataTable.GetDataTable<DRChanceRefresh>();
            GameEntry.Event.Subscribe(EventId.ChanceDataChanged, OnChanceDataChanged);

            ShowMoneyFreeTimes();
        }

        protected override void OnResume()
        {
            base.OnResume();

            //for (int i = 0; i < (int)ChanceType.ChanceTypeCount - 1; i++)
            //{
            //    GameEntry.LobbyLogic.RequestChanceInfo((ChanceType)i);
            //}
            ShowMoneyFreeTimes();
        }

        private void ShowMoneyFreeTimes()
        {
            var chancerRefresh = m_ChanceRefresh.GetDataRow((int)ChanceType.Coin);
            int freeTimes = chancerRefresh.GiveFreeCount - GameEntry.Data.Chances.GetChanceData(ChanceType.Coin).FreeChancedTimes;
            if (freeTimes <= 0)
            {
                freeTimes = 0;
            }
            m_MoneyFreeTimes.text = GameEntry.Localization.GetString("UI_TEXT_FREEGEIVE_TIME_LIMITED", freeTimes, chancerRefresh.GiveFreeCount);
        }

        private void OnChanceDataChanged(object sender, GameEventArgs e)
        {
            ShowMoneyFreeTimes();
        }

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Event.Unsubscribe(EventId.ChanceDataChanged, OnChanceDataChanged);
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            for (int i = 0; i < (int)ChanceType.ChanceTypeCount; i++)
            {
                ChanceData chanceData = GameEntry.Data.Chances.GetChanceData((ChanceType)i + 1);
                if (!chanceData.Ready)
                {
                    m_NextFreeTime[i].text = string.Empty;
                }
                else
                {
                    var chanceRefresh = m_ChanceRefresh.GetDataRow(i + 1);
                    DateTime nextFreeTimes;
                    bool isFree = false;
                    if (i == (int)ChanceType.Money - 1)
                    {
                        nextFreeTimes = chanceData.FreeTime.AddSeconds(chanceRefresh.GiveFreeInterval);
                        isFree = nextFreeTimes - GameEntry.Time.LobbyServerUtcTime <= TimeSpan.Zero;
                    }
                    else
                    {
                        if (GameEntry.Data.Chances.GetChanceData(ChanceType.Coin).FreeChancedTimes < chanceRefresh.GiveFreeCount)
                        {
                            nextFreeTimes = chanceData.FreeTime.AddSeconds(chanceRefresh.UseFreeInterval);
                            isFree = nextFreeTimes - GameEntry.Time.LobbyServerUtcTime <= TimeSpan.Zero;
                        }
                        else
                        {
                            nextFreeTimes = chanceData.FreeTime.AddSeconds(chanceRefresh.GiveFreeInterval);
                            isFree = nextFreeTimes - GameEntry.Time.LobbyServerUtcTime <= TimeSpan.Zero;
                        }
                    }

                    TimeSpan nextFreeTime = nextFreeTimes - GameEntry.Time.LobbyServerUtcTime;

                    string nextFreeTimeText = GameEntry.Localization.GetString("UI_TEXT_HOUR_MINUTE_SECOND", (int)nextFreeTime.TotalHours, nextFreeTime.Minutes, (int)nextFreeTime.Seconds);
                    m_NextFreeTime[i].text = isFree ? GameEntry.Localization.GetString("UI_BUTTON_FREEGIVE") : GameEntry.Localization.GetString("UI_TEXT_NEXT_FREEGIVE", nextFreeTimeText);
                    m_NoticeObj[i].SetActive(!isFree);
                    if ((i == (int)ChanceType.Coin) && GameEntry.Data.Chances.GetChanceData(ChanceType.Coin).FreeChancedTimes >= chanceRefresh.GiveFreeCount)
                    {
                        m_NextFreeTime[i].text = "";
                        m_NoticeObj[i].SetActive(false);
                    }
                }
            }
        }

        public void OnClickChanceButton(UIIntKey intKey)
        {
            ChanceType chanceType = (ChanceType)intKey.Key;
            ChanceData chanceData = GameEntry.Data.Chances.GetChanceData(chanceType);
            if (!chanceData.Ready)
            {
                return;
            }

            GameEntry.UI.OpenUIForm(UIFormId.ChanceDetailForm, new ChanceDetailDisplayData { ChanceType = chanceType });
        }
    }
}
