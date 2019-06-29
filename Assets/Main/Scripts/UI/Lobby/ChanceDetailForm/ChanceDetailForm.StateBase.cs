using GameFramework;
using GameFramework.Fsm;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        /// <summary>
        /// 状态基类。
        /// </summary>
        private abstract class StateBase : FsmState<ChanceDetailForm>
        {
            protected ChanceDetailForm m_Form = null;

            protected IList<ChanceDetailPreviewItem> PreviewItems { get { return m_Form.m_PreviewItems; } }

            protected List<ChanceDetailCardItem> AnimatedCards { get { return m_Form.m_AnimatedCards; } }

            protected IList<ChanceDetailCardItem> CardItems { get { return m_Form.m_CardItems; } }

            protected GameFrameworkAction<IFsm<ChanceDetailForm>, float, float> m_UpdateSubState;

            protected override void OnInit(IFsm<ChanceDetailForm> fsm)
            {
                m_Form = fsm.Owner;
            }

            protected override void OnEnter(IFsm<ChanceDetailForm> fsm)
            {
                base.OnEnter(fsm);
                Log.Info("[ChanceDetailForm] {0} OnEnter", GetType().Name);
            }

            protected override void OnLeave(IFsm<ChanceDetailForm> fsm, bool isShutdown)
            {
                Log.Info("[ChanceDetailForm] {0} OnLeave", GetType().Name);
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                m_Form.UpdateNextFreeTime();
                m_Form.UpdateNextRefreshTime();
                m_Form.UpdateCardScalesAndAlphas();
            }

            public virtual void OnClickRefreshButton(IFsm<ChanceDetailForm> fsm)
            {

            }

            public virtual void OnClickBuyAllButton(IFsm<ChanceDetailForm> fsm)
            {

            }

            public virtual void OnClickOpenChanceButton(IFsm<ChanceDetailForm> fsm, int index)
            {

            }

            public virtual void OnPostOpen()
            {

            }

            public virtual void OnChanceDataChanged(ChanceDataChangedEventArgs e)
            {

            }

            public virtual void OnCardScrollViewDragStarted(IFsm<ChanceDetailForm> fsm)
            {

            }

            public virtual void OnCardScrollViewStoppedMoving(IFsm<ChanceDetailForm> fsm)
            {

            }

            public virtual void OnCenterOnCard(IFsm<ChanceDetailForm> fsm)
            {

            }

            public virtual void OnCardScrollViewSpringPanelFinished(IFsm<ChanceDetailForm> m_Fsm)
            {

            }
        }
    }
}
