namespace Genesis.GameClient
{
    public abstract class PacketHeadBase
    {
        public abstract PacketType PacketType
        {
            get;
        }

        public abstract int PacketActionId
        {
            get;
        }
    }
}
