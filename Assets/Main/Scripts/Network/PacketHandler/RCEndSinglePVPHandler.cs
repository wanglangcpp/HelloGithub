﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class RCEndSinglePVPHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.RoomServerToClient, 5011); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            Handle(sender, packet as RCEndSinglePVP);
        }

        public static void Handle(object sender, RCEndSinglePVP response)
        {
            UnityEngine.Time.timeScale = 1f;
            int result = 0;
            if (response.Winner==0)
            {
                result = 3;
            }
            else if (response.Winner==GameEntry.Data.Player.Id)
            {
                //GameEntry.SceneLogic.SinglePvpInstanceLogic.SetInstanceSuccess((InstanceSuccessReason)response.Reason, () => { });
                result = 1;
            }
            else
            {
                //GameEntry.SceneLogic.SinglePvpInstanceLogic.SetInstanceFailure((InstanceFailureReason)response.Reason, true, () => { });
                result = 2;
            }
            GameEntry.Data.Room.ClearData();
            CloseRoom();
            //强制返回主城
            //GameEntry.Event.Fire(sender, new GetRoomStatusEventArgs((int)RoomStateType.Finish));
            /*Undefined = 0,
            Win = 1,
            Lose = 2,
            Draw = 3,*/
            SinglePvpResultDisplayData displayData = new SinglePvpResultDisplayData();
            displayData.WinnerId = response.Winner;
            displayData.WinnerName = response.WinnerName;
            displayData.WinnerServerName = response.WinnerServerName;
            displayData.ResultType = (InstanceResultType)result;
            displayData.Reason = response.Reason;
            displayData.Settlements.AddRange(response.Settlements.ToArray());
            
            GameEntry.Data.SingleMatchData.SetDisplayData(displayData);
            GameEntry.Event.Fire(sender, new RoomBattleResultPushedEventArgs(result, response.Reason));
        }
        private static void CloseRoom()
        {
            var nc = GameEntry.Network.GetNetworkChannel(Constant.Network.RoomNetworkChannelName);
            if (nc == null)
            {
                GameFramework.Log.Error("Network channel 'Room' doesn't exist.");
                return;
            }
            nc.Close();
        }
    }
}
