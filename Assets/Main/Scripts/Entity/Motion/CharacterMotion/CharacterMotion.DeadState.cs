using GameFramework.Fsm;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        /// <summary>
        /// 死亡状态。
        /// </summary>
        private class DeadState : StateBase
        {
            private float m_KeepTime = 0f;
            private bool m_NotifiedKeepTimeReached = false;

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
                    return true;
                }
            }

            public override void Revive(IFsm<CharacterMotion> fsm)
            {
                fsm.SetData(FsmDataKey_DontCrossFade, (VarBool)true);
                ChangeState<StandingState>(fsm);
            }

            protected override void OnInit(IFsm<CharacterMotion> fsm)
            {
                base.OnInit(fsm);
            }

            protected override void OnEnter(IFsm<CharacterMotion> fsm)
            {
                GameEntry.RoomLogic.AddLog("CharacterMotion DeadState", "OnEnter");

                fsm.Owner.Owner.Data.IsDead = true;
                fsm.Owner.DeadlyImpactSourceEntity = fsm.Owner.ImpactSourceEntity;
                fsm.Owner.DeadlyImpactSourceType = fsm.Owner.ImpactSourceType;

                fsm.Owner.Owner.HideAndRemoveAllEffect();
                fsm.Owner.Owner.ClearBuffs();

                fsm.Owner.Owner.PlayAnimation("Dying");
                fsm.Owner.Owner.PlayAnimation("Dead", false, false, true);
                //切换层级，不接受阴影
                fsm.Owner.transform.SetLayerRecursively(Constant.Layer.AffectedByProjectorLayerId);
                fsm.Owner.Owner.ImpactCollider.enabled = false;

                m_KeepTime = fsm.Owner.Owner.DeadKeepTime;
                m_NotifiedKeepTimeReached = false;
                GameEntry.Event.FireNow(this, new CharacterDeadEventArgs(fsm.Owner.Owner.Data, fsm.Owner.Owner, fsm.Owner.DeadlyImpactSourceEntity, fsm.Owner.DeadlyImpactSourceType));

                base.OnEnter(fsm);
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);

                fsm.Owner.DeadlyImpactSourceEntity = null;
                fsm.Owner.DeadlyImpactSourceType = ImpactSourceType.Unknown;

                if (!isShutdown)
                {
                    fsm.Owner.Owner.ImpactCollider.enabled = true;
                }
            }

            protected override void OnUpdate(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                if (!fsm.Owner.DeadKeepTimeIsReached)
                {
                    return;
                }

                if (!m_NotifiedKeepTimeReached)
                {
                    m_NotifiedKeepTimeReached = true;
                    GameEntry.Event.Fire(this, new DeadKeepTimeReachedEventArgs(fsm.Owner.Owner));
                }
            }

            public override bool DeadKeepTimeIsReached(IFsm<CharacterMotion> fsm)
            {
                return fsm.CurrentStateTime >= m_KeepTime;
            }

            public override bool PerformHPDamage(IFsm<CharacterMotion> fsm)
            {
                // Don't call base.PerformHPDamage here.
                return false;
            }

            public override bool PerformGoDie(IFsm<CharacterMotion> fsm)
            {
                // Don't call base.PerformGoDie here.
                return false;
            }
        }
    }
}
