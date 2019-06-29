using BehaviorDesigner.Runtime;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// Logic class for single player instances (dungeons).
    /// </summary>
    [Serializable]
    public abstract partial class BaseSinglePlayerInstanceLogic : BaseInstanceLogic, IDropGoodsInstance, IPropagandaInstance
    {
        [SerializeField]
        protected InstanceData m_Data = null;

        public InstanceData Data
        {
            get
            {
                return m_Data;
            }
        }

        public override PlayerHeroesData MyHeroesData
        {
            get { return Data.MeHeroesData; }
        }

        protected string m_InstanceNpcsDTName = string.Empty;

        protected string m_InstanceBuildingsDTName = string.Empty;

        protected string m_SceneBehaviorName = string.Empty;

        protected BehaviorTree m_SceneBehavior = null;

        private HashSet<BehaviorTree> m_BehaviorsToLoad = new HashSet<BehaviorTree>();

        private bool m_BossHasDied = false;

        public bool BossHasDied
        {
            get
            {
                return m_BossHasDied;
            }
        }

        protected int m_DeadDropCoins = 0;

        public override string GetRequest(int index)
        {
            return GameEntry.Localization.GetString(DRInstance.GetRequestDescription(index));
        }

        protected abstract DRInstance DRInstance { get; }

        public override void Init(int instanceOrSceneId, object userData)
        {
            base.Init(instanceOrSceneId, userData);
            GameEntry.Event.Subscribe(EventId.CharacterDead, OnCharacterDead);
            GameEntry.Event.Subscribe(EventId.BuildingDead, OnBuildingDead);
            GameEntry.Event.Subscribe(EventId.ReviveHeroes, OnReviveHeroes);
            GameEntry.Event.Subscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Subscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);

            m_InstanceId = instanceOrSceneId;
            m_DeadDropCoins = 0;
            GameEntry.Impact.ShouldScreenHPDamage += ShouldLockHP;
        }

        public override void Shutdown()
        {
            if (!GameEntry.IsAvailable) return;

            GameEntry.Impact.ShouldScreenHPDamage -= ShouldLockHP;

            GameEntry.Event.Unsubscribe(EventId.CharacterDead, OnCharacterDead);
            GameEntry.Event.Unsubscribe(EventId.BuildingDead, OnBuildingDead);
            GameEntry.Event.Unsubscribe(EventId.ReviveHeroes, OnReviveHeroes);
            GameEntry.Event.Unsubscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Unsubscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);

            m_InstanceStat.Shutdown();

            DeinitPropagandaManager();
            GameEntry.Input.MeHeroCharacter = null;
            GameEntry.Input.JoystickActivated = false;
            GameEntry.Input.SkillActivated = false;
            m_Data = null;
            m_ResultData = null;
            m_Me.Shutdown();
            m_Me = null;
            base.Shutdown();
        }

        public override void RequestSwitchHero(int index, bool ignoreCD = false)
        {
            if ((!ignoreCD && HeroIsCoolingDown(index)) || !m_Me.GetCurrentHeroCharacter().CanSwitchHero)
            {
                return;
            }

            m_Me.ApplySwitchHero(index);
        }

        public override void OnTargetableShow(TargetableObject targetable)
        {
            OnTargetableUpdate(targetable);
        }

        public override void OnTargetableHide(TargetableObject targetable)
        {
            if (!targetable)
            {
                return;
            }

            for (int i = 0; i < m_RegionsToObserve.Count; ++i)
            {
                var region = m_RegionsToObserve[i];
                UpdateTargetableOutOfRegion(targetable, region);
            }
        }

        public override void OnTargetableUpdate(TargetableObject targetable)
        {
            if (!targetable)
            {
                return;
            }

            for (int i = 0; i < m_RegionsToObserve.Count; ++i)
            {
                InstanceRegionData region = m_RegionsToObserve[i];

                if (AIUtility.TargetCanBeSelected(targetable) && region.Contains(targetable.CachedTransform.localPosition.ToVector2()))
                {
                    UpdateTargetableObjectInRegion(targetable, region);
                }
                else
                {
                    UpdateTargetableOutOfRegion(targetable, region);
                }
            }
        }

        protected override void OnAllHeroesDead()
        {
            base.OnAllHeroesDead();
            GameEntry.TimeScale.PauseGame();
            GameEntry.UI.OpenUIForm(UIFormId.InstanceFailureForm, new InstanceFailureData { FailureReason = InstanceFailureReason.HasBeenBeaten });
        }

        protected void InitInstanceData(Vector2 spawnPosition, float spawnRotation)
        {
            var instanceData = new InstanceData();
            var heroesData = instanceData.MeHeroesData;
            PrepareMyHeroesData(heroesData, spawnPosition, spawnRotation);
            heroesData.SwitchHero(0);
            m_Data = instanceData;
        }

        protected virtual void PrepareMyHeroesData(PlayerHeroesData heroesData, Vector2 spawnPosition, float spawnRotation)
        {
            List<int> heroTypes = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType;
            for (int i = 0; i < heroTypes.Count; i++)
            {
                if (heroTypes[i] <= 0)
                {
                    continue;
                }

                heroesData.Add(GetHeroData(GameEntry.Entity.GetSerialId(), heroTypes[i], spawnPosition, spawnRotation));
            }
        }

        protected override void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            base.OnShowEntitySuccess(sender, e);

            var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;

            if (ne.EntityLogicType == typeof(Building))
            {
                Building building = ne.Entity.Logic as Building;
                if (building == null)
                {
                    Log.Warning("Building is invalid.");
                    return;
                }

                m_BuildingsBeingLoaded.Remove(building.Data.BuildingIndex);
                m_LivingBuildings.Add(building);

                if (IsBuildingDead(building.Data.BuildingIndex))
                {
                    KillBuilding(building);
                    return;
                }

                m_CampTargetableObjects[building.Data.Camp].Add(building);
                return;
            }

            if (ne.EntityLogicType == typeof(NpcCharacter))
            {
                NpcCharacter npc = ne.Entity.Logic as NpcCharacter;
                if (npc == null)
                {
                    Log.Warning("NPC is invalid.");
                    return;
                }

                m_LivingNpcCharacters.Add(npc);

                // 有可能NPC还没刷出来的时候，已经置位死亡了，再检查一次
                if (IsNpcDead(GetNpcIndex(npc.Id)))
                {
                    KillNpc(npc);
                }
            }

            Character character = ne.Entity.Logic as Character;
            if (character == null)
            {
                return;
            }

            if (character is MeHeroCharacter)
            {
                m_Me.OnHeroCharacterShown(character as MeHeroCharacter);
            }
            else
            {
                m_CampTargetableObjects[character.Data.Camp].Add(character);
            }
        }

        protected override void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            base.OnShowEntityFailure(sender, e);
            var ne = e as UnityGameFramework.Runtime.ShowEntityFailureEventArgs;
            BuildingData buildingData = ne.UserData as BuildingData;
            if (buildingData != null)
            {
                m_BuildingsBeingLoaded.Remove(buildingData.BuildingIndex);
            }
        }

        protected override void OnShowWeaponsComplete(object sender, GameEventArgs e)
        {
            base.OnShowWeaponsComplete(sender, e);

            var ne = e as ShowWeaponsCompleteEventArgs;
            var myHeroDatas = m_Data.MeHeroesData.GetHeroes();

            for (int i = 0; i < myHeroDatas.Length; ++i)
            {
                if (ne.HeroData.Id == myHeroDatas[i].Id)
                {
                    m_Me.OnHeroWeaponsShown(ne.HeroData);
                    return;
                }
            }
        }

        protected DR GetInstanceDataRow<DR>(int instanceId) where DR : DRInstance
        {
            IDataTable<DR> dtInstance = GameEntry.DataTable.GetDataTable<DR>();

            DR dr = dtInstance.GetDataRow(instanceId);
            if (dr == null)
            {
                Log.Warning("Can not find instance '{0}' for logic type '{1}'.", instanceId, Type);
            }

            return dr;
        }

        protected void InitInstanceDataBefore(DRInstance drInstance)
        {
            m_SceneId = drInstance.SceneId;
            m_SceneRegionIds = drInstance.GetSceneRegionIds();
            m_SceneBehaviorName = drInstance.AIBehaviors[0];
            m_InstanceNpcsDTName = drInstance.InstanceNpcs;
            m_InstanceBuildingsDTName = drInstance.InstanceBuildings;

            InitInstanceData(new Vector2(drInstance.SpawnPointX, drInstance.SpawnPointY), drInstance.SpawnAngle);
            m_Me = new Me(GameEntry.Data.Player.Id, m_Data.MeHeroesData, m_CampTargetableObjects, this);
        }

        protected void InitGuidePoints(DRInstance drInstance)
        {
            int guidePointSetId = drInstance.GuidePointSetId;
            if (guidePointSetId <= 0)
            {
                return;
            }

            var drGuidePointSet = GameEntry.DataTable.GetDataTable<DRGuidePointSet>().GetDataRow(guidePointSetId);
            if (drGuidePointSet == null)
            {
                Log.Warning("Guide point set '{0}' not found in data table.", guidePointSetId.ToString());
                return;
            }

            m_GuidePointSet.AddGuidePoints(drGuidePointSet.GetGuidePoints(), drGuidePointSet.GetGuidePointGroups());
            m_GuidePointSet.ActivateGroup(0);
        }

        protected void InitNpcDataTable(DRInstance drInstance)
        {
            string[] splitNames = drInstance.InstanceNpcs.Split('_');
            if (splitNames.Length != 2)
            {
                Log.Warning("Instance Buildings table is invalid.");
                return;
            }

            m_InstanceNpcs = GameEntry.DataTable.GetDataTable<DRInstanceNpcs>(splitNames[1]);
            if (m_InstanceNpcs == null)
            {
                Log.Warning("Can not load instance NPCs data table '{0}'.", splitNames[1]);
                return;
            }
        }

        protected void InitBuildingDataTable(DRInstance drInstance)
        {
            if (string.IsNullOrEmpty(drInstance.InstanceBuildings))
            {
                return;
            }

            string[] splitNames = drInstance.InstanceBuildings.Split('_');
            if (splitNames.Length != 2)
            {
                Log.Warning("Instance Buildings table is invalid.");
                return;
            }

            m_InstanceBuildings = GameEntry.DataTable.GetDataTable<DRInstanceBuildings>(splitNames[1]);
            if (m_InstanceBuildings == null)
            {
                Log.Warning("Can not load instance bulidings data table '{0}'.", splitNames[1]);
                return;
            }
        }

        protected void InitTimer(DRInstance drInstance)
        {
            m_InstanceTimer = new InstanceTimer(drInstance.TimerType, drInstance.TimerDuration, drInstance.TimerAlert);
        }

        protected override void OnDeadKeepTimeReached(object sender, GameEventArgs e)
        {
            base.OnDeadKeepTimeReached(sender, e);
            var ne = e as DeadKeepTimeReachedEventArgs;

            var npc = ne.Targetable as NpcCharacter;
            if (npc != null)
            {
                GameEntry.Entity.HideEntity(npc.Entity);
                return;
            }

            var building = ne.Targetable as Building;
            if (building != null)
            {
                GameEntry.Entity.HideEntity(building.Entity);
            }
        }

        protected override AbstractInstancePreparer CreateInstancePreparer()
        {
            return new Preparer();
        }

        protected override AbstractInstanceWaiter CreateInstanceWaiter()
        {
            return new DefaultInstanceWaiter();
        }

        protected override AbstractInstanceRunner CreateInstanceRunner()
        {
            return new Runner();
        }

        private bool ShouldLockHP(ITargetable entity)
        {
            var targetableObj = entity as TargetableObject;
            if (!AIUtility.TargetCanBeSelected(targetableObj))
            {
                return true;
            }

            var myHeroCharacter = targetableObj as MeHeroCharacter;
            if (myHeroCharacter != null)
            {
                return m_BossHasDied;
            }

            return !m_Data.MeHeroesData.AnyIsAlive;
        }

        private void OnLoadBehaviorFailure(object sender, GameEventArgs e)
        {
            var ne = e as LoadBehaviorFailureEventArgs;
            Log.Warning("Behavior '{0}' cannot be found.", ne.Behavior.name);
        }

        protected void StopAllAIsAndProhibitFurtherUse()
        {
            var npcCharacters = GetLivingNpcCharacters();
            for (int i = 0; i < npcCharacters.Count; ++i)
            {
                if (npcCharacters[i] != null && npcCharacters[i].Behavior != null)
                {
                    npcCharacters[i].Behavior.DisableBehavior();
                }
            }

            var buildings = GetLivingBuildings();
            for (int i = 0; i < buildings.Count; ++i)
            {
                if (buildings[i] != null && buildings[i].Behavior != null)
                {
                    buildings[i].Behavior.DisableBehavior();
                }
            }

            var bullets = GetBullets();
            for (int i = 0; i < bullets.Count; ++i)
            {
                if (bullets[i] != null && bullets[i].Behavior != null)
                {
                    bullets[i].Behavior.DisableBehavior();
                }
            }

            DisableBehaviorTrees();
            DisableAutoFight();

            ShouldProhibitAI = true;
        }
    }
}
