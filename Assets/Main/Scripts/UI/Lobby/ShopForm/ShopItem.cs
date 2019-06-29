using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 商店中的商品项。
    /// </summary>
    public class ShopItem : MonoBehaviour
    {
        [SerializeField]
        private GeneralItemView m_ItemView = null;

        [SerializeField]
        private UIButton m_BuyButton = null;

        [SerializeField]
        private UISprite[] m_CurrencyIcons = null;

        [SerializeField]
        private UILabel m_Price = null;

        [SerializeField]
        private UILabel m_NameLabel = null;

        [SerializeField]
        private GameObject m_HasBuyedObj = null;

        [SerializeField]
        private float m_LeftAnchors = 0;

        [SerializeField]
        private float m_RightAnchor = 0;

        [SerializeField]
        private UIRect m_BuyBtnRect = null;

        private ShopItemData m_CachedData = null;
        private ShopForm m_CachedShopForm = null;
        private int m_Index = 0;

        public ShopItemData CachedData
        {
            get
            {
                return m_CachedData;
            }
        }

        /// <summary>
        /// 刷新数据。
        /// </summary>
        /// <param name="shopForm">商店界面。</param>
        /// <param name="data">商品项数据。</param>
        public void RefreshData(ShopForm shopForm, ShopItemData data, int index)
        {
            m_Index = index;
            m_CachedShopForm = shopForm;
            m_CachedData = data;
            var itemData = data.ItemInfo;
            m_HasBuyedObj.SetActive(!data.CanBuy);
            m_BuyButton.isEnabled = data.CanBuy;
            m_ItemView.InitGeneralItem(itemData.Key, itemData.Count);
            m_Price.text = data.CurrencyPrice.ToString();
            for (int i = 0; i < m_CurrencyIcons.Length; i++)
            {
                if (m_CurrencyIcons[i] != null)
                {
                    m_CurrencyIcons[i].gameObject.SetActive((int)data.CurrencyCategory == i);
                }
            }

            m_NameLabel.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(itemData.Key));
        }

        public void SetBuyBtnAnchor(GameObject anchorTarget)
        {
            m_BuyBtnRect.leftAnchor.Set(anchorTarget.transform, 0, m_LeftAnchors);
            m_BuyBtnRect.rightAnchor.Set(anchorTarget.transform, 1, m_RightAnchor);
        }

        private void OnLoadSpriteSuccess(UISprite sprite, string spriteName, object userData)
        {
            if (sprite == null)
            {
                return;
            }

            var button = sprite.GetComponent<UIButton>();
            if (button == null)
            {
                return;
            }

            button.normalSprite = sprite.spriteName;
        }

        // Called by NGUI via reflection.
        public void OnClickBuyButton()
        {
            if (!UIUtility.CheckCurrency(m_CachedData.CurrencyCategory, m_CachedData.CurrencyPrice))
            {
                return;
            }

            m_CachedShopForm.Buy(m_Index);
        }
    }
}
