using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 购买商店商品
    /// </summary>
    public class PurchaseInShopEventArgs : GameEventArgs
    {
        public PurchaseInShopEventArgs(int shopType, ReceivedGeneralItemsViewData itemViewData)
        {
            ShopType = shopType;
            ItemViewData = itemViewData;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.PurchaseInShop;
            }
        }

        public int ShopType { get; set; }

        public ReceivedGeneralItemsViewData ItemViewData { get; set; }
    }
}
