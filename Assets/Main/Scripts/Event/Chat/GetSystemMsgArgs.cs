using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GetSystemMsgArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.GetSystemMsg;
            }
        }
        public GetSystemMsgArgs(LCSystemNotify sender)
        {
            Sender = sender;
        }
        public LCSystemNotify Sender { get; private set; }
    }
}
