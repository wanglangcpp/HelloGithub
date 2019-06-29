using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        private partial class StateHasTeam
        {
            /// <summary>
            /// 升级状态。
            /// </summary>
            private class StateLevelUp : StateBase
            {
                private Transform m_OldLevelIndicator = null;
                private Transform m_NewLevelIndicator = null;
                private Animation m_NewLevelIndicatorAnim = null;
                private Animation m_OldLevelIndicatorAnim = null;
                private GameObject m_NewDecalRotation = null;
                private bool m_StartPlayOldAnim = false;
                private bool m_StartPlayNewAnim = false;

                private const string InClipInName = "FoundryMelteHighIn";
                private const string InClipOutName = "FoundryMelterOut";

                private const float HalfRoundAngle = 180;

                protected override void OnEnter(IFsm<StateHasTeam> fsm)
                {
                    base.OnEnter(fsm);

                    if (m_OuterFsm.Owner.RewardLevel >= 0 || m_OuterFsm.Owner.IsCoolingDown)
                    {
                        m_OuterFsm.Owner.CloseCDMaskDoor(true);
                    }

                    var levelIndicators = m_OuterFsm.Owner.m_LevelIndicators;
                    for (int i = 0; i < levelIndicators.Length; ++i)
                    {
                        if (levelIndicators[i].gameObject.activeSelf)
                        {
                            m_OldLevelIndicator = levelIndicators[i];
                            break;
                        }
                    }

                    int bgLevel = m_OuterFsm.Owner.RewardLevel >= 0 ? m_OuterFsm.Owner.RewardLevel : m_OuterFsm.Owner.m_SrcData.Progress.CurrentLevel;
                    m_NewLevelIndicator = levelIndicators[bgLevel];
                    m_NewDecalRotation = m_OuterFsm.Owner.m_DecalRotation[bgLevel];
                    m_NewLevelIndicatorAnim = m_NewLevelIndicator.GetComponent<Animation>();
                    m_OldLevelIndicatorAnim = m_OldLevelIndicator.GetComponent<Animation>();
                    PlayAnim(m_OldLevelIndicatorAnim, InClipOutName);
                    PlayAnim(m_OuterFsm.Owner.m_CDAnimation, InClipOutName);
                    m_StartPlayOldAnim = true;
                    m_StartPlayNewAnim = false;
                    m_OuterFsm.Owner.CloseCDMaskDoor(true);
                    m_OuterFsm.Owner.m_FireAnimation.transform.parent.gameObject.SetActive(false);
                }

                protected override void OnLeave(IFsm<StateHasTeam> fsm, bool isShutdown)
                {
                    m_OldLevelIndicator.gameObject.SetActive(false);
                    m_OldLevelIndicator = null;
                    m_NewLevelIndicator.gameObject.SetActive(true);
                    m_NewLevelIndicator = null;
                    m_NewLevelIndicatorAnim = null;
                    m_OldLevelIndicatorAnim = null;
                    m_NewDecalRotation = null;
                    m_StartPlayOldAnim = false;
                    m_StartPlayNewAnim = false;
                    m_OuterFsm.Owner.m_FireAnimation.transform.parent.gameObject.SetActive(true);
                    base.OnLeave(fsm, isShutdown);
                }

                protected override void OnUpdate(IFsm<StateHasTeam> fsm, float elapseSeconds, float realElapseSeconds)
                {
                    base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                    if (m_StartPlayOldAnim)
                    {
                        if (!m_OldLevelIndicatorAnim.IsPlaying(InClipOutName))
                        {
                            m_NewLevelIndicator.gameObject.SetActive(true);
                            m_OldLevelIndicator.gameObject.SetActive(false);

                            PlayAnim(m_NewLevelIndicatorAnim, InClipInName);
                            PlayAnim(m_OuterFsm.Owner.m_CDAnimation, InClipInName);
                            m_StartPlayOldAnim = false;
                            m_StartPlayNewAnim = true;
                        }
                    }

                    if (m_StartPlayNewAnim)
                    {
                        if (!m_NewLevelIndicatorAnim.IsPlaying(InClipInName))
                        {
                            TweenRotation tweenRo = m_NewDecalRotation.GetComponent<TweenRotation>();
                            if (tweenRo == null)
                            {
                                tweenRo = m_NewDecalRotation.gameObject.AddComponent<TweenRotation>();
                            }
                            tweenRo.from = new Vector3(0, 0, -HalfRoundAngle);
                            tweenRo.to = new Vector3(0, 0, HalfRoundAngle);
                            tweenRo.duration = 1.0f;
                            tweenRo.ResetToBeginning();
                            tweenRo.PlayForward();
                            CheckStateChange();
                        }
                    }
                }

                private void CheckStateChange()
                {
                    if (m_OuterFsm.Owner.RewardLevel >= 0)
                    {
                        ChangeState<StateHasTeam.StateReward>(m_CachedFsm);
                    }
                    else if (m_OuterFsm.Owner.IsCoolingDown)
                    {
                        ChangeState<StateHasTeam.StateCD>(m_CachedFsm);
                    }
                    else
                    {
                        ChangeState<StateHasTeam.StateFoundry>(m_CachedFsm);
                    }
                }

                private void PlayAnim(Animation anim, string clipName)
                {
                    var animState = anim[clipName];
                    animState.time = 0;
                    animState.speed = 1.0f;
                    animState.clip.SampleAnimation(anim.gameObject, 0f);
                    anim.Play(clipName);
                }
            }
        }
    }
}
