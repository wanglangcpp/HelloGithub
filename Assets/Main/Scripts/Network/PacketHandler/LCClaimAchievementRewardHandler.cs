﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class LCClaimAchievementRewardHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 1052); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            Handle(sender, packet as LCClaimAchievementReward);
        }

        public static void Handle(object sender, LCClaimAchievementReward response)
        {
            var achieveData = GameEntry.Data.Achievements.GetData(response.AchievementInfo.AchievementId);
            if (achieveData == null)
            {
                GameEntry.Data.Achievements.AddData(response.AchievementInfo);
            }
            else
            {
                achieveData.UpdateData(response.AchievementInfo);
            }
            var helper = new RewardCollectionHelper();
            helper.AddCompoundItems(response.CompoundItemInfo);
            GameEntry.Event.Fire(sender, new PlayerDataChangedEventArgs());
            GameEntry.Event.Fire(sender, new ItemDataChangedEventArgs());
            GameEntry.Event.Fire(sender, new ClaimAchievementRewardEventArgs(response.AchievementInfo.AchievementId, helper.ReceiveGoodsData));
        }
    }
}
