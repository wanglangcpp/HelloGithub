using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        /// <summary>
        /// 卡牌排版状态。
        /// </summary>
        private class StateNewCardsLayingOut : StateBase
        {
            protected override void OnEnter(IFsm<ChanceDetailForm> fsm)
            {
                base.OnEnter(fsm);
                m_Form.m_CardScrollViewCache.SetActive(true);
                m_Form.m_CardScrollViewCache.ResetPosition();
                m_Form.m_CardCenterOnChild.CenterOnImmediately(m_Form.m_CardItems[0].CachedTransform, m_Form.m_CardScrollView);
                m_Form.UpdateCardScalesAndAlphas();
            }

            protected override void OnUpdate(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                m_Form.DestroyAnimatedCards();
                ChangeState<StateNormal>(fsm);
            }

            protected override void OnLeave(IFsm<ChanceDetailForm> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }
        }
    }
}
