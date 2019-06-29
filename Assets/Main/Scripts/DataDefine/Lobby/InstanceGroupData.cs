using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class InstanceGroupData
    {
        public class InstanceData
        {
            public InstanceData(DRInstance config)
            {
                m_LevelConfig = config;
                m_ProgressData = GameEntry.Data.InstanceProgresses.GetData(config.Id);
            }

            private InstanceProgressData m_ProgressData;
            private DRInstance m_LevelConfig;

            /// <summary>
            /// 副本关卡的配置文件数据
            /// </summary>
            public DRInstance LevelConfig
            {
                get
                {
                    return m_LevelConfig;
                }
            }

            public int Id
            {
                get { return m_LevelConfig.Id; }
            }

            public string Name
            {
                get { return m_LevelConfig.Name; }
            }

            public bool NeedShowNpcIcon
            {
                get { return m_LevelConfig.ShowNpcIcon; }
            }

            private DRNpc m_NpcInfo = null;
            public DRNpc NpcInfo
            {
                get
                {
                    if (m_NpcInfo == null)
                    {
                        m_NpcInfo = GameEntry.DataTable.GetDataTable<DRNpc>().GetDataRow(LevelConfig.NpcId);
                    }

                    return m_NpcInfo;
                }
            }

            /// <summary>
            /// 如果此关卡打完，服务器一定会下发数据的
            /// </summary>
            public bool IsDone
            {
                get
                {
                    return m_ProgressData != null;
                }
            }

            /// <summary>
            /// 关卡是否开启
            /// 目前的判断条件是：
            /// 玩家等级大于关卡所需最低等级 并且 前置关卡通关
            /// </summary>
            public bool IsOpen
            {
                get
                {
                    if (GameEntry.Data.Player.Level < m_LevelConfig.PrerequisitePlayerLevel)
                        return false;

                    // 没有前置关卡，那么是第一关，只要满足上面条件就可以了
                    if (m_LevelConfig.PrerequisiteInstanceId <= 0)
                        return true;

                    return GameEntry.Data.InstanceProgresses.GetData(m_LevelConfig.PrerequisiteInstanceId) != null;
                }
            }
            
            /// <summary>
            /// 当前置关卡通关后，但是当前关卡的开启等级不足，需要显示提示，XX级开始
            /// </summary>
            public bool NeedShowHint
            {
                get
                {
                    return GameEntry.Data.InstanceProgresses.GetData(m_LevelConfig.PrerequisiteInstanceId) != null 
                        && GameEntry.Data.Player.Level < m_LevelConfig.PrerequisitePlayerLevel;
                }
            }

            /// <summary>
            /// 关卡的星级
            /// </summary>
            public int StarCount
            {
                get
                {
                    if (m_ProgressData == null)
                        return 0;
                    else
                        return m_ProgressData.StarCount;
                }
            }

        }

        private DRInstanceGroup m_ChapterConfig = null;

        /// <summary>
        /// 初始化章节的配置数据
        /// </summary>
        /// <param name="config">章节数据</param>
        public InstanceGroupData(DRInstanceGroup config)
        {
            m_ChapterConfig = config;
        }

        /// <summary>
        /// 副本章节的关卡数据
        /// Key:关卡ID
        /// Value:关卡数据
        /// </summary>
        private Dictionary<int, InstanceData> m_InstanceMap = new Dictionary<int, InstanceData>();

        /// <summary>
        /// 章节ID
        /// </summary>
        public int ChapterId { get { return ChapterConfig.Id; } }

        /// <summary>
        /// 章节的配置文件数据
        /// </summary>
        public DRInstanceGroup ChapterConfig
        {
            get { return m_ChapterConfig; }
        }

        /// <summary>
        /// 判断当前关卡是否已经完成
        /// True：完成
        /// False：未完成
        /// </summary>
        public bool IsDone
        {
            get
            {
                foreach (var level in m_InstanceMap.Values)
                    if (!level.IsDone)
                        return false;

                return true;
            }
        }

        /// <summary>
        /// 当前关卡是否已经开启
        /// True：开启
        /// False：未开启
        /// </summary>
        public bool IsOpen
        {
            get
            {
                foreach (var level in m_InstanceMap.Values)
                    if (level.IsOpen)
                        return true;

                return false;
            }
        }

        /// <summary>
        /// 当前章节获得的星星数量
        /// </summary>
        public int CurrentStarCount
        {
            get
            {
                int count = 0;

                foreach (var level in m_InstanceMap.Values)
                    count += level.StarCount;

                return count;
            }
        }

        /// <summary>
        /// 当前章节需要的全部数量
        /// </summary>
        public int TotalStarCount
        {
            get
            {
                return m_InstanceMap.Count * 3;
            }
        }

        /// <summary>
        /// 当前章节通关的关卡数
        /// </summary>
        public int DoneLevelCount
        {
            get
            {
                int count = 0;
                foreach (var level in m_InstanceMap.Values)
                    if (level.IsDone)
                        count++;

                return count;
            }
        }

        public int TotalCount
        {
            get { return m_InstanceMap.Count; }
        }

        private InstanceData m_FirstLevelData = null;
        public InstanceData FirstLevelData
        {
            get
            {
                if (m_FirstLevelData == null)
                {
                    foreach (var level in m_InstanceMap.Values)
                    {
                        if (!m_InstanceMap.ContainsKey(level.LevelConfig.PrerequisiteInstanceId))
                        {
                            m_FirstLevelData = level;
                            break;
                        }
                    }

                    if (m_FirstLevelData == null)
                    {
                        Log.Error("Configuration table error.Can not find out first level in chapter {0}", ChapterId);
                    }
                }

                return m_FirstLevelData;
            }
        }

        public void RefreshFirstLevel()
        {
            m_FirstLevelData = null;
        }

        /// <summary>
        /// 添加或者刷新关卡数据
        /// </summary>
        /// <param name="levelData">关卡数据</param>
        public void AddOrModifyLevel(DRInstance levelData)
        {
            if (levelData == null)
                return;

            m_InstanceMap[levelData.Id] = new InstanceData(levelData);
        }

        /// <summary>
        /// 刷新关卡数据
        /// </summary>
        /// <param name="levelId">关卡ID</param>
        public void ModifyLevel(int levelId)
        {
            var levels = GameEntry.DataTable.GetDataTable<DRInstance>();
            var levelData = levels.GetDataRow(levelId);

            if(m_InstanceMap.ContainsKey(levelId))
                AddOrModifyLevel(levelData);
        }

        public void ModifyLevel(DRInstance levelData)
        {
            if (m_InstanceMap.ContainsKey(levelData.Id))
                AddOrModifyLevel(levelData);
        }

        /// <summary>
        /// 根据当前关卡查找开启的其他关卡ID
        /// 这里只是找到的表里配置的前置关卡ID是CurrentID的，并不代表是下面要开启的关卡
        /// </summary>
        /// <param name="currentId">当前的关卡ID</param>
        /// <returns></returns>
        public List<int> GetNextLevelId(int currentId)
        {
            List<int> nextLevels = new List<int>();

            foreach (var level in m_InstanceMap.Values)
            {
                if (level.LevelConfig.PrerequisiteInstanceId == currentId)
                    nextLevels.Add(level.Id);
            }

            return nextLevels;
        }

        /// <summary>
        /// 获取当前的关卡数据
        /// </summary>
        /// <param name="levelId">想要获得的关卡ID</param>
        /// <returns></returns>
        public InstanceData GetLevelDataById(int levelId)
        {
            InstanceData level = null;
            m_InstanceMap.TryGetValue(levelId, out level);

            return level;
        }
    }
}
