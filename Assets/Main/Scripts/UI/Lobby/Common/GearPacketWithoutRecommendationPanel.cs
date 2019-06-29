using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class GearPacketWithoutRecommendationPanel : MonoBehaviour, IGearPacket
    {
        [SerializeField]
        private GameObject m_GearPacketItem = null;

        [SerializeField]
        private UITable m_GearPacket = null;

        [SerializeField]
        private UIScrollView m_GearPacketScrollView = null;

        [SerializeField]
        private UILabel m_FilterButtonText = null;

        private bool m_IsInPacket;
        private bool m_IsMultiChoice;
        private Predicate<GearData> m_Filter = null;
        private GearData[] m_ChosenGears = new GearData[Constant.GearComposeMaterialCount];
        private List<GearPacketItem> m_Gears = new List<GearPacketItem>();
        private int m_SlotIndex = 0;

        public void Init(Dictionary<string, object> userDict)
        {
            RefreshData(userDict);
            ShowGearPacket(userDict);
        }

        private void ShowGearPacket(Dictionary<string, object> userDict)
        {
            if (m_IsInPacket)
            {
                var gears = GameEntry.Data.Gears.Data;
                gears.Sort(Comparer.GearDataComparer);
                foreach (GearData gear in gears)
                {
                    AddGearItem(gear, 0);
                }
            }
            else
            {
                List<GearDataWithHero> data = GeneralItemUtility.GetAllGearData();
                data.Sort(Comparer.GearDataWithHeroComparer);
                foreach (var heroGear in data)
                {
                    AddGearItem(heroGear.GearData, heroGear.HeroType);
                }
            }

            m_GearPacket.Reposition();
            m_GearPacketScrollView.ResetPosition();
            m_GearPacketScrollView.enabled = (m_GearPacketItem.GetComponent<UISprite>().height * m_GearPacket.GetChildList().Count >= m_GearPacketScrollView.GetComponent<UIPanel>().height);
        }

        public void OnSelectGear(GearPacketItem gearItem)
        {
            if (m_IsMultiChoice)
            {
                if (gearItem.IsMultiSelected)
                {
                    m_Gears.Remove(gearItem);
                    gearItem.UnMultiSelected();
                    for (int i = 0; i < m_Gears.Count; i++)
                    {
                        m_Gears[i].MultiSelected(i + 1);
                    }
                }
                else
                {
                    if (m_Gears.Count == Constant.GearComposeMaterialCount)
                    {
                        Destroy(this.gameObject);
                    }
                    m_Gears.Add(gearItem);
                    gearItem.MultiSelected(m_Gears.Count);
                    if (m_Gears.Count == Constant.GearComposeMaterialCount)
                    {
                        List<GearData> eventArg = new List<GearData>();
                        for (int i = 0; i < Constant.GearComposeMaterialCount; i++)
                        {
                            eventArg.Add(m_Gears[i].GearInfo);
                        }
                        GameEntry.Event.Fire(this, new GearMultiSelectedEventArgs(eventArg));
                        Destroy(this.gameObject);
                    }
                }
            }
            else
            {
                GameEntry.Event.Fire(this, new GearSelectedEventArgs(gearItem.GearInfo, m_SlotIndex));
                Destroy(this.gameObject);
            }
        }

        public void OnClickWholeScreenButton(Transform transformToDeactivate)
        {
            Destroy(transformToDeactivate.gameObject);
        }

        private void RefreshData(Dictionary<string, object> userData)
        {
            m_FilterButtonText.text = GameEntry.Localization.GetString("UI_BUTTON_FILTER");
            m_IsInPacket = (bool)userData[Constant.UI.IsInPacket];
            m_IsMultiChoice = (bool)userData[Constant.UI.IsMultiChoice];
            if (userData.ContainsKey(Constant.UI.GearFilter) && userData[Constant.UI.GearFilter] != null)
            {
                m_Filter = userData[Constant.UI.GearFilter] as Predicate<GearData>;
            }
            if (userData.ContainsKey(Constant.UI.ChosenGears) && userData[Constant.UI.ChosenGears] != null)
            {
                m_ChosenGears = userData[Constant.UI.ChosenGears] as GearData[];
            }
            if (userData.ContainsKey(Constant.UI.SlotIndex) && userData[Constant.UI.SlotIndex] != null)
            {
                m_SlotIndex = (int)userData[Constant.UI.SlotIndex];
            }
        }

        private void AddGearItem(GearData gear, int heroId = 0)
        {
            if (m_Filter == null || m_Filter(gear))
            {
                var go = NGUITools.AddChild(m_GearPacket.gameObject, m_GearPacketItem);
                var script = go.GetComponent<GearPacketItem>();
                script.InitData(gear, this, heroId);
                var gearItem = script;//AddGearItem(gear, heros[i].Type);
                if (m_IsMultiChoice)
                {
                    for (int j = 0; j < Constant.GearComposeMaterialCount; j++)
                    {
                        if (m_ChosenGears[j] != null && gearItem.GearInfo.Id == m_ChosenGears[j].Id)
                        {
                            m_Gears.Add(gearItem);
                            gearItem.MultiSelected(j + 1);
                        }
                    }
                }
            }
        }
    }
}
