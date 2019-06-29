using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class InstanceResultData : UIFormBaseUserData
    {
        public bool[] RequestsComplete { get; set; }

        public int CompleteRequestCount { get; set; }

        public InstanceLogicType Type { get; set; }

        public int InstanceGoToType { get; set; }
        public class Player
        {
            public int PortraitId { get; set; }
            public string Name { get; set; }
            public int OldLevel { get; set; }
            public int OldExp { get; set; }
            public int NewLevel { get; set; }
            public int NewExp { get; set; }
            public int OldCoin { get; set; }
            public int NewCoin { get; set; }
            public int OldMeridianEnergy { get; set; }
            public int NewMeridianEnergy { get; set; }

            public int CoinEarned
            {
                get
                {
                    return NewCoin - OldCoin;
                }
            }

            public bool HasLevelUp
            {
                get
                {
                    return NewLevel > OldLevel;
                }
            }
        }

        public class Hero
        {
            public string PortraitSpriteName { get; set; }
            public string Name { get; set; }
            public int OldLevel { get; set; }
            public int OldExp { get; set; }
            public int NewLevel { get; set; }
            public int NewExp { get; set; }
            public int Profession { get; set; }
            public int ElementId { get; set; }

            public bool HasLevelUp
            {
                get
                {
                    return NewLevel > OldLevel;
                }
            }
        }

        public Player ItsPlayer { get; set; }

        public List<Hero> Heroes { get; set; }

        public InstanceResultData()
        {
            RequestsComplete = new bool[Constant.InstanceRequestCount];
            ItsPlayer = new Player();
            Heroes = new List<Hero>();
        }
    }
}
