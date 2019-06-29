﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class LCGetRecommendedPlayersHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 3102); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            LCGetRecommendedPlayers response = packet as LCGetRecommendedPlayers;

            GameEntry.Event.Fire(this, new GetRecommendedPlayersSuccessEventArgs(response));
        }
    }
}
