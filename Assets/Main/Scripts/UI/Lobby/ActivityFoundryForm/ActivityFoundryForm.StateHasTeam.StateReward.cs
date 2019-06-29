using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        private partial class StateHasTeam
        {
            /// <summary>
            /// 可领奖状态。
            /// </summary>
            private class StateReward : StateBase
            {
                private UIButton m_CachedRewardBtn = null;
                private Animation m_CachedAnimation = null;
                private int m_EffectReceiveRewardKey = 0;
                private UIEffectsController m_EffectsController = null;
                private const string EffectReceiveReward = "EffectReceiveBtn";
                private string[] EffectReceiveLevel = { "Primary", "Secondary", "Teritiary" };

                protected override void OnEnter(IFsm<StateHasTeam> fsm)
                {
                    base.OnEnter(fsm);

                    GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
                    GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.CloseUIFormComplete, OnCloseUIFormComplete);

                    m_CachedRewardBtn = m_OuterFsm.Owner.m_ClaimRewardButtons[m_OuterFsm.Owner.RewardLevel];
                    m_CachedRewardBtn.gameObject.SetActive(true);
                    m_CachedAnimation = m_CachedRewardBtn.GetComponent<Animation>();
                    m_CachedAnimation.enabled = true;
                    m_CachedAnimation.Rewind();
                    m_CachedAnimation.Play();
                    m_EffectsController = m_OuterFsm.Owner.m_EffectsController;
                    m_EffectReceiveRewardKey = m_EffectsController.ShowEffect(EffectReceiveReward + EffectReceiveLevel[m_OuterFsm.Owner.RewardLevel]);
                }

                protected override void OnLeave(IFsm<StateHasTeam> fsm, bool isShutdown)
                {
                    m_CachedRewardBtn.gameObject.SetActive(false);
                    m_CachedRewardBtn = null;
                    m_EffectReceiveRewardKey = 0;
                    m_EffectsController = null;
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.CloseUIFormComplete, OnCloseUIFormComplete);

                    base.OnLeave(fsm, isShutdown);
                }

                protected override void OnUpdate(IFsm<StateHasTeam> fsm, float elapseSeconds, float realElapseSeconds)
                {
                    base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                    m_CachedRewardBtn.isEnabled = !m_CachedAnimation.isPlaying;
                }

                public override void OnRewardClaimed(object sender, GameEventArgs e)
                {
                    base.OnRewardClaimed(sender, e);

                    var ne = e as GearFoundryRewardClaimedEventArgs;
                    var rewardsData = new ReceivedGeneralItemsViewData();

                    for (int i = 0; i < ne.Items.Count; ++i)
                    {
                        rewardsData.AddItem(ne.Items[i].Type, ne.Items[i].Count);
                    }
                 
                    //if (rewardsData.Count > 0)
                    //{
                    //    GameEntry.UI.OpenUIForm(UIFormId.ReceiveGearForm, rewardsData.GetShowGearData(false));
                    //}

                    if (m_EffectsController != null && m_EffectReceiveRewardKey != 0)
                    {
                        m_EffectsController.DestroyEffect(m_EffectReceiveRewardKey);
                    }
                }

                private void CheckAndChangeState()
                {
                    if (m_OuterFsm.Owner.IsComplete)
                    {
                        ChangeState<StateCompleting>(m_CachedFsm);
                    }
                    else if (m_OuterFsm.Owner.RewardLevel >= 0)
                    {
                        ChangeState<StateReward>(m_CachedFsm);
                    }
                    else
                    {
                        ChangeState<StateLevelUp>(m_CachedFsm);
                    }
                }

                private void OnCloseUIFormComplete(object sender, GameEventArgs e)
                {
                    var ne = e as UnityGameFramework.Runtime.CloseUIFormCompleteEventArgs;
                    if (ne.UIFormTypeId == (int)(UIFormId.ReceiveGearForm))
                    {
                        CheckAndChangeState();
                    }
                }

                private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
                {

                }
            }
        }
    }
}
