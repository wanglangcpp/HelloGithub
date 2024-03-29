﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class LCClaimDailyLoginGiftHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 1079); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            Handle(sender, packet as LCClaimDailyLoginGift);
        }

        public static void Handle(object sender, LCClaimDailyLoginGift response)
        {
            GameEntry.Data.EveryDaySignInData.UpDataBoxData(response.ClaimGifts);
            GameEntry.Event.Fire(sender, new ClaimSignInBoxEventArgs());
        }
    }
}
