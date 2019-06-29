using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class InstanceResultForm
    {
        private class RequestState : StateBase
        {
            private const string WinIconAnimName = "InstanceWin";
            private const string BigStarAnimName = "InstanceWinStarIn";
            private const string RequestAnimName = "InstanceWinQuest";

            private float m_Times = 0;
            private float m_GoToNextStepTime = 0.7f;
            private int m_CurrentAnimIndex = 0;
            private float m_CurrentDuration = 0;
            private bool m_StopAnim = false;

            private enum AnimationState
            {
                BigStarAnim0,
                BigStarAnim1,
                BigStarAnim2,
                SmallStarAnim0,
                SmallStarAnim1,
                SmallStarAnim2,
                None,
            }

            private class RequestAnimation
            {
                public Animation RequestAnim = null;
                public AnimationState state = AnimationState.None;
                public string AnimName = "";
                public string EffectKey = "";
            }

            private List<RequestAnimation> m_AnimList = new List<RequestAnimation>();

            private void ChangeNextSate(IFsm<InstanceResultForm> fsm, float realElapseSeconds)
            {
                m_Times += realElapseSeconds;
                if (!fsm.Owner.m_MeridianEnergy.isPlaying && m_Times >= m_GoToNextStepTime)
                {
                    m_Times = 0;
                    GoToNextStep(fsm);
                }
            }

            public override void GoToNextStep(IFsm<InstanceResultForm> fsm)
            {
                m_StopAnim = false;
                fsm.Owner.m_CongratulationsSubPanel.gameObject.SetActive(false);
                ChangeState(fsm, m_NextStateType);
            }

            public override void SkipAnimation(IFsm<InstanceResultForm> fsm)
            {
                if (m_StopAnim)
                {
                    return;
                }

                m_CurrentAnimIndex = m_AnimList.Count;
                m_StopAnim = true;

                for (int i = 0; i < m_AnimList.Count; i++)
                {
                    RequestAnimation anim = m_AnimList[i];

                    if (anim.state >= AnimationState.SmallStarAnim0 && anim.state <= AnimationState.SmallStarAnim2)
                    {
                        var requests = fsm.Owner.m_Requests;
                        int index = m_AnimList[i].state - AnimationState.SmallStarAnim0;
                        requests[index].GetSmallStar().SetActive(true);
                        requests[index].SetComplete(fsm.Owner.m_Data.RequestsComplete[index]);
                        requests[index].SetDesc(GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.GetRequest(i));
                    }
                    anim.RequestAnim.gameObject.SetActive(true);
                    anim.RequestAnim[anim.AnimName].speed = 1;
                    anim.RequestAnim[anim.AnimName].normalizedTime = 1.0f;
                    anim.RequestAnim.Sample();
                }
            }

            protected override void OnEnter(IFsm<InstanceResultForm> fsm)
            {
                base.OnEnter(fsm);
                var requests = fsm.Owner.m_Requests;

                if (!GameEntry.SceneLogic.IsInstance)
                {
                    return;
                }

                m_StopAnim = false;

                fsm.Owner.m_RequestSubPanel.gameObject.SetActive(true);
                fsm.Owner.m_RequestSubPanel.gameObject.transform.localPosition = Vector3.zero;
                m_AnimList.Clear();

                for (int i = 0; i < GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.RequestCompleteCount && i < Constant.InstanceRequestCount; ++i)
                {
                    AddAnimList(fsm.Owner.m_BigStars[i].GetComponent<Animation>(), (AnimationState)i, BigStarAnimName,
                        string.Format("EffectStar{0}", (i + 1).ToString()));
                }

                for (int i = 0; i < Constant.InstanceRequestCount; ++i)
                {
                    if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.IsRequestComplete(i))
                    {
                        AddAnimList(requests[i].GetSmallStar().GetComponent<Animation>(), (AnimationState)(i + (int)AnimationState.SmallStarAnim0), RequestAnimName);
                    }

                    requests[i].SetIncompleteDesc(GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.GetRequest(i));
                    requests[i].GetDesc().SetActive(false);
                    requests[i].GetSmallStar().SetActive(false);
                }

                UIUtility.SetStarLevel(fsm.Owner.m_BigStars, 0);

                m_CurrentAnimIndex = -1;
            }

            protected override void OnUpdate(IFsm<InstanceResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                if (m_StopAnim)
                {
                    m_CurrentDuration += elapseSeconds;
                    if (m_CurrentDuration >= fsm.Owner.m_ExpSubPanelDuration)
                    {
                        ChangeNextSate(fsm, realElapseSeconds);
                        m_CurrentDuration = 0.0f;
                    }
                    return;
                }

                if (UpdatePlayRequestAnimation(fsm, elapseSeconds))
                {
                    var playerData = fsm.Owner.m_Data.ItsPlayer;
                    int meridianEnergyCount = playerData.NewMeridianEnergy - playerData.OldMeridianEnergy;
                    if (meridianEnergyCount > 0)
                    {
                        fsm.Owner.m_CongratulationsSubPanel.gameObject.SetActive(true);
                        fsm.Owner.m_MeridianEnergyCount.text = meridianEnergyCount.ToString();
                        ChangeNextSate(fsm, realElapseSeconds);
                    }
                    else
                    {
                        ChangeNextSate(fsm, realElapseSeconds);
                    }
                }
            }

            private void StartPlayRequestAnimation(IFsm<InstanceResultForm> fsm)
            {
                if (m_AnimList == null || m_AnimList.Count < 1)
                    return; 
                var animData = m_AnimList[m_CurrentAnimIndex];
                if (animData.state >= AnimationState.SmallStarAnim0 && animData.state <= AnimationState.SmallStarAnim2)
                {
                    var requests = fsm.Owner.m_Requests;
                    int index = (int)m_AnimList[m_CurrentAnimIndex].state - (int)AnimationState.SmallStarAnim0;
                    requests[index].SetComplete(fsm.Owner.m_Data.RequestsComplete[index]);
                    requests[index].SetDesc(GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.GetRequest(index));
                }

                animData.RequestAnim.gameObject.SetActive(true);
                animData.RequestAnim[animData.AnimName].speed = fsm.Owner.m_BigStarsAnimationSpeed;
                animData.RequestAnim[animData.AnimName].normalizedTime = 0f;
                animData.RequestAnim.Sample();
                animData.RequestAnim.Play();

                var effectKey = animData.EffectKey;
                if (!string.IsNullOrEmpty(effectKey))
                {
                    fsm.Owner.m_EffectsController.ShowEffect(effectKey);
                }
            }

            private bool UpdatePlayRequestAnimation(IFsm<InstanceResultForm> fsm, float elapseSeconds)
            {
                if (m_CurrentAnimIndex < 0)
                {
                    m_CurrentAnimIndex = 0;
                    StartPlayRequestAnimation(fsm);
                    return false;
                }

                if (m_CurrentAnimIndex >= m_AnimList.Count)
                {
                    return true;
                }

                if (m_AnimList[m_CurrentAnimIndex].RequestAnim.isPlaying)
                {
                    return false;
                }

                m_CurrentAnimIndex++;
                if (m_CurrentAnimIndex >= m_AnimList.Count)
                {
                    return true;
                }

                StartPlayRequestAnimation(fsm);
                return false;
            }

            private void AddAnimList(Animation anim, AnimationState state, string animName, string effectKey = "")
            {
                RequestAnimation requestAnim = new RequestAnimation();
                requestAnim.RequestAnim = anim;
                requestAnim.state = state;
                requestAnim.AnimName = animName;
                requestAnim.EffectKey = effectKey;
                m_AnimList.Add(requestAnim);
            }

            protected override void OnLeave(IFsm<InstanceResultForm> fsm, bool isShutdown)
            {
                var animComp = fsm.Owner.m_RequestSubPanel.GetComponent<Animation>();
                animComp.Rewind(OutwardClipName);
                animComp.Play(OutwardClipName);
                int index = 1;
                for (int i = 0; i < fsm.Owner.m_StarsMoveAnimations.Length; i++)
                {
                    if (index > 3)
                    {
                        index = 1;
                    }
                    string animationIndex = StarsMoveAnimation + index++;
                    fsm.Owner.m_StarsMoveAnimations[i].Play(animationIndex);
                }

                base.OnLeave(fsm, isShutdown);
            }

            public RequestState(Type nextStateType, Transform currentSubPanel, Transform lastSubPanel)
                : base(nextStateType, currentSubPanel, lastSubPanel)
            {

            }
        }
    }
}
