using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChanceDataChangedEventArgs : GameEventArgs
    {
        public ChanceDataChangedEventArgs(int chanceType, ReceivedGeneralItemsViewData receiveGoodsData = null)
        {
            ChanceType = chanceType;
            ReceiveGoodsData = receiveGoodsData;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChanceDataChanged;
            }
        }

        public int ChanceType
        {
            get;
            private set;
        }

        public ReceivedGeneralItemsViewData ReceiveGoodsData
        {
            get;
            private set;
        }
    }
}
