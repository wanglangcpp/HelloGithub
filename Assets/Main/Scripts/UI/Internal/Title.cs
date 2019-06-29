using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class Title : MonoBehaviour
    {
        [SerializeField]
        private int m_Offset = 0;

        [SerializeField]
        private int m_Margin = 0;

        [SerializeField]
        private Animation m_Animation = null;

        [SerializeField]
        private UIWidget m_Anchor = null;

        [SerializeField]
        private UISprite m_Background = null;

        [SerializeField]
        private UIButton m_BackButton = null;

        [SerializeField]
        private UIButton m_HelpButton = null;

        [SerializeField]
        private UILabel m_TitleText = null;

        [SerializeField]
        private Currency m_Money = null;

        [SerializeField]
        private Currency m_Coin = null;

        [SerializeField]
        private Currency m_Energy = null;

        [SerializeField]
        private Currency m_Sprite = null;

        [SerializeField]
        private Currency m_MeridianEnergy = null;

        [SerializeField]
        private Currency m_ArenaToken = null;

        [SerializeField]
        private Currency m_PvpToken = null;

        private Action m_OnClickBackButton = null;
        private Action m_OnClickHelpButton = null;

        public Animation Animation
        {
            get
            {
                return m_Animation;
            }
        }

        public UIWidget Anchor
        {
            get
            {
                return m_Anchor;
            }
        }

        public bool BackgroundVisible
        {
            get
            {
                return m_Background.enabled;
            }
            set
            {
                m_Background.enabled = value;
            }
        }

        public bool BackButtonVisible
        {
            get
            {
                return m_BackButton.gameObject.activeSelf;
            }
            set
            {
                m_BackButton.gameObject.SetActive(value);
            }
        }

        public bool HelpButtonVisible
        {
            get
            {
                return m_HelpButton.gameObject.activeSelf;
            }
            set
            {
                m_HelpButton.gameObject.SetActive(value);
            }
        }

        public bool TitleVisible
        {
            get
            {
                return m_TitleText.gameObject.activeSelf;
            }
            set
            {
                m_TitleText.gameObject.SetActive(value);
            }
        }

        public bool MoneyVisible
        {
            get
            {
                return m_Money.Panel.gameObject.activeSelf;
            }
            set
            {
                m_Money.Panel.gameObject.SetActive(value);
            }
        }

        public bool CoinVisible
        {
            get
            {
                return m_Coin.Panel.gameObject.activeSelf;
            }
            set
            {
                m_Coin.Panel.gameObject.SetActive(value);
            }
        }

        public bool EnergyVisible
        {
            get
            {
                return m_Energy.Panel.gameObject.activeSelf;
            }
            set
            {
                m_Energy.Panel.gameObject.SetActive(value);
            }
        }

        public bool SpriteVisible
        {
            get
            {
                return m_Sprite.Panel.gameObject.activeSelf;
            }
            set
            {
                m_Sprite.Panel.gameObject.SetActive(value);
            }
        }

        public bool MeridianEnergyVisible
        {
            get
            {
                return m_MeridianEnergy.Panel.gameObject.activeSelf;
            }
            set
            {
                m_MeridianEnergy.Panel.gameObject.SetActive(value);
            }
        }

        public bool ArenaTokenVisible
        {
            get
            {
                return m_ArenaToken.Panel.gameObject.activeSelf;
            }
            set
            {
                m_ArenaToken.Panel.gameObject.SetActive(value);
            }
        }

        public bool PvpTokenVisible
        {
            get
            {
                return m_PvpToken.Panel.gameObject.activeSelf;
            }
            set
            {
                m_PvpToken.Panel.gameObject.SetActive(value);
            }
        }

        public string TitleText
        {
            get
            {
                return m_TitleText.text;
            }
            set
            {
                m_TitleText.text = value;
            }
        }

        public void OnClickBackButton()
        {
            if (m_OnClickBackButton != null)
            {
                m_OnClickBackButton();
            }
        }

        public void OnClickHelpButton()
        {
            if (m_OnClickHelpButton != null)
            {
                m_OnClickHelpButton();
            }
        }

        public void OnClickAddMoneyButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChargeForm);
        }

        public void OnClickAddCoinButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.CostConfirmDialog, new CostConfirmDialogDisplayData { Mode = CostConfirmDialogType.Coin });
        }

        public void OnClickAddEnergyButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.CostConfirmDialog, new CostConfirmDialogDisplayData { Mode = CostConfirmDialogType.Energy });
        }

        public void OnClickAddSpriteButton()
        {

        }

        public void OnClickAddMeridianEnergyButton()
        {

        }

        public void OnClickAddArenaTokenButton()
        {

        }

        public void SetBackButtonAction(Action action)
        {
            m_OnClickBackButton = action;
        }

        public void SetHelpButtonAction(Action action)
        {
            m_OnClickHelpButton = action;
        }

        public void RefreshCurrencyPosition()
        {
            int count = 0;
            Currency[] currency = new Currency[] { m_Money, m_Coin, m_Energy, m_Sprite, m_MeridianEnergy, m_ArenaToken, m_PvpToken };
            Transform anchorTransform = m_Anchor.transform;
            for (int i = 0; i < currency.Length; i++)
            {
                if (currency[i].Panel.gameObject.activeSelf)
                {
                    currency[i].Panel.leftAnchor.target = anchorTransform;
                    currency[i].Panel.leftAnchor.relative = 1f;
                    currency[i].Panel.leftAnchor.absolute = -m_Offset - m_Margin * count;
                    currency[i].Panel.rightAnchor.target = anchorTransform;
                    currency[i].Panel.rightAnchor.relative = 1f;
                    currency[i].Panel.rightAnchor.absolute = -m_Offset - m_Margin * (count + 1);
                    count++;
                }
            }
        }

        private void OnEnable()
        {
            var uiRoot = GetComponentInParent<UIRoot>();
            m_Anchor.width = uiRoot.manualHeight * Screen.width / Screen.height + 4;
        }

        private void Start()
        {
            GameEntry.Event.Subscribe(EventId.PlayerDataChanged, OnPlayerDataChanged);
            RefreshCurrencyData();
            GameEntry.Event.Fire(EventId.LoadTitleSuccess, new LoadTitleSuccessEventArgs());
        }

        private void OnDestroy()
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.PlayerDataChanged, OnPlayerDataChanged);
            }
        }

        private void OnPlayerDataChanged(object sender, GameEventArgs e)
        {
            RefreshCurrencyData();
        }

        private void RefreshCurrencyData()
        {
            m_Money.NumberText = GameEntry.Data.Player.Money.ToString();
            m_Coin.NumberText = GameEntry.Data.Player.Coin.ToString();
            m_Energy.NumberText = GameEntry.Localization.GetString("UI_TEXT_SLASH", GameEntry.Data.Player.Energy.ToString(), Constant.PlayerMaxEnergy.ToString());
            m_Sprite.NumberText = GameEntry.Data.Player.Spirit.ToString();
            m_MeridianEnergy.NumberText = GameEntry.Data.Player.MeridianEnergy.ToString();
            m_ArenaToken.NumberText = GameEntry.Data.Player.ArenaToken.ToString();
            m_PvpToken.NumberText = GameEntry.Data.Player.PvpToken.ToString();
        }

        [Serializable]
        private class Currency
        {
            [SerializeField]
            private UIWidget m_Panel = null;

            [SerializeField]
            private UILabel m_NumberText = null;

            public UIWidget Panel
            {
                get
                {
                    return m_Panel;
                }
            }

            public string NumberText
            {
                get
                {
                    return m_NumberText.text;
                }
                set
                {
                    m_NumberText.text = value;
                }
            }
        }
    }
}
