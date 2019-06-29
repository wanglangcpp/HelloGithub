using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class ChancesData
    {
        [SerializeField]
        private ChanceData[] m_Chances = null;

        public ChancesData()
        {
            m_Chances = new ChanceData[(int)ChanceType.ChanceTypeCount];
        }

        public ChanceData GetChanceData(ChanceType chanceType)
        {
            int chanceTypeId = (int)chanceType - 1;
            if (chanceTypeId >= m_Chances.Length)
            {
                return null;
            }

            return m_Chances[chanceTypeId];
        }

        public void UpdateData(List<PBChanceInfo> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                m_Chances[i].UpdateData(datas[i]);
            }
        }

        public void UpdateData(LCOpenChance data)
        {
            int chanceTypeId = data.ChanceType - 1;
            if (chanceTypeId >= m_Chances.Length)
            {
                Log.Warning("Chance type '{0}' is invalid.", chanceTypeId.ToString());
                return;
            }

            m_Chances[chanceTypeId].UpdateData(data);
        }

        public void UpdateData(LCOpenAllChances data)
        {
            int chanceTypeId = data.ChanceType - 1;
            if (chanceTypeId >= m_Chances.Length)
            {
                Log.Warning("Chance type '{0}' is invalid.", chanceTypeId.ToString());
                return;
            }

            m_Chances[chanceTypeId].UpdateData(data);
        }

        public void UpdateData(LCRefreshChance data)
        {
            int chanceTypeId = data.ChanceInfo.ChanceType - 1;
            if (chanceTypeId >= m_Chances.Length)
            {
                Log.Warning("Chance type '{0}' is invalid.", chanceTypeId.ToString());
                return;
            }

            m_Chances[chanceTypeId].UpdateData(data);
        }
    }
}
