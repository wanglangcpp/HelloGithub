using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        /// <summary>
        /// 站立状态。
        /// </summary>
        private class StandingState : StateBase
        {
            private float? m_PlayIdleAnimationLeftTime = null;
            private float m_UpdatePlayerLoginPositonDuration = 10;
            private bool m_HasUpdatePosition = true;
            public override StateForImpactCalc StateForImpactCalc
            {
                get
                {
                    return StateForImpactCalc.Normal;
                }
            }

            public override CharacterMotionStateCategory StateForCharacterMotion
            {
                get
                {
                    return CharacterMotionStateCategory.Ground;
                }
            }

            public override bool DontTurnOnHit
            {
                get
                {
                    return false;
                }
            }

            private bool IsLobby
            {
                get
                {
                    if (!GameEntry.IsAvailable)
                    {
                        return false;
                    }

                    return GameEntry.SceneLogic.BaseInstanceLogic.IsLobby;
                }
            }

            protected override void OnInit(IFsm<CharacterMotion> fsm)
            {
                base.OnInit(fsm);
            }

            protected override void OnEnter(IFsm<CharacterMotion> fsm)
            {
                base.OnEnter(fsm);
                m_UpdatePlayerLoginPositonDuration = IsLobby ? GameEntry.ServerConfig.GetFloat(Constant.ServerConfig.Player.ReportLobbyPositionInterval, 10) : 0;
                m_HasUpdatePosition = true;

                bool dontCrossFade = false;
                if (fsm.HasData(FsmDataKey_DontCrossFade))
                {
                    dontCrossFade = fsm.GetData<VarBool>(FsmDataKey_DontCrossFade).Value;
                    fsm.RemoveData(FsmDataKey_DontCrossFade);
                }

                fsm.Owner.Owner.PlayAnimation("Stand", dontCrossFade: dontCrossFade);
                //恢复原有层级，接受阴影
                fsm.Owner.transform.SetLayerRecursively(Constant.Layer.TargetableObjectLayerId);
                if (fsm.Owner.Owner is NpcCharacter)
                {
                    RefreshPlayIdleAnimationLeftTime();
                }
                else
                {
                    m_PlayIdleAnimationLeftTime = null;
                }
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                if (IsLobby && m_HasUpdatePosition &&
                    GameEntry.SceneLogic.BaseInstanceLogic != null &&
                    GameEntry.SceneLogic.BaseInstanceLogic.MeHeroCharacter != null &&
                    GameEntry.SceneLogic.BaseInstanceLogic.MeHeroCharacter.Data.Id == fsm.Owner.Owner.Id &&
                    fsm.CurrentStateTime >= m_UpdatePlayerLoginPositonDuration)
                {
                    m_HasUpdatePosition = false;
                    GameEntry.Data.Player.LoginPostion = GameEntry.SceneLogic.BaseInstanceLogic.MeHeroCharacter.transform.position.ToVector2();
                    GameEntry.LobbyLogic.UpdateNearbyPlayersPosition();
                }

                if (m_PlayIdleAnimationLeftTime != null)
                {
                    m_PlayIdleAnimationLeftTime -= elapseSeconds;
                    if (m_PlayIdleAnimationLeftTime <= 0f)
                    {
                        fsm.Owner.Owner.PlayAnimation("Idle");
                        fsm.Owner.Owner.PlayAnimation("Stand", false, false, true);
                        RefreshPlayIdleAnimationLeftTime();
                    }
                }
            }

            public override bool StartMove(IFsm<CharacterMotion> fsm)
            {
                ChangeState<MovingState>(fsm);
                return true;
            }

            public override void PerformSkill(IFsm<CharacterMotion> fsm, bool forcePerform)
            {
                ChangeState<SkillState>(fsm);
            }

            public override bool PerformStiffness(IFsm<CharacterMotion> fsm)
            {
                ChangeState<StiffnessState>(fsm);

                return true;
            }

            public override bool PerformHardHit(IFsm<CharacterMotion> fsm)
            {
                ChangeState<StiffnessState>(fsm);

                return true;
            }

            public override bool PerformBlownAway(IFsm<CharacterMotion> fsm)
            {
                ChangeState<FloatState>(fsm);

                return true;
            }

            public override bool PerformFloat(IFsm<CharacterMotion> fsm)
            {
                ChangeState<FloatState>(fsm);

                return true;
            }

            public override bool PerformStun(IFsm<CharacterMotion> fsm)
            {
                ChangeState<StunState>(fsm);
                return true;
            }

            public override bool PerformFreeze(IFsm<CharacterMotion> fsm)
            {
                ChangeState<FreezeState>(fsm);
                return true;
            }

            public override bool PlayEntering(IFsm<CharacterMotion> fsm)
            {
                ChangeState<EnteringState>(fsm);

                return true;
            }

            private void RefreshPlayIdleAnimationLeftTime()
            {
                m_PlayIdleAnimationLeftTime = Random.Range(10f, 20f);
            }

            public override bool StartLingering(IFsm<CharacterMotion> fsm)
            {
                ChangeState<LingeringState>(fsm);

                return true;
            }
        }
    }
}
