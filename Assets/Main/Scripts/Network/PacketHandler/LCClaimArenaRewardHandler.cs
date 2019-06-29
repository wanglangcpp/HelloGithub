﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class LCClaimArenaRewardHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 2304); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            Handle(sender, packet as LCClaimArenaReward);
        }

        public static void Handle(object sender, LCClaimArenaReward response)
        {
            if (response.CompoundItemInfo.Count > 0)
            {
                var helper = new RewardCollectionHelper();
                helper.AddCompoundItems(response.CompoundItemInfo);

                GameEntry.Event.Fire(sender, new OfflineArenaLivenessRewardClaimedEventArgs(helper));
                GameEntry.Event.Fire(sender, new PlayerDataChangedEventArgs());
            }

            GameEntry.Data.OfflineArena.UpdateArenaRewardData(int.MinValue, false);
            GameEntry.Event.Fire(sender, new OfflineArenaDataChangedEventArgs());
        }
    }
}
