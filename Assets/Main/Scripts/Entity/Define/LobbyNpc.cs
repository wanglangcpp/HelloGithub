using UnityEngine;
using GameFramework.Fsm;
using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// 主城Npc实体
    /// </summary>
    public class LobbyNpc : Entity
    {
        [SerializeField]
        private LobbyNpcData m_LobbyNpcData = null;

        public LobbyNpcData NpcData
        {
            get
            {
                return m_LobbyNpcData;
            }
        }

        private IFsm<LobbyNpc> m_Fsm = null;

        public CapsuleCollider OpenFormTrigger
        {
            get;
            protected set;
        }

        private LobbyNpcNameBoard m_NameBoard = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            OpenFormTrigger = gameObject.GetOrAddComponent<CapsuleCollider>();
            OpenFormTrigger.enabled = false;
            Rigidbody rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            CachedTransform.SetLayerRecursively(Constant.Layer.ColliderTriggerLayerId);
        }

        protected override void OnShow(object userData)
        {
            m_LobbyNpcData = userData as LobbyNpcData;
            base.OnShow(userData);
            OpenFormTrigger.enabled = true;
            OpenFormTrigger.center = new Vector3(m_LobbyNpcData.ColliderCenterX, m_LobbyNpcData.ColliderCenterY, m_LobbyNpcData.ColliderCenterZ);
            OpenFormTrigger.radius = m_LobbyNpcData.ColliderRadius;
            OpenFormTrigger.height = m_LobbyNpcData.ColliderHeight;
            OpenFormTrigger.isTrigger = true;
            CachedTransform.localPosition = AIUtility.SamplePosition(m_LobbyNpcData.Position);
            CachedTransform.localRotation = Quaternion.Euler(new Vector3(0f, m_LobbyNpcData.Rotation, 0f));
            CachedTransform.localScale = Vector3.one * m_LobbyNpcData.Scale;

            m_NameBoard = GameEntry.Impact.CreateNameBoard(this, NameBoardMode.ShowBySelf) as LobbyNpcNameBoard;
            m_NameBoard.OnClickNpcButtonReturnAction = OnClickOpenUI;
            m_NameBoard.SetName(GameEntry.Localization.GetString(NpcData.Name));
            float distance = AIUtility.GetDistance(this, GameEntry.SceneLogic.BaseInstanceLogic.MeHeroCharacter);
            m_NameBoard.SetLobbyNpcButtonVisible(distance <= m_LobbyNpcData.ColliderRadius);
            m_NameBoard.SetLobbyNpcButtonIcon(m_LobbyNpcData.LobbyNpcButtonIcon);

            m_Fsm = GameEntry.Fsm.CreateFsm(Id.ToString(), this,
                        new StandState(),
                        new IdleState());
            m_Fsm.Start<StandState>();
        }

        private void OnClickOpenUI()
        {
            if (!GameEntry.TaskComponent.CheckNpcHasTask(m_LobbyNpcData.LobbyNpcId))
            {
                GameEntry.UI.OpenUIForm(m_LobbyNpcData.OpenUIId);
            }
        }

        protected override void OnHide(object userData)
        {
            GameEntry.Impact.DestroyNameBoard(m_NameBoard);
            m_NameBoard = null;

            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_Fsm = null;

            base.OnHide(userData);
        }

        private void OnTriggerEnter(Collider collider)
        {
            var character = collider.GetComponent<MeHeroCharacter>();
            if (character == null)
            {
                return;
            }
            m_NameBoard.SetLobbyNpcButtonVisible(true);
        }

        private void OnTriggerExit(Collider collider)
        {
            var character = collider.GetComponent<MeHeroCharacter>();
            if (character == null)
            {
                return;
            }
            m_NameBoard.SetLobbyNpcButtonVisible(false);
        }

        private float PlayAnimation(string animationName, bool needRewind = false, bool dontCrossFade = false, bool queued = false)
        {
            if (string.IsNullOrEmpty(animationName))
            {
                //Log.Warning("Animation alias name is invalid.");
                return 0f;
            }

            AnimationState animationState = CachedAnimation[animationName];
            if (animationState == null)
            {
                Log.Warning("Can not find animation '{0}' for character '{1}'.", animationName, Data.Id.ToString());
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
            return animationState.length;
        }

        private abstract class StateBase : FsmState<LobbyNpc>
        {
            protected override void OnEnter(IFsm<LobbyNpc> fsm)
            {
                base.OnEnter(fsm);
            }

            protected override void OnLeave(IFsm<LobbyNpc> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }

            public virtual bool StartInit(IFsm<LobbyNpc> fsm)
            {
                return false;
            }
        }

        private class StandState : StateBase
        {
            private const float MinRandomTime = 20.0f;
            private const float MaxRandomTime = 40.0f;
            private float m_CurTime = 0.0f;
            private float m_EndTime = 0.0f;

            protected override void OnEnter(IFsm<LobbyNpc> fsm)
            {
                base.OnEnter(fsm);
                fsm.Owner.PlayAnimation(fsm.Owner.NpcData.StandAnimationName);
                m_CurTime = 0.0f;
                m_EndTime = Random.Range(MinRandomTime, MaxRandomTime);
            }

            protected override void OnUpdate(IFsm<LobbyNpc> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                m_CurTime += elapseSeconds;
                if (m_CurTime >= m_EndTime)
                {
                    ChangeState<IdleState>(fsm);
                }
            }

            protected override void OnLeave(IFsm<LobbyNpc> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }
        }

        private class IdleState : StateBase
        {
            private float m_AnimationLength = 0f;
            protected override void OnEnter(IFsm<LobbyNpc> fsm)
            {
                base.OnEnter(fsm);
                m_AnimationLength = fsm.Owner.PlayAnimation(fsm.Owner.NpcData.IdleAnimationName);
            }

            protected override void OnUpdate(IFsm<LobbyNpc> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                if (fsm.CurrentStateTime >= m_AnimationLength)
                {
                    ChangeState<StandState>(fsm);
                }
            }

            protected override void OnLeave(IFsm<LobbyNpc> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }
        }
    }
}
