using GameFramework;
using GameFramework.DataTable;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        /// <summary>
        /// 眩晕状态。
        /// </summary>
        private class StunState : StateBase
        {
            private const int DefaultStunEffectId = 6;

            private string m_DefaultStunEffectPoint = null;
            private string m_DefaultStunEffectResourceName = null;
            private int m_StunEffectId = 0;

            public override StateForImpactCalc StateForImpactCalc
            {
                get
                {
                    return StateForImpactCalc.Stunned;
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

                IDataTable<DRCharacter> dtCharacter = GameEntry.DataTable.GetDataTable<DRCharacter>();
                DRCharacter drCharacter = dtCharacter.GetDataRow(fsm.Owner.Owner.Data.CharacterId);
                if (drCharacter == null)
                {
                    Log.Warning("Can not find character id '{0}'.", fsm.Owner.Owner.Data.CharacterId.ToString());
                    return;
                }

                m_DefaultStunEffectPoint = drCharacter.StunEffectPoint;

                IDataTable<DREffect> dtEffect = GameEntry.DataTable.GetDataTable<DREffect>();
                DREffect drEffect = dtEffect.GetDataRow(DefaultStunEffectId);
                if (drEffect == null)
                {
                    Log.Warning("Can not find default stun effect id '{0}'.", DefaultStunEffectId.ToString());
                    return;
                }

                m_DefaultStunEffectResourceName = drEffect.ResourceName;
            }

            protected override void OnEnter(IFsm<CharacterMotion> fsm)
            {
                base.OnEnter(fsm);

                fsm.Owner.Owner.PlayAnimation("Stun");
                m_StunEffectId = GameEntry.Entity.GetSerialId();
                var effectData = new EffectData(m_StunEffectId, m_DefaultStunEffectPoint, m_DefaultStunEffectResourceName, fsm.Owner.Owner.Id);
                GameEntry.Entity.ShowEffect(effectData);
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);

                if (m_StunEffectId != 0)
                {
                    if (GameEntry.Entity.HasEntity(m_StunEffectId))
                    {
                        GameEntry.Entity.HideEntity(m_StunEffectId);
                    }

                    m_StunEffectId = 0;
                }
            }

            protected override void OnUpdate(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (fsm.CurrentStateTime >= fsm.Owner.StunTime)
                {
                    ChangeState<StandingState>(fsm);
                }
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
        }
    }
}
