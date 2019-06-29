using GameFramework;
using GameFramework.DataTable;
using GameFramework.Fsm;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        /// <summary>
        /// 假死状态。
        /// </summary>
        private class FakeDeadState : StateBase
        {
            private float m_KeepTime = 0f;
            private float m_HPBarAnimTime = 0f;
            private float m_StandUpAfterFakeDeathAnimLength = 0f;
            private bool m_PlayedStandUpAnim = false;
            private bool m_PlayedHPBarAnim = false;

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

            protected override void OnInit(IFsm<CharacterMotion> fsm)
            {
                base.OnInit(fsm);
            }

            protected override void OnEnter(IFsm<CharacterMotion> fsm)
            {
                base.OnEnter(fsm);

                if (m_StandUpAfterFakeDeathAnimLength <= 0f)
                {
                    m_StandUpAfterFakeDeathAnimLength = fsm.Owner.Owner.GetAnimationLength("StandUpAfterFakeDeath");
                }

                CheckAndRemoveBuffs(fsm);

                fsm.Owner.Owner.PlayAnimation("FakeDying");
                fsm.Owner.Owner.PlayAnimation("FakeDead", false, false, true);

                fsm.Owner.Owner.NavAgent.enabled = false;
                fsm.Owner.Owner.ImpactCollider.enabled = false;

                m_PlayedStandUpAnim = false;
                m_PlayedHPBarAnim = false;

                NpcCharacter npcCharacter = fsm.Owner.Owner as NpcCharacter;
                if (npcCharacter != null)
                {
                    IDataTable<DRNpc> dtNpc = GameEntry.DataTable.GetDataTable<DRNpc>();
                    DRNpc drNpc = dtNpc.GetDataRow(npcCharacter.Data.NpcId);
                    if (drNpc != null)
                    {
                        m_KeepTime = drNpc.FakeKeepTime;
                        m_HPBarAnimTime = drNpc.FakeRecoverTime;
                        return;
                    }

                    Log.Warning("Cannot find NPC '{0}'.", npcCharacter.Data.NpcId.ToString());
                }

                m_KeepTime = 3f;
                m_HPBarAnimTime = 0f;
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);

                fsm.Owner.Owner.NavAgent.enabled = true;
                fsm.Owner.Owner.ImpactCollider.enabled = true;
            }

            protected override void OnUpdate(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (!m_PlayedStandUpAnim && fsm.CurrentStateTime >= m_KeepTime - m_StandUpAfterFakeDeathAnimLength)
                {
                    m_PlayedStandUpAnim = true;
                    fsm.Owner.Owner.PlayAnimation("StandUpAfterFakeDeath");
                }

                if (!m_PlayedHPBarAnim && fsm.CurrentStateTime >= m_KeepTime - m_HPBarAnimTime)
                {
                    m_PlayedHPBarAnim = true;
                    GameEntry.Impact.CreateNameBoard(fsm.Owner.Owner, NameBoardMode.HPBarOnly, fsm.Owner.FakeDeathRecoverHPRatio, m_HPBarAnimTime);
                }

                if (fsm.CurrentStateTime >= m_KeepTime)
                {
                    fsm.Owner.Owner.Data.HP = Mathf.RoundToInt(fsm.Owner.Owner.Data.MaxHP * fsm.Owner.FakeDeathRecoverHPRatio);
                    ChangeState<StandingState>(fsm);
                }
            }

            public override bool PerformHPDamage(IFsm<CharacterMotion> fsm)
            {
                // Don't call base.PerformHPDamage here.
                return false;
            }

            private void CheckAndRemoveBuffs(IFsm<CharacterMotion> fsm)
            {
                var buffs = fsm.Owner.Owner.Data.Buffs;
                var idsToRemove = new HashSet<int>();
                for (int i = 0; i < buffs.Count; ++i)
                {
                    var buffData = buffs[i];
                    if (!buffData.KeepOnFakeDeath)
                    {
                        idsToRemove.Add(buffData.BuffId);
                    }
                }

                fsm.Owner.Owner.RemoveBuffByIds(new List<int>(idsToRemove));
            }
        }
    }
}
