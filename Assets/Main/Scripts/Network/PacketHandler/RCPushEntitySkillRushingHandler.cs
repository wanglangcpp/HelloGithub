﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class RCPushEntitySkillRushingHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.RoomServerToClient, 4107); } }

        public override void Handle(object sender, Packet packet)
        {
            if (GameEntry.Data.Room.HasReconnected)
            {
                return;
            }
            base.Handle(sender, packet);
            RCPushEntitySkillRushing response = packet as RCPushEntitySkillRushing;

            if (GameEntry.SceneLogic.SinglePvpInstanceLogic.PlayerIsMe(response.PlayerId))
            {
                return;
            }

            GameEntry.Event.Fire(this, new EntitySkillRushingFromNetworkEventArgs(response));
        }
    }
}
