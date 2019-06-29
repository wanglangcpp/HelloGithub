using GameFramework;
using GameFramework.DataTable;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class BaseBuffPool : IBuffPool
    {
        private static long m_NextSerialId = -1;

        public static long GetNextSerialId()
        {
            return m_NextSerialId--;
        }

        public IBuffTargetData TargetData
        {
            get;
            private set;
        }

        public int Capacity
        {
            get;
            private set;
        }

        protected IDataTable<DRBuff> m_BuffDataTable = null;
        private IDataTable<DRBuffReplace> m_BuffReplaceDataTable = null;
        protected BuffData m_EarliestStartBuff = null;
        private Predicate<BuffData> m_CheckCustomRuleForAdding = null;
        private GameFrameworkFunc<BuffData, IList<BuffData>> m_CheckCustomRuleForRemoving = null;

        protected static List<BuffData> s_ToRemove = new List<BuffData>();

        public BaseBuffPool(IBuffTargetData targetData, IDataTable<DRBuff> buffDataTable, IDataTable<DRBuffReplace> replaceDataTable, int capacity,
            Predicate<BuffData> customRuleForAdding, GameFrameworkFunc<BuffData, IList<BuffData>> customRuleForRemoving)
        {
            TargetData = targetData;
            Capacity = capacity;
            m_BuffDataTable = buffDataTable;
            m_BuffReplaceDataTable = replaceDataTable;
            SetCustomRuleForAdding(customRuleForAdding);
            SetCustomRuleForRemoving(customRuleForRemoving);
            UpdateEarliestStartBuff();
        }

        public BuffData[] Buffs
        {
            get
            {
                return (TargetData.Buffs as List<BuffData>).ToArray();
            }
        }

        public void Update(float currentTime)
        {
            var removed = RemoveTimeoutBuffs(currentTime);

            if (removed.Count > 0)
            {
                TargetData.OnBuffPoolChanged(null, removed);
            }

            CheckBuffHeartBeat(currentTime);
        }

        public virtual void Add(int buffId, EntityData ownerData, long buffSerialId, float startTime, object userData = null)
        {
            DRBuff drBuff = m_BuffDataTable.GetDataRow(buffId);
            if (drBuff == null)
            {
                Log.Fatal("Buff ID {0} doesn't exist in data table.", buffId.ToString());
                return;
            }

            BuffData buffData = new BuffData(drBuff, ownerData, buffSerialId, startTime, -1, userData);
            AddInternal(buffData);
        }

        protected void SetCustomRuleForAdding(Predicate<BuffData> customRuleForAdding)
        {
            m_CheckCustomRuleForAdding = customRuleForAdding;
        }

        protected void SetCustomRuleForRemoving(GameFrameworkFunc<BuffData, IList<BuffData>> customRuleForRemoving)
        {
            m_CheckCustomRuleForRemoving = customRuleForRemoving;
        }

        public void AddTransferred(BuffData buffData)
        {
            var newData = new BuffData(buffData.DRBuff, buffData.OwnerData, buffData.SerialId, buffData.StartTime, -1);
            AddInternal(newData);
        }

        protected void AddInternal(BuffData buffData)
        {
            if (CheckAndAddBuffIfEmpty(buffData)) // Buff 池为空，可直接添加 Buff。
            {
                return;
            }

            if (!CheckCustomRuleForAdding(buffData)) // 自定义规则导致无法添加 Buff。
            {
                return;
            }

            BuffData added;
            List<BuffData> removed = new List<BuffData>();
            if (CheckAndPerformReplacement(buffData, out added, removed) ||
                CheckAndPerformExclusion(buffData, out added, removed) ||
                CheckAndAddBuffWhenNotFull(buffData, out added, removed) ||
                CheckAndAddBuffWhenFull(buffData, out added, removed))
            {
                var toRemove = CheckCustomRuleForRemoving(added);

                if (toRemove != null)
                {
                    for (int i = 0; i < toRemove.Count; i++)
                    {
                        TargetData.Buffs.Remove(toRemove[i]);
                    }

                    removed.AddRange(toRemove);
                }

                if (added != null || removed.Count > 0)
                {
                    TargetData.OnBuffPoolChanged(added, removed);
                }
            }
        }

        private IList<BuffData> CheckCustomRuleForRemoving(BuffData added)
        {
            if (m_CheckCustomRuleForRemoving == null)
            {
                return null;
            }

            return m_CheckCustomRuleForRemoving(added);
        }

        private bool CheckCustomRuleForAdding(BuffData buffData)
        {
            if (m_CheckCustomRuleForAdding == null)
            {
                return true;
            }

            return m_CheckCustomRuleForAdding(buffData);
        }

        public virtual void RemoveByType(BuffType type)
        {
            s_ToRemove.Clear();
            for (int i = 0; i < TargetData.Buffs.Count; ++i)
            {
                var buff = TargetData.Buffs[i];

                if (buff.BuffType == type)
                {
                    s_ToRemove.Add(buff);
                }
            }

            for (int i = 0; i < s_ToRemove.Count; ++i)
            {
                TargetData.Buffs.Remove(s_ToRemove[i]);
            }

            if (s_ToRemove.Count > 0)
            {
                UpdateEarliestStartBuff();
                TargetData.OnBuffPoolChanged(null, new List<BuffData>(s_ToRemove));
            }
        }

        public virtual void RemoveByIds(IList<int> buffIds)
        {
            s_ToRemove.Clear();
            for (int i = 0; i < TargetData.Buffs.Count; ++i)
            {
                var buff = TargetData.Buffs[i];

                if (buffIds.Contains(buff.BuffId))
                {
                    s_ToRemove.Add(buff);
                }
            }

            for (int i = 0; i < s_ToRemove.Count; ++i)
            {
                TargetData.Buffs.Remove(s_ToRemove[i]);
            }

            if (s_ToRemove.Count > 0)
            {
                UpdateEarliestStartBuff();
                TargetData.OnBuffPoolChanged(null, new List<BuffData>(s_ToRemove));
            }
        }

        public BuffData GetByType(BuffType buffType)
        {
            for (int i = 0; i < TargetData.Buffs.Count; ++i)
            {
                var buffData = TargetData.Buffs[i];
                if (buffType == buffData.BuffType)
                {
                    return buffData;
                }
            }

            return null;
        }

        public BuffData GetById(int id)
        {
            for (int i = 0; i < TargetData.Buffs.Count; ++i)
            {
                var buffData = TargetData.Buffs[i];
                if (id == buffData.BuffId)
                {
                    return buffData;
                }
            }

            return null;
        }

        public virtual void Clear()
        {
            var buffs = new List<BuffData>(TargetData.Buffs);
            TargetData.Buffs.Clear();
            if (TargetData != null)
            {
                TargetData.OnBuffPoolChanged(null, buffs);
            }
        }

        private void CheckBuffHeartBeat(float currentTime)
        {
            for (int i = 0; i < TargetData.Buffs.Count; ++i)
            {
                var buff = TargetData.Buffs[i];

                if (buff.HeartBeat <= 0f)
                {
                    continue;
                }

                if (currentTime > buff.LastHeartBeatTime + buff.HeartBeat)
                {
                    buff.LastHeartBeatTime = currentTime;
                    TargetData.OnBuffHeartBeat(buff.SerialId, buff);
                }
            }
        }

        protected virtual IList<BuffData> RemoveTimeoutBuffs(float currentTime)
        {
            s_ToRemove.Clear();
            for (int i = 0; i < TargetData.Buffs.Count; ++i)
            {
                var buff = TargetData.Buffs[i];
                if (currentTime > buff.EndTime)
                {
                    s_ToRemove.Add(buff);
                }
            }

            for (int i = 0; i < s_ToRemove.Count; ++i)
            {
                var buff = s_ToRemove[i];
                TargetData.Buffs.Remove(buff);
                if (buff == m_EarliestStartBuff)
                {
                    UpdateEarliestStartBuff();
                }
            }

            return new List<BuffData>(s_ToRemove);
        }

        private bool CheckAndAddBuffIfEmpty(BuffData buffData)
        {
            if (TargetData.Buffs.Count > 0)
            {
                return false;
            }

            TargetData.Buffs.Add(buffData);
            m_EarliestStartBuff = buffData;
            TargetData.OnBuffPoolChanged(buffData, null);
            return true;
        }

        private bool CheckAndPerformReplacement(BuffData buffData, out BuffData toAdd, List<BuffData> removed)
        {
            bool replaceAny = false;
            toAdd = null;
            BuffData replaceBuffData;
            while (buffData != null && ShouldReplace(buffData, out replaceBuffData))
            {
                removed.AddRange(RemoveByIdInternal(buffData.BuffId));
                replaceAny = true;

                BuffData excluded;
                if (ShouldTryExclusion(replaceBuffData, out excluded))
                {
                    if (PerformExclusion(replaceBuffData, excluded))
                    {
                        toAdd = replaceBuffData;
                        removed.Add(excluded);
                    }
                    else
                    {
                        toAdd = null;
                    }
                }
                else
                {
                    toAdd = replaceBuffData;
                }

                buffData = toAdd;
            }

            if (replaceAny)
            {
                if (toAdd != null)
                {
                    TargetData.Buffs.Add(toAdd);
                }
            }

            return replaceAny;
        }

        private bool CheckAndPerformExclusion(BuffData buffData, out BuffData added, List<BuffData> removed)
        {
            BuffData excluded;
            added = null;

            if (ShouldTryExclusion(buffData, out excluded))
            {
                if (PerformExclusion(buffData, excluded))
                {
                    added = buffData;
                    removed.Add(excluded);
                }

                return true;
            }

            return false;
        }

        private bool CheckAndAddBuffWhenNotFull(BuffData buffData, out BuffData added, List<BuffData> removed)
        {
            added = null;
            if (TargetData.Buffs.Count >= Capacity)
            {
                return false;
            }

            TargetData.Buffs.Add(buffData);
            if (m_EarliestStartBuff.StartTime > buffData.StartTime)
            {
                m_EarliestStartBuff = buffData;
            }

            added = buffData;
            return true;
        }

        protected virtual bool CheckAndAddBuffWhenFull(BuffData buffData, out BuffData added, List<BuffData> removed)
        {
            added = null;
            if (m_EarliestStartBuff.StartTime > buffData.StartTime)
            {
                return false;
            }

            //Log.Warning("Buff pool capacity reached.");
            TargetData.Buffs.Remove(m_EarliestStartBuff);
            TargetData.Buffs.Add(buffData);
            UpdateEarliestStartBuff();
            added = buffData;
            removed.Add(m_EarliestStartBuff);
            return true;
        }

        private bool ShouldTryExclusion(BuffData newBuff, out BuffData excluded)
        {
            excluded = null;

            if (newBuff.ExcludingGroup == DRBuff.NonExcludingGroup)
            {
                return false;
            }

            for (int i = 0; i < TargetData.Buffs.Count; ++i)
            {
                var buff = TargetData.Buffs[i];
                if (newBuff.ExcludingGroup == buff.ExcludingGroup)
                {
                    if (newBuff.ExcludingLevel > buff.ExcludingLevel)
                    {
                        excluded = buff;
                    }
                    else if (newBuff.ExcludingLevel < buff.ExcludingLevel)
                    {
                        excluded = newBuff;
                    }
                    else
                    {
                        excluded = newBuff.StartTime >= buff.StartTime ? buff : newBuff;
                    }
                    return true;
                }
            }

            return false;
        }

        private bool ShouldReplace(BuffData buff, out BuffData newBuff)
        {
            newBuff = null;

            if (m_BuffReplaceDataTable == null)
            {
                return false;
            }

            DRBuffReplace replaceDataRow = m_BuffReplaceDataTable.GetDataRow(buff.BuffId);
            if (replaceDataRow == null)
            {
                return false;
            }

            if (GetCountByBuffIdAndOwnerId(buff.BuffId, buff.OwnerData) + 1 < replaceDataRow.NeededCount)
            {
                return false;
            }

            DRBuff newBuffDataRow = m_BuffDataTable.GetDataRow(replaceDataRow.TargetBuffId);
            if (newBuffDataRow == null)
            {
                throw new Exception(string.Format("Buff '{0}' not found", replaceDataRow.TargetBuffId));
            }

            newBuff = new BuffData(newBuffDataRow, buff.OwnerData, GetNextSerialId(), buff.StartTime, -1);
            return true;
        }

        protected virtual bool PerformExclusion(BuffData buffData, BuffData excluded)
        {
            if (buffData == excluded)
            {
                return false;
            }

            TargetData.Buffs.Remove(excluded);
            TargetData.Buffs.Add(buffData);
            UpdateEarliestStartBuff();
            return true;
        }

        protected void UpdateEarliestStartBuff()
        {
            m_EarliestStartBuff = null;
            for (int i = 0; i < TargetData.Buffs.Count; ++i)
            {
                var buff = TargetData.Buffs[i];

                if (m_EarliestStartBuff == null)
                {
                    m_EarliestStartBuff = buff;
                    continue;
                }

                if (m_EarliestStartBuff.StartTime > buff.StartTime)
                {
                    m_EarliestStartBuff = buff;
                }
            }
        }

        protected virtual IList<BuffData> RemoveByIdInternal(int buffId)
        {
            s_ToRemove.Clear();
            for (int i = 0; i < TargetData.Buffs.Count; ++i)
            {
                var buff = TargetData.Buffs[i];

                if (buff.BuffId == buffId)
                {
                    s_ToRemove.Add(buff);
                }
            }

            for (int i = 0; i < s_ToRemove.Count; ++i)
            {
                TargetData.Buffs.Remove(s_ToRemove[i]);
            }

            return new List<BuffData>(s_ToRemove);
        }

        private int GetCountByBuffIdAndOwnerId(int buffId, EntityData ownerData)
        {
            int ret = 0;
            for (int i = 0; i < TargetData.Buffs.Count; ++i)
            {
                var currentOwnerData = TargetData.Buffs[i].OwnerData;
                if (TargetData.Buffs[i].BuffId == buffId && currentOwnerData == ownerData)
                {
                    ret++;
                }
            }

            return ret;
        }
    }
}
