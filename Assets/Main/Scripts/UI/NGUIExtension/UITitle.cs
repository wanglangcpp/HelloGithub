using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    [RequireComponent(typeof(UIAnimation))]
    public class UITitle : MonoBehaviour
    {
        [SerializeField]
        private bool m_ShowBackground = true;

        [SerializeField]
        private bool m_ShowBackButton = true;

        [SerializeField]
        private bool m_ShowHelpButton = false;

        [SerializeField]
        private bool m_ShowTitle = true;

        [SerializeField]
        private string m_Title = null;

        [SerializeField]
        private bool m_ShowMoney = true;

        [SerializeField]
        private bool m_ShowCoin = true;

        [SerializeField]
        private bool m_ShowEnergy = true;

        [SerializeField]
        private bool m_ShowSprite = false;

        [SerializeField]
        private bool m_ShowMeridianEnergy = false;

        [SerializeField]
        private bool m_ShowArenaToken = false;

        [SerializeField]
        private bool m_ShowPvpToken = false;

        private NGUIForm m_CachedUIForm = null;
        private Title m_CachedTitle = null;

        public void SetTitle(string title)
        {
            m_Title = title;
            m_CachedTitle.TitleText = GameEntry.Localization.GetString(title);
        }

        private void Awake()
        {
            m_CachedUIForm = GetComponent<NGUIForm>();
            if (m_CachedUIForm == null)
            {
                Log.Error("UI form is invalid.");
                return;
            }

            m_CachedTitle = NGUITools.AddChild(gameObject, GameEntry.UIBackground.TitleTemplate).GetComponent<Title>();

            if (m_CachedTitle.Animation != null)
            {
                UIAnimation uiAnimation = GetComponent<UIAnimation>();
                if (uiAnimation != null)
                {
                    uiAnimation.AddOpenAnimation(m_CachedTitle.Animation);
                    uiAnimation.AddCloseAnimation(m_CachedTitle.Animation, true);
                }
            }
        }

        private void Start()
        {
            m_CachedTitle.gameObject.SetActive(true);

            m_CachedTitle.BackgroundVisible = m_ShowBackground;
            m_CachedTitle.BackButtonVisible = m_ShowBackButton;
            m_CachedTitle.HelpButtonVisible = m_ShowHelpButton;
            m_CachedTitle.TitleVisible = m_ShowTitle;
            if (!string.IsNullOrEmpty(m_Title))
            {
                SetTitle(m_Title);
            }

            m_CachedTitle.MoneyVisible = m_ShowMoney;
            m_CachedTitle.CoinVisible = m_ShowCoin;
            m_CachedTitle.EnergyVisible = m_ShowEnergy;
            m_CachedTitle.SpriteVisible = m_ShowSprite;
            m_CachedTitle.MeridianEnergyVisible = m_ShowMeridianEnergy;
            m_CachedTitle.ArenaTokenVisible = m_ShowArenaToken;
            m_CachedTitle.PvpTokenVisible = m_ShowPvpToken;
            m_CachedTitle.RefreshCurrencyPosition();

            m_CachedTitle.SetBackButtonAction(m_CachedUIForm.OnClickBackButton);
        }

        private void OnDestroy()
        {
            if (m_CachedTitle != null)
            {
                Destroy(m_CachedTitle);
                m_CachedTitle = null;
            }
        }

        public bool ShowArenaToken
        {
            set
            {
                m_ShowArenaToken = value;
                if (m_CachedTitle != null)
                {

                    m_CachedTitle.ArenaTokenVisible = value;
                    m_CachedTitle.RefreshCurrencyPosition();
                }
            }
        }
    }
}
