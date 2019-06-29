using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class CleanOutInstanceEventArgs : GameEventArgs
    {
        public CleanOutInstanceEventArgs(List<PBItemInfo> rewards, int obtainedCoin, int obtainedExperience)
        {
            Rewards = rewards;
            ObtainedCoinCount = obtainedCoin;
            ObtainedExperienceCount = obtainedExperience;

        }

        public int ObtainedCoinCount
        {
            get;
            private set;
        }

        public int ObtainedExperienceCount
        {
            get;
            private set;
        }

        public List<PBItemInfo> Rewards
        {
            get;
            private set;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.CleanOutInstance;
            }
        }
    }
}
