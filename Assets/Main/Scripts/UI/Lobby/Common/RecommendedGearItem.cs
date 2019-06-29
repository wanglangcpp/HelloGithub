using UnityEngine;

namespace Genesis.GameClient
{
    public class RecommendedGearItem : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_GearIcon = null;

        private GearData m_Gear = null;
        private GearPacketPanel m_GearPacketPanel = null;

        public GearData GearInfo
        {
            get { return m_Gear; }
            private set { m_Gear = value; }
        }

        public void InitData(GearData gear, GearPacketPanel parent)
        {
            m_Gear = gear;
            m_GearPacketPanel = parent;
            m_GearIcon.LoadAsync(gear.IconId);
        }

        public void OnClickMe()
        {
            m_GearPacketPanel.OnSelectRecommendedGear(this);
        }
    }
}
