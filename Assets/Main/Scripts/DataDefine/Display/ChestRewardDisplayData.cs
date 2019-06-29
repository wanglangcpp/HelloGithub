using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class ChestRewardDisplayData : UIFormBaseUserData
    {
        public class Reward
        {
            public Reward(int id, int count)
            {
                Id = id;
                Count = count;
            }

            public int Id
            {
                get; private set;
            }

            public int Count
            {
                get; private set;
            }
        }

        public ChestRewardDisplayData(List<Reward> rewards, string title , string instruction)
        {
            Rewards = rewards;
            InstructionLabelString = instruction;
            TitleLabelString = title;
        }

        public string TitleLabelString
        {
            get;
            private set;
        }

        public string InstructionLabelString
        {
            get;
            private set;
        }

        public List<Reward> Rewards
        {
            get;
            private set;
        }
    }
}