using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class InventoryForm
    {
        [Serializable]
        protected class DatailPanel
        {
            [SerializeField]
            public UISprite m_DetailIcon = null;

            [SerializeField]
            public UILabel m_DetailName = null;

            [SerializeField]
            public UILabel m_DetailLevel = null;

            [SerializeField]
            public UILabel m_DetailNumber = null;

            [SerializeField]
            public UILabel m_DetailType = null;

            [SerializeField]
            public GameObject m_DetailStarPanel = null;

            [SerializeField]
            public GameObject m_SoulEpigraphStarPanel = null;

            [SerializeField]
            public UISprite[] m_DetailStars = null;

            [SerializeField]
            public UISprite[] m_SoulEpigraphStars = null;

            [SerializeField]
            public UILabel m_DetailDescription = null;

            [SerializeField]
            public UIScrollView m_DetailScrollView = null;

            [SerializeField]
            public UIGrid m_DetailListView = null;

            [SerializeField]
            public UIButton m_DetailUpgradeButton = null;

            [SerializeField]
            public UIButton m_DetailStrengthenButton = null;

            [SerializeField]
            public UIButton m_DetailUseButton = null;

            [SerializeField]
            public UIButton m_DetailSaleButton = null;

            [SerializeField]
            public UIButton m_DetailToHeroButton = null;

            [SerializeField]
            public UIButton m_DetailExchangeButton = null;
        }

    }
}
