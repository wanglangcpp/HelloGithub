using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class ItemData : IGenericData<ItemData, PBItemInfo>
    {
        [SerializeField]
        private int m_Type;

        [SerializeField]
        private int m_Count;

        public int Key { get { return m_Type; } }
        public int Type { get { return m_Type; } }
        public int Count { set { m_Count = value; } get { return m_Count; } }
        public PBItemInfo ItemInfo { set; get; }

        public void UpdateData(PBItemInfo data)
        {
            if (data.Type <= 0)
            {
                Log.Error("Update item data with type ID '{0}'.", data.Type);
                return;
            }

            ItemInfo = data;
            m_Type = data.Type;
            m_Count = data.Count;
        }
    }
}
