using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
namespace Genesis.GameClient
{
    public class InstanceForBossData
    {
        public InstanceForBossData()
        {

        }
        private const int freeChallengeCount = 3;

        private IDataTable<DRInstanceGroupForBoss> dataTable;

        private Dictionary<int, List<DRInstance>> dicChapter;
        private Dictionary<int, List<DRInstance>> dicChapterElite;

        public IDataTable<DRInstanceGroupForBoss> DataTable { get { return dataTable; } }

        private Dictionary<int, int> dicInstanceStarCount = new Dictionary<int, int>();
        public Dictionary<int, int> DicInstanceStarCount { get { return dicInstanceStarCount; } }

        public Dictionary<int, int> DicInstanceChallengeCount = new Dictionary<int, int>();

        private Dictionary<int, int> dicChapterChestStatus = new Dictionary<int, int>();
        public Dictionary<int, int> DicChapterChestStatus { get { return dicChapterChestStatus; } }

        public int GetInstanceChallengeCount(int instanceId)
        {
            if (DicInstanceChallengeCount.ContainsKey(instanceId))
            {
                return freeChallengeCount - DicInstanceChallengeCount[instanceId];
            }
            return freeChallengeCount;
        }
        public int GetInstanceInstanceStarCount(int instanceId)
        {
            if (dicInstanceStarCount.ContainsKey(instanceId))
                return dicInstanceStarCount[instanceId];
            else
                return 0;
        }

        public void InitData()
        {
            if (dataTable != null)
            {
                return;
            }
            dataTable = GameEntry.DataTable.GetDataTable<DRInstanceGroupForBoss>();
            DRInstance[] tableInstance = GameEntry.DataTable.GetDataTable<DRInstance>().GetAllDataRows();
            dicChapter = new Dictionary<int, List<DRInstance>>();
            dicChapterElite = new Dictionary<int, List<DRInstance>>();
            DRInstanceGroupForBoss[] allRows = dataTable.GetAllDataRows();
            foreach (var row in allRows)
            {
                if (!dicChapter.ContainsKey(row.Id))
                {
                    dicChapter[row.Id] = new List<DRInstance>();
                }
                if (!dicChapterElite.ContainsKey(row.Id))
                {
                    dicChapterElite[row.Id] = new List<DRInstance>();
                }
            }
            foreach (var instance in tableInstance)
            {
                if (dicChapter.ContainsKey(instance.InstanceGroupId) && instance.InstanceType == 2)
                {
                    dicChapter[instance.InstanceGroupId].Add(instance);
                    continue;
                }
                if (dicChapterElite.ContainsKey(instance.InstanceGroupId) && instance.InstanceType == 3)
                {
                    dicChapterElite[instance.InstanceGroupId].Add(instance);
                    continue;
                }
            }
        }
        /// <summary>
        /// 2-boss 3-boss精英
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="instanceType"></param>
        /// <returns></returns>
        public List<DRInstance> GetDatas(int chapterId, int instanceType)
        {
            if (dataTable == null)
            {
                return null;
            }
            if (instanceType == 2)
            {
                return dicChapter[chapterId];
            }
            else if (instanceType == 3)
            {
                return dicChapterElite[chapterId];
            }
            return null;
        }

        public void UpdataChestStatusData(List<PBInstanceForBossGroupChestStatus> chastStatus)
        {
            foreach (var item in chastStatus)
            {
                if (!dicChapterChestStatus.ContainsKey(item.Id))
                {
                    dicChapterChestStatus.Add(item.Id, item.ChestStatus);
                }
                else
                {
                    dicChapterChestStatus[item.Id] = item.ChestStatus;
                }
            }

        }
        public void UpdataChallengeCount(List<PBInstanceForGroupBossJoinCountsInfo> challengeCount)
        {
            foreach (var item in challengeCount)
            {
                if (!DicInstanceChallengeCount.ContainsKey(item.Id))
                {
                    DicInstanceChallengeCount.Add(item.Id, item.Count);
                }
                else
                {
                    DicInstanceChallengeCount[item.Id] = item.Count;
                }
            }
        }
        public void UpdataStarCount(List<PBInstanceForGroupBossProgressInfo> starCount)
        {
            foreach (var item in starCount)
            {
                if (!dicInstanceStarCount.ContainsKey(item.Id))
                {
                    dicInstanceStarCount.Add(item.Id, item.StarCount);
                }
                else
                {
                    dicInstanceStarCount[item.Id] = item.StarCount;
                }
            }
        }
    }

}