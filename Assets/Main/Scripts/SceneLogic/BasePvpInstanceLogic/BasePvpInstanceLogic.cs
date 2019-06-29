using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// Base logic class for online PVP instances (dungeons).
    /// </summary>
    public abstract partial class BasePvpInstanceLogic : BaseInstanceLogic
    {
        [SerializeField]
        private PlayerHeroesData m_MyHeroesData = null;

        [SerializeField]
        private PlayerHeroesData m_OppHeroesData = null;

        private OtherPlayer m_OtherPlayer = null;

        protected bool m_RoomClosed = false;

        protected bool m_RoomReconnect = false;

        public int OtherPlayerId
        {
            get
            {
                return m_OtherPlayer == null ? 0 : m_OtherPlayer.Id;
            }
        }

        public override PlayerHeroesData MyHeroesData
        {
            get
            {
                return m_MyHeroesData;
            }
        }

        public PlayerHeroesData OppHeroesData
        {
            get
            {
                return m_OppHeroesData;
            }
        }

        public override bool CanShowGuideIndicator
        {
            get
            {
                return false;
            }
        }

        private DRSinglePvpInstance m_DRInstance;

        protected DRInstance DRInstance { get { return m_DRInstance; } }

        //public override Vector3 GetPosSyncVelocity(Vector3 displacementToUpdate, float duration)
        //{
        //    var dist = displacementToUpdate.magnitude;
        //    if (dist < 0.01f)
        //    {
        //        return Vector3.zero;
        //    }

        //    return Mathf.Max(duration <= 0f ? 1f : dist / duration, GameEntry.RoomLogic.Config.PositionGradualSyncMaxSpeed) * displacementToUpdate.normalized;
        //}

        public override void Init(int instanceOrSceneId, object userData)
        {
            base.Init(instanceOrSceneId, userData);

            GameEntry.Event.Subscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Subscribe(EventId.RoomBattleResultPushed, OnRoomBattleResultPushed);
            GameEntry.Event.Subscribe(EventId.NetworkCustomError, OnNetworkCustomError);
            // TODO: Delay to subclass when needed.
            var dt = GameEntry.DataTable.GetDataTable<DRSinglePvpInstance>();
            m_DRInstance = dt.GetDataRow(instanceOrSceneId);
            if (m_DRInstance == null)
            {
                Log.Error("Instance '{0}' cannot be found for single PVP.", instanceOrSceneId.ToString());
                return;
            }

            m_SceneId = m_DRInstance.SceneId;
            m_SceneRegionIds = m_DRInstance.GetSceneRegionIds();
        }

        public override void Shutdown()
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
                GameEntry.Event.Unsubscribe(EventId.RoomBattleResultPushed, OnRoomBattleResultPushed);
                GameEntry.Event.Unsubscribe(EventId.NetworkCustomError, OnNetworkCustomError);
            }

            base.Shutdown();
        }

        public override void Update(float elapseTime, float realElapseTime)
        {
            base.Update(elapseTime, realElapseTime);

            if (IsRunning)
            {
                m_Me.Update(elapseTime, realElapseTime);
                m_OtherPlayer.Update(elapseTime, realElapseTime);
            }
        }

        public override void RequestSwitchHero(int index, bool ignoreCD = false)
        {
            if ((!ignoreCD && HeroIsCoolingDown(index)) || !m_Me.GetCurrentHeroCharacter().CanSwitchHero)
            {
                return;
            }

            if (index == m_MyHeroesData.CurrentHeroIndex)
            {
                return;
            }
            int oldEntityId = m_MyHeroesData.CurrentHeroData.Id;
            HeroData newHeroData = m_MyHeroesData.GetHero(index);
            GameEntry.RoomLogic.RequestSwitchHero(oldEntityId, newHeroData.Id, newHeroData.HP);
        }

        public override void TryGoDie(TargetableObject targetableObject)
        {
            // Do nothing, wait network.
            Character character = targetableObject as Character;
            //character.Entity.Id;
            //if (GameEntry.SceneLogic.SinglePvpInstanceLogic.MeHeroCharacter)
            //{
            //    return;
            //}
            Log.Debug("===============>TryGoDie=>" + targetableObject.Id);
            if (character.Id != GameEntry.SceneLogic.SinglePvpInstanceLogic.MeHeroCharacter.Id)
            {
                ApplyGoDie(m_OtherPlayer.Id, character.Id);
            }
            else
            {
                return;
                ApplyGoDie(m_Me.Id, character.Id);
                Log.Debug("===============>Send Dead");
                CRRequestEntityDie request = new CRRequestEntityDie();
                request.DeadEntityId = character.Entity.Id;
                GameEntry.Network.Send(request);
            }


        }

        public bool PlayerIsMe(int playerId)
        {
            return m_Me.Id == playerId;
        }

        public void ApplyGoDie(int playerId, int deadEntityId)
        {
            if (m_Me.Id == playerId)
            {
                HeroData[] heroDatas = m_MyHeroesData.GetHeroes();
                for (int i = 0; i < heroDatas.Length; i++)
                {
                    if (heroDatas[i].Id != deadEntityId)
                    {
                        continue;
                    }
                    if (heroDatas[i].IsDead)
                    {
                        return;
                    }
                    if (GameEntry.Entity.GetEntity(heroDatas[i].Id) != null && (GameEntry.Entity.GetEntity(heroDatas[i].Id).Logic as Character).IsDying)
                    {
                        return;
                    }
                    m_Me.ApplyGoDie(heroDatas[i]);
                    if (CheckBattleEnd(heroDatas))
                    {
                        //Time.timeScale = 0.5f;
                    }
                    return;
                }
                Log.Warning("Can not find dead entity in my heroes.");
            }
            else if (m_OtherPlayer.Id == playerId)
            {
                var heroDatas = m_OppHeroesData.GetHeroes();
                for (int i = 0; i < heroDatas.Length; i++)
                {
                    if (heroDatas[i].Id != deadEntityId)
                    {
                        continue;
                    }
                    if (heroDatas[i].IsDead)
                    {
                        return;
                    }
                    m_OtherPlayer.ApplyGoDie(heroDatas[i]);
                    //检查对手有没有死光光了
                    if (CheckBattleEnd(heroDatas))
                    {
                        //Time.timeScale = 0.5f;
                    }
                    return;
                }
                Log.Warning("Can not find dead entity in opp heroes.");
            }
            else
            {
                Log.Warning("Invalid player id.");
            }
        }

        bool CheckBattleEnd(HeroData[] heros)
        {
            for (int i = 0; i < heros.Length; i++)
            {
                if (heros[i].IsDead == false)
                {
                    return false;
                }
            }
            return true;
        }
        public void ApplySwitchHero(int playerId, int oldEntityId, int newEntityId, int hp, float steadyValue, bool steadyStatus, long lastBreakTime)
        {
            var data = ApplySwitchHero(playerId, oldEntityId, newEntityId, hp);
            data.Steady.SteadyStatus = steadyStatus;
            if (steadyStatus)
            {
                data.Steady.Steady = steadyValue;
            }
            else
            {
                DateTime breakTime = new DateTime(lastBreakTime, DateTimeKind.Utc);
                var duration = (GameEntry.Time.LobbyServerUtcTime - breakTime).TotalSeconds;
                data.Steady.Steady = (float)duration * data.Steady.SteadyRecoverSpeed;
            }
        }

        public HeroData ApplySwitchHero(int playerId, int oldEntityId, int newEntityId, int hp)
        {
            HeroData data = null;
            if (m_Me.Id == playerId)
            {
                var heroDatas = m_MyHeroesData.GetHeroes();
                for (int i = 0; i < heroDatas.Length; i++)
                {
                    if (heroDatas[i].Id != newEntityId)
                    {
                        continue;
                    }

                    data = heroDatas[i];
                    if (hp >= 0)
                    {
                        heroDatas[i].HP = hp;
                    }

                    m_Me.ApplySwitchHero(i);
                    return data;
                }

                Log.Error("Switching to a hero whose entity ID that doesn't belong to the current player.");
            }
            else if (m_OtherPlayer.Id == playerId)
            {
                // TODO: Verify player id.
                var heroDatas = m_OppHeroesData.GetHeroes();
                for (int i = 0; i < heroDatas.Length; i++)
                {
                    if (heroDatas[i].Id != newEntityId)
                    {
                        continue;
                    }

                    GameEntry.RoomLogic.AddLog("Apply Other Switch Hero", "playerId: {0}, oldEntityId: {1}, newEntityId: {2}, HP: {3}", playerId.ToString(), oldEntityId.ToString(), newEntityId.ToString(), hp.ToString());
                    data = heroDatas[i];
                    if (hp >= 0)
                    {
                        heroDatas[i].HP = hp;
                    }

                    m_OtherPlayer.ApplySwitchHero(i);
                    return data;
                }

                Log.Error("ApplyOtherSwitchHero: Switching to a hero whose entity ID doesn't belong to the current player.");
            }
            else
            {
                Log.Warning("Unknown player id.");
            }
            return data;
        }

        public override bool CanAddBuffInSkillTimeLine(ICampable origin, TargetableObject target)
        {
            var entity = origin as Entity;

            while (entity != null)
            {
                if (entity is MeHeroCharacter)
                {
                    return true;
                }

                entity = entity.Owner;
            }

            return false;
        }

        protected override void OnInstanceTimeOut()
        {
            // Do nothing.
        }

        protected override AbstractInstancePreparer CreateInstancePreparer()
        {
            return new Preparer();
        }

        protected override AbstractInstanceWaiter CreateInstanceWaiter()
        {
            return new Waiter();
        }

        protected override AbstractInstanceRunner CreateInstanceRunner()
        {
            return new Runner();
        }

        private void OnRoomBattleResultPushed(object sender, GameEventArgs e)
        {
            var ne = e as RoomBattleResultPushedEventArgs;

            if (ne.Result == InstanceResultType.Win)
            {
                GameEntry.RoomLogic.AddLog("Result Pushed", "Result = {0}, Reason = {1}", ne.Result, ne.SuccessReason);
                SetInstanceSuccess(ne.SuccessReason);
            }
            else if (ne.Result == InstanceResultType.Lose)
            {
                GameEntry.RoomLogic.AddLog("Result pushed", "Result = {0}, Reason = {1}", ne.Result, ne.FailureReason);
                SetInstanceFailure(ne.FailureReason);
            }
            else if (ne.Result == InstanceResultType.Draw)
            {
                GameEntry.RoomLogic.AddLog("Result Pushed", "Result = {0}, Reason = {1}", ne.Result, ne.DrawReason);
                SetInstanceDraw(ne.DrawReason);
            }

            GameEntry.RoomLogic.SaveLogFile();
        }

        protected override void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            base.OnShowEntitySuccess(sender, e);

            var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;

            // TODO: Move the following to FSM states.
            var character = ne.Entity.Logic as Character;
            if (character == null)
            {
                return;
            }

            character.NavAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

            var meHeroCharacter = character as MeHeroCharacter;
            if (meHeroCharacter != null)
            {
                m_Me.OnHeroCharacterShown(meHeroCharacter);
                BaseNameBoard nameBoard = GameEntry.Impact.CreateNameBoard(meHeroCharacter, NameBoardMode.HPBarOnly);
                nameBoard.SetNameColor(nameBoard.OtherNameColor);
                nameBoard.RefreshPosition();
                return;
            }

            var heroCharacter = character as HeroCharacter;
            if (heroCharacter != null)
            {
                m_OtherPlayer.OnHeroCharacterShown(heroCharacter);
                BaseNameBoard nameBoard = GameEntry.Impact.CreateNameBoard(heroCharacter, NameBoardMode.HPBarOnly);
                nameBoard.SetNameColor(nameBoard.OtherNameColor);
                nameBoard.RefreshPosition();
                return;
            }

            m_CampTargetableObjects[character.Data.Camp].Add(character);
        }

        protected override void OnShowWeaponsComplete(object sender, GameEventArgs e)
        {
            base.OnShowWeaponsComplete(sender, e);

            var ne = e as ShowWeaponsCompleteEventArgs;

            var myHeroDatas = m_MyHeroesData.GetHeroes();
            for (int i = 0; i < myHeroDatas.Length; ++i)
            {
                if (ne.HeroData.Id == myHeroDatas[i].Id)
                {
                    m_Me.OnHeroWeaponsShown(ne.HeroData);
                    return;
                }
            }

            var oppHeroDatas = m_OppHeroesData.GetHeroes();
            for (int i = 0; i < oppHeroDatas.Length; ++i)
            {
                if (ne.HeroData.Id == oppHeroDatas[i].Id)
                {
                    m_OtherPlayer.OnHeroWeaponsShown(ne.HeroData);
                    return;
                }
            }
        }

        protected override void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            base.OnShowEntityFailure(sender, e);
        }
        /// <summary>
        /// 主动请求放弃
        /// </summary>
        public void RequestGiveUpPvp()
        {
            GameEntry.RoomLogic.RequestGiveUpPvp();
        }
        protected override void OnInstanceSuccess(InstanceSuccessReason reason, GameFrameworkAction onComplete)
        {
            m_Result = InitSuccessResult(reason, onComplete);
            base.OnInstanceSuccess(reason, onComplete);
        }

        protected override void OnInstanceFailure(InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
        {
            m_Result = InitFailureResult(reason, shouldOpenUI, onComplete);
            base.OnInstanceFailure(reason, shouldOpenUI, onComplete);
        }

        protected override void OnInstanceDraw(InstanceDrawReason reason, GameFrameworkAction onComplete)
        {
            m_Result = InitDrawResult(reason, onComplete);
            base.OnInstanceDraw(reason, onComplete);
        }

        private void GoBackToLobby(object userData)
        {
            GameEntry.SceneLogic.GoBackToLobby();
        }

        private void EnforceGoBackToLobby()
        {
            GameEntry.Loading.Hide();
            GoBackToLobby(null);
        }

        private void PrepareData()
        {
            var MyHeroesData = new PlayerHeroesData();

            var sceneTable = GameEntry.DataTable.GetDataTable<DRSinglePvpInstance>();
            DRSinglePvpInstance sceneConfig = sceneTable.GetDataRow(GameEntry.Data.SingleMatchData.SceneId);
            for (int i = 0; i < GameEntry.Data.SingleMatchData.HeroDatas.Count; i++)
            {

                var hero = GameEntry.Data.SingleMatchData.HeroDatas[i];
                Vector2 pos = GameEntry.Data.SingleMatchData.Index == 0 ? new Vector2(sceneConfig.SpawnPointX, sceneConfig.SpawnPointY) : new Vector2(sceneConfig.SpawnPointX2, sceneConfig.SpawnPointY2);
                float angle = GameEntry.Data.SingleMatchData.Index == 0 ? sceneConfig.SpawnAngle : sceneConfig.SpawnAngle2;
                //HeroData data = new HeroData(hero.CharacterId, true, CharacterDataModifierType.Online);
                int EntryId = GameEntry.Data.SingleMatchData.Index == 0 ? hero.CharacterId + 10000 : hero.CharacterId + 20000;
                HeroData data = PrepareHeroesData(EntryId, true, hero, pos, angle, 1f);

                MyHeroesData.Add(data);
            }
            //m_MyHeroesData = PrepareHeroesData(GameEntry.Data.PvpArenaOpponent.Position, GameEntry.Data.PvpArenaOpponent.Rotation, i);
            //m_MyHeroesData.SwitchHero(GameEntry.Data.PvpArenaOpponent.CurrentHeroIndex);
            //m_MyCamp = m_MyHeroesData.GetHero(GameEntry.Data.PvpArenaOpponent.CurrentHeroIndex).Camp;
            //m_Me = new Me(GameEntry.Data.Player.Id, m_MyHeroesData, m_CampTargetableObjects, this);

            m_MyHeroesData = MyHeroesData;
            m_MyHeroesData.SwitchHero(GameEntry.Data.SingleMatchData.CurrentHeroIndex);
            m_MyCamp = m_MyHeroesData.GetHero(GameEntry.Data.SingleMatchData.CurrentHeroIndex).Camp;
            m_Me = new Me(GameEntry.Data.Player.Id, m_MyHeroesData, m_CampTargetableObjects, this);


            var OppHeroesData = new PlayerHeroesData();

            for (int i = 0; i < GameEntry.Data.PvpArenaOpponent.HeroDatas.Count; i++)
            {
                var hero = GameEntry.Data.PvpArenaOpponent.HeroDatas[i];
                Vector2 pos = GameEntry.Data.PvpArenaOpponent.Index == 0 ? new Vector2(sceneConfig.SpawnPointX, sceneConfig.SpawnPointY) : new Vector2(sceneConfig.SpawnPointX2, sceneConfig.SpawnPointY2);
                float angle = GameEntry.Data.PvpArenaOpponent.Index == 0 ? sceneConfig.SpawnAngle : sceneConfig.SpawnAngle2;
                //HeroData data = new HeroData(hero.CharacterId + 10000, true, CharacterDataModifierType.Online);
                int EntryId = GameEntry.Data.PvpArenaOpponent.Index == 0 ? hero.CharacterId + 10000 : hero.CharacterId + 20000;
                HeroData data = PrepareHeroesData(EntryId, false, hero, pos, angle, 1f);
                OppHeroesData.Add(data);
            }
            m_OppHeroesData = OppHeroesData;
            m_OppHeroesData.SwitchHero(GameEntry.Data.PvpArenaOpponent.CurrentHeroIndex);
            m_OtherPlayer = new OtherPlayer(GameEntry.Data.PvpArenaOpponent.Player.Id, m_OppHeroesData, m_CampTargetableObjects, this);
        }

        private HeroData PrepareHeroesData(int entryId, bool isMe, LobbyHeroData hero, Vector2 spawnPosition, float spawnRotation, float scale)
        {
            var table = GameEntry.DataTable.GetDataTable<DRHero>();
            var heroConfig = table.GetDataRow(hero.Type);
            var heroData = new HeroData(entryId, isMe, CharacterDataModifierType.Online);
            heroData.HeroId = hero.Key;
            heroData.CharacterId = hero.CharacterId;
            heroData.WeaponSuiteId = hero.WeaponSuiteId;
            for (int j = 0; j < Constant.TotalSkillGroupCount; j++)
            {
                heroData.SetSkillLevel(j, hero.SkillLevels[j]);
            }
            heroData.MaxHP = hero.MaxHP;
            heroData.HP = hero.HP;
            heroData.StarLevel = hero.StarLevel;
            heroData.Level = hero.Level;
            heroData.Camp = isMe ? CampType.Player : CampType.Player2;
            heroData.SkillsBadges = hero.GetAllSkillBadges();
            heroData.PhysicalAttack = hero.PhysicalAttack;
            heroData.MagicAttack = hero.MagicAttack;
            heroData.CriticalHitRate = hero.CriticalHitRate;
            heroData.CriticalHitProb = hero.CriticalHitProb;
            heroData.DamageReductionRate = hero.DamageReductionRate;
            heroData.DamageRandomRate = heroConfig.DamageRandomRate;
            heroData.ElementId = heroConfig.ElementId;
            heroData.MagicAtkHPAbsorbRate = hero.MagicAtkHPAbsorbRate;
            heroData.MagicAtkReflectRate = hero.MagicAtkReflectRate;
            heroData.MagicDefense = hero.MagicDefense;
            heroData.PhysicalAtkHPAbsorbRate = hero.PhysicalAtkHPAbsorbRate;
            heroData.PhysicalAtkReflectRate = hero.PhysicalAtkReflectRate;
            heroData.PhysicalDefense = hero.PhysicalDefense;
            heroData.Profession = hero.Profession;
            heroData.RecoverHP = hero.RecoverHP;
            heroData.ReducedHeroSwitchCD = hero.ReducedHeroSwitchCD;
            heroData.ReducedSkillCoolDown = hero.ReducedSkillCoolDown;
            //data.SkillCD
            heroData.Speed = hero.Speed;
            //data.SwitchSkillCD
            heroData.Rotation = spawnRotation;
            heroData.Position = spawnPosition;
            heroData.UserData = hero;
            heroData.WeaponSuiteId = hero.WeaponSuiteId;
            heroData.Scale = heroConfig.Scale;
            heroData.OppPhysicalDfsReduceRate = hero.OppPhysicalDfsReduceRate;
            heroData.OppMagicDfsReduceRate = hero.OppMagicDfsReduceRate;
            heroData.Name = hero.Name;
            //data.MinHP = hero.MaxHPBase;
            heroData.AntiCriticalHitProb = hero.AntiCriticalHitProb;
            //data.AttackOwnerTarget
            heroData.AdditionalDamage = hero.AdditionalDamage;
            //hero.AngerIncreaseRate
            //hero.PortraitSpriteName
            heroData.DebutOnShow = false;
            //不带入技能徽章
            //heroData.SkillsBadges = BaseLobbyHeroData.GetEmptySkillBadges();
            heroData.SwitchSkillLevel = hero.GetSkillLevel(Constant.SwitchSkillIndex);
            heroData.SwitchSkillCD.Reset(heroConfig.CDWhenStart, false);
            heroData.ReplaceSkillsAndCacheDataRows();
            heroData.RefreshDodgeSkillEnergyData();
            heroData.InitSkillLevelLocks();
            heroData.IsDead = (heroData.HP <= 0);
            heroData.MeleeLevel = 1;
            heroData.MeleeExpAtCurrentLevel = 0;
            heroData.Steady.MaxSteady = hero.Steady;
            heroData.Steady.Steady = hero.Steady - 1;
            heroData.Steady.SteadyRecoverSpeed = hero.SteadyRecoverSpeed;
            //heroData.Steady
            return heroData;
        }

        private void PrepareAndShowOppHero()
        {
            m_OtherPlayer.PrepareAndShowHero();
        }

        private void SetRoomBattleStatus(RCGetRoomBattleStatus battleStatus)
        {
            //更新英雄数据
            var myHeroes = m_MyHeroesData.GetHeroes();
            //int currentIndex = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData.CurrentHeroIndex;
            //更换英雄
            //m_MyHeroesData.SwitchHero(battleStatus.MeCurCharacterIndex);

            
            
            for (int i = 0; i < myHeroes.Length; i++)
            {
                HeroData curHero = myHeroes[i];
                PBRoomOnlineCharacterInfo curData = battleStatus.MeCharacterInfo[i];
                curHero.MaxHP = curData.MaxHP;
                curHero.HP = curData.HP;
                (curHero.BuffPool as OnlineBuffPool).AddBuffList(curData.BuffInfos, curHero);
                curHero.Steady.Steady = curData.SteadyValue;
                //curHero.Steady.MaxSteady = curData.SteadyMax;
                curHero.Steady.SteadyStatus = curData.SteadyStatus;
                //curHero TODO:技能同步
                curHero.IsDead = curData.Dead;
            }
            if (battleStatus.MeTransform != null)
            {
                m_MeHeroCharacter.UpdateTransform(battleStatus.MeTransform);
            }
            m_Me.ApplySwitchHero(battleStatus.MeCurCharacterIndex);
            //GameEntry.Event.Fire(this, new MyHeroMovingUpdateEventArgs(myHeroes[battleStatus.MeCurCharacterIndex].Id,
            //    new Vector3(battleStatus.MeTransform.PositionX, battleStatus.MeTransform.PositionY, 0f), battleStatus.MeTransform.Rotation, false));
            //GameEntry.Event.Subscribe(EventId.MyHeroMovingUpdate, OnMyHeroMovingUpdate);
            //GameEntry.Event.Subscribe(EventId.OtherHeroMovingUpdate, OnOtherHeroMovingUpdate);

            //GameEntry.Event.Fire(this, new SwitchHeroCompleteEventArgs(response.PlayerId));
            //if (response.HasSteadyBarStatus)
            //{
            //    GameEntry.SceneLogic.SinglePvpInstanceLogic.ApplySwitchHero(response.PlayerId, response.OldEntityId, response.NewEntityId, response.HP, response.SteadyValue, response.SteadyBarStatus, response.LastBreakSteadyTime);
            //}
            //else
            //{
            //    GameEntry.SceneLogic.SinglePvpInstanceLogic.ApplySwitchHero(response.PlayerId, response.OldEntityId, response.NewEntityId, response.HP);
            //}
            //敌方英雄设置
            var oppHeroes = m_OppHeroesData.GetHeroes();
            //m_OppHeroesData.SwitchHero(battleStatus.OppCurCharacterIndex);
            
            for (int i = 0; i < oppHeroes.Length; i++)
            {
                HeroData curHero = oppHeroes[i];
                PBRoomOnlineCharacterInfo curData = battleStatus.OppCharacterInfos[i];
                curHero.MaxHP = curData.MaxHP;
                curHero.HP = curData.HP;
                (curHero.BuffPool as OnlineBuffPool).AddBuffList(curData.BuffInfos, curHero);
                curHero.Steady.Steady = curData.SteadyValue;
                //curHero.Steady.MaxSteady = curData.SteadyMax;
                curHero.Steady.SteadyStatus = curData.SteadyStatus;
                //curHero TODO:技能同步
                curHero.IsDead = curData.Dead;
            }
            if (battleStatus.OppTransform != null)
            {
                m_OtherPlayer.GetCurrentHeroCharacter().UpdateTransform(battleStatus.OppTransform);
            }
            m_OtherPlayer.ApplySwitchHero(battleStatus.OppCurCharacterIndex);
            
            //GameEntry.Event.Fire(this, new OtherHeroMovingUpdateEventArgs(m_OtherPlayer.Id, oppHeroes[battleStatus.OppCurCharacterIndex].Id,
            //    new Vector2(battleStatus.OppTransform.PositionX, battleStatus.OppTransform.PositionY), battleStatus.OppTransform.Rotation));
        }

        protected override HeroData GetHeroData(int entityId, BaseLobbyHeroData rawHeroData, bool isMe, CampType camp, Vector2 position, float rotation)
        {
            return null;
            //var heroData = base.GetHeroData(entityId, rawHeroData, isMe, camp, position, rotation);

            //var roomHeroData = rawHeroData as RoomHeroData;
            //if (roomHeroData == null)
            //{
            //    return heroData;
            //}

            //if (!m_RoomClosed && m_RoomReconnect)
            //{
            //    IDataTable<DRHero> dtHero = GameEntry.DataTable.GetDataTable<DRHero>();
            //    DRHero drHero = dtHero.GetDataRow(heroData.HeroId);
            //    if (drHero == null)
            //    {
            //        Log.Warning("Can not find hero '{0}'.", heroData.HeroId.ToString());
            //        return null;
            //    }

            //    heroData.SwitchSkillCD.Reset(drHero.CDAfterChangeHero, false);

            //    IDataTable<DRSkillGroup> dtSkillGroup = GameEntry.DataTable.GetDataTable<DRSkillGroup>();
            //    var dtSkill = GameEntry.DataTable.GetDataTable<DRSkill>();
            //    var skillCDs = heroData.SkillCD;
            //    for (int i = 0; i < skillCDs.Length; i++)
            //    {
            //        DRSkillGroup drSkillGroup = dtSkillGroup.GetDataRow(drHero.GetSkillGroupId(i));
            //        if (drSkillGroup == null)
            //        {
            //            Log.Warning("Can not load skill group '{0}' from data table.", drHero.GetSkillGroupId(i).ToString());
            //            return heroData;
            //        }

            //        int skillId = drSkillGroup.SkillId;
            //        DRSkill drSkill = dtSkill.GetDataRow(skillId);
            //        if (drSkill == null)
            //        {
            //            Log.Warning("Can not load skill '{0}' from data table.", skillId.ToString());
            //            return heroData;
            //        }
            //        skillCDs[i].Reset(drSkill.CoolDownTime, false);
            //    }
            //}
            //heroData.Steady.SteadyStatus = roomHeroData.SteadyBarStatus;
            //heroData.Steady.MaxSteady = roomHeroData.MaxSteadyValue;
            //heroData.Steady.SteadyRecoverSpeed = rawHeroData.SteadyRecoverSpeed;
            //heroData.Steady.Steady = roomHeroData.Steady;
            //var buffPool = (heroData.BuffPool as OnlineBuffPool);
            //buffPool.AddBuffList(roomHeroData.Buffs, heroData);
            //return heroData;
        }
        private static readonly List<ServerErrorCode> s_BuyFailureErrorCodes = new List<ServerErrorCode>
        {
            ServerErrorCode.NotFindRoom,
            ServerErrorCode.NotRegister
        };
        public  void OnNetworkCustomError(object sender, GameEventArgs e)
        {
            var ne = e as NetworkCustomErrorEventArgs;

            if (!s_BuyFailureErrorCodes.Contains(ne.ServerErrorCode))
            {
                return;
            }
            GameEntry.Restart();
            //EnforceGoBackToLobby();
        }
    }
}
