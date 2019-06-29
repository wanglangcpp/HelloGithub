﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class LCLoginServerHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 1000); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            LCLoginServer response = packet as LCLoginServer;
            GameEntry.Event.Fire(this, new LoginServerEventArgs(response.Authorized, response.NewAccount,response.RestrictServer));
        }
    }
}
