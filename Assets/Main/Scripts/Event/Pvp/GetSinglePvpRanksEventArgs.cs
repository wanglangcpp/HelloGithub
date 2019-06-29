using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GetSinglePvpRanksEventArgs : GameEventArgs
    {
        public GetSinglePvpRanksEventArgs(LCGetSinlgePVPRanks packet)
        {
            Packet = packet;
        }

        public override int Id
        {
            get { return (int)EventId.GetSinglePvpRank; }
        }

        public LCGetSinlgePVPRanks Packet
        {
            get;
            private set;
        }
    }
}
