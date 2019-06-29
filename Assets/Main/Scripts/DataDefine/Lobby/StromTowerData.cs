using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class StromTowerData : IGenericData<StromTowerData, PBInstanceForTowerInfo>
    {
        public int Key
        {
            get { return m_Id; }
        }
        private int m_Id;
        /// <summary>
        /// 当前挑战信息
        /// </summary>
        public PBInstanceForTowerInfo StromTowerInfo { get { return m_StromTowerInfo; } }
        private PBInstanceForTowerInfo m_StromTowerInfo = null;

        public void UpdateData(PBInstanceForTowerInfo data)
        {
            m_StromTowerInfo = data;
        }
    }
}
