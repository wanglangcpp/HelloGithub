using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本的数据。每当副本数据发生变化的时候，需要调用一下ModifyChapterData以保证数据的正确性
    /// GameEntry.Data.InstanceGroups.ModifyChapterData
    /// </summary>
    public class InstanceGroupsData
    {
        /// <summary>
        /// 副本组数据
        /// Key:副本章节ID
        /// Value:章节数据
        /// </summary>
        private Dictionary<int, InstanceGroupData> m_ChapterMap = new Dictionary<int, InstanceGroupData>();

        /// <summary>
        /// 副本章节的开启顺序，按Index排序的。
        /// </summary>
        private List<int> m_SortedChapterId = new List<int>();

        private bool m_Initialized = false;
        /// <summary>
        /// 初始化副本数据。
        /// 这个初始化没有做成实例化的时候初始化是因为这里面的数据用到了服务器下发的关卡数据，因此时机把握不好很难掌控
        /// 所以在用副本的数据的时候，需要调用一下InsureInitialized方法，以确保数据是正确的
        /// </summary>
        private void InitGroupsData()
        {
            var chapters = GameEntry.DataTable.GetDataTable<DRInstanceGroup>().GetAllDataRows();

            for (int i = 0; i < chapters.Length; i++)
            {
                InstanceGroupData chapter = new InstanceGroupData(chapters[i]);
                m_ChapterMap[chapter.ChapterId] = chapter;
                if (!m_SortedChapterId.Contains(chapter.ChapterId))
                    m_SortedChapterId.Add(chapter.ChapterId);
                else
                    Log.Warning("The chapter id '{0}' is reduplicative.", chapter.ChapterId);
            }

            m_SortedChapterId.Sort((n1, n2) => { return n1.CompareTo(n2); });

            var levels = GameEntry.DataTable.GetDataTable<DRInstance>().GetAllDataRows();
            for (int i = 0; i < levels.Length; i++)
            {
                InstanceGroupData chapter = null;
                if (m_ChapterMap.TryGetValue(levels[i].InstanceGroupId, out chapter))
                    chapter.AddOrModifyLevel(levels[i]);
            }

            m_Initialized = true;
        }

        /// <summary>
        /// 保证数据是初始化过的。
        /// </summary>
        private void InsureInitialized()
        {
            if (!m_Initialized)
                InitGroupsData();
        }

        /// <summary>
        /// 判断某一个章节是否通关
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <returns>True:通关，False:未通关</returns>
        public bool IsChapterDone(int chapterId)
        {
            InsureInitialized();

            InstanceGroupData chapter = null;
            if (m_ChapterMap.TryGetValue(chapterId, out chapter))
                return chapter.IsDone;
            else
                return false;
        }

        /// <summary>
        /// 判断某一章节是否开启
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <returns>True:开启，False：未开启</returns>
        public bool IsChapterOpen(int chapterId)
        {
            InsureInitialized();

            InstanceGroupData chapter = null;
            if (m_ChapterMap.TryGetValue(chapterId, out chapter))
                return chapter.IsOpen;
            else
                return false;
        }

        /// <summary>
        /// 获取某一个章节的数据
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <returns></returns>
        public InstanceGroupData GetChapterDataById(int chapterId)
        {
            InsureInitialized();

            InstanceGroupData chapter = null;
            m_ChapterMap.TryGetValue(chapterId, out chapter);

            return chapter;
        }

        /// <summary>
        /// 刷新某一个章节的数据
        /// </summary>
        /// <param name="levelId"></param>
        public void ModifyLevel(int levelId)
        {
            var level = GameEntry.DataTable.GetDataTable<DRInstance>().GetDataRow(levelId);
            if (level.InstanceType != 0 && level.InstanceType != 1)
            {
                return;
            }
            var chapter = GetChapterDataById(level.InstanceGroupId);
            chapter.ModifyLevel(level);

            if (level.Id == chapter.FirstLevelData.Id)
                chapter.RefreshFirstLevel();
        }

        public InstanceGroupData.InstanceData GetLevelById(int levelId)
        {
            var level = GameEntry.DataTable.GetDataTable<DRInstance>().GetDataRow(levelId);
            var chapter = GetChapterDataById(level.InstanceGroupId);
            return chapter.GetLevelDataById(levelId);
        }

        public InstanceGroupData GetCurrentChapterData()
        {
            InsureInitialized();

            for (int i = 0; i < m_SortedChapterId.Count; i++)
            {
                var data = GetChapterDataById(m_SortedChapterId[i]);
                if (!data.IsDone)
                    return data;
            }

            return m_ChapterMap[m_SortedChapterId[m_SortedChapterId.Count - 1]];
        }

        /// <summary>
        /// 刷新章节中某一关的数据
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="levelId">关卡ID</param>
        public void ModifyChapterData(int chapterId, int levelId)
        {
            InsureInitialized();

            InstanceGroupData chapter = null;
            if (m_ChapterMap.TryGetValue(chapterId, out chapter))
                chapter.ModifyLevel(levelId);
        }

        /// <summary>
        /// Key: 章节ID
        /// Value: 每一位代表一个状态，第0位代表第一个宝箱的状态，0为未领取，1为已领取
        /// </summary>
        private Dictionary<int, int> m_ChapterChestMap = new Dictionary<int, int>();

        public void InitializeChestData(List<PBInstanceGroupChestStatus> chests)
        {
            m_ChapterChestMap.Clear();

            for (int i = 0; i < chests.Count; i++)
                m_ChapterChestMap[chests[i].Id] = chests[i].ChestStatus;
        }

        /// <summary>
        /// 设置宝箱的领取状态。
        /// </summary>
        /// <param name="chapterId">副本章节ID</param>
        /// <param name="chestIndex">宝箱的序号，0开始计数</param>
        /// <param name="isPicked">是否已经领取了，True为已经领取了</param>
        public void SetChestStatus(int chapterId, int chestIndex, bool isPicked)
        {
            int chestsStatus = 0;
            if (m_ChapterChestMap.TryGetValue(chapterId, out chestsStatus))
                m_ChapterChestMap[chapterId] = chestsStatus | ((isPicked ? 1 : 0) << chestIndex);
            else
                m_ChapterChestMap[chapterId] = (isPicked ? 1 : 0) << chestIndex;  //如果不存在Key、说明当前章节没有领取过数据，那么添加上这个章节的数据在字典里
        }

        /// <summary>
        /// 获取章节宝箱的状态。
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="chestIndex">宝箱序号，0开始计数</param>
        /// <returns>True:已经领取过奖励了。False:还未领取奖励</returns>
        public bool GetChestStatus(int chapterId, int chestIndex)
        {
            int chestsStatus = 0;
            if (m_ChapterChestMap.TryGetValue(chapterId, out chestsStatus))
                return ((chestsStatus >> chestIndex) & 0x1) == 1;

            return false;
        }
    }
}
