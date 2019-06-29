﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework;
using GameFramework.Network;

namespace Genesis.GameClient
{
    public class RCGetRoomStatusHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.RoomServerToClient, 5006); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            Handle(sender, packet as RCGetRoomStatus);
        }

        public static void Handle(object sender, RCGetRoomStatus response)
        {
            if (response.RoomStatus == (int)ServerErrorCode.RoomStatusError || response.RoomStatus == (int)RoomStateType.Finish)
            {
                GameEntry.Data.Room.ClearData();
                CloseRoom();
            }
            else
            {
                GameEntry.Data.Room.State = (RoomStateType)response.RoomStatus;
            }
            GameEntry.Event.Fire(sender, new GetRoomStatusEventArgs(response.RoomStatus));
        }

        private static void CloseRoom()
        {
            var nc = GameEntry.Network.GetNetworkChannel(Constant.Network.RoomNetworkChannelName);
            if (nc == null)
            {
                Log.Error("Network channel 'Room' doesn't exist.");
                return;
            }

            nc.Close();
        }
    }
}
