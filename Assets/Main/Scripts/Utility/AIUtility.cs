using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// AI 工具类。
    /// </summary>
    public static class AIUtility
    {
        private static IDictionary<KeyValuePair<CampType, RelationType>, CampType[]> s_CampAndRelationToCamps = new Dictionary<KeyValuePair<CampType, RelationType>, CampType[]>();

        private struct CampPair
        {
            public CampType First { get; set; }
            public CampType Second { get; set; }
        }

        private static IDictionary<CampPair, RelationType> s_CampPairToRelation = new Dictionary<CampPair, RelationType>();

        static AIUtility()
        {
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player, Second = CampType.Player }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player, Second = CampType.Enemy }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player, Second = CampType.Neutral }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player, Second = CampType.Player2 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player, Second = CampType.Enemy2 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player, Second = CampType.Neutral2 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player, Second = CampType.PlayerFriend }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player, Second = CampType.Player3 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player, Second = CampType.Player4 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player, Second = CampType.Player5 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player, Second = CampType.Player6 }, RelationType.Hostile);

            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy, Second = CampType.Enemy }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy, Second = CampType.Neutral }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy, Second = CampType.Player2 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy, Second = CampType.Enemy2 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy, Second = CampType.Neutral2 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy, Second = CampType.PlayerFriend }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy, Second = CampType.Player3 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy, Second = CampType.Player4 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy, Second = CampType.Player5 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy, Second = CampType.Player6 }, RelationType.Hostile);

            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral, Second = CampType.Neutral }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral, Second = CampType.Player2 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral, Second = CampType.Enemy2 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral, Second = CampType.Neutral2 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral, Second = CampType.PlayerFriend }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral, Second = CampType.Player3 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral, Second = CampType.Player4 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral, Second = CampType.Player5 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral, Second = CampType.Player6 }, RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair { First = CampType.Player2, Second = CampType.Player2 }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player2, Second = CampType.Enemy2 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player2, Second = CampType.Neutral2 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player2, Second = CampType.PlayerFriend }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player2, Second = CampType.Player3 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player2, Second = CampType.Player4 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player2, Second = CampType.Player5 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player2, Second = CampType.Player6 }, RelationType.Hostile);

            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy2, Second = CampType.Enemy2 }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy2, Second = CampType.Neutral2 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy2, Second = CampType.PlayerFriend }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy2, Second = CampType.Player3 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy2, Second = CampType.Player4 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy2, Second = CampType.Player5 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Enemy2, Second = CampType.Player6 }, RelationType.Hostile);

            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral2, Second = CampType.Neutral2 }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral2, Second = CampType.PlayerFriend }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral2, Second = CampType.Player3 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral2, Second = CampType.Player4 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral2, Second = CampType.Player5 }, RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Neutral2, Second = CampType.Player6 }, RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair { First = CampType.PlayerFriend, Second = CampType.PlayerFriend }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.PlayerFriend, Second = CampType.Player3 }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.PlayerFriend, Second = CampType.Player4 }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.PlayerFriend, Second = CampType.Player5 }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.PlayerFriend, Second = CampType.Player6 }, RelationType.Friendly);

            s_CampPairToRelation.Add(new CampPair { First = CampType.Player3, Second = CampType.Player3 }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player3, Second = CampType.Player4 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player3, Second = CampType.Player5 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player3, Second = CampType.Player6 }, RelationType.Hostile);

            s_CampPairToRelation.Add(new CampPair { First = CampType.Player4, Second = CampType.Player4 }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player4, Second = CampType.Player5 }, RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player4, Second = CampType.Player6 }, RelationType.Hostile);

            s_CampPairToRelation.Add(new CampPair { First = CampType.Player5, Second = CampType.Player5 }, RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair { First = CampType.Player5, Second = CampType.Player6 }, RelationType.Hostile);

            s_CampPairToRelation.Add(new CampPair { First = CampType.Player6, Second = CampType.Player6 }, RelationType.Friendly);
        }

        /// <summary>
        /// 获取平面坐标在地图上的三维位置。
        /// </summary>
        /// <param name="position">平面坐标。</param>
        /// <returns>平面坐标在地图上的三维位置。</returns>
        public static Vector3 SamplePosition(Vector2 position, bool allowCarvedPoint = false)
        {
            Vector3 result;
            if (!TrySamplePosition(position, allowCarvedPoint, out result))
            {
                Log.Info("Can not find raycast hit position at '{0}'.", position.ToString());
                return position.ToVector3();
            }

            return result;
        }

        /// <summary>
        /// 尝试获取平面坐标在地图上的三维位置。
        /// </summary>
        /// <param name="position">平面坐标。</param>
        /// <param name="allowCarvedPoint">是否允许使用导航网格上被动态消除的部分。</param>
        /// <param name="result">平面坐标在地图上的三维位置。</param>
        /// <returns></returns>
        public static bool TrySamplePosition(Vector2 position, bool allowCarvedPoint, out Vector3 result)
        {
            result = Vector3.zero;
            RaycastHit raycastHit;
            if (!Physics.Raycast(position.ToVector3(50f), Vector3.down, out raycastHit, 100f, LayerMask.GetMask(Constant.Layer.StaticColliderLayerName)))
            {
                return false;
            }

            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(raycastHit.point, out navMeshHit, 1f, NavMesh.AllAreas))
            {
                result = navMeshHit.position;
                return true;
            }

            if (allowCarvedPoint)
            {
                result = raycastHit.point;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取两个阵营之间的关系。
        /// </summary>
        /// <param name="first">阵营一</param>
        /// <param name="second">阵营二</param>
        /// <returns>阵营间关系</returns>
        public static RelationType GetRelation(CampType first, CampType second)
        {
            if (first > second)
            {
                var tmp = first;
                first = second;
                second = tmp;
            }

            return s_CampPairToRelation[new CampPair { First = first, Second = second }];
        }

        /// <summary>
        /// 获取和指定具有特定关系的所有阵营。
        /// </summary>
        /// <param name="camp">指定阵营</param>
        /// <param name="relation">关系</param>
        /// <returns>满足条件的阵营数组</returns>
        public static CampType[] GetCamps(CampType camp, RelationType relation)
        {
            KeyValuePair<CampType, RelationType> key = new KeyValuePair<CampType, RelationType>(camp, relation);
            CampType[] result = null;
            if (s_CampAndRelationToCamps.TryGetValue(key, out result))
            {
                return result;
            }

            List<CampType> camps = new List<CampType>();
            foreach (CampType i in Enum.GetValues(typeof(CampType)))
            {
                if (GetRelation(camp, i) == relation)
                {
                    camps.Add(i);
                }
            }

            result = camps.ToArray();
            s_CampAndRelationToCamps[key] = result;

            return result;
        }

        /// <summary>
        /// 返回最近目标。
        /// </summary>
        /// <param name="self">有阵营的实体</param>
        /// <param name="campType">目标阵营</param>
        /// <param name="maxDistance">最大距离</param>
        /// <returns>满足条件的最近目标</returns>
        public static ITargetable GetNearestTarget(ICampable self, CampType campType, float maxDistance = float.MaxValue)
        {
            CampType[] camps = new CampType[] { campType };
            return GetNearestTarget(self, camps, maxDistance);
        }

        /// <summary>
        /// 返回最近目标。
        /// </summary>
        /// <param name="self">有阵营的实体</param>
        /// <param name="relation">目标与实体的阵营关系</param>
        /// <param name="maxDistance">最大距离</param>
        /// <returns>满足条件的最近目标</returns>
        public static ITargetable GetNearestTarget(ICampable self, RelationType relation, float maxDistance = float.MaxValue)
        {
            CampType[] camps = GetCamps(self.Camp, relation);
            return GetNearestTarget(self, camps, maxDistance);
        }

        /// <summary>
        /// 检查后台英雄是否有生存且非冷却的。
        /// </summary>
        /// <param name="heroesData">玩家英雄数据。</param>
        /// <returns>背景英雄是否有存活且不在冷却状态的。</returns>
        public static bool AnyBgHeroIsAliveAndNotCoolingDown(PlayerHeroesData heroesData)
        {
            var heroes = heroesData.GetHeroes();
            for (int i = 0; i < heroes.Length; ++i)
            {
                if (i == heroesData.CurrentHeroIndex)
                {
                    continue;
                }

                if (!heroes[i].IsDead && heroes[i].SwitchSkillCD.IsReady)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查目标是否处于可选状态。
        /// </summary>
        /// <param name="target">目标实体。</param>
        /// <returns>目标是否可选。</returns>
        public static bool TargetCanBeSelected(ITargetable target)
        {
            if (target == null || target as Entity == null || !target.IsAvailable || target.IsDead || target.IsFakeDead || target.IsEntering)
            {
                return false;
            }

            var buildingTarget = target as Building;
            if (buildingTarget != null)
            {
                return buildingTarget.ImpactCollider.enabled;
            }

            return true;
        }

        /// <summary>
        /// 获取实体间的距离。
        /// </summary>
        /// <returns>实体间的距离。</returns>
        public static float GetDistance(Entity fromEntity, Entity toEntity)
        {
            var fromTransform = fromEntity.CachedTransform;
            var toTransform = toEntity.CachedTransform;
            var dir = (toTransform.position - fromTransform.position).ToVector2();
            var baseDist = dir.magnitude;
            return baseDist <= float.Epsilon ? 0f : baseDist;
        }

        /// <summary>
        /// 获取实体间的攻击距离。
        /// </summary>
        /// <param name="attacker">攻击实体。</param>
        /// <param name="defender">被攻击实体。</param>
        /// <returns>攻击距离。</returns>
        public static float GetAttackDistance(ICampable attacker, ITargetable defender)
        {
            var attackerEntity = attacker as Entity;
            var attackerTransform = attackerEntity.CachedTransform;
            var defenderEntity = defender as Entity;
            var defenderTransform = defenderEntity.CachedTransform;
            var dir = (defenderTransform.position - attackerTransform.position).ToVector2();
            var baseDist = dir.magnitude;
            if (baseDist <= float.Epsilon)
            {
                return 0f;
            }

            var defenderTargetableObj = defenderEntity as TargetableObject;
            if (defenderTargetableObj == null)
            {
                return baseDist;
            }

            var defenderCollider = defenderTargetableObj.ImpactCollider;
            dir += defenderTransform.TransformVector(defenderCollider.center).ToVector2();

            var defenderRadius = defenderCollider.radius * defenderTransform.lossyScale.x;

            if (dir.magnitude <= defenderRadius)
            {
                return 0f;
            }

            dir -= dir.normalized * defenderRadius;
            return dir.magnitude;
        }

        public static bool EntityDataIsMine(EntityData entityData)
        {
            if (entityData == null)
            {
                return false;
            }

            EntityData finalOwnerData = GetFinalOwnerData(entityData);
            if (finalOwnerData.Entity == null)
            {
                return false;
            }

            return finalOwnerData.Entity is MeHeroCharacter;
        }

        public static EntityData GetFinalOwnerData(EntityData entityData)
        {
            if (entityData == null)
            {
                return null;
            }

            while (entityData.HasOwner)
            {
                entityData = GameEntry.Data.GetEntityData(entityData.OwnerId);
            }

            return entityData;
        }

        public static void Shuffle<T>(IList<T> list)
        {
            for (int i = 0; i < list.Count - 1; ++i)
            {
                int randomIndex = UnityEngine.Random.Range(i + 1, list.Count);
                T tmp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = tmp;
            }
        }

        /// <summary>
        /// 获取目标实体和自身实体面向的夹角。
        /// </summary>
        /// <param name="self">自身实体。</param>
        /// <param name="target">目标实体。</param>
        /// <returns>目标实体和自身实体面向的夹角。</returns>
        public static float GetFaceAngle(Entity self, Entity target)
        {
            return Vector3.Angle(self.CachedTransform.forward, target.CachedTransform.position - self.CachedTransform.position);
        }

        /// <summary>
        /// 增加乱斗经验。
        /// </summary>
        /// <param name="data">英雄或假英雄数据。</param>
        /// <param name="deltaExp">经验增量。</param>
        /// <param name="onMeleeLevelUp">升级时的回调。</param>
        public static void AddMeleeExp(this IMeleeHeroData data, int deltaExp, GameFrameworkAction onMeleeLevelUp)
        {
            var dr = GameEntry.DataTable.GetDataTable<DRMimicMeleeBase>().GetDataRow(data.MeleeLevel);

            while (deltaExp > 0)
            {
                if (dr.LevelUpExp < 0 || data.MeleeExpAtCurrentLevel + deltaExp < dr.LevelUpExp)
                {
                    data.MeleeExpAtCurrentLevel += deltaExp;
                    deltaExp = 0;
                }
                else
                {
                    deltaExp -= dr.LevelUpExp - data.MeleeExpAtCurrentLevel;
                    data.MeleeExpAtCurrentLevel = 0;
                    data.MeleeLevel += 1;
                    if (onMeleeLevelUp != null)
                    {
                        onMeleeLevelUp();
                    }

                    dr = GameEntry.DataTable.GetDataTable<DRMimicMeleeBase>().GetDataRow(data.MeleeLevel);
                }
            }
        }

        private static ITargetable GetNearestTarget(ICampable self, CampType[] camps, float maxDistance)
        {
            if (GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.NonInstance)
            {
                return null;
            }

            Entity selfEntity = self as Entity;
            if (selfEntity == null)
            {
                return null;
            }

            ITargetable nearestTarget = null;
            float nearestCharacterDistance = float.MaxValue;
            for (int i = 0; i < camps.Length; i++)
            {
                ITargetable[] targetableObjects = GameEntry.SceneLogic.BaseInstanceLogic.GetCampTargetableObjects(camps[i]);
                for (int j = 0; j < targetableObjects.Length; j++)
                {
                    if (!TargetCanBeSelected(targetableObjects[j]))
                    {
                        continue;
                    }

                    Entity targetEntity = targetableObjects[j] as Entity;
                    if (targetEntity == null)
                    {
                        continue;
                    }

                    Building targetBuilding = targetEntity as Building;
                    if (targetBuilding != null && !targetBuilding.Data.CanBeSelectedAsTargetByAI)
                    {
                        continue;
                    }

                    float distance = Vector2.Distance(selfEntity.CachedTransform.position.ToVector2(), targetEntity.CachedTransform.position.ToVector2());
                    if (distance > maxDistance)
                    {
                        continue;
                    }

                    if (distance < nearestCharacterDistance)
                    {
                        nearestCharacterDistance = distance;
                        nearestTarget = targetableObjects[j];
                    }
                }
            }

            return nearestTarget;
        }
    }
}
