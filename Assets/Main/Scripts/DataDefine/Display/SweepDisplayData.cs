namespace Genesis.GameClient
{
    public class SweepDisplayData : UIFormBaseUserData
    {
        public enum ShowType
        {
            InstanceAuto,   // 副本进入，自动扫荡
            WhereToGet      // 物品获得途径进入
        }

        public void SetFromInstanceData(int levelId)
        {
            SweepEntranceType = ShowType.InstanceAuto;
            LevelId = levelId;
        }

        public void SetFromWhereToGetData(DRWhereToGet whereToGetConfig, int wannaGetItemId, int wannaGetItemCount)
        {
            SweepEntranceType = ShowType.WhereToGet;
            WhereToGetConfig = whereToGetConfig;
            WannaGetItemId = wannaGetItemId;
            WannaGetItemCount = wannaGetItemCount;
            LevelId = int.Parse(whereToGetConfig.Params[1]);
        }

        /// <summary>
        /// 扫荡入口类型
        /// </summary>
        public ShowType SweepEntranceType { get; private set; }

        /// <summary>
        /// 要扫荡的章节的ID
        /// </summary>
        public int LevelId{ get; private set; }

        /// <summary>
        /// 从哪里获得的配置文件
        /// </summary>
        public DRWhereToGet WhereToGetConfig { get; private set; }

        /// <summary>
        /// 想要得到的物品ID
        /// </summary>
        public int WannaGetItemId { get; private set; }

        /// <summary>
        /// 想要得到的物品数量
        /// </summary>
        public int WannaGetItemCount { get; private set; }

        /// <summary>
        /// 最大的扫荡次数
        /// </summary>
        public int MaxSweepCount
        {
            get
            {
                if (WannaGetItemCount >= MaxCount)
                    return MaxCount;

                if (WannaGetItemCount >= MiddleCount)
                    return MiddleCount;

                return MinCount;
            }
        }

        // 这三个值是策划在文档里配置好的
        private const int MaxCount = 20;
        private const int MiddleCount = 15;
        private const int MinCount = 10;
    }
}