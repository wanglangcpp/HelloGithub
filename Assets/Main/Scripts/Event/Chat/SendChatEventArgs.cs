using GameFramework.Event;

namespace Genesis.GameClient
{
    public class SendChatEventArgs : GameEventArgs
    {
        public SendChatEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.SendChat;
            }
        }
    }
}
