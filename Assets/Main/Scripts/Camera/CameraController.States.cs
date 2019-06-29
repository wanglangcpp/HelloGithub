using GameFramework;
using GameFramework.Fsm;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public partial class CameraController
    {
        public enum StateId
        {
            Init,
            Following,
            LoadingAnimation,
            PlayingAnimation,
            PreparePortage,
            Portaging,
        }

        public StateId CurrentStateId
        {
            get
            {
                return (m_Fsm.CurrentState as StateBase).StateId;
            }
        }

        private IFsm<CameraController> m_Fsm = null;

        private const string FsmDataKey_AnimAssetName = "AnimAssetName";
        private const string FsmDataKey_AnimClipName = "AnimClipName";
        private const string FsmDataKey_AboutToStart = "AboutToStart";
        private const string FsmDataKey_AboutToStop = "AboutToStop";

        private abstract class StateBase : FsmState<CameraController>
        {
            public abstract StateId StateId { get; }

            public virtual void OnOwnerStart(IFsm<CameraController> fsm)
            {
                // Empty.
            }

            public virtual bool SetTarget(IFsm<CameraController> fsm, Transform targetTransform)
            {
                var owner = fsm.Owner;
                if (owner.m_TargetTransform != null)
                {
                    Log.Error("You've already had a target.");
                    return false;
                }

                if (targetTransform == null)
                {
                    Log.Error("targetTransform is invalid.");
                    return false;
                }

                owner.m_TargetTransform = targetTransform;
                return true;
            }

            public void ResetTarget(IFsm<CameraController> fsm)
            {
                var owner = fsm.Owner;
                owner.m_TargetTransform = null;
            }

            public virtual void ChangeParams(IFsm<CameraController> m_Fsm, Vector3? cameraOffset, float? centerOffset, float duration)
            {
                Log.Warning("You cannot change params in state '{0}'.", StateId.ToString());
            }

            public virtual bool StartAnimation(IFsm<CameraController> m_Fsm, string animName, float aboutToStart, float aboutToStop)
            {
                Log.Warning("You cannot start animation in state '{0}'.", StateId.ToString());
                return false;
            }

            public virtual void StartPortage(IFsm<CameraController> m_Fsm, int portageFromId, int portageToId)
            {
                Log.Warning("You cannot start protage in state '{0}'.", StateId.ToString());
            }

            public virtual void StopAnimation(IFsm<CameraController> m_Fsm)
            {

            }

            public virtual void FollowImmediately(IFsm<CameraController> fsm)
            {
                if (!(fsm.CurrentState is StateFollowing))
                {
                    ChangeState<StateFollowing>(fsm);
                }

                fsm.Owner.FollowImmediately_Internal();
            }

            protected override void OnEnter(IFsm<CameraController> fsm)
            {
                Log.Info("[CameraController.{0} OnEnter]", GetType().Name);
            }

            protected override void OnLeave(IFsm<CameraController> fsm, bool isShutdown)
            {
                Log.Info("[CameraController.{0} OnLeave]", GetType().Name);
            }

            protected override void OnUpdate(IFsm<CameraController> fsm, float elapseSeconds, float realElapseSeconds)
            {
                UpdateOcclusionRaycastIfNeeded(fsm);
            }

            protected static void UpdateOcclusionRaycastIfNeeded(IFsm<CameraController> fsm)
            {
                if (fsm.Owner.m_ShouldUpdateOcclusionRaycast)
                {
                    fsm.Owner.UpdateOcclusionRaycast();
                }
            }
        }

        private class StateInit : StateBase
        {
            public override StateId StateId
            {
                get
                {
                    return StateId.Init;
                }
            }

            protected override void OnEnter(IFsm<CameraController> fsm)
            {
                base.OnEnter(fsm);
                var owner = fsm.Owner;
                owner.m_OriginalCameraOffset = owner.m_CameraOffset;
                owner.m_OriginalCenterOffset = owner.m_CenterOffset;

                ChangeState<StateFollowing>(fsm);
            }

            public override void FollowImmediately(IFsm<CameraController> fsm)
            {
                Log.Error("Cannot follow immediately in state '{0}'.", GetType().ToString());
            }
        }

        private class StateFollowing : StateBase
        {
            private bool m_HasSetTarget = false;

            public override StateId StateId
            {
                get
                {
                    return StateId.Following;
                }
            }

            public override bool SetTarget(IFsm<CameraController> fsm, Transform targetTransform)
            {
                if (!base.SetTarget(fsm, targetTransform)) return false;

                var owner = fsm.Owner;
                owner.m_TargetTransform = targetTransform;

                if (!m_HasSetTarget)
                {
                    OnFirstTimeSetTarget(owner);
                }

                return true;
            }

            public override void ChangeParams(IFsm<CameraController> fsm, Vector3? cameraOffset, float? centerOffset, float duration)
            {
                var owner = fsm.Owner;

                if (duration <= 0f)
                {
                    owner.m_AimCameraOffset = owner.m_CameraOffset = cameraOffset ?? owner.m_CameraOffset;
                    owner.m_AimCenterOffset = owner.m_CenterOffset = centerOffset ?? owner.m_CenterOffset;
                    owner.m_ParamChangingDuration = 0f;
                    owner.FollowImmediately_Internal();
                    return;
                }

                owner.m_AimCameraOffset = cameraOffset ?? owner.m_CameraOffset;
                owner.m_AimCenterOffset = centerOffset ?? owner.m_CenterOffset;
                owner.m_ParamChangingDuration = duration;
            }

            public override void OnOwnerStart(IFsm<CameraController> fsm)
            {
                UpdateCamera(fsm);
            }

            public override bool StartAnimation(IFsm<CameraController> fsm, string animName, float aboutToStart, float aboutToStop)
            {
                if (string.IsNullOrEmpty(animName))
                {
                    throw new ArgumentException("'animName' cannot be empty.", animName);
                }

                fsm.SetData(FsmDataKey_AnimAssetName, (VarString)animName);
                fsm.SetData(FsmDataKey_AboutToStart, (VarFloat)aboutToStart);
                fsm.SetData(FsmDataKey_AboutToStop, (VarFloat)aboutToStop);
                ChangeState<StateLoadingAnimation>(fsm);
                return true;
            }

            public override void FollowImmediately(IFsm<CameraController> fsm)
            {
                fsm.Owner.FollowImmediately_Internal();
            }

            protected override void OnEnter(IFsm<CameraController> fsm)
            {
                base.OnEnter(fsm);

                m_HasSetTarget = false;

                if (fsm.Owner.m_TargetTransform != null)
                {
                    OnFirstTimeSetTarget(fsm.Owner);
                }
            }

            protected override void OnUpdate(IFsm<CameraController> fsm, float elapseSeconds, float realElapseSeconds)
            {
                UpdateCamera(fsm);
            }

            public override void StartPortage(IFsm<CameraController> fsm, int portageFromId, int portageToId)
            {
                if (fsm.Owner.m_TargetTransform == null)
                {
                    Log.Warning("Can't protage cause of null target.");
                    return;
                }

                fsm.Owner.PortageFromId = portageFromId;
                fsm.Owner.PortageToId = portageToId;
                ChangeState<StatePreparePortage>(fsm);
            }

            private void OnFirstTimeSetTarget(CameraController owner)
            {
                m_HasSetTarget = true;
                owner.m_AimPosition = owner.m_CachedTransform.position = owner.m_TargetTransform.position + owner.m_CameraOffset;
                owner.UpdateLookAt();
                owner.UpdateCameraParams();
            }

            private void UpdateCamera(IFsm<CameraController> fsm)
            {
                var owner = fsm.Owner;
                if (owner.m_TargetTransform == null)
                {
                    return;
                }

                owner.UpdateCameraParams();
                Vector3 lastPos = owner.m_CachedTransform.position;
                owner.FollowLazily();
                owner.m_LastCameraPos = lastPos;
                owner.m_LastDeltaTime = Time.deltaTime;
                owner.m_TargetLastPosition = owner.m_TargetTransform.position;
                UpdateOcclusionRaycastIfNeeded(fsm);
            }
        }

        /// <summary>
        /// 传送前的准备状态。停止自动战斗、渐变场景颜色等操作。
        /// </summary>
        private class StatePreparePortage : StateBase
        {
            private float m_WaitAnimationTimer;
            private const float MaskFadeInTime = 0.2f;

            public override StateId StateId
            {
                get
                {
                    return StateId.PreparePortage;
                }
            }

            protected override void OnEnter(IFsm<CameraController> fsm)
            {
                base.OnEnter(fsm);

                GameEntry.Event.FireNow(this, new PortagingEffectEventArgs(fsm.Owner.PortageFromId,
                    fsm.Owner.m_TargetLastPosition,
                    fsm.Owner.m_TargetTransform.rotation.eulerAngles.y,
                    true));

                // 缓存是否开启了自动战斗
                fsm.Owner.IsAutoFighting = GameEntry.SceneLogic.BaseInstanceLogic.AutoFightIsEnabled;
                GameEntry.SceneLogic.BaseInstanceLogic.DisableAutoFight();
                // Play mask fade in animation
                GameEntry.Event.Fire(this, new PortagingAnimationEventArgs());
            }

            protected override void OnUpdate(IFsm<CameraController> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                bool isAnimationFadeInOver = (m_WaitAnimationTimer += realElapseSeconds) > MaskFadeInTime;
                // 蒙版渐入效果结束，那么切状态机。
                if (isAnimationFadeInOver)
                {
                    ChangeState<StatePortaging>(fsm);
                    m_WaitAnimationTimer = 0;
                }
            }

            public override void StartPortage(IFsm<CameraController> m_Fsm, int portageFromId, int portageToId)
            {

            }
        }

        /// <summary>
        /// 传送时候的状态。
        /// </summary>
        private class StatePortaging : StateBase
        {
            public override StateId StateId
            {
                get
                {
                    return StateId.Portaging;
                }
            }

            protected override void OnEnter(IFsm<CameraController> fsm)
            {
                base.OnEnter(fsm);

                // 摄像机跟随。
                fsm.Owner.FollowImmediately_Internal();

                ChangeState<StateFollowing>(fsm);
            }

            protected override void OnLeave(IFsm<CameraController> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);

                // 离开本状态的时候，还原传送之前的所有状态。
                if (fsm.Owner.IsAutoFighting)
                    GameEntry.SceneLogic.BaseInstanceLogic.EnableAutoFight();

                // 到达传送点，播放抵达特效。
                GameEntry.Event.FireNow(this, new PortagingEffectEventArgs(fsm.Owner.PortageToId,
                    fsm.Owner.m_TargetTransform.position,
                    fsm.Owner.m_TargetTransform.rotation.eulerAngles.y,
                    false));

                fsm.Owner.PortageToId = int.MinValue;
                fsm.Owner.PortageFromId = int.MinValue;
            }
        }

        private class StateLoadingAnimation : StateBase
        {
            public override StateId StateId
            {
                get
                {
                    return StateId.LoadingAnimation;
                }
            }

            private bool m_Stopped = false;

            public override bool StartAnimation(IFsm<CameraController> fsm, string animName, float aboutToStart, float aboutToStop)
            {
                Log.Info("Trying to start animaion in state '{0}'. Ignore.", StateId.ToString());
                return false;
            }

            public override void StopAnimation(IFsm<CameraController> fsm)
            {
                m_Stopped = true;
            }

            protected override void OnEnter(IFsm<CameraController> fsm)
            {
                base.OnEnter(fsm);
                m_Stopped = false;

                var animName = fsm.GetData<VarString>(FsmDataKey_AnimAssetName).Value;
                var splitted = animName.Split('/');
                var clipName = splitted[splitted.Length - 1];

                var owner = fsm.Owner;
                if (owner.CachedAnimation.GetClip(animName) != null)
                {
                    fsm.SetData(FsmDataKey_AnimClipName, (VarString)clipName);
                    ChangeState<StatePlayingAnimation>(fsm);
                    return;
                }

                GameFramework.Resource.LoadAssetSuccessCallback onSuccess = delegate (string assetName, object asset, float duration, object userData)
                {
                    if (m_Stopped)
                    {
                        GameEntry.Event.Fire(this, new CameraAnimCancelEventArgs(animName));
                        ChangeState<StateFollowing>(fsm);
                        return;
                    }

                    var clip = asset as AnimationClip;
                    if (owner.CachedAnimation.GetClip(clip.name) == null)
                    {
                        owner.CachedAnimation.AddClip(clip, clip.name);
                    }

                    fsm.SetData(FsmDataKey_AnimClipName, (VarString)clip.name);
                    ChangeState<StatePlayingAnimation>(fsm);
                };

                GameFramework.Resource.LoadAssetFailureCallback onFailure = delegate (string assetName, GameFramework.Resource.LoadResourceStatus status, string errorMessage, object userData)
                {
                    Log.Warning("Camera animation '{0}' not found with error message '{1}'.", assetName, errorMessage);
                    GameEntry.Event.Fire(this, new CameraAnimLoadFailureEventArgs(animName));
                    ChangeState<StateFollowing>(fsm);
                };

                GameEntry.Resource.LoadAsset(AssetUtility.GetCameraAnimationAsset(animName), new LoadAssetCallbacks(onSuccess, onFailure));
            }
        }

        private class StatePlayingAnimation : StateBase
        {
            public override StateId StateId
            {
                get
                {
                    return StateId.PlayingAnimation;
                }
            }

            private float m_AboutToStart = 0f;
            private float m_AboutToStop = 0f;
            private bool m_HasStartedPlaying = false;
            private bool m_WillStop = false;
            private string m_ClipName;
            private string m_AnimName;

            public override bool StartAnimation(IFsm<CameraController> fsm, string animName, float aboutToStart, float aboutToStop)
            {
                Log.Info("Trying to start animaion in state '{0}'. Ignore.", StateId.ToString());
                return false;
            }

            public override void StopAnimation(IFsm<CameraController> fsm)
            {
                if (m_WillStop)
                {
                    return;
                }

                DoWillStop();
            }

            protected override void OnEnter(IFsm<CameraController> fsm)
            {
                base.OnEnter(fsm);
                m_HasStartedPlaying = false;
                m_WillStop = false;
                m_AboutToStart = fsm.GetData<VarFloat>(FsmDataKey_AboutToStart);
                m_AboutToStop = fsm.GetData<VarFloat>(FsmDataKey_AboutToStop);
                m_AnimName = fsm.GetData<VarString>(FsmDataKey_AnimAssetName);
                m_ClipName = fsm.GetData<VarString>(FsmDataKey_AnimClipName);
                GameEntry.Event.Fire(this, new CameraAnimAboutToStartEventArgs(m_AnimName));
            }

            protected override void OnLeave(IFsm<CameraController> fsm, bool isShutdown)
            {
                var animState = fsm.Owner.CachedAnimation[m_ClipName];

                Log.Info("Recycle camera animation '{0}'.", animState.clip.name);
                GameEntry.Resource.Recycle(animState.clip);

                if (!isShutdown)
                {
                    var animName = fsm.GetData<VarString>(FsmDataKey_AnimAssetName).Value;
                    GameEntry.Event.FireNow(this, new CameraAnimStoppedEventArgs(animName));
                }

                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<CameraController> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (!m_HasStartedPlaying)
                {
                    UpdateAboutToStart(fsm, elapseSeconds);
                    return;
                }

                if (!fsm.Owner.CachedAnimation.isPlaying)
                {
                    ChangeState<StateFollowing>(fsm);
                }
                else
                {
                    UpdateWillStop(fsm, elapseSeconds);
                }
            }

            private void UpdateWillStop(IFsm<CameraController> fsm, float elapseSeconds)
            {
                if (m_WillStop)
                {
                    if (m_AboutToStop > 0f)
                    {
                        m_AboutToStop -= elapseSeconds;
                    }
                    else
                    {
                        fsm.Owner.CachedAnimation.Stop();
                    }

                    return;
                }

                var animState = fsm.Owner.CachedAnimation[m_ClipName];
                var clip = animState.clip;
                if (clip.wrapMode != WrapMode.Once && clip.wrapMode != WrapMode.Default)
                {
                    return;
                }

                if (animState.time < clip.length - m_AboutToStop)
                {
                    DoWillStop();
                }
            }

            private void UpdateAboutToStart(IFsm<CameraController> fsm, float elapseSeconds)
            {
                if (m_AboutToStart > 0f)
                {
                    m_AboutToStart -= elapseSeconds;
                    return;
                }

                m_HasStartedPlaying = true;
                GameEntry.Event.FireNow(this, new CameraAnimStartToPlayEventArgs(m_AnimName));
                fsm.Owner.CachedAnimation.Play(m_ClipName);
            }

            private void DoWillStop()
            {
                m_WillStop = true;
                GameEntry.Event.Fire(this, new CameraAnimAboutToStopEventArgs(m_AnimName));
            }
        }
    }
}
