using UnityEngine;
using System.Collections;
using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class GetChargeItemStatusEventArgs : GameEventArgs
    {

        private List<int> m_FirshChargeItems = new List<int>();
        /// <summary>
        /// 购买过的钻石
        /// </summary>
        public List<int> FirshChargeItems { get { return m_FirshChargeItems; } }

        //private List<int> m_HsaGetGifts = new List<int>();
        ///// <summary>
        ///// 领取过的礼包
        ///// </summary>
        //public List<int> HasGetGifts { get { return m_HsaGetGifts; } }

        //private List<int> m_NoGetGifts = new List<int>();
        ///// <summary>
        ///// 未领取的礼包
        ///// </summary>
        //public List<int> NoGetGifts { get { return m_NoGetGifts; } }

        //private List<int> m_DailyRestricts = new List<int>();
        ///// <summary>
        ///// 每日限购的礼包
        ///// </summary>
        //public List<int> DailyRestricts { get { return m_DailyRestricts; } }

        private Dictionary<int, int> m_GiftStatus = new Dictionary<int, int>();
        /// <summary>
        /// 所有的礼包状态
        /// </summary>
        public Dictionary<int, int> GiftStatus { get { return m_GiftStatus; } }

        public GetChargeItemStatusEventArgs()
        {
            //Packet = packet;
            UpDataAllDictionary();
        }
        public override int Id
        {
            get
            {
                return (int)EventId.GetItemStatus;
            }
        }
        public LCGetItemStatus Packet { get; private set; }

        /// <summary>
        /// 解析所有的充值物品的状态
        /// </summary>
        private void UpDataAllDictionary()
        {
            m_FirshChargeItems = GameEntry.Data.ChargeStatusData.StatusData.FirstChargeItems;
            //m_NoGetGifts = GameEntry.Data.ChargeStatusData.StatusData.NoGetGifts;
            //m_HsaGetGifts = GameEntry.Data.ChargeStatusData.StatusData.HasGetGifts;
            //m_DailyRestricts = GameEntry.Data.ChargeStatusData.StatusData.DailyRestricts;
            m_GiftStatus = GameEntry.Data.ChargeStatusData.StatusData.GiftStatus;
        }
    }
}

