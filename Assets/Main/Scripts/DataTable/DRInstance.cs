using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本配置表。
    /// </summary>
    public class DRInstance : IDataRow
    {
        private const int AIBehaviorMaxCount = Constant.AIBehaviorMaxCountInInstance;
        private const int InInstanceChestsCount = Constant.ChestsMaxCountInInstance;

        public class PossibleDrop
        {
            public int ItemId { get; protected set; }

            public PossibleDrop(int iconId)
            {
                ItemId = iconId;
            }
        }

        /// <summary>
        /// 副本编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 副本类型
        ///     0：剧情副本
        ///     1：精英副本
        ///     2: boss副本
        ///     3:boss精英副本
        ///     4：爬塔副本
        /// </summary>
        public int InstanceType
        {
            get;
            protected set;
        }

        /// <summary>
        /// 推荐战斗力
        /// </summary>
        public int RecommendMight
        {
            get;
            protected set;
        }

        /// <summary>
        /// 副本的编号，这个与Prefab上绑定的IntKey值一一对应
        /// </summary>
        public int InstanceKeyNumber
        {
            get;
            protected set;
        }

        /// <summary>
        /// 副本名称。
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// 副本描述。
        /// </summary>
        public string Description
        {
            get;
            protected set;
        }

        /// <summary>
        /// 目标描述。
        /// </summary>
        public string[] RequestDescriptions
        {
            get;
            protected set;
        }

        /// <summary>
        /// 目标描述。
        /// </summary>
        /// <param name="index">目标索引。</param>
        /// <returns>目标描述。</returns>
        public string GetRequestDescription(int index)
        {
            return RequestDescriptions[index];
        }

        /// <summary>
        /// 前置副本编号。
        /// </summary>
        public int PrerequisiteInstanceId
        {
            get;
            protected set;
        }

        /// <summary>
        /// 副本开启等级。
        /// </summary>
        public int PrerequisitePlayerLevel
        {
            get;
            protected set;
        }

        /// <summary>
        /// 副本组编号。
        /// </summary>
        public int InstanceGroupId
        {
            get;
            protected set;
        }

        /// <summary>
        /// 推荐等级。
        /// </summary>
        public int RecommendLevel
        {
            get;
            protected set;
        }

        /// <summary>
        /// 玩家获得经验。
        /// </summary>
        public int PlayerExp
        {
            get;
            protected set;
        }

        /// <summary>
        /// 英雄获得经验。
        /// </summary>
        public int HeroExp
        {
            get;
            protected set;
        }

        /// <summary>
        /// 获得金币数量。
        /// </summary>
        public int Coin
        {
            get;
            protected set;
        }

        /// <summary>
        /// 最大杀怪奖励金币。
        /// </summary>
        public int MaxBonusCoin
        {
            get;
            protected set;
        }

        /// <summary>
        /// 奖励编号序列。
        /// </summary>
        public string DropIds
        {
            get;
            protected set;
        }

        /// <summary>
        /// 扫荡奖励编号序列。
        /// </summary>
        public string CleanOutDropIds
        {
            get;
            protected set;
        }

        /// <summary>
        /// 场景编号。
        /// </summary>
        public int SceneId
        {
            get;
            protected set;
        }

        /// <summary>
        /// 定时器类型（1-正计时；2-倒计时）。
        /// </summary>
        public int TimerType
        {
            get;
            protected set;
        }

        /// <summary>
        /// 定时器时长。
        /// </summary>
        public int TimerDuration
        {
            get;
            protected set;
        }

        /// <summary>
        /// 定时器报警时长。
        /// </summary>
        public int TimerAlert
        {
            get;
            protected set;
        }

        /// <summary>
        /// AI行为。
        /// </summary>
        protected List<string> m_AIBehaviors;

        public string[] AIBehaviors
        {
            get
            {
                return m_AIBehaviors.ToArray();
            }
        }

        /// <summary>
        /// 副本NPC。
        /// </summary>
        public string InstanceNpcs
        {
            get;
            protected set;
        }

        /// <summary>
        /// 副本建筑物。
        /// </summary>
        public string InstanceBuildings
        {
            get;
            protected set;
        }

        /// <summary>
        /// 出生点X坐标。
        /// </summary>
        public float SpawnPointX
        {
            get;
            protected set;
        }

        /// <summary>
        /// 出生点Y坐标。
        /// </summary>
        public float SpawnPointY
        {
            get;
            protected set;
        }

        /// <summary>
        /// 出生朝向。
        /// </summary>
        public float SpawnAngle
        {
            get;
            protected set;
        }

        protected List<PossibleDrop> m_PossibleDrops;

        /// <summary>
        /// 可能掉落物品的显示信息。
        /// </summary>
        public PossibleDrop[] PossibleDrops
        {
            get
            {
                return m_PossibleDrops.ToArray();
            }
        }

        /// <summary>
        /// 副本详情界面所需Texture的Id。
        /// </summary>
        public int TextureId
        {
            get;
            protected set;
        }

        public int SceneRegionMaskId
        {
            get;
            protected set;
        }

        /// <summary>
        /// 是否显示 NPC 图标。
        /// </summary>
        public bool ShowNpcIcon
        {
            get;
            protected set;
        }

        /// <summary>
        /// 用于显示图标的 NPC 编号。
        /// </summary>
        public int NpcId
        {
            get;
            protected set;
        }

        /// <summary>
        /// 寻径点集编号。
        /// </summary>
        public int GuidePointSetId
        {
            get;
            protected set;
        }

        public int[] GetSceneRegionIds()
        {
            if (SceneRegionMaskId <= 0)
            {
                return new int[0];
            }

            int index = 0;
            int sceneRegionMaskId = SceneRegionMaskId;
            List<int> sceneRegionIds = new List<int>();
            while (sceneRegionMaskId > 0)
            {
                index++;
                if ((sceneRegionMaskId & 1) > 0)
                {
                    sceneRegionIds.Add(index);
                }

                sceneRegionMaskId >>= 1;
            }

            return sceneRegionIds.ToArray();
        }

        public virtual void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            index = ParseRequestDescs(text, index);
            ShowNpcIcon = bool.Parse(text[index++]);
            NpcId = int.Parse(text[index++]);
            InstanceType = int.Parse(text[index++]);
            InstanceKeyNumber = int.Parse(text[index++]);
            RecommendMight = int.Parse(text[index++]);
            PrerequisiteInstanceId = int.Parse(text[index++]);
            PrerequisitePlayerLevel = int.Parse(text[index++]);
            InstanceGroupId = int.Parse(text[index++]);
            RecommendLevel = int.Parse(text[index++]);
            PlayerExp = int.Parse(text[index++]);
            HeroExp = int.Parse(text[index++]);
            Coin = int.Parse(text[index++]);
            MaxBonusCoin = int.Parse(text[index++]);
            DropIds = text[index++];
            CleanOutDropIds = text[index++];
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
            index = ParsePossibleDrops(text, index);
            index += InInstanceChestsCount; // 跳过副本内宝箱数据
            SceneRegionMaskId = int.Parse(text[index++]);
            TextureId = int.Parse(text[index++]);
            GuidePointSetId = int.Parse(text[index++]);
        }

        protected int ParseRequestDescs(string[] text, int index)
        {
            RequestDescriptions = new string[Constant.InstanceRequestCount];
            for (int i = 0; i < Constant.InstanceRequestCount; i++)
            {
                RequestDescriptions[i] = text[index++];
            }
            return index;
        }

        protected int ParseAIBehaviors(string[] text, int index)
        {
            m_AIBehaviors = new List<string>();
            for (int i = 0; i < AIBehaviorMaxCount; ++i)
            {
                var behaviorName = text[index++];
                if (!string.IsNullOrEmpty(behaviorName))
                {
                    m_AIBehaviors.Add(behaviorName);
                }
            }
            return index;
        }

        protected int ParsePossibleDrops(string[] text, int index)
        {
            m_PossibleDrops = new List<PossibleDrop>();
            for (int i = 0; i < Constant.InstanceDropTypeMaxCount; i++)
            {
                var dropId = int.Parse(text[index++]);
                if (dropId < 0)
                {
                    continue;
                }
                m_PossibleDrops.Add(new PossibleDrop(dropId));
            }
            return index;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRInstance>();
        }
    }
}
