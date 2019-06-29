using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        protected IDataTable<DRInstanceNpcs> m_InstanceNpcs = null;
        protected IDataTable<DRInstanceBuildings> m_InstanceBuildings = null;
        private readonly IDictionary<int, int> m_EntityIdToNpcIndex = new Dictionary<int, int>();
        private readonly HashSet<int> m_DeadNpcIndices = new HashSet<int>();
        private readonly HashSet<int> m_LivingNpcIndices = new HashSet<int>();
        private readonly HashSet<int> m_ForbiddenNpcIndices = new HashSet<int>();
        private readonly HashSet<int> m_DeadBuildingIndices = new HashSet<int>();

        [SerializeField]
        private List<NpcCharacter> m_LivingNpcCharacters = new List<NpcCharacter>();

        [SerializeField]
        private List<Building> m_LivingBuildings = new List<Building>();
        private IDictionary<int, BuildingData> m_BuildingsBeingLoaded = new Dictionary<int, BuildingData>();

        protected List<NpcCharacter> GetLivingNpcCharacters()
        {
            return m_LivingNpcCharacters;
        }

        protected List<Building> GetLivingBuildings()
        {
            return m_LivingBuildings;
        }

        public NpcCharacter GetNpcFromIndex(int npcIndex)
        {
            foreach (var kv in m_EntityIdToNpcIndex)
            {
                if (kv.Value != npcIndex)
                {
                    continue;
                }

                return GameEntry.Entity.GetGameEntity(kv.Key) as NpcCharacter;
            }

            return null;
        }

        public int GetNpcIdFromIndex(int npcIndex)
        {
            if (m_InstanceNpcs == null)
            {
                Log.Warning("Instance NPCs data table is invalid.");
                return -1;
            }

            DRInstanceNpcs instanceNpcsDataRow = m_InstanceNpcs.GetDataRow(npcIndex);
            if (instanceNpcsDataRow == null)
            {
                Log.Warning("Can not find instance NPC index '{0}'.", npcIndex.ToString());
                return -1;
            }

            return instanceNpcsDataRow.NpcId;
        }

        public bool ShowBuilding(int buildingIndex)
        {
            if (m_InstanceBuildings == null)
            {
                Log.Warning("Instance Buildings data table is invalid.");
                return false;
            }

            DRInstanceBuildings drInstanceBuildings = m_InstanceBuildings.GetDataRow(buildingIndex);
            if (drInstanceBuildings == null)
            {
                Log.Error("Building '{0}' not found in instance.", buildingIndex.ToString());
                return false;
            }

            var dtBuilding = GameEntry.DataTable.GetDataTable<DRBuilding>();
            var buildingTypeId = drInstanceBuildings.BuildingId;

            DRBuilding drBuilding = dtBuilding.GetDataRow(buildingTypeId);
            if (drBuilding == null)
            {
                Log.Error("Building '{0}' not found.", buildingTypeId.ToString());
                return false;
            }

            var npcBaseDataTable = GameEntry.DataTable.GetDataTable<DRNpcBase>();
            int level = GetOverwrittenNpcLevel(drBuilding.Level);
            DRNpcBase npcBaseDataRow = npcBaseDataTable.GetDataRow(level);
            if (npcBaseDataRow == null)
            {
                Log.Error("NPC Level '{0}' not found.", level.ToString());
                return false;
            }

            var buildingData = new BuildingData(GameEntry.Entity.GetSerialId());
            buildingData.BuildingIndex = drInstanceBuildings.Id;
            buildingData.BuildingTypeId = buildingTypeId;
            buildingData.BuildingModelId = drBuilding.BuildingModelId;
            buildingData.Name = drBuilding.Name;
            buildingData.Scale = drBuilding.Scale;
            buildingData.Level = level;
            buildingData.Camp = drBuilding.CampType;
            buildingData.HP = buildingData.MaxHP = Mathf.Max(1, Mathf.RoundToInt(npcBaseDataRow.MaxHPBase * drBuilding.MaxHPFactor));
            buildingData.PhysicalAttack = Mathf.Max(0, Mathf.RoundToInt(npcBaseDataRow.PhysicalAttackBase * drBuilding.PhysicalAttackFactor));
            buildingData.PhysicalDefense = Mathf.Max(0, Mathf.RoundToInt(npcBaseDataRow.PhysicalDefenseBase * drBuilding.PhysicalDefenseFactor));
            buildingData.MagicAttack = Mathf.Max(0, Mathf.RoundToInt(npcBaseDataRow.MagicAttackBase * drBuilding.MagicAttackFactor));
            buildingData.MagicDefense = Mathf.Max(0, Mathf.RoundToInt(npcBaseDataRow.MagicDefenseBase * drBuilding.MagicDefenseFactor));
            buildingData.DamageRandomRate = drBuilding.DamageRandomRate;
            buildingData.Position = new Vector2(drInstanceBuildings.PositionX, drInstanceBuildings.PositionY);
            buildingData.Rotation = drInstanceBuildings.Rotation;
            buildingData.Category = drBuilding.Category;
            buildingData.CountForPlayerKill = drInstanceBuildings.CountForPlayerKill;
            buildingData.ElementId = drBuilding.ElementId;
            buildingData.ChestId = drBuilding.ChestId;
            buildingData.CanBeSelectedAsTargetByAI = drBuilding.CanBeSelectedAsTargetByAI;

            m_DeadBuildingIndices.Remove(buildingIndex);
            m_BuildingsBeingLoaded.Add(buildingIndex, buildingData);

            if (drBuilding.Category == NpcCategory.Boss)
            {
                GameEntry.Event.Fire(this, new ShowBossEventArgs(buildingData, drBuilding.HPBarCount));
            }

            GameEntry.Entity.ShowBuilding(buildingData);
            return true;
        }

        public bool HideLivingBuilding(int buildingIndex)
        {
            bool found = false;
            for (int i = 0; i < m_LivingBuildings.Count; ++i)
            {
                if (m_LivingBuildings[i].Data.BuildingIndex == buildingIndex)
                {
                    GameEntry.Entity.HideEntity(m_LivingBuildings[i].Entity);
                    m_LivingBuildings.RemoveAt(i);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                BuildingData buildingData;
                if (m_BuildingsBeingLoaded.TryGetValue(buildingIndex, out buildingData))
                {
                    GameEntry.Entity.HideEntity(buildingData.Id);
                    m_BuildingsBeingLoaded.Remove(buildingIndex);
                    found = true;
                }
            }

            return found;
        }

        public bool ShowNpcRelativeToNpc(int npcIndex, Vector2 objectivePos)
        {
            return ShowNpc(npcIndex, null, TransformType.Default, null, false, false, objectivePos);
        }

        public bool ShowNpc(int npcIndex)
        {
            return ShowNpc(npcIndex, null, TransformType.Default, null, false, false, null);
        }

        public bool SummonNpc(int npcIndex, Entity owner, TransformType transformType, float? randomRadius, bool attackOwnerTarget, bool dieWithOwner)
        {
            return ShowNpc(npcIndex, owner, transformType, randomRadius, attackOwnerTarget, dieWithOwner, null);
        }

        public bool ChangeNpcCamp(NpcCharacter npcCharacter, CampType camp)
        {
            if (npcCharacter == null)
            {
                return false;
            }

            var oldCamp = npcCharacter.Data.Camp;
            if (!npcCharacter.ChangeCamp(camp))
            {
                return false;
            }

            m_CampTargetableObjects[oldCamp].Remove(npcCharacter);
            m_CampTargetableObjects[camp].Add(npcCharacter);
            return true;
        }

        public bool IsNpcDead(int npcIndex)
        {
            return m_DeadNpcIndices.Contains(npcIndex);
        }

        public bool IsBuildingDead(int buildingId)
        {
            return m_DeadBuildingIndices.Contains(buildingId);
        }

        public bool IsNpcForbidden(int npcIndex)
        {
            return m_ForbiddenNpcIndices.Contains(npcIndex);
        }

        public void KillNpc(int npcIndex, bool forbiddenFurtherUse)
        {
            KillNpcs(new int[] { npcIndex }, forbiddenFurtherUse);
        }

        public void KillNpcs(int[] npcIndices, bool forbiddenFurtherUse)
        {
            for (int j = 0; j < npcIndices.Length; j++)
            {
                int npcIndex = npcIndices[j];

                // 先记录下来，避免怪此时还没刷出时无法正确杀死。
                m_DeadNpcIndices.Add(npcIndex);
                m_LivingNpcIndices.Remove(npcIndex);

                for (int i = 0; i < m_LivingNpcCharacters.Count; i++)
                {
                    if (GetNpcIndex(m_LivingNpcCharacters[i].Id) == npcIndex)
                    {
                        KillNpc(m_LivingNpcCharacters[i]);
                    }
                }
            }

            // 在此副本的这次运行中，不允许再使用该 npcIndex 的 NPC。
            if (forbiddenFurtherUse)
            {
                ForbidNpcFurtherUse(npcIndices);
            }
        }

        private void ForbidNpcFurtherUse(int[] npcIndices)
        {
            m_ForbiddenNpcIndices.UnionWith(npcIndices);
            for (int i = 0; i < m_Data.RandomShowNpcGroupList.Count; i++)
            {
                var randomShowNpcGroupData = m_Data.RandomShowNpcGroupList[i];

                for (int j = 0; j < npcIndices.Length; j++)
                {
                    randomShowNpcGroupData.IndicesToWeights.Remove(npcIndices[j]);
                }
            }
        }

        public void CallForTarget(Entity caller, string[] callKeys, float radius, ITargetable target)
        {
            if (caller == null || callKeys == null || callKeys.Length <= 0 || radius <= 0f || target == null)
            {
                return;
            }

            for (int i = 0; i < m_LivingNpcCharacters.Count; i++)
            {
                if (m_LivingNpcCharacters[i].HasTarget)
                {
                    continue;
                }

                if (m_LivingNpcCharacters[i].CallForTargetKeys == null || m_LivingNpcCharacters[i].CallForTargetKeys.Length <= 0)
                {
                    continue;
                }

                if (AIUtility.GetDistance(caller, m_LivingNpcCharacters[i]) > radius)
                {
                    continue;
                }

                for (int j = 0; j < m_LivingNpcCharacters[i].CallForTargetKeys.Length; j++)
                {
                    for (int k = 0; k < callKeys.Length; k++)
                    {
                        if (m_LivingNpcCharacters[i].CallForTargetKeys[j] == callKeys[k])
                        {
                            m_LivingNpcCharacters[i].Target = target;
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取覆盖 NPC 和建筑的等级。
        /// </summary>
        /// <param name="originalLevel">原始等级。</param>
        /// <returns>覆盖后等级。</returns>
        /// <remarks>此处提供平凡的实现，子类按需覆盖。</remarks>
        protected virtual int GetOverwrittenNpcLevel(int originalLevel)
        {
            return originalLevel;
        }

        private bool ShowNpc(int npcIndex, Entity owner, TransformType transformType, float? randomRadius, bool attackOwnerTarget, bool dieWithOwner, Vector2? objectivePos)
        {
            if (m_InstanceNpcs == null)
            {
                Log.Warning("Instance NPCs is invalid.");
                return false;
            }

            DRInstanceNpcs instanceNpcsDataRow = m_InstanceNpcs.GetDataRow(npcIndex);
            if (instanceNpcsDataRow == null)
            {
                Log.Warning("Can not find instance NPC index '{0}'.", npcIndex.ToString());
                return false;
            }

            IDataTable<DRNpc> dt = GameEntry.DataTable.GetDataTable<DRNpc>();
            DRNpc dataRow = dt.GetDataRow(instanceNpcsDataRow.NpcId);
            if (dataRow == null)
            {
                Log.Warning("Can not find NPC '{0}'.", instanceNpcsDataRow.NpcId.ToString());
                return false;
            }

            IDataTable<DRNpcBase> dtNpcBase = GameEntry.DataTable.GetDataTable<DRNpcBase>();
            int level = GetOverwrittenNpcLevel(dataRow.Level);
            DRNpcBase npcBaseDataRow = dtNpcBase.GetDataRow(level);
            if (npcBaseDataRow == null)
            {
                Log.Warning("Can not find NPC base '{0}'.", level.ToString());
                return false;
            }

            IDataTable<DRCharacter> dtCharacter = GameEntry.DataTable.GetDataTable<DRCharacter>();
            DRCharacter characterDataRow = dtCharacter.GetDataRow(dataRow.CharacterId);
            if (characterDataRow == null)
            {
                Log.Warning("Can not find character '{0}'.", dataRow.CharacterId);
                return false;
            }

            if (m_ForbiddenNpcIndices.Contains(npcIndex))
            {
                Log.Warning("You're trying to show an instance NPC '{0}' which has been forbidden to use.", npcIndex);
                return false;
            }

            if (m_LivingNpcIndices.Contains(npcIndex))
            {
                Log.Warning("NPC '{0}' is still living!", npcIndex);
                return false;
            }

            int entityId = GameEntry.Entity.GetSerialId();

            NpcCharacterData npcData = new NpcCharacterData(entityId);
            PopulateNpcData(npcData, dataRow, characterDataRow, npcBaseDataRow, instanceNpcsDataRow, level, attackOwnerTarget, dieWithOwner);

            if (!SetOwnerAndTransformForNpcData(npcData, owner, transformType, instanceNpcsDataRow, randomRadius, objectivePos))
            {
                return false;
            }

            m_DeadNpcIndices.Remove(npcIndex);
            m_LivingNpcIndices.Add(npcIndex);
            m_EntityIdToNpcIndex.Add(entityId, npcIndex);

            if (dataRow.Category == NpcCategory.Boss)
            {
                GameEntry.Event.Fire(this, new ShowBossEventArgs(npcData, dataRow.HPBarCount));
            }

            GameEntry.Entity.ShowNpc(npcData);
            return true;
        }

        protected virtual void PopulateNpcData(NpcCharacterData npcData, DRNpc dataRow, DRCharacter characterDataRow, DRNpcBase npcBaseDataRow, DRInstanceNpcs instanceNpcsDataRow,
            int level, bool attackOwnerTarget, bool dieWithOwner)
        {
            npcData.Name = GameEntry.Localization.GetString(dataRow.Name);
            npcData.CharacterId = dataRow.CharacterId;
            npcData.NpcId = dataRow.Id;
            npcData.Scale = dataRow.Scale;
            npcData.CountForPlayerKill = instanceNpcsDataRow.CountForPlayerKill;
            npcData.Level = level;
            npcData.Camp = dataRow.CampType;
            npcData.Steady.Steady = npcData.Steady.MaxSteady = dataRow.Steady;
            npcData.Steady.SteadyRecoverSpeed = dataRow.SteadyRecoverSpeed;
            npcData.HP = npcData.MaxHP = Mathf.Max(1, Mathf.RoundToInt(npcBaseDataRow.MaxHPBase * dataRow.MaxHPFactor));
            npcData.PhysicalAttack = Mathf.Max(0, Mathf.RoundToInt(npcBaseDataRow.PhysicalAttackBase * dataRow.PhysicalAttackFactor));
            npcData.PhysicalDefense = Mathf.Max(0, Mathf.RoundToInt(npcBaseDataRow.PhysicalDefenseBase * dataRow.PhysicalDefenseFactor));
            npcData.MagicAttack = Mathf.Max(0, Mathf.RoundToInt(npcBaseDataRow.MagicAttackBase * dataRow.MagicAttackFactor));
            npcData.MagicDefense = Mathf.Max(0, Mathf.RoundToInt(npcBaseDataRow.MagicDefenseBase * dataRow.MagicDefenseFactor));
            npcData.Speed = dataRow.Speed;
            npcData.DamageRandomRate = dataRow.DamageRandomRate;
            npcData.Category = dataRow.Category;
            npcData.ShowName = dataRow.ShowName;
            npcData.MaterialType = characterDataRow.MaterialType;
            npcData.AttackOwnerTarget = attackOwnerTarget;
            npcData.DieWithOwner = dieWithOwner;
            npcData.ElementId = dataRow.ElementId;
            npcData.CameraShakingIndexOnEnter = dataRow.CameraShakingIndexOnEnter;
            npcData.ChestId = dataRow.ChestId;
            npcData.MinHP = npcData.MinHP;
            npcData.MeleeExp = dataRow.MeleeExp;
            npcData.IsFakeHero = dataRow.IsFakeHero;
            npcData.MeleeLevel = 1;
            npcData.MeleeExpAtCurrentLevel = 0;
        }

        private bool SetOwnerAndTransformForNpcData(NpcCharacterData npcData, Entity owner, TransformType transformType, DRInstanceNpcs instanceNpcsDataRow, float? randomRadius, Vector2? objectivePos)
        {
            Vector2 defaultPosition = objectivePos != null ? objectivePos.Value : new Vector2(instanceNpcsDataRow.PositionX, instanceNpcsDataRow.PositionY);

            float defaultRotation = instanceNpcsDataRow.Rotation;

            Vector2 posOffset = Vector2.zero;

            if (randomRadius != null && randomRadius.Value > 0f)
            {
                posOffset += Random.insideUnitCircle * randomRadius.Value;
            }

            if (owner == null)
            {
                npcData.HomePosition = npcData.Position = defaultPosition + posOffset;
                npcData.Rotation = defaultRotation;
                return true;
            }

            npcData.OwnerId = owner.Id;

            Vector2 ownerPosition = owner.CachedTransform.localPosition.ToVector2();
            float ownerRotation = owner.CachedTransform.localRotation.eulerAngles.y;

            bool needSamplePos = false;
            switch (transformType)
            {
                case TransformType.Default:
                    npcData.HomePosition = npcData.Position = defaultPosition + posOffset;
                    npcData.Rotation = defaultRotation;
                    break;
                case TransformType.RelativeToTarget:
                    var npcOwner = owner as NpcCharacter;

                    if (npcOwner == null || !npcOwner.HasTarget)
                    {
                        npcData.HomePosition = npcData.Position = (randomRadius == null ? defaultPosition : posOffset) + ownerPosition;
                        npcData.Rotation = defaultRotation + ownerRotation;
                    }
                    else
                    {
                        Entity targetEntity = npcOwner.Target as Entity;
                        Vector2 targetPosition = targetEntity.CachedTransform.localPosition.ToVector2();
                        float targetRotation = targetEntity.CachedTransform.localRotation.eulerAngles.y;

                        npcData.HomePosition = npcData.Position = (randomRadius == null ? defaultPosition : posOffset) + targetPosition;
                        npcData.Rotation = defaultRotation + targetRotation;
                    }

                    needSamplePos = randomRadius.HasValue;
                    break;
                case TransformType.RelativeToOwner:
                    npcData.HomePosition = npcData.Position = (randomRadius == null ? defaultPosition : posOffset) + ownerPosition;
                    npcData.Rotation = defaultRotation + ownerRotation;
                    needSamplePos = randomRadius.HasValue;
                    break;
                default:
                    Log.Error("TransformType doesn't have value '{0}'.", transformType);
                    return false;
            }

            if (needSamplePos)
            {
                var npcPosition = AIUtility.SamplePosition(npcData.Position);
                var path = new NavMeshPath();
                bool gotPath = NavMesh.CalculatePath(owner.CachedTransform.localPosition, npcPosition, NavMesh.AllAreas, path);
                //Log.Info("[BaseSinglePlayerInstanceLogic SetOwnerAndTransformForNpcData] Got a path? {0}. Path status is '{1}'", gotPath, path.status);
                return gotPath && path.status == NavMeshPathStatus.PathComplete;
            }

            return true;
        }

        private void KillNpc(NpcCharacter npc)
        {
            if (npc == null)
            {
                Log.Warning("NPC is invalid.");
                return;
            }

            npc.Data.HP = 0;
            npc.Motion.PerformHPDamage(null, ImpactSourceType.InstanceLogic);
        }

        private void KillSummonedNpcs(NpcCharacter ownerNpc)
        {
            if (ownerNpc == null)
            {
                Log.Warning("NPC is invalid.");
                return;
            }

            List<NpcCharacter> npcCharacters = GetLivingNpcCharacters();
            for (int i = 0; i < npcCharacters.Count; i++)
            {
                if (npcCharacters[i].HasOwner && npcCharacters[i].Owner == ownerNpc && npcCharacters[i].Data.DieWithOwner)
                {
                    KillNpc(npcCharacters[i]);
                }
            }
        }

        private void KillBuilding(Building building)
        {
            if (building == null)
            {
                Log.Warning("Building is invalid.");
                return;
            }

            building.Data.HP = 0;
            building.Motion.PerformHPDamage(null, ImpactSourceType.InstanceLogic);
        }

        private int GetNpcIndex(int entityId)
        {
            int npcIndex = 0;
            if (m_EntityIdToNpcIndex.TryGetValue(entityId, out npcIndex))
            {
                return npcIndex;
            }

            Log.Warning("Can not find entity '{0}' in entity NPC indices.", entityId.ToString());
            return 0;
        }

        protected virtual void OnCharacterDead(object sender, GameEventArgs e)
        {
            var ne = e as CharacterDeadEventArgs;
            var npc = ne.Character as NpcCharacter;
            if (npc != null)
            {
                m_LivingNpcCharacters.Remove(npc);
                int npcIndex = GetNpcIndex(npc.Id);
                if (npcIndex > 0)
                {
                    m_LivingNpcIndices.Remove(npcIndex);
                    m_DeadNpcIndices.Add(npcIndex);
                    m_EntityIdToNpcIndex.Remove(npcIndex);
                }

                CheckDropGoods(npc);
                KillSummonedNpcs(npc);

                if (npc.Data.Category == NpcCategory.Boss)
                {
                    m_BossHasDied = true;
                }

                int coinMultiplier = 1;
                if (Data.HasMiscParam(Constant.Instance.ParamKey.DropCoinMultiplierOnNpcDie))
                {
                    coinMultiplier = Data.GetMiscParam<int>(Constant.Instance.ParamKey.DropCoinMultiplierOnNpcDie);
                }

                m_DeadDropCoins += npc.DeadDropCoinsCount * coinMultiplier;
                ShowNpcDropCoinsEffect(npc);
                GameEntry.Event.Fire(this, new NpcDeadDropIconsEventArgs(m_DeadDropCoins));
            }

            m_CampTargetableObjects[ne.CharacterData.Camp].Remove(ne.Character);
            m_Me.CheckHeroesDeath(ne.CharacterData.Id);
        }

        protected virtual void ShowNpcDropCoinsEffect(NpcCharacter npc)
        {
            if (npc == null || npc.DeadDropCoinsEffectId <= 0)
            {
                return;
            }
            var dt = GameEntry.DataTable.GetDataTable<DREffect>();
            DREffect dr = dt.GetDataRow(npc.DeadDropCoinsEffectId);
            if (dr == null)
            {
                Log.Warning("Effect '{0}' not found.", npc.DeadDropCoinsEffectId.ToString());
                return;
            }
            var effectData = new EffectData(GameEntry.Entity.GetSerialId(), dr.ResourceName, npc.transform.position, 0);
            effectData.CanAttachToDeadOwner = true;
            effectData.NeedSamplePosition = true;
            GameEntry.Entity.ShowEffect(effectData);
        }


        private void OnBuildingDead(object sender, GameEventArgs e)
        {
            var ne = e as BuildingDeadEventArgs;
            var building = ne.Building;
            if (building == null)
            {
                return;
            }

            m_LivingBuildings.Remove(building);
            m_DeadBuildingIndices.Add(building.Data.BuildingIndex);
            m_CampTargetableObjects[ne.BuildingData.Camp].Remove(ne.Building);

            if (building.Data.Category == NpcCategory.Boss)
            {
                m_BossHasDied = true;
            }

            if (building.DeadDropInfo == null)
            {
                return;
            }

            if (building.DeadDropInfo.Type == BuildingDropType.Coin)
            {
                m_DeadDropCoins += building.DeadDropInfo.Params[0];
                ShowBuildingDropCoinsEffect(building);
            }
            else
            {
                var targetOwerData = AIUtility.GetFinalOwnerData(ne.ImpactSourceEntity.Data);
                var targetOwer = targetOwerData.Entity as TargetableObject;
                if (targetOwer == null)
                {
                    return;
                }
                targetOwer.AddBuff(building.DeadDropInfo.Params[0], null, BaseBuffPool.GetNextSerialId(), null);
            }

            GameEntry.Event.Fire(this, new BuildingDeadDropIconsEventArgs(m_DeadDropCoins));
        }

        protected virtual void ShowBuildingDropCoinsEffect(Building building)
        {
            if (building == null || building.DeadDropCoinsEffectId <= 0)
            {
                return;
            }
            var dt = GameEntry.DataTable.GetDataTable<DREffect>();
            DREffect dr = dt.GetDataRow(building.DeadDropCoinsEffectId);
            if (dr == null)
            {
                Log.Warning("Effect '{0}' not found.", building.DeadDropCoinsEffectId.ToString());
                return;
            }
            var effectData = new EffectData(GameEntry.Entity.GetSerialId(), dr.ResourceName, building.transform.position, 0);
            effectData.CanAttachToDeadOwner = true;
            effectData.NeedSamplePosition = true;
            GameEntry.Entity.ShowEffect(effectData);
        }
    }
}
