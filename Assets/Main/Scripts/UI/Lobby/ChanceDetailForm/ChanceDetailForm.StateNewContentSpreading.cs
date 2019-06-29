using GameFramework.Fsm;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        /// <summary>
        /// 新内容展开状态。
        /// </summary>
        private class StateNewContentsSpreading : StateBase
        {
            private const string GameObjectNameFormatForRefreshAnimation = "Preview Item Animation ({0})";
            private const string CardPopUpAnimationName = "ChanceItemDeal";
            private const string PreviewItemMaskFadeOutAnimationName = "ChancePreviewItemMaskOut";

            private List<ChanceDetailPreviewItem> m_AnimatedPreviewItems = new List<ChanceDetailPreviewItem>();
            private Animation m_FirstCardAnim = null;
            private List<float> m_CardMovingTimes = new List<float>();

            private Animation RefreshAnimationComponent { get { return m_Form.m_RefreshAnimationComponent; } }

            private Transform RefreshAnimationParent { get { return m_Form.m_RefreshAnimationParent; } }

            private GameObject CardTemplate { get { return m_Form.CardTemplate; } }

            private Transform AnimatedCardsParent { get { return m_Form.m_AnimatedCardsParent; } }

            private float AnimatedCardYOffset { get { return m_Form.m_CardListView.transform.localPosition.y; } }

            private float AnimatedCardXInterval { get { return m_Form.m_CardListView.cellWidth; } }

            private int AnimatedCardCount { get { return m_Form.m_AnimatedCardCount; } }

            private bool ShouldCreateNextCard
            {
                get
                {
                    if (AnimatedCards.Count >= AnimatedCardCount)
                    {
                        return false;
                    }

                    if (AnimatedCards.Count == 1)
                    {
                        return true;
                    }

                    return m_CardMovingTimes[AnimatedCards.Count - 2] <= m_Form.m_AnimatedCardMovingTime - m_Form.m_AnimatedCardTimeInterval;
                }
            }

            private bool AnyCardIsMoving
            {
                get
                {
                    for (int i = 0; i < m_CardMovingTimes.Count; ++i)
                    {
                        if (m_CardMovingTimes[i] > 0f)
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }

            protected override void OnEnter(IFsm<ChanceDetailForm> fsm)
            {
                base.OnEnter(fsm);
                m_CardMovingTimes.Clear();
                AnimatedCards.Clear();
                StartGatheringPreviewItems();
                m_UpdateSubState = UpdateAnimatingPreviewItems;
            }

            protected override void OnUpdate(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                m_UpdateSubState(fsm, elapseSeconds, realElapseSeconds);
            }

            protected override void OnLeave(IFsm<ChanceDetailForm> fsm, bool isShutdown)
            {
                for (int i = 0; i < PreviewItems.Count; ++i)
                {
                    PreviewItems[i].MaskAlpha = 0f;
                }

                DestroyAnimatedPreviewItems();

                m_FirstCardAnim = null;
                m_UpdateSubState = null;
                base.OnLeave(fsm, isShutdown);
            }

            private static void AddDepth(GameObject go, int depthChange)
            {
                UIWidget[] widgets = go.GetComponentsInChildren<UIWidget>(true);

                for (int j = 0; j < widgets.Length; ++j)
                {
                    widgets[j].depth += depthChange;
                }
            }

            private void StartGatheringPreviewItems()
            {
                for (int i = 0; i < PreviewItems.Count; ++i)
                {
                    var go = RefreshAnimationParent.gameObject.AddChild(PreviewItems[i].gameObject);
                    var trans = go.transform;
                    trans.position = PreviewItems[i].transform.position;
                    go.name = string.Format(GameObjectNameFormatForRefreshAnimation, (i + 1).ToString());

                    var previewItemForAnim = go.GetComponent<ChanceDetailPreviewItem>();
                    previewItemForAnim.GeneralItemView.SetOnClickDelegate(null);
                    m_AnimatedPreviewItems.Add(previewItemForAnim);
                    AddDepth(go, m_Form.m_AnimatedPreviewItemDepthChange);

                    PreviewItems[i].MaskAlpha = 1f;
                    previewItemForAnim.MaskAlpha = 0f;
                }

                RefreshAnimationComponent.Play();

                var card = AnimatedCardsParent.gameObject.AddChild(CardTemplate);
                AddDepth(card, m_Form.m_AnimatedCardItemDepthChange);
                var cardScript = card.GetComponent<ChanceDetailCardItem>();
                cardScript.CachedTransform.SetLocalPositionY(AnimatedCardYOffset);
                cardScript.Button.isEnabled = false;
                AnimatedCards.Add(cardScript);
                cardScript.MaskAlpha = 0f;
                m_FirstCardAnim = cardScript.CachedAnimation;
                m_FirstCardAnim.Play(CardPopUpAnimationName);
            }

            private void StartHidingPreviewItemMasks()
            {
                for (int i = 0; i < PreviewItems.Count; ++i)
                {
                    PreviewItems[i].CachedAnimation.Play(PreviewItemMaskFadeOutAnimationName);
                }
            }

            private void DestroyAnimatedPreviewItems()
            {
                for (int i = 0; i < m_AnimatedPreviewItems.Count; ++i)
                {
                    if (m_AnimatedPreviewItems[i] == null)
                    {
                        continue;
                    }

                    Destroy(m_AnimatedPreviewItems[i].gameObject);
                }

                m_AnimatedPreviewItems.Clear();
            }

            private void UpdateAnimatingPreviewItems(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (RefreshAnimationComponent.isPlaying || m_FirstCardAnim.IsPlaying(CardPopUpAnimationName))
                {
                    return;
                }
                DestroyAnimatedPreviewItems();

                m_UpdateSubState = UpdateCardsSpreading;
            }

            private void UpdateCardsSpreading(IFsm<ChanceDetailForm> form, float elapseSeconds, float realElapseSeconds)
            {
                if (AnimatedCards.Count >= AnimatedCardCount && !AnyCardIsMoving)
                {
                    StartHidingPreviewItemMasks();
                    m_UpdateSubState = UpdateHidingPreivewItemMasks;
                    return;
                }

                UpdateCardAlphas();

                ChanceDetailCardItem cardScript;

                for (int i = 0; i < m_CardMovingTimes.Count; ++i)
                {
                    int cardIndex = i + 1;
                    m_CardMovingTimes[i] = Mathf.Max(m_CardMovingTimes[i] - elapseSeconds, 0f);

                    cardScript = AnimatedCards[cardIndex];
                    if (m_CardMovingTimes[i] <= 0f)
                    {
                        cardScript.CachedTransform.SetLocalPositionX(cardIndex * AnimatedCardXInterval);
                        continue;
                    }

                    cardScript.CachedTransform.SetLocalPositionX((cardIndex - m_CardMovingTimes[i] / m_Form.m_AnimatedCardMovingTime) * AnimatedCardXInterval);
                }

                if (AnimatedCards.Count >= AnimatedCardCount)
                {
                    return;
                }

                if (!ShouldCreateNextCard)
                {
                    return;
                }

                var lastCardScript = AnimatedCards[AnimatedCards.Count - 1];
                var card = AnimatedCardsParent.gameObject.AddChild(lastCardScript.gameObject);
                AddDepth(card, m_Form.m_AnimatedCardItemDepthChange);
                cardScript = card.GetComponent<ChanceDetailCardItem>();
                cardScript.CachedTransform.SetLocalPositionY(AnimatedCardYOffset);
                cardScript.CachedTransform.SetLocalPositionX((AnimatedCards.Count - 1) * AnimatedCardXInterval);
                cardScript.CachedTransform.localScale = Vector3.one * m_Form.GetCardScaleForDistance(AnimatedCards.Count * AnimatedCardXInterval);
                cardScript.Button.isEnabled = false;
                AnimatedCards.Add(cardScript);
                m_CardMovingTimes.Add(m_Form.m_AnimatedCardMovingTime);
            }

            private void UpdateHidingPreivewItemMasks(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (PreviewItems[0].CachedAnimation.IsPlaying(PreviewItemMaskFadeOutAnimationName))
                {
                    return;
                }

                ChangeState<StateNewCardsLayingOut>(fsm);
            }

            private void UpdateCardAlphas()
            {
                for (int i = 0; i < AnimatedCards.Count; ++i)
                {
                    var card = AnimatedCards[i];
                    if (card == null)
                    {
                        continue;
                    }

                    card.MaskAlpha = m_Form.GetCardAlphaForDistance(card.CachedTransform.localPosition.x);
                }
            }

        }
    }
}
