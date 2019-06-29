using System;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class DailyLoginedEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.DailyLogined;
            }
        }

        public DailyLoginedEventArgs(RewardCollectionHelper rewards)
        {
            ShowRewards = rewards;
        }

        public RewardCollectionHelper ShowRewards
        {
            get;
            private set;
        }
    }
}