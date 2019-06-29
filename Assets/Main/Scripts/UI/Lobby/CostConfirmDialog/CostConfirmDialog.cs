using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CostConfirmDialog : NGUIForm
    {
        [SerializeField]
        private UILabel m_Title = null;

        [SerializeField]
        private UILabel m_PreMessage = null;

        [SerializeField]
        private UILabel m_PostMessage = null;

        [SerializeField]
        private UISprite m_ButtonCurrencyIcon = null;

        [SerializeField]
        private UILabel m_ButtonCurrencyCount = null;

        [SerializeField]
        private UILabel m_ItemCount = null;

        [SerializeField]
        private UISprite m_ItemIcon = null;

        private StrategyBase m_Strategy = null;
        private CostConfirmDialogDisplayData m_Data = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.PlayerDataChanged, OnPlayerDataChanged);
            m_Data = userData as CostConfirmDialogDisplayData;
            InitStrategy();
            RefreshData();
        }

        protected override void OnClose(object userData)
        {
            m_Data = null;
            DeinitStrategy();
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.PlayerDataChanged, OnPlayerDataChanged);
            }
            base.OnClose(userData);
        }

        public override void OnClickBackButton()
        {
            base.OnClickBackButton();
            if (m_Data.OnClickCancel != null)
            {
                m_Data.OnClickCancel(m_Data.UserData);
            }
        }

        private void OnPlayerDataChanged(object sender, GameEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            m_Strategy.RefreshData();
        }

        private void InitStrategy()
        {
            m_Strategy = CreateStrategy(m_Data.Mode);
            m_Strategy.Init(this);
        }

        private void DeinitStrategy()
        {
            m_Strategy.Shutdown();
            m_Strategy = null;
        }

        public void OnClickBuyButton()
        {
            m_Strategy.OnClickBuyButton();
        }
    }
}
