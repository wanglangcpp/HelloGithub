using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class OnlineBuffPool : BaseBuffPool
    {
        public OnlineBuffPool(IBuffTargetData target, IDataTable<DRBuff> buffDataTable, IDataTable<DRBuffReplace> replaceDataTable, int capacity,
            Predicate<BuffData> customRuleForAdding, GameFrameworkFunc<BuffData, IList<BuffData>> customRuleForRemoving)
            : base(target, buffDataTable, replaceDataTable, capacity, customRuleForAdding, customRuleForRemoving)
        {
        }

        public override void Add(int buffId, EntityData ownerData, long buffSerialId, float startTime, object userData = null)
        {
            var data = TargetData as TargetableObjectData;

            // TODO: 如果需要，传递 userData 中的技能徽章部分给服务器。
            GameEntry.RoomLogic.AddBuff(ownerData == null ? 0 : ownerData.Id, data.Id, buffId);
        }

        public override void Clear()
        {
            base.Clear();
        }

        public void AddOfflineBuff(int buffId, EntityData ownerData, long buffSerialId, float startTime)
        {
            base.Add(buffId, ownerData, buffSerialId, startTime);
        }

        public void AddBuffList(List<PBBuffInfo> buffs, EntityData ownerData)
        {
            if (buffs == null || buffs.Count == 0)
            {
                return;
            }

            DateTime now = GameEntry.Time.LobbyServerUtcTime;
            for (int i = 0; i < buffs.Count; i++)
            {
                DateTime startTime = new DateTime(buffs[i].BuffStartTime, DateTimeKind.Utc);
                var time = Time.time - (float)(now - startTime).TotalSeconds;
                base.Add(buffs[i].BuffId, ownerData, buffs[i].SerialId, time);
            }
        }

        public override void RemoveByType(BuffType type)
        {
            base.RemoveByType(type);
            RemoveBuffsFromServer(s_ToRemove);
        }

        public override void RemoveByIds(IList<int> buffIds)
        {
            base.RemoveByIds(buffIds);
            RemoveBuffsFromServer(s_ToRemove);
        }

        public void RemoveBuffByIdsFromServer(List<PBBuffInfo> buffs)
        {
            s_ToRemove.Clear();
            var buffIds = new List<int>();
            for (int i = 0; i < buffs.Count; i++)
            {
                buffIds.Add(buffs[i].SerialId);
            }

            for (int i = 0; i < TargetData.Buffs.Count; ++i)
            {
                var buff = TargetData.Buffs[i];

                if (buffIds.Contains((int)buff.SerialId))
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

        protected override IList<BuffData> RemoveTimeoutBuffs(float currentTime)
        {
            var buffs = base.RemoveTimeoutBuffs(currentTime);
            RemoveBuffsFromServer(s_ToRemove);
            return buffs;
        }

        protected override bool CheckAndAddBuffWhenFull(BuffData buffData, out BuffData added, List<BuffData> removed)
        {
            bool result = base.CheckAndAddBuffWhenFull(buffData, out added, removed);
            if (result)
            {
                var buffs = new List<BuffData>();
                buffs.Add(m_EarliestStartBuff);
                RemoveBuffsFromServer(buffs);
            }
            return result;
        }

        protected override bool PerformExclusion(BuffData buffData, BuffData excluded)
        {
            bool result = base.PerformExclusion(buffData, excluded);
            if (result)
            {
                var buffs = new List<BuffData>();
                buffs.Add(excluded);
                RemoveBuffsFromServer(buffs);
            }
            return result;
        }

        protected override IList<BuffData> RemoveByIdInternal(int buffId)
        {
            var buff = base.RemoveByIdInternal(buffId);
            RemoveBuffsFromServer(s_ToRemove);
            return buff;
        }

        private void RemoveBuffsFromServer(List<BuffData> buffs)
        {
            if (buffs == null || buffs.Count == 0)
            {
                return;
            }
            int entityId = buffs[0].OwnerData.Id;
            List<PBBuffInfo> buffRemoves = new List<PBBuffInfo>();
            for (int i = 0; i < buffs.Count; i++)
            {
                PBBuffInfo info = new PBBuffInfo();
                info.BuffId = buffs[i].BuffId;
                info.SerialId = (int)buffs[i].SerialId;
                buffRemoves.Add(info);
            }
            GameEntry.RoomLogic.RemoveBuff(entityId, entityId, buffRemoves);
        }
    }
}
