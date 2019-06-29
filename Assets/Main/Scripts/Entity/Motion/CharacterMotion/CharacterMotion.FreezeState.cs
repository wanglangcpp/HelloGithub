using GameFramework;
using GameFramework.DataTable;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        /// <summary>
        /// 冰冻状态。
        /// </summary>
        private class FreezeState : StateBase
        {
            private const int DefaultFreezeColorChangerId = 4;
            private const int DefaultFreezeEffectId = 4;
            private const int DefaultFreezeBrokenEffectId = 5;
            private const int DefaultFreezeBrokenSoundId = 9000;

            private CharacterMotionStateCategory m_CurrentStateCategory;
            private string m_DefaultFreezeEffectPoint = null;
            private string m_DefaultFreezeEffectResourceName = null;
            private string m_DefaultFreezeBrokenEffectPoint = null;
            private string m_DefaultFreezeBrokenEffectResourceName = null;
            private int m_ColorChangerId = 0;
            private int m_FreezeEffectId = 0;

            public override StateForImpactCalc StateForImpactCalc
            {
                get
                {
                    return StateForImpactCalc.Frozen;
                }
            }

            public override CharacterMotionStateCategory StateForCharacterMotion
            {
                get
                {
                    return m_CurrentStateCategory;
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

                m_DefaultFreezeEffectPoint = drCharacter.FreezeEffectPoint;
                m_DefaultFreezeBrokenEffectPoint = drCharacter.FreezeBrokenEffectPoint;

                IDataTable<DREffect> dtEffect = GameEntry.DataTable.GetDataTable<DREffect>();
                DREffect drEffect = dtEffect.GetDataRow(DefaultFreezeEffectId);
                if (drEffect == null)
                {
                    Log.Warning("Can not find default freeze effect id '{0}'.", DefaultFreezeEffectId.ToString());
                    return;
                }

                m_DefaultFreezeEffectResourceName = drEffect.ResourceName;

                drEffect = dtEffect.GetDataRow(DefaultFreezeBrokenEffectId);
                if (drEffect == null)
                {
                    Log.Warning("Can not find default freeze broken effect id '{0}'.", DefaultFreezeBrokenEffectId.ToString());
                    return;
                }

                m_DefaultFreezeBrokenEffectResourceName = drEffect.ResourceName;
            }

            protected override void OnEnter(IFsm<CharacterMotion> fsm)
            {
                base.OnEnter(fsm);

                m_CurrentStateCategory = fsm.Owner.FreezeStateCategory;
                fsm.Owner.Owner.CachedAnimation.Stop();
                m_ColorChangerId = fsm.Owner.Owner.StartColorChange(DefaultFreezeColorChangerId);
                m_FreezeEffectId = GameEntry.Entity.GetSerialId();
                var effectData = new EffectData(m_FreezeEffectId, m_DefaultFreezeEffectPoint, m_DefaultFreezeEffectResourceName, fsm.Owner.Owner.Id);
                GameEntry.Entity.ShowEffect(effectData);
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);

                if (m_FreezeEffectId != 0)
                {
                    if (GameEntry.Entity.HasEntity(m_FreezeEffectId))
                    {
                        GameEntry.Entity.HideEntity(m_FreezeEffectId);
                    }

                    m_FreezeEffectId = 0;
                }

                fsm.Owner.Owner.StopColorChange(m_ColorChangerId);

                if (!isShutdown)
                {
                    var effectData = new EffectData(GameEntry.Entity.GetSerialId(), m_DefaultFreezeBrokenEffectPoint, m_DefaultFreezeBrokenEffectResourceName, fsm.Owner.Owner.Id);
                    GameEntry.Entity.ShowEffect(effectData);
                    GameEntry.Sound.PlaySound(DefaultFreezeBrokenSoundId, fsm.Owner.Owner);
                }
            }

            protected override void OnUpdate(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (fsm.CurrentStateTime >= fsm.Owner.FreezeTime)
                {
                    if (fsm.Owner.FreezeStateCategory == CharacterMotionStateCategory.Air)
                    {
                        ChangeState<FloatFallingState>(fsm);
                    }
                    else
                    {
                        ChangeState<StandingState>(fsm);
                    }
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
