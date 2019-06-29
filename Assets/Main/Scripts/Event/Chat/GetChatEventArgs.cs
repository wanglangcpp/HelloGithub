using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GetChatEventArgs : GameEventArgs
    {
        public GetChatEventArgs(LCReceiveChat msg)
        {
            Msg = msg;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.GetChat;
            }
        }

        public LCReceiveChat Msg { get; set; }
    }
}
