using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 日常任务配置表。
    /// </summary>
    public class DRDailyQuest : IDataRow
    {
        /// <summary>
        /// 任务编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 任务名称。
        /// </summary>
        public string QuestName { get; private set; }

        /// <summary>
        /// 任务描述。
        /// </summary>
        public string QuestDesc { get; private set; }

        /// <summary>
        /// 任务类型。
        /// </summary>
        public int QuestType { get; private set; }

        /// <summary>
        /// 任务目标值。
        /// </summary>
        public int TargetProgressCount { get; private set; }

        /// <summary>
        /// 参数。
        /// </summary>
        public int[] Params { get; private set; }

        public int GetParam(int index)
        {
            return Params[index];
        }

        /// <summary>
        /// 前置玩家等级。
        /// </summary>
        public int PrePlayerLevel { get; private set; }

        /// <summary>
        /// 奖励道具编号。
        /// </summary>
        public int[] RewardItemIds { get; private set; }

        public int GetRewardItemId(int index)
        {
            return RewardItemIds[index];
        }

        /// <summary>
        /// 奖励道具数量。
        /// </summary>
        public int[] RewardItemCounts { get; private set; }

        public int GetRewardItemCount(int index)
        {
            return RewardItemCounts[index];
        }

        /// <summary>
        /// 每日任务IconId。
        /// </summary>
        public int DailyQuestIconId { get; private set; }

        /// <summary>
        /// 每日任务获取奖励描述。
        /// </summary>
        public string DailyQuestRewardDesc { get; private set; }

        /// <summary>
        /// 界面寻路Id。
        /// </summary>
        public int WhereToGoId { get; private set; }

        /// <summary>
        /// 排序权重。
        /// </summary>
        public int Weight { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            QuestName = text[index++];
            QuestDesc = text[index++];
            QuestType = int.Parse(text[index++]);
            TargetProgressCount = int.Parse(text[index++]);
            Params = new int[Constant.DailyQuestParamCount];
            for (int i = 0; i < Constant.DailyQuestParamCount; ++i)
            {
                Params[i] = int.Parse(text[index++]);
            }

            PrePlayerLevel = int.Parse(text[index++]);
            RewardItemIds = new int[Constant.DailyQuestRewardCount];
            RewardItemCounts = new int[Constant.DailyQuestRewardCount];
            for (int i = 0; i < Constant.DailyQuestRewardCount; ++i)
            {
                RewardItemIds[i] = int.Parse(text[index++]);
                RewardItemCounts[i] = int.Parse(text[index++]);
            }
            DailyQuestIconId = int.Parse(text[index++]);
            DailyQuestRewardDesc = text[index++];
            WhereToGoId = int.Parse(text[index++]);
            Weight = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRDailyQuest>();
        }
    }
}
