using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class InstanceDropData : IGenericData<InstanceDropData, PBDropInfo>
    {
        [SerializeField]
        private int m_DropId;

        [SerializeField]
        private int m_Count;

        public int Key { get { return m_DropId; } }
        public int DropId { get { return m_DropId; } }
        public int Count { get { return m_Count; } }

        public void UpdateData(PBDropInfo data)
        {
            m_DropId = data.DropId;

            m_Count = data.DropCount;
        }
    }
}
