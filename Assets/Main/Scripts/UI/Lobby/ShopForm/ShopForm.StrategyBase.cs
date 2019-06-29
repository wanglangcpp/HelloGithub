using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ShopForm
    {
        private abstract class StrategyBase
        {
            protected ShopForm m_Form = null;

            protected abstract ShopScenario ShopType { get; }

            public abstract GameObject ShopItemTemplate { get; }

            public virtual void Init(ShopForm form)
            {
                m_Form = form;
                m_Form.RefreshLabel();
                m_Form.RefreshShopItems(ShopType);
                m_Form.RefreshNextAutoRefreshTime();
            }

            public virtual void ShutDown()
            {
                m_Form = null;
            }

            public abstract void OnShopDataChanged(object sender, GameEventArgs e);

            public abstract void OnPurchaseComplete(object sender, GameEventArgs e);

            public abstract void Buy(int index);

            public abstract void PerformManualRefresh();
        }
    }
}
