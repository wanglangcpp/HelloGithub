using GameFramework;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class ChargeStatusData : IGenericData<ChargeStatusData, ChargeStatus>
    {
        public int Key
        {
            get { return m_Id; }
        }
        private int m_Id;
        private ChargeStatus data = null;
        public ChargeStatus StatusData { get { return data; } }
        public void UpdateData(ChargeStatus data)
        {
            this.data = data;
            m_Id = data.Id;
        }
    }
    public class ChargeStatus
    {
        public int Id;
        public List<int> FirstChargeItems = new List<int>();

        //public List<int> HasGetGifts = new List<int>();

        //public List<int> NoGetGifts = new List<int>();

        //public List<int> DailyRestricts = new List<int>();

        public Dictionary<int, int> GiftStatus = new Dictionary<int, int>();

        public Dictionary<int, int> MonthCardTime = new Dictionary<int, int>();

    }
}

