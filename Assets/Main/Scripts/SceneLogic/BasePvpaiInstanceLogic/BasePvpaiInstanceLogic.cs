using GameFramework;
using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// Base logic class for player v.s. player AI instances (dungeons).
    /// </summary>
    [Serializable]
    public abstract partial class BasePvpaiInstanceLogic : BaseInstanceLogic
    {
        [SerializeField]
        private PlayerHeroesData m_MyHeroesData = null;

        [SerializeField]
        private PlayerHeroesData m_OppHeroesData = null;

        [SerializeField]
        private Opponent m_Opponent = null;

        private DRPvpaiInstance m_InstanceDataRow = null;

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

        public bool OppIsSwitchingHero
        {
            get
            {
                return m_Opponent.IsSwitchingHero;
            }
        }

        public bool OppHeroIsCoolingDown(int index)
        {
            return m_Opponent.HeroIsCoolingDown(index);
        }

        public void OppTryAutoSwitchHero()
        {
            m_Opponent.TryAutoSwitchHero();
        }

        protected abstract int OppPlayerId { get; }

        public override void Init(int instanceOrSceneId, object userData)
        {
            base.Init(instanceOrSceneId, userData);
            GameEntry.Event.Subscribe(EventId.CharacterDead, OnCharacterDead);
            GameEntry.Event.Subscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Subscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);

            m_InstanceId = instanceOrSceneId;

            var dt = GameEntry.DataTable.GetDataTable<DRPvpaiInstance>();
            m_InstanceDataRow = dt.GetDataRow(m_InstanceId);
            if (m_InstanceDataRow == null)
            {
                Log.Error("Instance '{0}' for player v.s. player AI doesn't exist.", m_InstanceId);
                return;
            }

            m_SceneId = m_InstanceDataRow.SceneId;
            m_SceneRegionIds = m_InstanceDataRow.GetSceneRegionIds();

            m_MyHeroesData = PrepareMyHeroesData(new Vector2(m_InstanceDataRow.SpawnPointX, m_InstanceDataRow.SpawnPointY), m_InstanceDataRow.SpawnAngle);
            m_MyHeroesData.SwitchHero(0);
            m_OppHeroesData = PrepareOppHeroesData();
            m_OppHeroesData.SwitchHero(0);
            m_Me = new Me(GameEntry.Data.Player.Id, m_MyHeroesData, m_CampTargetableObjects, this);
            m_Opponent = new Opponent(OppPlayerId, m_OppHeroesData, m_CampTargetableObjects, this);
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

        /// <summary>
        /// 对手是否处于公共冷却状态。
        /// </summary>
        public bool IsDuringOppCommonCoolDown
        {
            get
            {
                return m_Opponent.IsDuringCommonCoolDown;
            }
        }

        /// <summary>
        /// 开启对手的公共冷却。供 AI 使用。
        /// </summary>
        /// <param name="coolDownTime">冷却时间。</param>
        public void StartOppCommonCoolDown(float coolDownTime)
        {
            m_Opponent.StartCommonCoolDown(coolDownTime);
        }

        /// <summary>
        /// 停止对手的公共冷却。供 AI 使用。
        /// </summary>
        public void StopOppCommonCoolDown()
        {
            m_Opponent.StopCommonCoolDown();
        }

        /// <summary>
        /// 快进对手的公共冷却。供 AI 使用。
        /// </summary>
        /// <param name="amount"></param>
        public void FastForwardOppCommonCoolDown(float amount)
        {
            m_Opponent.FastForwardCommonCoolDown(amount);
        }

        /// <summary>
        /// 对手是否正在切换英雄。供 AI 使用。
        /// </summary>
        public bool IsOppSwitchingHero
        {
            get
            {
                return m_Opponent.IsSwitchingHero;
            }
        }

        /// <summary>
        /// 请求切换对手英雄。供 AI 调用。
        /// </summary>
        /// <param name="index">目标英雄编号。</param>
        public void OppSwitchHero(int index)
        {
            m_Opponent.RequestSwitchHero(index);
        }

        protected abstract PlayerHeroesData PrepareMyHeroesData(Vector2 spawnPosition, float spawnRotation);

        protected abstract PlayerHeroesData PrepareOppHeroesData();

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            if (IsRunning)
            {
                m_Me.Update(elapseSeconds, realElapseSeconds);
                m_Opponent.Update(elapseSeconds, realElapseSeconds);
            }
        }

        public override void Shutdown()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Input.MeHeroCharacter = null;
            GameEntry.Input.JoystickActivated = false;
            GameEntry.Input.SkillActivated = false;
            DeinitMe();
            DeinitOpponent();
            m_InstanceId = 0;

            GameEntry.Event.Unsubscribe(EventId.CharacterDead, OnCharacterDead);
            GameEntry.Event.Unsubscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Unsubscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);

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

        private void DeinitOpponent()
        {
            if (m_Opponent != null)
            {
                m_Opponent.Shutdown();
                m_Opponent = null;
            }
        }

        private void DeinitMe()
        {
            if (m_Me != null)
            {
                m_Me.Shutdown();
                m_Me = null;
            }
        }

        protected override void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            base.OnShowEntitySuccess(sender, e);

            var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;

            Character character = ne.Entity.Logic as Character;
            if (character == null)
            {
                return;
            }

            var meHeroCharacter = character as MeHeroCharacter;
            if (meHeroCharacter != null)
            {
                m_Me.OnHeroCharacterShown(meHeroCharacter);
                return;
            }

            var heroCharacter = character as HeroCharacter;
            if (heroCharacter != null)
            {
                m_Opponent.OnHeroCharacterShown(heroCharacter);
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
                    m_Opponent.OnHeroWeaponsShown(ne.HeroData);
                    return;
                }
            }
        }

        private void OnCharacterDead(object sender, GameEventArgs e)
        {
            var ne = e as CharacterDeadEventArgs;
            if (ne.Character != null)
            {
                m_CampTargetableObjects[ne.CharacterData.Camp].Remove(ne.Character);
            }

            m_Me.CheckHeroesDeath(ne.CharacterData.Id);
        }

        private void OnLoadBehaviorFailure(object sender, GameEventArgs e)
        {
            LoadBehaviorFailureEventArgs ne = e as LoadBehaviorFailureEventArgs;
            Log.Warning("Can not load behavior '{0}' from '{1}' with error message '{2}'.", ne.BehaviorName, ne.BehaviorAssetName, ne.ErrorMessage);
        }
    }
}
