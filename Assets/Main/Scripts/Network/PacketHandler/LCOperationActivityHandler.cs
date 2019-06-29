﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class LCOperationActivityHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 2500); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            Handle(sender, packet as LCOperationActivity);
        }

        public static void Handle(object sender, LCOperationActivity response)
        {
            if (response.PlayerInfo != null)
            {
                GameEntry.Data.Player.UpdateData(response.PlayerInfo);
                GameEntry.Event.Fire(sender, new PlayerDataChangedEventArgs());
            }

            if (response.ReceivedItems != null)
            {
                var helper = new RewardCollectionHelper();
                helper.AddItems(response.ReceivedItems.ItemInfo);           
                GameEntry.RewardViewer.RequestShowRewards(helper.ReceiveGoodsData, false);
            }

            var responseData = new Dictionary<string, string>();

            for (int i = 0; i < response.Params.Count; ++i)
            {
                var kv = response.Params[i];
                responseData.Add(kv.Key, kv.Value);
            }

            GameEntry.Event.Fire(sender, new OperationActivityResponseEventArgs(responseData));
        }
    }
}
