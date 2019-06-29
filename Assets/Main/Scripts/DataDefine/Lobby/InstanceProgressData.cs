using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class InstanceProgressData : IGenericData<InstanceProgressData, PBInstanceProgressInfo>
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private int m_StarCount;

        public int Key { get { return m_Id; } }
        public int Id { get { return m_Id; } }
        public int StarCount { get { return m_StarCount; } }

        public void UpdateData(PBInstanceProgressInfo data)
        {
            m_Id = data.Id;
            m_StarCount = data.StarCount;
        }
    }
}
