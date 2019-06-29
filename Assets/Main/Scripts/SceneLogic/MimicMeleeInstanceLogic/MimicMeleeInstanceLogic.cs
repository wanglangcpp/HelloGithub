using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 模拟乱斗副本逻辑。
    /// </summary>
    public partial class MimicMeleeInstanceLogic : BaseSinglePlayerInstanceLogic
    {
        private const string MyPlayerNameFormat = "[0bd564]{0}[-]";

        private bool m_VictimIsMe = false;

        /// <summary>
        /// 使用到的阵营。
        /// </summary>
        public static readonly CampType[] MimicPlayerCamps = new CampType[]
        {
            CampType.Player,
            CampType.Player2,
            CampType.Player3,
        };

        /// <summary>
        /// 阵营到名称的映射。
        /// </summary>
        public static readonly Dictionary<CampType, string> CampToName = new Dictionary<CampType, string>
        {
            { CampType.Player, "NPC_MELEE_CAMP_1_NAME" },
            { CampType.Player2, "NPC_MELEE_CAMP_2_NAME" },
            { CampType.Player3, "NPC_MELEE_CAMP_3_NAME" },
        };

        public override InstanceLogicType Type
        {
            get
            {
                return InstanceLogicType.MimicMelee;
            }
        }

        private DRMimicMeleeInstance m_DRInstance;

        protected override DRInstance DRInstance
        {
            get
            {
                return m_DRInstance;
            }
        }

        /// <summary>
        /// 复活前等待时间。
        /// </summary>
        public float ReviveWaitTime
        {
            get
            {
                if (m_DRInstance == null) return 0f;
                return m_DRInstance.ReviveWaitTime;
            }
        }

        /// <summary>
        /// 小地图纹理编号。
        /// </summary>
        public int MiniMapTextureId
        {
            get
            {
                if (m_DRInstance == null) return 0;
                return m_DRInstance.MiniMapTextureId;
            }
        }

        /// <summary>
        /// 小地图比例。
        /// </summary>
        public float MiniMapScale
        {
            get
            {
                if (m_DRInstance == null) return 1f;
                return m_DRInstance.MiniMapScale;
            }
        }

        /// <summary>
        /// 小地图偏移。
        /// </summary>
        public Vector2 MiniMapOffset
        {
            get
            {
                if (m_DRInstance == null) return Vector2.zero;
                return m_DRInstance.MiniMapOffset;
            }
        }

        private IDictionary<int, CachedFakeHeroData> m_CachedFakedHeroDatas = null;

        private HashSet<int> m_TargetableObjectEntityIds = new HashSet<int>();

        public HashSet<int> GetTargetableObjectEntityIds()
        {
            return m_TargetableObjectEntityIds;
        }

        public override void Init(int instanceOrSceneId, object userData)
        {
            base.Init(instanceOrSceneId, userData);
            m_DRInstance = GetInstanceDataRow<DRMimicMeleeInstance>(m_InstanceId);
            InitInstanceDataBefore(m_DRInstance);
            m_CachedFakedHeroDatas = new Dictionary<int, CachedFakeHeroData>();
        }

        public override void RequestStartInstance()
        {
            base.RequestStartInstance();
            InitNpcDataTable(m_DRInstance);
            InitBuildingDataTable(m_DRInstance);
            InitTimer(m_DRInstance);
        }

        public override void Shutdown()
        {
            m_CachedFakedHeroDatas.Clear();
            base.Shutdown();
        }

        /// <summary>
        /// 获取给定阵营的积分。
        /// </summary>
        /// <param name="campType">阵营。</param>
        /// <returns>积分。</returns>
        public int GetScoreForCamp(CampType campType)
        {
            int ret = 0;
            if (MeHeroCharacter != null && MeHeroCharacter.Camp == campType)
            {
                ret += MeHeroCharacter.Data.MeleeScore;
            }

            var targetableObjects = m_CampTargetableObjects[campType];
            HashSet<int> npcIds = new HashSet<int>();
            for (int i = 0; i < targetableObjects.Count; i++)
            {
                var npc = targetableObjects[i] as NpcCharacter;
                if (npc != null && npc.Data.IsFakeHero)
                {
                    if (npc.Camp == campType)
                    {
                        npcIds.Add(npc.Data.NpcId);
                        ret += npc.Data.MeleeScore;
                    }
                }
            }

            foreach (var kv in m_CachedFakedHeroDatas)
            {
                if (npcIds.Contains(kv.Key))
                {
                    continue;
                }

                if (kv.Value.CampType == campType)
                {
                    ret += kv.Value.MeleeScore;
                }
            }

            return ret;
        }

        protected override void OnInstanceTimeOut()
        {
            // TODO：根据积分决定胜负。
            SetInstanceSuccess(InstanceSuccessReason.TimeOut);
        }

        protected override void PrepareSuccessData()
        {
            // Empty.
        }

        protected override void OnAllHeroesDead()
        {
            ReviveAtRandomPos();
        }

        protected override void PrepareMyHeroesData(PlayerHeroesData heroesData, Vector2 spawnPosition, float spawnRotation)
        {
            List<int> heroTypes = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType;
            // 仅把队长带入。
            heroesData.Add(GetHeroData(GameEntry.Entity.GetSerialId(), heroTypes[0], spawnPosition, spawnRotation));
        }

        private void ReviveAtRandomPos()
        {
            if (MeHeroCharacter == null)
            {
                Log.Warning("MeHeroCharacter is invalid.");
                return;
            }

            var revivePoints = m_DRInstance.RevivePoints;

            Vector2 revivePoint = Vector2.zero;
            if (revivePoints.Length >= 1)
            {
                revivePoint = revivePoints[UnityEngine.Random.Range(0, revivePoints.Length)];
            }

            MeHeroCharacter.NavAgent.enabled = false;
            MeHeroCharacter.CachedTransform.localPosition = AIUtility.SamplePosition(revivePoint);
            MeHeroCharacter.NavAgent.enabled = true;

            GameEntry.SceneLogic.BaseInstanceLogic.CameraController.FollowImmediately();

            MeHeroCharacter.Revive();
            CreateNameBoard(MeHeroCharacter);
        }

        protected override void PopulateNpcData(NpcCharacterData npcData, DRNpc dataRow, DRCharacter characterDataRow, DRNpcBase npcBaseDataRow, DRInstanceNpcs instanceNpcsDataRow, int level, bool attackOwnerTarget, bool dieWithOwner)
        {
            base.PopulateNpcData(npcData, dataRow, characterDataRow, npcBaseDataRow, instanceNpcsDataRow, level, attackOwnerTarget, dieWithOwner);

            if (npcData.IsFakeHero)
            {
                CachedFakeHeroData cachedData;
                if (m_CachedFakedHeroDatas.TryGetValue(npcData.NpcId, out cachedData))
                {
                    npcData.MeleeLevel = cachedData.MeleeLevel;
                    npcData.MeleeExpAtCurrentLevel = cachedData.MeleeExpAtCurrentLevel;
                    npcData.MeleeScore = cachedData.MeleeScore;
                }
                else
                {
                    npcData.MeleeLevel = 1;
                    npcData.MeleeExpAtCurrentLevel = 0;
                    npcData.MeleeScore = 0;
                }

                var mimicMeleeBaseDataRow = GameEntry.DataTable.GetDataTable<DRMimicMeleeBase>().GetDataRow(npcData.MeleeLevel);
                npcData.Steady.Steady = npcData.Steady.MaxSteady = mimicMeleeBaseDataRow.Steady;
                npcData.Steady.SteadyRecoverSpeed = mimicMeleeBaseDataRow.SteadyRecoverSpeed;
                npcData.HP = npcData.MaxHP = mimicMeleeBaseDataRow.MaxHPBase;
                npcData.PhysicalAttack = mimicMeleeBaseDataRow.PhysicalAttackBase;
                npcData.PhysicalDefense = mimicMeleeBaseDataRow.PhysicalDefenseBase;
                npcData.MagicAttack = 0;
                npcData.MagicDefense = 0;
                npcData.DamageRandomRate = mimicMeleeBaseDataRow.DamageRandomRate;
            }
        }

        protected override void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            base.OnShowEntitySuccess(sender, e);

            var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;
            var npc = ne.Entity.Logic as NpcCharacter;
            if (npc != null && npc.Data.IsFakeHero)
            {
                if (m_CachedFakedHeroDatas.Remove(npc.Data.NpcId))
                {
                    npc.AddBuff(Constant.Buff.ReviveBuffId, null, BaseBuffPool.GetNextSerialId(), null);
                }
            }
            var meHeroCharacter = ne.Entity.Logic as MeHeroCharacter;
            if (meHeroCharacter != null)
            {
                CreateNameBoard(meHeroCharacter);
            }
        }

        public override void OnTargetableShow(TargetableObject targetable)
        {
            base.OnTargetableShow(targetable);
            m_TargetableObjectEntityIds.Add(targetable.Id);
            GameEntry.Event.Fire(this, new TargetableObjectShowInMimicMeleeEventArgs(targetable.Id));
        }

        public override void OnTargetableHide(TargetableObject targetable)
        {
            m_TargetableObjectEntityIds.Remove(targetable.Id);
            GameEntry.Event.Fire(this, new TargetableObjectHideInMimicMeleeEventArgs(targetable.Id));
            base.OnTargetableHide(targetable);
        }

        protected override void OnCharacterDead(object sender, GameEventArgs e)
        {
            base.OnCharacterDead(sender, e);

            var ne = e as CharacterDeadEventArgs;
            var npcData = ne.CharacterData as NpcCharacterData;
            int expForKillers = 0;
            int scoreForKillers = 0;
            string victimName = string.Empty;
            if (npcData != null)
            {
                if (npcData.IsFakeHero) // 死者为假英雄。
                {
                    m_CachedFakedHeroDatas[npcData.NpcId] = new CachedFakeHeroData
                    {
                        NpcId = npcData.NpcId,
                        MeleeExpAtCurrentLevel = npcData.MeleeExpAtCurrentLevel,
                        MeleeLevel = npcData.MeleeLevel,
                        MeleeScore = npcData.MeleeScore,
                        CampType = npcData.Camp,
                    };

                    victimName = npcData.Name;
                    m_VictimIsMe = false;
                    var dr = GameEntry.DataTable.GetDataTable<DRMimicMeleeBase>().GetDataRow(npcData.MeleeLevel);
                    expForKillers = dr.MeleeExp;
                    scoreForKillers = dr.MeleeScore;
                }
                else // 死者为普通 NPC。
                {
                    expForKillers = npcData.MeleeExp;
                    var drNpcInMimicMelee = GameEntry.DataTable.GetDataTable<DRNpcInMimicMelee>().GetDataRow(npcData.NpcId);
                    if (drNpcInMimicMelee != null)
                    {
                        scoreForKillers = drNpcInMimicMelee.ScoreForKiller;
                    }
                }
            }
            else
            {
                var myHeroCharacter = ne.Character as MeHeroCharacter;
                if (myHeroCharacter != null) // 死者为当前玩家控制的英雄。
                {
                    victimName = string.Format(MyPlayerNameFormat, GameEntry.Data.Player.Name);
                    m_VictimIsMe = true;
                    var dr = GameEntry.DataTable.GetDataTable<DRMimicMeleeBase>().GetDataRow(myHeroCharacter.Data.MeleeLevel);
                    expForKillers = dr.MeleeExp;
                    scoreForKillers = dr.MeleeScore;
                }
            }

            var killer = ne.DeadlyImpactSourceEntity;
            string killerName = string.Empty;
            while (killer != null)
            {
                var myHeroCharacter = killer as MeHeroCharacter;
                if (myHeroCharacter != null)
                {
                    myHeroCharacter.Data.MeleeScore += scoreForKillers;
                    DistributeExp(myHeroCharacter, expForKillers);
                    killerName = string.Format(MyPlayerNameFormat, GameEntry.Data.Player.Name);
                    break;
                }

                var fakeHero = killer as NpcCharacter;
                if (fakeHero != null && fakeHero.Data.IsFakeHero)
                {
                    fakeHero.Data.MeleeScore += scoreForKillers;
                    DistributeExp(fakeHero, expForKillers);
                    killerName = fakeHero.Data.Name;
                    break;
                }

                killer = killer.Owner;
            }

            GameEntry.Event.Fire(this, new OnMimicMeleeChangedEventArgs() { KillerName = killerName, VictimName = victimName, VictimIsMe = m_VictimIsMe });
            //if (!string.IsNullOrEmpty(killerName) && !string.IsNullOrEmpty(victimName))
            //{
            //    GameEntry.Event.Fire(this, new OnMimicMeleeChangedEventArgs() { KillerName = killerName, VictimName = victimName, VictimIsMe = m_VictimIsMe });
            //}
        }

        private void DistributeExp(MeHeroCharacter myHeroKiller, int expForKillers)
        {
            var targetableObjects = m_CampTargetableObjects[myHeroKiller.Camp];
            Dictionary<int, NpcCharacter> npcCharacters = new Dictionary<int, NpcCharacter>();
            for (int i = 0; i < targetableObjects.Count; i++)
            {
                NpcCharacter npc = targetableObjects[i] as NpcCharacter;
                if (npc != null && npc.Data.IsFakeHero)
                {
                    npcCharacters.Add(npc.Data.NpcId, npc);
                    continue;
                }
            }

            Dictionary<int, CachedFakeHeroData> cachedFakeHeroDatas = new Dictionary<int, CachedFakeHeroData>();
            foreach (var kv in m_CachedFakedHeroDatas)
            {
                if (npcCharacters.ContainsKey(kv.Key))
                {
                    continue;
                }

                cachedFakeHeroDatas.Add(kv.Key, kv.Value);
            }

            int totalCount = npcCharacters.Count + cachedFakeHeroDatas.Count;

            int mainExp = Mathf.RoundToInt(expForKillers * m_DRInstance.ExpDistrMainCoef / (m_DRInstance.ExpDistrMainCoef + m_DRInstance.ExpDistrSecondaryCoef * totalCount));
            int secondaryExp = Mathf.RoundToInt(expForKillers * m_DRInstance.ExpDistrSecondaryCoef / (m_DRInstance.ExpDistrMainCoef + m_DRInstance.ExpDistrSecondaryCoef * totalCount));
            myHeroKiller.Data.AddMeleeExp(mainExp);
            foreach (var kv in npcCharacters)
            {
                kv.Value.Data.AddMeleeExp(secondaryExp);
            }

            foreach (var kv in cachedFakeHeroDatas)
            {
                kv.Value.AddMeleeExp(secondaryExp);
            }
        }

        private void DistributeExp(NpcCharacter fakeHeroKiller, int expForKillers)
        {
            bool includeMe = MeHeroCharacter.Camp == fakeHeroKiller.Camp;
            var targetableObjects = m_CampTargetableObjects[fakeHeroKiller.Camp];

            Dictionary<int, NpcCharacter> npcCharacters = new Dictionary<int, NpcCharacter>();
            for (int i = 0; i < targetableObjects.Count; i++)
            {
                NpcCharacter npc = targetableObjects[i] as NpcCharacter;
                if (npc != null && npc.Data.IsFakeHero && npc.Data.NpcId != fakeHeroKiller.Data.NpcId)
                {
                    npcCharacters.Add(npc.Data.NpcId, npc);
                    continue;
                }
            }

            Dictionary<int, CachedFakeHeroData> cachedFakeHeroDatas = new Dictionary<int, CachedFakeHeroData>();
            foreach (var kv in m_CachedFakedHeroDatas)
            {
                if (npcCharacters.ContainsKey(kv.Key) || kv.Key == fakeHeroKiller.Data.NpcId)
                {
                    continue;
                }

                cachedFakeHeroDatas.Add(kv.Key, kv.Value);
            }

            int totalCount = npcCharacters.Count + cachedFakeHeroDatas.Count + (includeMe ? 1 : 0);
            int mainExp = Mathf.RoundToInt(expForKillers * m_DRInstance.ExpDistrMainCoef / (m_DRInstance.ExpDistrMainCoef + m_DRInstance.ExpDistrSecondaryCoef * totalCount));
            fakeHeroKiller.Data.AddMeleeExp(mainExp);
            int secondaryExp = Mathf.RoundToInt(expForKillers * m_DRInstance.ExpDistrSecondaryCoef / (m_DRInstance.ExpDistrMainCoef + m_DRInstance.ExpDistrSecondaryCoef * totalCount));
            foreach (var kv in npcCharacters)
            {
                kv.Value.Data.AddMeleeExp(secondaryExp);
            }

            foreach (var kv in cachedFakeHeroDatas)
            {
                kv.Value.AddMeleeExp(secondaryExp);
            }

            if (includeMe)
            {
                MeHeroCharacter.Data.AddMeleeExp(secondaryExp);
            }
        }

        protected override void PopulateHeroData(BaseLobbyHeroData rawHeroData, CampType camp, Vector2 position, float rotation, int heroType, DRHero drHero, DRCharacter drCharacter, HeroData heroData)
        {
            heroData.Name = rawHeroData.Name;
            heroData.CharacterId = drHero.CharacterId;
            heroData.HeroId = heroType;
            heroData.Scale = drHero.Scale;
            heroData.Position = position;
            heroData.Rotation = rotation;
            heroData.Level = rawHeroData.Level;
            heroData.StarLevel = rawHeroData.StarLevel;
            heroData.WeaponSuiteId = rawHeroData.WeaponSuiteId;
            heroData.Camp = camp;
            heroData.Speed = rawHeroData.Speed;

            var mimicMeleeBaseDataRow = GameEntry.DataTable.GetDataTable<DRMimicMeleeBase>().GetDataRow(1);
            heroData.HP = heroData.MaxHP = mimicMeleeBaseDataRow.MaxHPBase;
            heroData.Steady.Steady = heroData.Steady.MaxSteady = mimicMeleeBaseDataRow.Steady;
            heroData.Steady.SteadyRecoverSpeed = mimicMeleeBaseDataRow.SteadyRecoverSpeed;
            heroData.PhysicalAttack = mimicMeleeBaseDataRow.PhysicalAttackBase;
            heroData.PhysicalDefense = mimicMeleeBaseDataRow.PhysicalDefenseBase;
            heroData.MagicAttack = 0;
            heroData.MagicDefense = 0;
            heroData.OppPhysicalDfsReduceRate = 0;
            heroData.OppMagicDfsReduceRate = 0;
            heroData.PhysicalAtkHPAbsorbRate = 0;
            heroData.MagicAtkHPAbsorbRate = 0;
            heroData.PhysicalAtkReflectRate = 0;
            heroData.MagicAtkReflectRate = 0;
            heroData.DamageReductionRate = 0;
            heroData.CriticalHitProb = mimicMeleeBaseDataRow.CriticalHitProb;
            heroData.CriticalHitRate = mimicMeleeBaseDataRow.CriticalHitRate;
            heroData.AntiCriticalHitProb = 0;
            heroData.DamageRandomRate = mimicMeleeBaseDataRow.DamageRandomRate;
            heroData.AdditionalDamage = 0;
            heroData.MaterialType = drCharacter.MaterialType;
            heroData.ElementId = drHero.ElementId;
            heroData.RecoverHP = 0;
            heroData.ReducedSkillCoolDown = 0;
            heroData.ReducedHeroSwitchCD = 0;

            // 不带入技能徽章。
            heroData.SkillsBadges = BaseLobbyHeroData.GetEmptySkillBadges();

            for (int i = 0; i < Constant.TotalSkillGroupCount; i++)
            {
                heroData.SkillsBadges.Add(null);
                heroData.SetSkillLevel(i, Mathf.Min(rawHeroData.GetSkillLevel(i), 1));
            }

            heroData.DebutOnShow = false;
            heroData.SwitchSkillLevel = 1;
            heroData.SwitchSkillCD.Reset(drHero.CDWhenStart, false);
            heroData.ReplaceSkillsAndCacheDataRows();
            heroData.RefreshDodgeSkillEnergyData();
            heroData.IsDead = (heroData.HP <= 0);
            heroData.MeleeLevel = 1;
            heroData.MeleeExpAtCurrentLevel = 0;
        }

        private void CreateNameBoard(MeHeroCharacter meHeroCharacter)
        {
            BaseNameBoard nameBoard = GameEntry.Impact.CreateNameBoard(meHeroCharacter, NameBoardMode.HPBarOnly);
            nameBoard.SetNameColor(nameBoard.OtherNameColor);
            nameBoard.RefreshPosition();
        }

        private class CachedFakeHeroData : IMeleeHeroData
        {
            public int NpcId;
            public int MeleeLevel { get; set; }
            public int MeleeExpAtCurrentLevel { get; set; }
            public int MeleeScore { get; set; }
            public CampType CampType;

            /// <summary>
            /// 增加乱斗经验值。
            /// </summary>
            /// <param name="deltaExp">经验增量。</param>
            public void AddMeleeExp(int deltaExp)
            {
                this.AddMeleeExp(deltaExp, null);
            }
        }
    }
}
