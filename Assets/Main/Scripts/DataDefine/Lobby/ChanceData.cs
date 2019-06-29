using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class ChanceData
    {
        [SerializeField]
        private ItemsData m_GoodsForView = null;

        [SerializeField]
        private List<int> m_DummyIndex = null;

        [SerializeField]
        private long m_FreeTime;

        [SerializeField]
        private TimeSpan m_NextRefreshTime;

        [SerializeField]
        private int m_FreeChancedTimes;

        [SerializeField]
        private List<DRChanceCost> m_ChanceCost = new List<DRChanceCost>();

        public List<DRChanceCost> ChanceCost { get { return m_ChanceCost; } }
        public bool Ready { get { return m_GoodsForView.Data.Count > 0; } }
        public ItemsData GoodsForView { get { return m_GoodsForView; } }
        public List<int> DummyIndex { get { return m_DummyIndex; } }
        public DateTime FreeTime { get { return new DateTime(m_FreeTime); } }
        public TimeSpan NextRefreshTime { get { return m_NextRefreshTime; } }
        public int FreeChancedTimes { get { return m_FreeChancedTimes; } }

        public int ChancedCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < m_DummyIndex.Count; i++)
                {
                    if (m_DummyIndex.IndexOf(i) >= 0)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public void UpdateData(PBChanceInfo data)
        {
            m_DummyIndex.Clear();
            m_GoodsForView.ClearData();

            if (m_ChanceCost.Count == 0)
            {
                var chanceCost = GameEntry.DataTable.GetDataTable<DRChanceCost>().GetAllDataRows();
                for (int i = 0; i < chanceCost.Length; i++)
                {
                    if (data.ChanceType == (int)ChanceType.Money && i < 10)
                    {
                        m_ChanceCost.Add(chanceCost[i]);
                    }
                    if (data.ChanceType == (int)ChanceType.Coin && i > 9)
                    {
                        m_ChanceCost.Add(chanceCost[i]);
                    }
                }
            }

            var drChanceRefresh = GameEntry.DataTable.GetDataTable<DRChanceRefresh>();
            var chanceRefresh = drChanceRefresh.GetDataRow((int)ChanceType.Coin);

            for (int i = 0; i < data.ChanceItems.Count; i++)
            {
                m_GoodsForView.AddData(data.ChanceItems[i].ItemInfo);
                m_DummyIndex.Add(data.ChanceItems[i].DummyIndex);
                m_FreeChancedTimes = data.UsedFreeCount;
                m_FreeTime = data.FreeTime;
                m_NextRefreshTime = TimeSpan.Parse(chanceRefresh.RefreshUtcTime);
            }
        }

        public void UpdateData(LCOpenChance data)
        {
            m_DummyIndex[data.OpenedRealIndex] = data.OpenedChanceItem.DummyIndex;
            m_FreeChancedTimes = data.UsedFreeCount;
            m_FreeTime = data.FreeTime;
        }

        public void UpdateData(LCOpenAllChances data)
        {
            m_DummyIndex.Clear();
            m_DummyIndex.AddRange(data.OpenedRealIndices);
        }

        public void UpdateData(LCRefreshChance data)
        {
            m_GoodsForView.ClearData();
            m_DummyIndex.Clear();
            for (int i = 0; i < data.ChanceInfo.ChanceItems.Count; i++)
            {
                m_DummyIndex.Add(data.ChanceInfo.ChanceItems[i].DummyIndex);
                m_GoodsForView.AddData(data.ChanceInfo.ChanceItems[i].ItemInfo);
            }
        }
    }
}
