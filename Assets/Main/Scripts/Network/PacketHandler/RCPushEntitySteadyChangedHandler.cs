﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class RCPushEntitySteadyChangedHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.RoomServerToClient, 4111); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            Handle(sender, packet as RCPushEntitySteadyChanged);
        }

        public static void Handle(object sender, RCPushEntitySteadyChanged response)
        {
            if (GameEntry.Data.Room.HasReconnected)
            {
                return;
            }
            //if (response.PlayerId)
            //{

            //}
            GameEntry.Event.Fire(sender, new PushEntitySteadyChangedEventArgs(response));
        }
    }
}
