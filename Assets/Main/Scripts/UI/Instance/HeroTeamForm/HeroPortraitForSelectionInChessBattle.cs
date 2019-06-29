using UnityEngine;

namespace Genesis.GameClient
{
    public class HeroPortraitForSelectionInChessBattle : HeroPortraitForSelection
    {
        [SerializeField]
        private UISprite m_CurHP = null;

        [SerializeField]
        private Color m_UninteractableColor = Color.white;

        public float CurHPRatio
        {
            get
            {
                return m_CurHP.fillAmount;
            }

            set
            {
                m_CurHP.fillAmount = value;
                bool interactable = m_CurHP.fillAmount > 0;

                for (int i = 0; i < CachedWidgets.Length; ++i)
                {
                    CachedWidgets[i].color = interactable ? Color.white : m_UninteractableColor;
                }
            }
        }

        protected override void OnClickButton()
        {
            if (CurHPRatio <= 0f)
            {
                return;
            }

            base.OnClickButton();
        }
    }
}
