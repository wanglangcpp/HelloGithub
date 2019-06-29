using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BasePvpaiInstanceLogic
    {
        private class Runner : AbstractInstanceRunner
        {
            public override void Init(BaseInstanceLogic instanceLogic)
            {
                base.Init(instanceLogic);

                var il = m_InstanceLogic as BasePvpaiInstanceLogic;
                var dataRow = il.m_InstanceDataRow;
                il.m_InstanceTimer = new InstanceTimer(dataRow.TimerType, dataRow.TimerDuration, dataRow.TimerAlert);
                il.m_Target = new TargetManager();
                ShowOpponent();
            }

            public override void OnWin(InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                var instanceLogic = m_InstanceLogic as BasePvpaiInstanceLogic;
                var success = instanceLogic.InitSuccessResult(reason, onComplete);
                instanceLogic.m_Result = success;
                FireShouldGoToResult();
            }

            public override void OnLose(InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
            {
                var instanceLogic = m_InstanceLogic as BasePvpaiInstanceLogic;
                var failure = instanceLogic.InitFailureResult(reason, shouldOpenUI, onComplete);
                instanceLogic.m_Result = failure;
                FireShouldGoToResult();
            }

            public override void OnDraw(InstanceDrawReason reason, GameFrameworkAction onComplete)
            {
                var instanceLogic = m_InstanceLogic as BasePvpaiInstanceLogic;
                var draw = instanceLogic.InitDrawResult(reason, onComplete);
                instanceLogic.m_Result = draw;
                FireShouldGoToResult();
            }

            private void ShowOpponent()
            {
                var instanceLogic = m_InstanceLogic as BasePvpaiInstanceLogic;
                var instanceDataRow = instanceLogic.m_InstanceDataRow;
                var position = new Vector2(instanceDataRow.OppSpawnPointX, instanceDataRow.OppSpawnPointY);
                var rotation = instanceDataRow.OppSpawnAngle;

                var oppHeroesData = instanceLogic.m_OppHeroesData.GetHeroes();
                for (int i = 0; i < oppHeroesData.Length; ++i)
                {
                    oppHeroesData[i].Position = position;
                    oppHeroesData[i].Rotation = rotation;
                }

                instanceLogic.m_Opponent.PrepareAndShowHero();
            }
        }
    }
}
