using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 成就配置表。
    /// </summary>
    public class DRAchievement : IDataRow
    {
        /// <summary>
        /// 成就编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 成就名称。
        /// </summary>
        public string AchievementName { get; private set; }

        /// <summary>
        /// 成就描述。
        /// </summary>
        public string AchievementDesc { get; private set; }

        /// <summary>
        /// 成就类型。
        /// </summary>
        public int AchievementType { get; private set; }

        /// <summary>
        /// 成就目标值。
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
        /// 前置成就编号。
        /// </summary>
        public int PreAchievementId { get; private set; }

        /// <summary>
        /// 奖励道具编号1。
        /// </summary>
        public int[] RewardItemIds { get; private set; }

        public int GetRewardItemId(int index)
        {
            return RewardItemIds[index];
        }

        /// <summary>
        /// 奖励道具数量1。
        /// </summary>
        public int[] RewardItemCounts { get; private set; }

        public int GetRewardItemCount(int index)
        {
            return RewardItemCounts[index];
        }

        /// <summary>
        /// 成就IconId。
        /// </summary>
        public int AchievementIconId { get; private set; }

        /// <summary>
        /// 成就获取奖励描述。
        /// </summary>
        public string AchievementRewardDesc { get; private set; }

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
            AchievementName = text[index++];
            AchievementDesc = text[index++];
            AchievementType = int.Parse(text[index++]);
            TargetProgressCount = int.Parse(text[index++]);
            Params = new int[Constant.AchievementParamCount];
            for (int i = 0; i < Constant.AchievementParamCount; i++)
            {
                Params[i] = int.Parse(text[index++]);
            }
            PrePlayerLevel = int.Parse(text[index++]);
            PreAchievementId = int.Parse(text[index++]);
            RewardItemIds = new int[Constant.AchievementRewardCount];
            RewardItemCounts = new int[Constant.AchievementRewardCount];
            for (int i = 0; i < Constant.AchievementRewardCount; i++)
            {
                RewardItemIds[i] = int.Parse(text[index++]);
                RewardItemCounts[i] = int.Parse(text[index++]);
            }
            AchievementIconId = int.Parse(text[index++]);
            AchievementRewardDesc = text[index++];
            WhereToGoId = int.Parse(text[index++]);
            Weight = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRAchievement>();
        }
    }
}
