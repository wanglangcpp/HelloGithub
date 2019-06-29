using GameFramework.Event;
using Genesis.GameClient;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class ReceivePayItemEventArgs : GameEventArgs
    {
        public ReceivePayItemEventArgs(ReceivedGeneralItemsViewData listItemInfo)
        {
            CompoundItemInfo = listItemInfo;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ReceiveBuyItem;
            }
        }

        public ReceivedGeneralItemsViewData CompoundItemInfo
        {
            get;
            private set;
        }

    }
}
