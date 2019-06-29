using GameFramework.Fsm;
using System;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        /// <summary>
        /// 初始状态。
        /// </summary>
        private class StateInit : StateBase
        {
            private bool m_OpenAnimIsComplete = false;
            private bool m_RequestingData = false;
            private bool m_NeedRequestData = false;

            protected override void OnEnter(IFsm<ChanceDetailForm> fsm)
            {
                base.OnEnter(fsm);
                m_OpenAnimIsComplete = false;
                m_NeedRequestData = false;
                m_RequestingData = false;

                m_Form.OnOpenInternal();                
                m_Form.CreatePreviewItems();
                m_Form.CreateCardItems();
                
                if (m_Form.m_CurrentChanceData.ChancedCount >= Constant.MaxChancedCardCount)
                {
                    m_NeedRequestData = true;
                    m_RequestingData = true;
                    GameEntry.LobbyLogic.RequestChanceInfo(m_Form.m_CurrentChanceType);
                }
                else
                {
                    m_Form.RefreshPreviewItems();
                    m_Form.RefreshCardItems();
                }
            }

            protected override void OnUpdate(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (!GameEntry.IsAvailable || !m_OpenAnimIsComplete || m_Form.IsLoadingPreviewItems || m_Form.IsLoadingCardItems)
                {
                    return;
                }

                if ((!m_Form.HasOpenedThisFormToday && !m_Form.HasBoughtAny) || (m_NeedRequestData && !m_RequestingData))
                {
                    ChangeState<StateNewContentsSpreading>(fsm);
                }
                else
                {
                    ChangeState<StateNewCardsLayingOut>(fsm);
                }
            }

            protected override void OnLeave(IFsm<ChanceDetailForm> fsm, bool isShutdown)
            {
                m_Form.UpdateOpenTime();
                m_OpenAnimIsComplete = false;
                base.OnLeave(fsm, isShutdown);
            }

            public override void OnPostOpen()
            {
                m_OpenAnimIsComplete = true;
            }

            public override void OnChanceDataChanged(ChanceDataChangedEventArgs e)
            {
                m_RequestingData = false;
                m_Form.RefreshPreviewItems();
                m_Form.RefreshCardItems();
            }
        }
    }
}
