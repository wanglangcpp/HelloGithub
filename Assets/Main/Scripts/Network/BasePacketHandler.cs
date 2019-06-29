using GameFramework.Network;

namespace Genesis.GameClient
{
    public abstract class BasePacketHandler : IPacketHandler
    {
        public abstract int OpCode { get; }

        public virtual void Handle(object sender, Packet packet)
        {
            PacketType packetType;
            int packetActionId;

            NetworkHelper.ParseOpCode(packet.Id, out packetType, out packetActionId);
            if (NetworkHelper.NeedWaiting(packetType, packetActionId))
            {
                GameEntry.Waiting.StopWaiting(WaitingType.Network, packetActionId.ToString());
            }
#if CSF
            NetLog.LogPackage(packet);
#endif
        }
    }
}
