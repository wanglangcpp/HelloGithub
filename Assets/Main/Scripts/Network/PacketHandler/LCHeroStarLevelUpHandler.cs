﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class LCHeroStarLevelUpHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 1010); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            LCHeroStarLevelUp response = packet as LCHeroStarLevelUp;
            GameEntry.Data.LobbyHeros.GetData(response.LobbyHeroInfo.Type).UpdateData(response.LobbyHeroInfo);
            GeneralItemUtility.UpdateItemsData(response.ItemInfo);
            GameEntry.Event.Fire(this, new ItemDataChangedEventArgs());
            GameEntry.Event.Fire(this, new LobbyHeroDataChangedEventArgs());
            GameEntry.Event.Fire(this, new HerostarLevelUpEventArgs());

            GameEntry.Data.Player.UpdateData(response.PlayerInfo);
            GameEntry.Event.Fire(this, new PlayerDataChangedEventArgs());
        }
    }
}
