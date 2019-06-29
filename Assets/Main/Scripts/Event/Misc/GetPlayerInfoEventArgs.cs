using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class GetPlayerInfoEventArgs : GameEventArgs
    {
        public GetPlayerInfoEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.GetPlayerInfo;
            }
        }

        public PlayerData PlayerData
        {
            get;
            set;
        }

        public List<LobbyHeroData> Heroes
        {
            get;
            set;
        }

        public List<int> HeroTeam
        {
            get;
            set;
        }
    }
}
