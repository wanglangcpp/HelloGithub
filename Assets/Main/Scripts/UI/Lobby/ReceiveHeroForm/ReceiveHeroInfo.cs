using UnityEngine;

namespace Genesis.GameClient
{
    public class ReceiveHeroInfo : MonoBehaviour
    {
        [SerializeField]
        private GoodsView m_Element = null;

        [SerializeField]
        private UILabel m_Lv = null;

        [SerializeField]
        private UILabel m_HeroName = null;

        [SerializeField]
        private UILabel m_HeroChip = null;

        [SerializeField]
        private UISprite[] m_Stars = null;

        public void InitReceiveHeroInfo(DRHero heroInfo, int chipCount)
        {
            m_Element.InitElementView(heroInfo.ElementId);
            m_Lv.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", 1);
            m_HeroName.text = GameEntry.Localization.GetString(heroInfo.Name);
            m_HeroChip.gameObject.SetActive(chipCount > 1);
            if (chipCount > 1)
            {
                m_HeroChip.text = GameEntry.Localization.GetString("UI_TEXT_RECEIVEHERO_HEROTOPIECES", chipCount);
            }
            UIUtility.SetStarLevel(m_Stars, heroInfo.DefaultStarLevel);
        }
    }
}
