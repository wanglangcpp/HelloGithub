using GameFramework.Event;

namespace Genesis.GameClient
{
    public class EntitySkillFFFromNetworkEventArgs : GameEventArgs
    {
        public EntitySkillFFFromNetworkEventArgs(RCPushEntityPerformSkillFF packet)
        {
            Packet = packet;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.EntitySkillFFFromNetwork;
            }
        }

        public RCPushEntityPerformSkillFF Packet { get; private set; }
    }
}
