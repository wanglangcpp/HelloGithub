using UnityEngine;

namespace Genesis.GameClient
{
    public class GearPacketItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_GearLevelText = null;

        [SerializeField]
        private UILabel m_GearNameText = null;

        [SerializeField]
        private GameObject[] m_GearStars = null;

        [SerializeField]
        private GameObject m_SelectedIcon = null;

        [SerializeField]
        private GameObject m_MultiSelectIcon = null;

        [SerializeField]
        private UILabel m_MultiSelectText = null;

        //         [SerializeField]
        //         private GameObject m_HeroBG = null;
        [SerializeField]
        private UISprite m_GearIcon = null;

        [SerializeField]
        private GameObject m_HeroIcon = null;

        private GearData m_Gear = null;
        private IGearPacket m_ParentPanel = null;

        public GearData GearInfo
        {
            get { return m_Gear; }
            private set { m_Gear = value; }
        }

        public bool IsMultiSelected
        {
            get { return m_MultiSelectIcon.activeSelf; }
        }

        public void InitData(GearData gear, IGearPacket parent, int heroId)
        {
            m_Gear = gear;
            m_ParentPanel = parent;
            m_GearIcon.LoadAsync(gear.IconId);
            m_GearNameText.text = GameEntry.Localization.GetString(gear.Name);
            m_GearNameText.color = ColorUtility.GetColorForQuality(gear.Quality);
            m_GearLevelText.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", gear.Level);
            for (int i = 0; i < gear.StrengthenLevel; i++)
            {
                m_GearStars[i].SetActive(true);
            }
            if (heroId > 0)
            {
                m_HeroIcon.SetActive(true);
            }
        }

        public void UnSelected()
        {
            m_SelectedIcon.SetActive(false);
        }

        public void Selected()
        {
            m_SelectedIcon.SetActive(true);
        }

        public void MultiSelected(int index)
        {
            m_MultiSelectIcon.SetActive(true);
            m_MultiSelectText.text = index.ToString();
        }

        public void UnMultiSelected()
        {
            m_MultiSelectIcon.SetActive(false);
        }

        public void OnClickMe()
        {
            m_ParentPanel.OnSelectGear(this);
        }
    }
}
