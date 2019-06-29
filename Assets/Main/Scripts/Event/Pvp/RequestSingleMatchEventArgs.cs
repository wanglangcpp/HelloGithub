using GameFramework.Event;


namespace Genesis.GameClient
{
    internal class RequestSingleMatchEventArgs : GameEventArgs
    {
        public RequestSingleMatchEventArgs()
        {

        }

        public override int Id
        {
            get { return (int)(EventId.RequestSingleMatch); }
        }
    }
}
