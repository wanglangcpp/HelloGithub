using GameFramework;
using System;

namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        private class Runner : AbstractInstanceRunner
        {
            private BaseSinglePlayerInstanceLogic m_BaseSinglePlayerInstanceLogic = null;

            public BaseSinglePlayerInstanceLogic InstanceLogic
            {
                get
                {
                    if (m_BaseSinglePlayerInstanceLogic == null)
                    {
                        m_BaseSinglePlayerInstanceLogic = m_InstanceLogic as BaseSinglePlayerInstanceLogic;
                    }

                    return m_BaseSinglePlayerInstanceLogic;
                }
            }

            public override void OnDraw(InstanceDrawReason reason, GameFrameworkAction onComplete)
            {
                throw new NotSupportedException();
            }

            public override void OnLose(InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
            {
                InstanceLogic.m_Result = InstanceLogic.InitFailureResult(reason, shouldOpenUI, onComplete);
                FireShouldGoToResult();
            }

            public override void OnWin(InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                InstanceLogic.m_Result = InstanceLogic.InitSuccessResult(reason, onComplete);
                FireShouldGoToResult();
            }

            public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(elapseSeconds, realElapseSeconds);
                InstanceLogic.m_Me.Update(elapseSeconds, realElapseSeconds);
                InstanceLogic.UpdatePropaganda(realElapseSeconds);
            }
        }
    }
}
