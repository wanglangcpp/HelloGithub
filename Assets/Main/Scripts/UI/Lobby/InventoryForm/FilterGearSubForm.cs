using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class FilterGearSubForm : NGUISubForm
    {
        private enum FilterGearType
        {
            WhiteType = 0,
            GreenType = 1,
            BlueType = 2,
            PurpleType = 3,
            OrengeType = 4,
            RedType = 5,
            DressedType = 6,
            UndressedType = 7,
            WeaponType = 8,
            HeadType = 9,
            ClothType = 10,
            ShoeType = 11,
            RingType = 12,
            NeckLaceType = 13,
            FilterGearTypeCount,
        }

        [SerializeField]
        private GameObject[] m_FilterSelectIcons = null;

        [SerializeField]
        private UILabel[] m_QuilityLabels = null;

        public delegate int FilterGearsHandler(List<GearDataWithHero> gears);

        private FilterGearsHandler m_SelectFilterReturn = null;

        private List<GearDataWithHero> m_FilterGearsData = new List<GearDataWithHero>();

        private bool m_HidePosition = false;

        public void RefreshFilter(List<GearDataWithHero> gearsData, FilterGearsHandler selectFilterReturn, bool isAll, bool hidePosition = false)
        {
            m_HidePosition = hidePosition;
            m_SelectFilterReturn = selectFilterReturn;
            for (int i = 0; i < m_QuilityLabels.Length; i++)
            {
                m_QuilityLabels[i].color = ColorUtility.GetColorForQuality(i + 1);
            }
            m_FilterGearsData = new List<GearDataWithHero>(gearsData);
            if (isAll)
            {
                for (int i = 0; i < m_FilterSelectIcons.Length; i++)
                {
                    m_FilterSelectIcons[i].SetActive(true);
                }
            }
        }

        public void OnClickWholeScreenButton()
        {
            SetFilterSelects();
            if (m_SelectFilterReturn != null)
            {
                m_SelectFilterReturn(m_FilterGearsData);
            }
        }

        public void OnClickFilterButton(int type)
        {
            m_FilterSelectIcons[type].SetActive(!m_FilterSelectIcons[type].activeSelf);
        }

        private void SetFilterSelects()
        {
            if (m_FilterGearsData == null || m_FilterGearsData.Count == 0)
            {
                return;
            }
            for (int j = 0; j < m_FilterGearsData.Count;)
            {
                if (m_FilterGearsData[j].HeroType > 0)
                {
                    if (!m_FilterSelectIcons[(int)FilterGearType.DressedType].activeSelf)
                    {
                        m_FilterGearsData.Remove(m_FilterGearsData[j]);
                    }
                    else
                    {
                        j++;
                    }
                }
                else
                {
                    if (!m_FilterSelectIcons[(int)FilterGearType.UndressedType].activeSelf)
                    {
                        m_FilterGearsData.Remove(m_FilterGearsData[j]);
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            for (int i = 0; i <= (int)FilterGearType.RedType; i++)
            {
                if (!m_FilterSelectIcons[i].activeSelf)
                {
                    for (int j = 0; j < m_FilterGearsData.Count;)
                    {
                        if (m_FilterGearsData[j].GearData.Quality == i + 1)
                        {
                            m_FilterGearsData.Remove(m_FilterGearsData[j]);
                        }
                        else
                        {
                            j++;
                        }
                    }
                }
            }

            for (int i = (int)FilterGearType.WeaponType; (i < (int)FilterGearType.FilterGearTypeCount) && !m_HidePosition; i++)
            {
                if (!m_FilterSelectIcons[i].activeSelf)
                {
                    for (int j = 0; j < m_FilterGearsData.Count;)
                    {
                        if (m_FilterGearsData[j].GearData.Position == (i - (int)FilterGearType.UndressedType))
                        {
                            m_FilterGearsData.Remove(m_FilterGearsData[j]);
                        }
                        else
                        {
                            j++;
                        }
                    }
                }
            }
        }
    }
}
