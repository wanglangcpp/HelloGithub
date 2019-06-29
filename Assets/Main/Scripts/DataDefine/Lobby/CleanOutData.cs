using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class CleanOutData : IGenericData<CleanOutData, PBInstanceDrop>
    {
        [SerializeField]
        private InstanceDropsData m_Drops = new InstanceDropsData();

        public int Key { get { return 0; } }

        public InstanceDropsData Drops
        {
            get
            {
                return m_Drops;
            }
        }

        public void UpdateData(PBInstanceDrop data)
        {
            m_Drops.ClearAndAddData(data.Items);
        }
    }
}
