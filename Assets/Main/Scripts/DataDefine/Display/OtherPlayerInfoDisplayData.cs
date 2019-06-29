using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class OtherPlayerInfoDisplayData : UIFormBaseUserData
    {
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
