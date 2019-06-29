﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class LCRefreshRankListHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 3401); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            Handle(sender, packet as LCRefreshRankList);
        }

        public static void Handle(object sender, LCRefreshRankList response)
        {
            GameEntry.Event.Fire(sender, new GetRankListDataEventArgs(response));
        }
    }
}
