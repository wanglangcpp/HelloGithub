using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 资源副本（活动副本的一种）配置表。
    /// </summary>
    public class DRInstanceForResource : DRInstance
    {
        private const int RewardLevelCount = 5;

        /// <summary>
        /// 资源类型。
        /// </summary>
        public InstanceForResourceType InstanceResourceType
        {
            get;
            private set;
        }

        /// <summary>
        /// 等级范围。
        /// </summary>
        public int LevelRangeId
        {
            get;
            private set;
        }

        /// <summary>
        /// 金币奖励列表。
        /// </summary>
        public int[] CoinRewardsPerLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 掉落奖励列表。
        /// </summary>
        public string[] DropIdsPerLevel
        {
            get;
            private set;
        }

        public override void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            InstanceResourceType = (InstanceForResourceType)int.Parse(text[index++]);
            LevelRangeId = int.Parse(text[index++]);

            CoinRewardsPerLevel = new int[RewardLevelCount];
            for (int i = 0; i < RewardLevelCount; i++)
            {
                CoinRewardsPerLevel[i] = int.Parse(text[index++]);
            }

            MaxBonusCoin = int.Parse(text[index++]);

            DropIdsPerLevel = new string[RewardLevelCount];
            for (int i = 0; i < RewardLevelCount; i++)
            {
                DropIdsPerLevel[i] = text[index++];
            }

            SceneId = int.Parse(text[index++]);
            TimerType = int.Parse(text[index++]);
            TimerDuration = int.Parse(text[index++]);
            TimerAlert = int.Parse(text[index++]);
            index = ParseAIBehaviors(text, index);
            InstanceNpcs = text[index++];
            InstanceBuildings = text[index++];
            SpawnPointX = float.Parse(text[index++]);
            SpawnPointY = float.Parse(text[index++]);
            SpawnAngle = float.Parse(text[index++]);
            SceneRegionMaskId = int.Parse(text[index++]);
            GuidePointSetId = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRInstanceForResource>();
        }
    }
}
