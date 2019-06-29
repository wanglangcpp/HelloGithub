using GameFramework.Event;

namespace Genesis.GameClient
{
    internal class GetSinglePvpInfoEventArgs : GameEventArgs
    {
        public GetSinglePvpInfoEventArgs()
        {

        }

        public override int Id
        {
            get { return (int)(EventId.SinglePvpInfoChanged); }
        }
    }
}
