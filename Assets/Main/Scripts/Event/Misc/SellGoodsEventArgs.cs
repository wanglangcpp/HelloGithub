using GameFramework.Event;

namespace Genesis.GameClient
{
    public class SellGoodsEventArgs : GameEventArgs
    {
        public SellGoodsEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.SellGoods;
            }
        }
    }
}
