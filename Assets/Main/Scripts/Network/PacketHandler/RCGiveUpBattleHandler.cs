﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class RCGiveUpBattleHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.RoomServerToClient, 5003); } }

        public override void Handle(object sender, Packet packet)
        {
            if (GameEntry.Data.Room.HasReconnected)
            {
                return;
            }
            GameEntry.SceneLogic.BaseInstanceLogic.SetInstanceFailure(InstanceFailureReason.AbandonedByUser, false, delegate () { GameEntry.SceneLogic.GoBackToLobby(); });
            base.Handle(sender, packet);
        }
    }
}
