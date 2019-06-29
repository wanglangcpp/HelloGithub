using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本组配置表。
    /// </summary>
    public class DRInstanceGroup : IDataRow
    {
        public class Reward
        {
            public Reward(int id, int count)
            {
                Id = id;
                Count = count;
            }
            public int Id { get; private set; }
            public int Count { get; private set; }
        }

        /// <summary>
        /// 副本组编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 副本名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 副本描述。
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 副本选择界面对应的图片名称。
        /// </summary>
        public string ChapterIconName { get; private set; }

        /// <summary>
        /// 宝箱1打开需要的星星数量。
        /// </summary>
        public int Chest1NeedStar { get; private set; }
        
        /// <summary>
        /// 宝箱2打开需要的星星数量。
        /// </summary>
        public int Chest2NeedStar { get; private set; }
        
        /// <summary>
        /// 宝箱3打开需要的星星数量。
        /// </summary>
        public int Chest3NeedStar { get; private set; }

        /// <summary>
        /// 宝箱1的奖励。
        /// </summary>
        public List<Reward> Chest1Rewards { get; private set; }

        /// <summary>
        /// 宝箱2的奖励。
        /// </summary>
        public List<Reward> Chest2Rewards { get; private set; }

        /// <summary>
        /// 宝箱3的奖励。
        /// </summary>
        public List<Reward> Chest3Rewards { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            ChapterIconName = text[index++];

            int maxItemTypeCount = 4;   // 每个宝箱最大的奖励类型数量

            Chest1NeedStar = int.Parse(text[index++]);
            Chest1Rewards = new List<Reward>();
            for (int i = 0; i < maxItemTypeCount; i++)
            {
                int id = int.Parse(text[index++]);
                if (id <= 0)
                {
                    index++;
                    continue;
                }
                Chest1Rewards.Add(new Reward(id, int.Parse(text[index++])));
            }

            Chest2NeedStar = int.Parse(text[index++]);
            Chest2Rewards = new List<Reward>();
            for (int i = 0; i < maxItemTypeCount; i++)
            {
                int id = int.Parse(text[index++]);
                if (id <= 0)
                {
                    index++;
                    continue;
                }
                Chest2Rewards.Add(new Reward(id, int.Parse(text[index++])));
            }

            Chest3NeedStar = int.Parse(text[index++]);
            Chest3Rewards = new List<Reward>();
            for (int i = 0; i < maxItemTypeCount; i++)
            {
                int id = int.Parse(text[index++]);
                if (id <= 0)
                {
                    index++;
                    continue;
                }
                Chest3Rewards.Add(new Reward(id, int.Parse(text[index++])));
            }
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRInstanceGroup>();
        }
    }
}
