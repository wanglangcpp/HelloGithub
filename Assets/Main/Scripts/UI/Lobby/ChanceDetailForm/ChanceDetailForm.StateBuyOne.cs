using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        /// <summary>
        /// 单次抽奖状态。
        /// </summary>
        private class StateBuyOne : StateBase
        {
            private const string OpenChanceAnimClipName = "ChanceItemFlop";

            private ReceivedGeneralItemsViewData m_ReceiveGoodsData;

            private ChanceDetailCardItem CenteredCard { get { return m_Form.m_CenteredCard; } }

            protected override void OnEnter(IFsm<ChanceDetailForm> fsm)
            {
                base.OnEnter(fsm);
                int cardIndex = m_Form.m_CenteredCard.IntKey.Key;
                m_UpdateSubState = UpdateWaitOpenChance;
                GameEntry.Event.Subscribe(EventId.NetworkCustomError, OnNetworkCustomError);

                if (m_Form.m_CanUseFree)
                {
                    GameEntry.LobbyLogic.OpenChance(m_Form.m_CurrentChanceType, cardIndex);
                }
                else if (CheckCurrency())
                {
                    GameEntry.UI.OpenUIForm(UIFormId.CostConfirmDialog, new CostConfirmDialogDisplayData
                    {
                        Mode = CostConfirmDialogType.Other,
                        PreMessage = GameEntry.Localization.GetString(m_Form.SpendCurrencyNoticeKey, m_Form.BuyOneCost.ToString()),
                        UseCurrencyType = m_Form.m_CurrentChanceType == ChanceType.Coin ? CurrencyType.Coin : CurrencyType.Money,
                        CurrencyCount = m_Form.BuyOneCost,
                        OnClickConfirm = (o) => { GameEntry.LobbyLogic.OpenChance(m_Form.m_CurrentChanceType, cardIndex); },
                        OnClickCancel = (o) => { ChangeState<StateNormal>(fsm); },
                    });
                }
                else
                {
                    ChangeState<StateNormal>(fsm);
                }
            }

            protected override void OnLeave(IFsm<ChanceDetailForm> fsm, bool isShutdown)
            {
                GameEntry.Event.Unsubscribe(EventId.NetworkCustomError, OnNetworkCustomError);
                m_UpdateSubState = null;
                m_ReceiveGoodsData = null;
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                m_UpdateSubState(fsm, elapseSeconds, realElapseSeconds);
            }

            public override void OnChanceDataChanged(ChanceDataChangedEventArgs ne)
            {
                m_ReceiveGoodsData = ne.ReceiveGoodsData;
                StartLoadCardToOpen();
                m_UpdateSubState = UpdateLoadCardToOpen;
            }

            private void OnNetworkCustomError(object sender, GameEventArgs e)
            {
                var ne = e as NetworkCustomErrorEventArgs;

                if (!s_BuyFailureErrorCodes.Contains(ne.ServerErrorCode))
                {
                    return;
                }

                ChangeState<StateNormal>(m_Form.m_Fsm);
            }

            private void UpdateWaitOpenChance(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                // Do nothing.
            }

            private void UpdateLoadCardToOpen(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (m_Form.IsLoadingCardItems)
                {
                    return;
                }

                StartPlayOpenChanceAnim();
                m_UpdateSubState = UpdatePlayOpenChanceAnim;
            }

            private void UpdatePlayOpenChanceAnim(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (CenteredCard.CachedAnimation.IsPlaying(OpenChanceAnimClipName))
                {
                    return;
                }

                CenteredCard.Front.SetActive(true);
                CenteredCard.Back.SetActive(false);
                GameEntry.RewardViewer.RequestShowRewards(m_ReceiveGoodsData, true, AfterReceiveItems, null);
                m_UpdateSubState = UpdateWaitReceiveItemFeedbacks;
            }

            private void UpdateWaitReceiveItemFeedbacks(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                // Do nothing.
            }

            private void StartLoadCardToOpen()
            {
                int cardIndex = CenteredCard.IntKey.Key;
                var itemDataIndex = m_Form.m_CurrentChanceData.DummyIndex.IndexOf(cardIndex);
                var itemData = m_Form.m_CurrentChanceData.GoodsForView.Data[itemDataIndex];
                m_Form.LoadCardItemWithData(CenteredCard, itemData);
            }

            private void StartPlayOpenChanceAnim()
            {
                CenteredCard.Back.SetActive(true);
                CenteredCard.BackWidget.alpha = .03f;
                CenteredCard.Front.SetActive(true);
                CenteredCard.CachedAnimation.Play(OpenChanceAnimClipName);
            }

            private void AfterReceiveItems(object userData = null)
            {
                if (CenteredCard != null)
                {
                    CenteredCard.Back.SetActive(false);
                    CenteredCard.BackWidget.alpha = 1f;
                    CenteredCard.Back.transform.localScale = Vector3.one;
                    CenteredCard.Front.transform.localScale = Vector3.one;
                }

                if (m_Form != null)
                {
                    if (m_Form.m_CurrentChanceData.ChancedCount >= m_Form.m_CurrentChanceData.GoodsForView.Data.Count)
                    {
                        ChangeState<StateRequestData>(m_Form.m_Fsm);
                    }
                    else
                    {
                        ChangeState<StateNormal>(m_Form.m_Fsm);
                    }
                }
            }

            private bool CheckCurrency()
            {
                return UIUtility.CheckCurrency(m_Form.CurrencyType, m_Form.BuyOneCost);
            }
        }
    }
}
