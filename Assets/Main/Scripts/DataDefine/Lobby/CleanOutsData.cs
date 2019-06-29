using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class CleanOutsData : GenericData<CleanOutData, PBInstanceDrop>
    {
        [SerializeField]
        private InstanceResultData m_ResultData = new InstanceResultData();

        [SerializeField]
        private int m_Count = 0;

        [SerializeField]
        private int m_Money = 0;

        public int Money
        {
            get
            {
                return m_Money;
            }
            set
            {
                m_Money = value;
            }
        }

        public int Count
        {
            get
            {
                return m_Count;
            }
            set
            {
                m_Count = value;
            }
        }

        public InstanceResultData HeroResultData
        {
            get { return m_ResultData; }
        }

        public void SetOldHeroInfo()
        {

        }

        public void SetNewHeroInfo()
        {

        }

    }
}
