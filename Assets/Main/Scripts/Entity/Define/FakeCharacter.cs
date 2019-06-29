using GameFramework;
using GameFramework.DataTable;
using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    /// <summary>
    /// 用于展示的角色。
    /// </summary>
    public class FakeCharacter : Entity
    {
        [SerializeField]
        private FakeCharacterData m_FakeCharacterData = null;

        [SerializeField]
        protected List<Weapon> m_Weapons = null;

        [SerializeField]
        private Transform m_OriginalTransform = null;

        private DRAnimation m_AnimationDataRow = null;

        private IFsm<FakeCharacter> m_Fsm = null;

        private Action m_DebutEndReturn = null;

        public new FakeCharacterData Data
        {
            get
            {
                return m_FakeCharacterData;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            CachedTransform.SetLayerRecursively(Constant.Layer.UIModelLayerId);

            if (m_Weapons == null)
            {
                m_Weapons = new List<Weapon>();
            }
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_FakeCharacterData = userData as FakeCharacterData;
            if (m_FakeCharacterData == null)
            {
                Log.Error("Fake character data is invalid.");
                return;
            }

            var dtAnim = GameEntry.DataTable.GetDataTable<DRAnimation>();
            m_AnimationDataRow = dtAnim.GetDataRow(Data.CharacterId);
            if (m_AnimationDataRow == null)
            {
                Log.Warning("Character animation '{0}' not found.", Data.CharacterId.ToString());
                return;
            }

            m_OriginalTransform = CachedTransform.parent;
            CachedTransform.parent = Data.ParentTransform;
            CachedTransform.localPosition = Data.Position.ToVector3();
            CachedTransform.localRotation = Quaternion.Euler(0f, Data.Rotation, 0f);

            AttachWeapons();
            InitFsm();

            if (!gameObject.activeInHierarchy)
            {
                GameEntry.Entity.HideEntity(Entity);
                return;
            }
        }

        protected override void OnHide(object userData)
        {
            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_Fsm = null;

            CachedTransform.parent = m_OriginalTransform;
            ResetTransform();
            base.OnHide(userData);
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);

            Weapon weapon = childEntity as Weapon;
            if (weapon != null)
            {
                m_Weapons.Add(weapon);
            }
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);

            Weapon weapon = childEntity as Weapon;
            if (weapon != null)
            {
                m_Weapons.Remove(weapon);
            }
        }

        public void ResetTransform()
        {
            CachedTransform.localPosition = Vector3.zero;
            CachedTransform.localRotation = Quaternion.identity;
            CachedTransform.localScale = Vector3.one;
        }

        /// <summary>
        /// 注册英雄获取界面出场动作的回调。
        /// </summary>
        public void RegisterDebutReceiveHeroEndCallback(Action debutEndReturn)
        {
            m_DebutEndReturn = debutEndReturn;
        }

        /// <summary>
        /// 试图播放ReceiveHero界面的默认动画。
        /// </summary>
        public void DefaultReceiveHero()
        {
            (m_Fsm.CurrentState as StateBase).DefaultReceiveHero(m_Fsm);
        }

        /// <summary>
        /// 试图播放出场动作。
        /// </summary>
        public void Debut()
        {
            (m_Fsm.CurrentState as StateBase).Debut(m_Fsm);
        }

        /// <summary>
        /// 试图播放交互动作。
        /// </summary>
        public void Interact()
        {
            (m_Fsm.CurrentState as StateBase).Interact(m_Fsm);
        }

        private void InitFsm()
        {
            m_Fsm = GameEntry.Fsm.CreateFsm(string.Format("Entity{0}", Id.ToString()), this,
                new StateDefault(),
                new StateDebut(),
                new StateInteraction(),
                new StateDebutReceiveHero(),
                new StateDefaultReceiveHero());

            switch (m_FakeCharacterData.ItsActionOnShow)
            {
                case FakeCharacterData.ActionOnShow.Debut:
                    m_Fsm.Start<StateDebut>();
                    break;
                case FakeCharacterData.ActionOnShow.DebutForReceive:
                    m_Fsm.Start<StateDebutReceiveHero>();
                    break;
                case FakeCharacterData.ActionOnShow.None:
                default:
                    m_Fsm.Start<StateDefault>();
                    break;
            }
        }


        private float PlayAnimation(string animationAliasName, bool needRewind = false, bool dontCrossFade = false, bool queued = false)
        {
            if (string.IsNullOrEmpty(animationAliasName))
            {
                Log.Warning("Animation alias name is invalid.");
                return 0f;
            }

            string animationName = m_AnimationDataRow.GetAnimationName(animationAliasName);
            if (string.IsNullOrEmpty(animationName))
            {
                return 0f;
            }

            AnimationState animationState = CachedAnimation[animationName];
            if (animationState == null)
            {
                Log.Warning("Can not find animation '{0}' for character '{1}'.", animationName, Data.CharacterId.ToString());
                return 0f;
            }

            if (needRewind)
            {
                CachedAnimation.Rewind(animationName);
            }

            if (dontCrossFade)
            {
                if (queued)
                {
                    CachedAnimation.PlayQueued(animationName);
                }
                else
                {
                    CachedAnimation.Play(animationName);
                }
            }
            else
            {
                if (queued)
                {
                    CachedAnimation.CrossFadeQueued(animationName);
                }
                else
                {
                    CachedAnimation.CrossFade(animationName);
                }
            }

            PlayWeaponAnimation(animationAliasName, needRewind, dontCrossFade, queued);
            return animationState.length;
        }

        private void PlayWeaponAnimation(string animationAliasName, bool needRewind, bool dontCrossFade, bool queued)
        {
            if (string.IsNullOrEmpty(animationAliasName))
            {
                Log.Warning("Weapon animation alias name is invalid.");
                return;
            }

            for (int i = 0; i < m_Weapons.Count; i++)
            {
                Weapon weapon = m_Weapons[i];
                if (!weapon.gameObject.activeSelf)
                {
                    continue;
                }

                string animationName = weapon.AnimationDataRow.GetAnimationName(animationAliasName);
                if (string.IsNullOrEmpty(animationName))
                {
                    continue;
                }

                AnimationState animationState = weapon.CachedAnimation[animationName];
                if (animationState == null)
                {
                    Log.Warning("Can not find weapon animation '{0}' for weapon '{1}'.", animationName, weapon.Data.WeaponId.ToString());
                    continue;
                }

                if (needRewind)
                {
                    weapon.CachedAnimation.Rewind(animationName);
                }

                if (dontCrossFade)
                {
                    if (queued)
                    {
                        weapon.CachedAnimation.PlayQueued(animationName);
                    }
                    else
                    {
                        weapon.CachedAnimation.Play(animationName);
                    }
                }
                else
                {
                    float corssFadeTime = weapon.AnimationCrossFadeDataRow.GetAnimationCrossFade(animationAliasName);
                    if (corssFadeTime > 0)
                    {
                        if (queued)
                        {
                            weapon.CachedAnimation.CrossFadeQueued(animationName, corssFadeTime);
                        }
                        else
                        {
                            weapon.CachedAnimation.CrossFade(animationName, corssFadeTime);
                        }
                    }
                    else
                    {
                        if (queued)
                        {
                            weapon.CachedAnimation.PlayQueued(animationName);
                        }
                        else
                        {
                            weapon.CachedAnimation.Play(animationName);
                        }
                    }
                }
            }
        }

        public static int Show(int characterId, int heroId, Transform parentTransform, Vector2? position = null, float rotation = 0f, FakeCharacterData.ActionOnShow actionOnShow = FakeCharacterData.ActionOnShow.None)
        {
            if (!position.HasValue)
            {
                position = Vector2.zero;
            }

            int serialId = GameEntry.Entity.GetSerialId();
            var data = new FakeCharacterData(serialId, characterId, heroId, parentTransform, actionOnShow);
            data.Position = position.Value;
            data.Rotation = rotation;

            GameEntry.Entity.ShowFakeCharacter(data);
            return serialId;
        }

        private void AttachWeapons()
        {
            IDataTable<DRWeaponSuite> dtWeaponSuite = GameEntry.DataTable.GetDataTable<DRWeaponSuite>();
            DRWeaponSuite drWeaponSuite = dtWeaponSuite.GetDataRow(Data.WeaponSuiteId);
            if (drWeaponSuite == null)
            {
                Log.Warning("Can not find weapon suite '{0}'.", Data.WeaponSuiteId.ToString());
                return;
            }

            for (int i = 0; i < Constant.MaxWeaponCountInSuite; i++)
            {
                if (!drWeaponSuite.IsWeaponAvailable(i))
                {
                    continue;
                }

                GameEntry.Entity.ShowWeapon(new WeaponData(GameEntry.Entity.GetSerialId(),
                    drWeaponSuite.GetWeaponId(i),
                    drWeaponSuite.Id,
                    i,
                    drWeaponSuite.GetAttachPointPath(i),
                    drWeaponSuite.GetVisibleByDefault(i),
                    WeaponData.WeaponShowType.ForShow,
                    Id));
            }
        }

        /// <summary>
        /// 状态基类
        /// </summary>
        private abstract class StateBase : FsmState<FakeCharacter>
        {
            protected FakeCharacter m_Owner = null;

            private List<int> m_TmpEffectEntityIds = new List<int>();

            protected override void OnInit(IFsm<FakeCharacter> fsm)
            {
                base.OnInit(fsm);
                m_Owner = fsm.Owner;
            }

            protected override void OnEnter(IFsm<FakeCharacter> fsm)
            {
                base.OnEnter(fsm);
                HideTmpEffects();
                // Log.Info("[{0} OnEnter] name={1}", GetType().Name, fsm.Owner.Name);
            }

            protected override void OnLeave(IFsm<FakeCharacter> fsm, bool isShutdown)
            {
                // Log.Info("[{0} OnLeave] name={1}", GetType().Name, fsm.Owner.Name);
                base.OnLeave(fsm, isShutdown);
            }

            public virtual void Debut(IFsm<FakeCharacter> fsm)
            {

            }

            public virtual void DebutReceiveHero(IFsm<FakeCharacter> fsm)
            {

            }

            public virtual void DefaultReceiveHero(IFsm<FakeCharacter> fsm)
            {

            }

            public virtual void Interact(IFsm<FakeCharacter> fsm)
            {

            }

            public virtual void OnEnable(IFsm<FakeCharacter> fsm)
            {
                HideTmpEffects();
            }

            public virtual void OnDisable(IFsm<FakeCharacter> fsm)
            {
                DeactivateTmpEffects();
            }

            protected void PlayEffect(string animationAlias)
            {
                int characterId = m_Owner.m_FakeCharacterData.CharacterId;
                var drCharacterEffect = GameEntry.DataTable.GetDataTable<DRCharacterEffect>().GetDataRow(characterId);
                if (drCharacterEffect == null)
                {
                    return;
                }

                var scenarioData = drCharacterEffect.CharacterEffectData.GetSenarioDataByAnimationAlias(animationAlias);
                for (int i = 0; i < scenarioData.Count; ++i)
                {
                    var resourcePath = scenarioData[i].Key;
                    if (string.IsNullOrEmpty(resourcePath))
                    {
                        continue;
                    }

                    var transformPath = scenarioData[i].Value;
                    var effectEntityId = GameEntry.Entity.GetSerialId();
                    m_TmpEffectEntityIds.Add(effectEntityId);
                    GameEntry.Entity.ShowEffect(new EffectData(effectEntityId, transformPath, resourcePath, m_Owner.Id));
                }
            }

            private void HideTmpEffects()
            {
                for (int i = 0; i < m_TmpEffectEntityIds.Count; i++)
                {
                    var effectEntityId = m_TmpEffectEntityIds[i];
                    if (GameEntry.Entity.IsLoadingEntity(effectEntityId) || GameEntry.Entity.HasEntity(effectEntityId))
                    {
                        GameEntry.Entity.HideEntity(effectEntityId);
                    }
                }

                m_TmpEffectEntityIds.Clear();
            }

            private void DeactivateTmpEffects()
            {
                for (int i = 0; i < m_TmpEffectEntityIds.Count; i++)
                {
                    var effectEntityId = m_TmpEffectEntityIds[i];
                    if (GameEntry.Entity.IsLoadingEntity(effectEntityId))
                    {
                        GameEntry.Entity.HideEntity(effectEntityId);
                    }
                    else if (GameEntry.Entity.HasEntity(effectEntityId))
                    {
                        GameEntry.Entity.GetGameEntity(effectEntityId).gameObject.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// 默认状态。
        /// </summary>
        private class StateDefault : StateBase
        {
            protected override void OnEnter(IFsm<FakeCharacter> fsm)
            {
                base.OnEnter(fsm);
                PlayAnimation();
            }

            public override void Debut(IFsm<FakeCharacter> fsm)
            {
                ChangeState<StateDebut>(fsm);
            }

            public override void Interact(IFsm<FakeCharacter> fsm)
            {
                ChangeState<StateInteraction>(fsm);
            }

            public override void DebutReceiveHero(IFsm<FakeCharacter> fsm)
            {
                ChangeState<StateDebutReceiveHero>(fsm);
            }

            public override void DefaultReceiveHero(IFsm<FakeCharacter> fsm)
            {
                ChangeState<StateDefaultReceiveHero>(fsm);
            }

            public override void OnEnable(IFsm<FakeCharacter> fsm)
            {
                base.OnEnable(fsm);
                PlayAnimation();
            }

            private void PlayAnimation()
            {
                m_Owner.PlayAnimation("ModelShowStand");
            }
        }

        /// <summary>
        /// ReceiveHero界面的默认状态。
        /// </summary>
        private class StateDefaultReceiveHero : StateBase
        {
            protected override void OnEnter(IFsm<FakeCharacter> fsm)
            {
                base.OnEnter(fsm);
                PlayAnimation();
            }

            public override void DebutReceiveHero(IFsm<FakeCharacter> fsm)
            {
                ChangeState<StateDebutReceiveHero>(fsm);
            }

            public override void OnEnable(IFsm<FakeCharacter> fsm)
            {
                PlayAnimation();
            }

            private void PlayAnimation()
            {
                m_Owner.PlayAnimation("ReceiveModelShowStand");
            }
        }

        /// <summary>
        /// 出场状态。
        /// </summary>
        private class StateDebut : StateBase
        {
            private float m_AnimationLength = 0f;

            protected override void OnEnter(IFsm<FakeCharacter> fsm)
            {
                base.OnEnter(fsm);
                string animationAlias = "ModelShowDebut";
                m_AnimationLength = m_Owner.PlayAnimation(animationAlias, dontCrossFade: true);
                PlayEffect(animationAlias);
            }

            protected override void OnUpdate(IFsm<FakeCharacter> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                if (fsm.CurrentStateTime >= m_AnimationLength - Constant.DefaultAnimCrossFadeDuration)
                {
                    EndDebut(fsm);
                }
            }

            public override void OnEnable(IFsm<FakeCharacter> fsm)
            {
                base.OnEnable(fsm);
                EndDebut(fsm);
            }

            private void EndDebut(IFsm<FakeCharacter> fsm)
            {
                ChangeState<StateDefault>(fsm);
            }

            public override void OnDisable(IFsm<FakeCharacter> fsm)
            {
                EndDebut(fsm);
                base.OnDisable(fsm);
            }
        }

        /// <summary>
        /// 出场状态。
        /// </summary>
        private class StateDebutReceiveHero : StateBase
        {
            private float m_AnimationLength = 0f;

            protected override void OnEnter(IFsm<FakeCharacter> fsm)
            {
                base.OnEnter(fsm);
                string animationAlias = "ReceiveModelShowDebut";
                m_AnimationLength = m_Owner.PlayAnimation(animationAlias, dontCrossFade: true);
                PlayEffect(animationAlias);
            }

            protected override void OnUpdate(IFsm<FakeCharacter> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                if (fsm.CurrentStateTime >= m_AnimationLength - Constant.DefaultAnimCrossFadeDuration)
                {
                    EndDebut(fsm);
                }
            }

            public override void DefaultReceiveHero(IFsm<FakeCharacter> fsm)
            {
                ChangeState<StateDefaultReceiveHero>(fsm);
            }

            public override void OnEnable(IFsm<FakeCharacter> fsm)
            {
                base.OnEnable(fsm);
                EndDebut(fsm);
            }

            private void EndDebut(IFsm<FakeCharacter> fsm)
            {
                ChangeState<StateDefaultReceiveHero>(fsm);
                if (fsm.Owner.m_DebutEndReturn != null)
                {
                    fsm.Owner.m_DebutEndReturn();
                }
            }

            public override void OnDisable(IFsm<FakeCharacter> fsm)
            {
                EndDebut(fsm);
                base.OnDisable(fsm);
            }
        }

        /// <summary>
        /// 交互状态。
        /// </summary>
        private class StateInteraction : StateBase
        {
            private float m_AnimationLength = 0f;
            private readonly string[] m_AvailableAnimationNames = new string[] { "ModelInteraction1", "ModelInteraction2", "ModelInteraction3" };

            protected override void OnEnter(IFsm<FakeCharacter> fsm)
            {
                base.OnEnter(fsm);
                string animationAlias = m_AvailableAnimationNames[UnityEngine.Random.Range(0, m_AvailableAnimationNames.Length)];
                m_AnimationLength = m_Owner.PlayAnimation(animationAlias, true);
                PlayEffect(animationAlias);
            }

            protected override void OnUpdate(IFsm<FakeCharacter> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (fsm.CurrentStateTime >= m_AnimationLength - Constant.DefaultAnimCrossFadeDuration)
                {
                    ChangeState<StateDefault>(fsm);
                }
            }

            protected override void OnLeave(IFsm<FakeCharacter> fsm, bool isShutdown)
            {
                m_AnimationLength = 0f;
                base.OnLeave(fsm, isShutdown);
            }

            public override void OnEnable(IFsm<FakeCharacter> fsm)
            {
                base.OnEnable(fsm);
                ChangeState<StateDefault>(fsm);
            }

            private void EndInteraction(IFsm<FakeCharacter> fsm)
            {
                ChangeState<StateDefault>(fsm);
            }

            public override void OnDisable(IFsm<FakeCharacter> fsm)
            {
                EndInteraction(fsm);
                base.OnDisable(fsm);
            }
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            if (m_Fsm != null && m_Fsm.IsRunning)
            {
                (m_Fsm.CurrentState as StateBase).OnEnable(m_Fsm);
            }
        }

        private void OnDisable()
        {
            if (m_Fsm != null && m_Fsm.IsRunning)
            {
                (m_Fsm.CurrentState as StateBase).OnDisable(m_Fsm);
            }
        }

        #endregion MonoBehaviour
    }
}
