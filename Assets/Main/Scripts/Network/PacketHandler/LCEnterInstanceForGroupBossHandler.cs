﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class LCEnterInstanceForGroupBossHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 2630); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            Handle(sender, packet as LCEnterInstanceForGroupBoss);
        }

        public static void Handle(object sender, LCEnterInstanceForGroupBoss response)
        {
            if (GameEntry.Procedure.CurrentProcedure is ProcedureMain)
            {
                GameEntry.Event.Fire(sender, new WillChangeSceneEventArgs(InstanceLogicType.SinglePlayer, response.InstanceType, true));
            }

        }
    }
}
