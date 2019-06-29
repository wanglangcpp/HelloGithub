﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework.Network;

namespace Genesis.GameClient
{
    public class LCComposeHeroHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 1032); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            LCComposeHero response = packet as LCComposeHero;

            GameEntry.Data.LobbyHeros.AddData(response.NewHero);
            GameEntry.Event.Fire(this, new LobbyHeroDataChangedEventArgs());
            GameEntry.Event.Fire(this, new ComposeHeroCompleteEventArgs(response.NewHero.Type));

            GeneralItemUtility.UpdateItemsData(response.HeroPieceItem);
            GameEntry.Event.Fire(this, new ItemDataChangedEventArgs());
        }
    }
}
