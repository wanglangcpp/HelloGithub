﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// You may need to modify 'Handle' method.
// Don't modify the rest unless you know what you're doing.
//----------------------------------------------------------------------------------------------------

using GameFramework;
using GameFramework.Network;

namespace Genesis.GameClient
{
    public class LCOpenMeridianStarHandler : BasePacketHandler
    {
        public override int OpCode { get { return NetworkHelper.GetOpCode(PacketType.LobbyServerToClient, 1018); } }

        public override void Handle(object sender, Packet packet)
        {
            base.Handle(sender, packet);
            LCOpenMeridianStar response = packet as LCOpenMeridianStar;
            if (response.LobbyHeroInfo.Count > 0)
            {
                for (int i = 0; i < GameEntry.Data.LobbyHeros.Data.Count; i++)
                {
                    var heroData = GameEntry.Data.LobbyHeros.GetData(response.LobbyHeroInfo[i].Type);
                    if (heroData == null)
                    {
                        Log.Error("Can not find heroData by {0}.", response.LobbyHeroInfo[i].Type);
                        return;
                    }
                    heroData.UpdateData(response.LobbyHeroInfo[i]);
                }
            }

            GameEntry.Data.Meridian.UpdateData(response.MeridianInfo);
            GameEntry.Data.Player.UpdateData(response.PlayerInfo);
            GameEntry.Event.Fire(this, new OpenMeridianStarEventArgs());
            GameEntry.Event.Fire(this, new PlayerDataChangedEventArgs());
            GameEntry.Event.Fire(this, new LobbyHeroDataChangedEventArgs());
        }
    }
}
