using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        [SerializeField]
        private List<InstanceRegionData> m_RegionsToObserve = new List<InstanceRegionData>();

        // 区域编号到最先进入该区域的角色数据的映射表
        private Dictionary<int, CharacterData> m_RegionIdToFirstArrivers = new Dictionary<int, CharacterData>();

        // 区域编号到进入过该区域的所有 NPC 的索引号的映射表
        private Dictionary<int, HashSet<int>> m_RegionIdToEnteredNpcIndices = new Dictionary<int, HashSet<int>>();

        // 区域编号到处于区域中的实体编号映射表。
        private Dictionary<int, HashSet<int>> m_RegionIdToInRegionTargetables = new Dictionary<int, HashSet<int>>();

        /// <summary>
        /// 获取区域是否从没角色进入过。
        /// </summary>
        /// <param name="regionId">区域编号。</param>
        /// <returns>区域是否从没角色进入过。</returns>
        public bool IsRegionVirgin(int regionId)
        {
            return !m_RegionIdToFirstArrivers.ContainsKey(regionId);
        }

        /// <summary>
        /// 获取第一个进入区域的角色数据。
        /// </summary>
        /// <param name="regionId">区域编号。</param>
        /// <returns>第一个进入区域的角色数据。</returns>
        public CharacterData GetFirstArriverInRegion(int regionId)
        {
            CharacterData ret = null;
            m_RegionIdToFirstArrivers.TryGetValue(regionId, out ret);
            return ret;
        }

        /// <summary>
        /// NPC 是否进入过指定区域。
        /// </summary>
        /// <param name="npcIndex">NPC 索引号。</param>
        /// <param name="regionId">区域编号。</param>
        /// <returns></returns>
        public bool HasNpcEnteredRegion(int npcIndex, int regionId)
        {
            if (!m_RegionIdToEnteredNpcIndices.ContainsKey(regionId))
            {
                return false;
            }

            return m_RegionIdToEnteredNpcIndices[regionId].Contains(npcIndex);
        }

        /// <summary>
        /// 增加观察区域（圆形）。
        /// </summary>
        /// <param name="regionId">区域编号。</param>
        /// <param name="center">区域中心点。</param>
        /// <param name="radius">区域半径。</param>
        /// <returns>是否增加成功。</returns>
        public bool AddRegionToObserve(int regionId, Vector2 center, float radius)
        {
            for (int i = 0; i < m_RegionsToObserve.Count; ++i)
            {
                if (regionId == m_RegionsToObserve[i].Id)
                {
                    return false;
                }
            }

            m_RegionsToObserve.Add(new InstanceRegionData_Circle(regionId, center, radius));
            return true;
        }

        /// <summary>
        /// 增加观察区域（矩形）。
        /// </summary>
        /// <param name="regionId">区域编号。</param>
        /// <param name="center">区域中心点。</param>
        /// <param name="rotation">区域旋转角度。</param>
        /// <param name="width">区域宽度。</param>
        /// <param name="height">区域高度。</param>
        /// <returns></returns>
        public bool AddRegionToObserve(int regionId, Vector2 center, float rotation, float width, float height)
        {
            for (int i = 0; i < m_RegionsToObserve.Count; ++i)
            {
                if (regionId == m_RegionsToObserve[i].Id)
                {
                    return false;
                }
            }

            m_RegionsToObserve.Add(new InstanceRegionData_Rect(regionId, center, rotation, width, height));
            return true;
        }

        /// <summary>
        /// 清除所有观察区域。
        /// </summary>
        public void ClearRegionsToObserve()
        {
            m_RegionsToObserve.Clear();
        }

        /// <summary>
        /// 更新区域 Buff。
        /// </summary>
        /// <param name="regionId">区域编号。</param>
        /// <param name="buffIds">Buff 编号列表。</param>
        /// <param name="camps">阵容类型列表。</param>
        /// <returns>是否成功。</returns>
        public bool UpdateBuffIdsToAddForRegion(int regionId, IList<int> buffIds, IList<CampType> camps)
        {
            for (int i = 0; i < m_RegionsToObserve.Count; ++i)
            {
                var region = m_RegionsToObserve[i];
                if (regionId != region.Id)
                {
                    continue;
                }

                HashSet<int> entityIds;
                if (!m_RegionIdToInRegionTargetables.TryGetValue(regionId, out entityIds))
                {
                    entityIds = new HashSet<int>();
                }

                var targetables = new List<TargetableObject>();
                foreach (int id in entityIds)
                {
                    var rawEntity = GameEntry.Entity.GetEntity(id);
                    if (rawEntity == null)
                    {
                        Log.Warning("Entity '{0}' not found.", id);
                        continue;
                    }

                    targetables.Add(rawEntity.Logic as TargetableObject);
                }

                var oldBuffIds = region.GetBuffIdsToAddOnEnter();
                var oldCamps = region.GetCampsForRegionBuffs();

                for (int j = 0; j < targetables.Count; ++j)
                {
                    var t = targetables[j];
                    if (!AIUtility.TargetCanBeSelected(t))
                    {
                        continue;
                    }

                    if (oldCamps.Contains(t.Camp))
                    {
                        t.RemoveBuffByIds(oldBuffIds);
                    }

                    if (camps.Contains(t.Camp))
                    {
                        for (int k = 0; k < buffIds.Count; ++k)
                        {
                            t.AddBuff(buffIds[k], null, OfflineBuffPool.GetNextSerialId(), null);
                        }
                    }
                }

                region.UpdateBuffIdsToAddOnEnter(buffIds);
                region.UpdateCampsForRegionBuffs(camps);
                return true;
            }

            return false;
        }

        private void CheckRegionContainsTargetable(InstanceRegionData region, TargetableObject targetableObject)
        {
            if (!AIUtility.TargetCanBeSelected(targetableObject) || !region.Contains(targetableObject.CachedTransform.localPosition.ToVector2()))
            {
                return;
            }

            var character = targetableObject as Character;
            var npcCharacter = targetableObject as NpcCharacter;

            if (character)
            {
                CheckFirstArriverForRegion(region, character);
            }

            if (npcCharacter)
            {
                AddEnteredNpcToRegion(region, npcCharacter);
            }
        }

        private void CheckFirstArriverForRegion(InstanceRegionData region, Character character)
        {
            if (!m_RegionIdToFirstArrivers.ContainsKey(region.Id))
            {
                m_RegionIdToFirstArrivers.Add(region.Id, character.Data);
            }
        }

        private void AddEnteredNpcToRegion(InstanceRegionData region, NpcCharacter npc)
        {
            HashSet<int> npcIndices = null;
            if (!m_RegionIdToEnteredNpcIndices.TryGetValue(region.Id, out npcIndices))
            {
                npcIndices = new HashSet<int>();
                m_RegionIdToEnteredNpcIndices.Add(region.Id, npcIndices);
            }

            int npcIndex = GetNpcIndex(npc.Id);
            m_RegionIdToEnteredNpcIndices[region.Id].Add(npcIndex);
        }

        private void UpdateTargetableObjectInRegion(TargetableObject targetable, InstanceRegionData region)
        {
            var entityId = targetable.Id;
            HashSet<int> entityIdsInRegion = null;
            m_RegionIdToInRegionTargetables.TryGetValue(region.Id, out entityIdsInRegion);

            bool isEnteringRegion = false;

            if (entityIdsInRegion == null)
            {
                entityIdsInRegion = new HashSet<int>();
                m_RegionIdToInRegionTargetables.Add(region.Id, entityIdsInRegion);
                entityIdsInRegion.Add(entityId);
                isEnteringRegion = true;
            }
            else if (!entityIdsInRegion.Contains(entityId))
            {
                entityIdsInRegion.Add(entityId);
                isEnteringRegion = true;
            }

            if (isEnteringRegion)
            {
                OnTargetableEnterRegion(region, targetable);
            }
        }

        private void UpdateTargetableOutOfRegion(TargetableObject targetable, InstanceRegionData region)
        {
            var entityId = targetable.Id;
            HashSet<int> entityIdsInRegion = null;
            m_RegionIdToInRegionTargetables.TryGetValue(region.Id, out entityIdsInRegion);

            bool isLeavingRegion = false;

            if (entityIdsInRegion == null)
            {
                isLeavingRegion = false;
            }
            else if (entityIdsInRegion.Contains(entityId))
            {
                entityIdsInRegion.Remove(entityId);
                isLeavingRegion = true;
            }

            if (isLeavingRegion)
            {
                OnTargetableLeaveRegion(region, targetable);
            }
        }

        private void OnTargetableEnterRegion(InstanceRegionData region, TargetableObject targetable)
        {
            CheckRegionContainsTargetable(region, targetable);

            var buffIdsToAdd = region.GetBuffIdsToAddOnEnter();
            var camps = region.GetCampsForRegionBuffs();

            if (!camps.Contains(targetable.Camp))
            {
                return;
            }

            for (int i = 0; i < buffIdsToAdd.Count; ++i)
            {
                targetable.AddBuff(buffIdsToAdd[i], null, OfflineBuffPool.GetNextSerialId(), null);
            }
        }

        private void OnTargetableLeaveRegion(InstanceRegionData region, TargetableObject targetable)
        {
            CheckRegionContainsTargetable(region, targetable);

            var camps = region.GetCampsForRegionBuffs();

            if (!camps.Contains(targetable.Camp))
            {
                return;
            }

            targetable.RemoveBuffByIds(region.GetBuffIdsToAddOnEnter());
        }
    }
}
