using UnityEngine;

namespace Genesis.GameClient
{
    public class DressedGearItem : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_GearIcon = null;

        [SerializeField]
        private UIButton m_GearBtn = null;

        [SerializeField]
        private GameObject m_Select = null;

        private GearData m_Gear = null;

        private int m_HeroId = 0;

        private bool m_IsMyHero = true;

        public bool IsSelect
        {
            get;
            set;
        }

        public GearData DressedGearData
        {
            get
            {
                return m_Gear;
            }
        }

        public int HeroId
        {
            get
            {
                return m_HeroId;
            }
        }

        public bool IsMyHero
        {
            get
            {
                return m_IsMyHero;
            }
        }

        public void SetSelect(bool isSelect)
        {
            IsSelect = isSelect;
            m_Select.SetActive(isSelect);
        }

        public void InitData(GearData gear, int heroId, bool isMyHero)
        {
            m_Select.SetActive(false);
            m_Gear = gear;
            m_HeroId = heroId;
            m_IsMyHero = isMyHero;
            m_GearIcon.LoadAsync(gear.IconId,
                 (sprite, spriteName, userData) => { m_GearBtn.normalSprite = spriteName; });
        }
    }
}
