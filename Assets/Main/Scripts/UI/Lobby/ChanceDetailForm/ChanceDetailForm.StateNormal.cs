using GameFramework.Fsm;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        /// <summary>
        /// 正常状态。
        /// </summary>
        private class StateNormal : StateBase
        {
            private int m_CenteredCardEffectId = -1;
            private string m_CenteredCardEffectKey = "EffectCardHov";
            private string m_CenteredCardMoneyEffectKey = "EffectCardMoneyHov";

            protected override void OnEnter(IFsm<ChanceDetailForm> fsm)
            {
                base.OnEnter(fsm);
                m_Form.RefreshCost();
                m_Form.m_CardScrollView.enabled = true;

                if (!fsm.Owner.m_ScrollViewIsMoving)
                {
                    ShowCenteredCardEffect(fsm);
                }
            }

            protected override void OnLeave(IFsm<ChanceDetailForm> fsm, bool isShutdown)
            {
                m_Form.m_CardScrollView.enabled = false;
                HideCenteredCardEffect(fsm);
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);              
            }

            public override void OnClickBuyAllButton(IFsm<ChanceDetailForm> fsm)
            {
                ChangeState<StateBuyAll>(fsm);
            }

            public override void OnClickOpenChanceButton(IFsm<ChanceDetailForm> fsm, int index)
            {
                var cardScript = m_Form.m_CardItems[index];
                if (cardScript == m_Form.m_CenteredCard)
                {
                    if (!m_Form.m_CurrentChanceData.DummyIndex.Contains(index)
                        && Mathf.Abs(m_Form.m_CardListViewTransform.localPosition.x + m_Form.m_CardScrollViewTransform.localPosition.x + cardScript.CachedTransform.localPosition.x) < 5f)
                    {
                        ChangeState<StateBuyOne>(fsm);
                    }
                }
                else
                {
                    m_Form.m_CardCenterOnChild.CenterOn(cardScript.CachedTransform);
                }
            }

            public override void OnClickRefreshButton(IFsm<ChanceDetailForm> fsm)
            {
                ChangeState<StateManualRefresh>(fsm);
            }

            public override void OnCardScrollViewDragStarted(IFsm<ChanceDetailForm> fsm)
            {
                HideCenteredCardEffect(fsm);
            }

            public override void OnCenterOnCard(IFsm<ChanceDetailForm> fsm)
            {
            }

            public override void OnCardScrollViewStoppedMoving(IFsm<ChanceDetailForm> fsm)
            {
                ShowCenteredCardEffect(fsm);
            }

            public override void OnCardScrollViewSpringPanelFinished(IFsm<ChanceDetailForm> fsm)
            {
                ShowCenteredCardEffect(fsm);
            }

            private void HideCenteredCardEffect(IFsm<ChanceDetailForm> fsm)
            {
                if (m_CenteredCardEffectId <= 0)
                {
                    return;
                }

                fsm.Owner.m_EffectsController.DestroyEffect(m_CenteredCardEffectId);
                m_CenteredCardEffectId = -1;
            }

            private void ShowCenteredCardEffect(IFsm<ChanceDetailForm> fsm)
            {
                var centeredCard = m_Form.m_CenteredCard;
                var index = centeredCard.IntKey.Key;
                if (centeredCard == null || fsm.Owner.m_CurrentChanceData.DummyIndex.Contains(index))
                {
                    if (m_CenteredCardEffectId > 0)
                    {
                        HideCenteredCardEffect(fsm);
                    }
                    return;
                }

                if (m_CenteredCardEffectId > 0)
                {
                    return;
                }
                if (fsm.Owner.m_CurrentChanceType == ChanceType.Coin)
                {
                    m_CenteredCardEffectId = fsm.Owner.m_EffectsController.ShowEffect(m_CenteredCardEffectKey);
                }
                else if (fsm.Owner.m_CurrentChanceType == ChanceType.Money)
                {
                    m_CenteredCardEffectId = fsm.Owner.m_EffectsController.ShowEffect(m_CenteredCardMoneyEffectKey);
                }
            }
        }
    }
}
