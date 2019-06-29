using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class InstanceGoodsData
    {
        [SerializeField]
        private List<ItemData> m_ItemData = new List<ItemData>();

        [SerializeField]
        private List<SoulData> m_SoulData = new List<SoulData>();

        public List<SoulData> InstanceSoulData
        {
            get { return m_SoulData; }
        }

        public List<ItemData> InstanceItemData
        {
            get { return m_ItemData; }
        }

        public int GoodsCount
        {
            get { return m_SoulData.Count + m_ItemData.Count; }
        }

        public int GetSameSoulCount(int type)
        {
            int count = 0;
            for (int i = 0; i < m_SoulData.Count; i++)
            {
                if (m_SoulData[i].Type == type)
                {
                    count++;
                }
            }
            return count;
        }

        public void ClearAndAddData(List<PBSoulInfo> datas)
        {
            m_SoulData.Clear();

            for (int i = 0; i < datas.Count; i++)
            {
                SoulData data = new SoulData();
                data.UpdateData(datas[i]);
                m_SoulData.Add(data);
            }
        }

        public void ClearAndAddData(List<PBItemInfo> datas)
        {
            m_ItemData.Clear();

            for (int i = 0; i < datas.Count; i++)
            {
                ItemData data = new ItemData();
                data.UpdateData(datas[i]);
                m_ItemData.Add(data);
            }
        }
    }
}
