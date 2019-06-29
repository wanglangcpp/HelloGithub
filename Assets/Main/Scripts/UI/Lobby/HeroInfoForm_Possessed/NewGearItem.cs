using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class NewGearItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_SelectObj = null;

        [SerializeField]
        private GeneralItemView m_NewGearItem = null;

        public bool IsSelect
        {
            set
            {
                m_SelectObj.SetActive(value);
            }
        }

        public void RefreshNewGear(int gearId, QualityType quality)
        {
            m_NewGearItem.InitNewGear(gearId, quality);
        }
    }
}
