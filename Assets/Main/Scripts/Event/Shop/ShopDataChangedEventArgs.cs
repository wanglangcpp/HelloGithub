using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 商店数据改变
    /// </summary>
    public class ShopDataChangedEventArgs : GameEventArgs
    {
        public ShopDataChangedEventArgs(int shopType)
        {
            ShopType = shopType;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ShopDataChanged;
            }
        }

        public int ShopType { get; set; }
    }
}
