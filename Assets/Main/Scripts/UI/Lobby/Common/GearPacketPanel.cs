using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class GearPacketPanel : MonoBehaviour, IGearPacket
    {
        [SerializeField]
        private UILabel m_FilterButtonText = null;

        [SerializeField]
        private UILabel m_PacketText = null;

        [SerializeField]
        private UILabel m_RecommendationText = null;

        [SerializeField]
        private UITable m_GearPacket = null;

        [SerializeField]
        private UIScrollView m_GearPacketScrollView = null;

        [SerializeField]
        private GameObject m_GearInfoPannel = null;

        [SerializeField]
        private GameObject m_GearPacketItem = null;

        [SerializeField]
        private UIGrid m_RecommendationList = null;

        [SerializeField]
        private UIScrollView m_RecommendationScrollView = null;

        [SerializeField]
        private GameObject m_RecommendationItem = null;

        private GameObject m_GearInfoInstance = null;
        private int m_HeroId = 0;

        private void Awake()
        {
            m_FilterButtonText.text = GameEntry.Localization.GetString("UI_BUTTON_FILTER");
            m_PacketText.text = GameEntry.Localization.GetString("UI_TEXT_PACKETNAME");
            m_RecommendationText.text = GameEntry.Localization.GetString("UI_BUTTON_RECOMMENDATION");
        }

        public void RefreshData(int heroId, int position)
        {
            m_HeroId = heroId;
            if (m_GearInfoInstance != null)
            {
                Destroy(m_GearInfoInstance);
            }
            List<GearData> allGears = GameEntry.LobbyLogic.GetOneTypeOfGear(position);
            List<GearData> recommendedGear = GameEntry.LobbyLogic.GetRecommendedGear(allGears, heroId);
            ShowRecommendedGearList(recommendedGear);
            ShowGearList(allGears);
        }

        public void ShowGearList(List<GearData> gearList)
        {
            StartCoroutine(ClearAndShowGearsCo(gearList));
        }

        private IEnumerator ClearAndShowGearsCo(List<GearData> gearList)
        {
            foreach (var child in m_GearPacket.GetChildList())
            {
                Destroy(child.gameObject);
            }

            yield return null;

            for (int i = 0; i < gearList.Count; i++)
            {
                var go = NGUITools.AddChild(m_GearPacket.gameObject, m_GearPacketItem);
                var script = go.GetComponent<GearPacketItem>();
                script.InitData(gearList[i], this, 0);
            }
            m_GearPacket.Reposition();
            m_GearPacketScrollView.ResetPosition();
        }

        public void ShowRecommendedGearList(List<GearData> recommendedGearList)
        {
            foreach (var child in m_RecommendationList.GetChildList())
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < recommendedGearList.Count; i++)
            {
                var go = NGUITools.AddChild(m_RecommendationList.gameObject, m_RecommendationItem);
                var script = go.GetComponent<RecommendedGearItem>();
                script.InitData(recommendedGearList[i], this);
            }
            m_RecommendationList.Reposition();
            m_RecommendationScrollView.ResetPosition();
        }

        public void OnSelectGear(GearPacketItem gearItem)
        {
            foreach (var child in m_GearPacket.GetChildList())
            {
                child.gameObject.GetComponent<GearPacketItem>().UnSelected();
            }
            gearItem.Selected();
            ShowGearInfo(gearItem.GearInfo);
        }

        public void OnSelectRecommendedGear(RecommendedGearItem gearItem)
        {
            ShowGearInfo(gearItem.GearInfo);
        }

        private void ShowGearInfo(GearData gear)
        {
            GameObject go;
            if (m_GearInfoInstance == null)
            {
                go = NGUITools.AddChild(this.gameObject, m_GearInfoPannel);
                m_GearInfoInstance = go;
            }
            else
            {
                go = m_GearInfoInstance;
            }
            go.GetComponent<UIPanel>().depth = this.gameObject.GetComponent<UIPanel>().depth;
            var script = go.GetComponent<GearInfoPanel>();
            script.RefreshData(gear, true, false, m_HeroId, true);
            return;
        }
    }
}
