using GameFramework.Event;

namespace Genesis.GameClient
{
    public class EntitySkillRushingFromNetworkEventArgs : GameEventArgs
    {
        public EntitySkillRushingFromNetworkEventArgs(RCPushEntitySkillRushing packet)
        {
            Packet = packet;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.EntitySkillRushingFromNetwork;
            }
        }

        public RCPushEntitySkillRushing Packet { get; private set; }
    }
}
