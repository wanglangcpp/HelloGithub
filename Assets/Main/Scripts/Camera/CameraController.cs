using GameFramework;
using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 摄像机控制器。
    /// </summary>
    public partial class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_CameraOffset = new Vector3(4.06f, 6.8f, -11f);

        [SerializeField]
        private float m_CenterOffset = 0.4f;

        [SerializeField]
        private float m_SpeedLimit = 15f;

        [SerializeField]
        private float m_ChaseUpTime = 0.2f;

        [SerializeField]
        private float m_LazyThDistance = 0.02f;

        private Animation m_CachedAnimation = null;

        private Animation CachedAnimation
        {
            get
            {
                if (m_CachedAnimation == null)
                {
                    m_CachedAnimation = gameObject.GetComponent<Animation>();
                }

                if (m_CachedAnimation == null)
                {
                    m_CachedAnimation = gameObject.AddComponent<Animation>();
                }

                return m_CachedAnimation;
            }
        }

        /// <summary>
        /// 被追踪者的 <see cref="UnityEngine.Transform"/>。
        /// </summary>
        private Transform m_TargetTransform = null;

        /// <summary>
        /// 自身的 <see cref="UnityEngine.Transform"/>。
        /// </summary>
        private Transform m_CachedTransform = null;

        /// <summary>
        /// 被追踪者上一帧位置。
        /// </summary>
        private Vector3 m_TargetLastPosition = Vector3.zero;

        /// <summary>
        /// 摄像机目标位置。
        /// </summary>
        private Vector3 m_AimPosition = Vector3.zero;

        /// <summary>
        /// 位置偏移目标值。
        /// </summary>
        private Vector3 m_AimCameraOffset;

        /// <summary>
        /// 朝向偏移目标值。
        /// </summary>
        private float m_AimCenterOffset;

        /// <summary>
        /// 位置偏移初始值。
        /// </summary>
        private Vector3 m_OriginalCameraOffset;

        /// <summary>
        /// 朝向偏移初始值。
        /// </summary>
        private float m_OriginalCenterOffset;

        /// <summary>
        /// 位置缓存值。
        /// </summary>
        private Vector3 m_LastCameraPos = Vector3.zero;

        /// <summary>
        /// 帧时间缓存值。
        /// </summary>
        private float m_LastDeltaTime = 0;

        /// <summary>
        /// 是否应该更新遮挡射线。
        /// </summary>
        private bool m_ShouldUpdateOcclusionRaycast = true;

        /// <summary>
        /// 缓存的主角防遮挡控制器。
        /// </summary>
        private List<AntiOcclusionController> m_CachedAntiOcclusionControllers = new List<AntiOcclusionController>();

        /// <summary>
        /// 参数改变所需时间。
        /// </summary>
        private float m_ParamChangingDuration = 0f;

        public bool IsAutoFighting { get; set; }

        public int PortageFromId { get; set; }

        public int PortageToId { get; set; }

        /// <summary>
        /// 是否正在修改参数。
        /// </summary>
        public bool IsChangingParams
        {
            get
            {
                return m_ParamChangingDuration > 0f;
            }
        }

        /// <summary>
        /// 设置和获取是否更新遮挡射线。
        /// </summary>
        public bool ShouldUpdateOcclusionRaycast
        {
            get
            {
                return m_ShouldUpdateOcclusionRaycast;
            }
            set
            {
                if (m_ShouldUpdateOcclusionRaycast && !value)
                {
                    for (int i = 0; i < m_CachedAntiOcclusionControllers.Count; ++i)
                    {
                        m_CachedAntiOcclusionControllers[i].StartRecover();
                    }

                    m_CachedAntiOcclusionControllers.Clear();
                }

                m_ShouldUpdateOcclusionRaycast = value;
            }
        }

        /// <summary>
        /// 工厂方法。
        /// </summary>
        /// <param name="cameraTransform">有摄像机组件的 <see cref="UnityEngine.Transform"/> 对象。</param>
        /// <returns>摄像机控制器。</returns>
        public static CameraController Create(Transform cameraTransform)
        {
            if (cameraTransform == null || cameraTransform.GetComponent<Camera>() == null)
            {
                Log.Error("cameraTransform is invalid.");
            }

            GameObject go = new GameObject("Camera Controller");
            var ret = go.AddComponent<CameraController>();
            ret.Init(cameraTransform);
            return ret;
        }

        /// <summary>
        /// 设置跟随目标。
        /// </summary>
        /// <param name="targetTransform">跟随目标。</param>
        public void SetTarget(Transform targetTransform)
        {
            (m_Fsm.CurrentState as StateBase).SetTarget(m_Fsm, targetTransform);
        }

        /// <summary>
        /// 重置跟随目标。
        /// </summary>
        public void ResetTarget()
        {
            (m_Fsm.CurrentState as StateBase).ResetTarget(m_Fsm);
        }

        /// <summary>
        /// 修改参数。
        /// </summary>
        /// <param name="cameraOffset">位置偏移值。</param>
        /// <param name="centerOffset">看向偏移值。</param>
        /// <param name="duration">渐变时间。</param>
        public void ChangeParams(Vector3? cameraOffset, float? centerOffset, float duration)
        {
            (m_Fsm.CurrentState as StateBase).ChangeParams(m_Fsm, cameraOffset, centerOffset, duration);
        }

        /// <summary>
        /// 重置参数。
        /// </summary>
        /// <param name="duration">渐变时间。</param>
        public void ResetParams(float duration)
        {
            ChangeParams(m_OriginalCameraOffset, m_OriginalCenterOffset, duration);
        }

        /// <summary>
        /// 开始播放动画。
        /// </summary>
        /// <param name="animName">动画名。</param>
        /// <param name="aboutToStart">抛出即将开始事件的提前量。</param>
        /// <param name="aboutToStop">抛出即将结束事件的提前量。</param>
        /// <returns>是否可以播放。</returns>
        public bool StartAnimation(string animName, float aboutToStart, float aboutToStop)
        {
            return (m_Fsm.CurrentState as StateBase).StartAnimation(m_Fsm, animName, aboutToStart, aboutToStop);
        }

        /// <summary>
        /// 开始传送。
        /// </summary>
        /// <param name="protageFromId">入口传送门编号。</param>
        /// <param name="portageToId">出口传送门编号。</param>
        public void StartPortage(int protageFromId, int portageToId)
        {
            m_TargetLastPosition = m_TargetTransform.position; // 缓存当前位置，用于释放特效。
            (m_Fsm.CurrentState as StateBase).StartPortage(m_Fsm, protageFromId, portageToId);
        }

        /// <summary>
        /// 停止播放动画。
        /// </summary>
        public void StopAnimation()
        {
            (m_Fsm.CurrentState as StateBase).StopAnimation(m_Fsm);
        }

        /// <summary>
        /// 立刻跟随。
        /// </summary>
        public void FollowImmediately()
        {
            (m_Fsm.CurrentState as StateBase).FollowImmediately(m_Fsm);
        }

        private void UpdateCameraParams()
        {
            if (!IsChangingParams)
            {
                return;
            }

            var deltaTime = Time.deltaTime;
            var proportion = deltaTime / m_ParamChangingDuration;
            m_CameraOffset += (m_AimCameraOffset - m_CameraOffset) * proportion;
            m_CenterOffset += (m_AimCenterOffset - m_CenterOffset) * proportion;
            m_ParamChangingDuration -= deltaTime;

            if (m_ParamChangingDuration <= 0f)
            {
                m_ParamChangingDuration = 0f;
                m_CameraOffset = m_AimCameraOffset;
                m_CenterOffset = m_AimCenterOffset;
                m_AimCameraOffset = Vector3.zero;
                m_AimCenterOffset = 0f;
                FollowImmediately_Internal();
            }
            else
            {
                m_AimPosition = m_TargetTransform.position + m_AimCameraOffset;
                m_CachedTransform.position += (m_AimPosition - m_CachedTransform.position) * proportion;

                var targetLookAtPoint = m_TargetTransform.position + Vector3.up * m_AimCenterOffset;
                var targetRotation = Quaternion.LookRotation(targetLookAtPoint - m_CachedTransform.position);
                m_CachedTransform.rotation = Quaternion.Lerp(m_CachedTransform.rotation, targetRotation, proportion);
            }
        }

        private void FollowLazily()
        {
            if (IsChangingParams)
            {
                return;
            }

            Vector3 velocity = Vector3.zero;
            if (m_LastDeltaTime > 0f)
            {
                velocity = (m_CachedTransform.position - m_LastCameraPos) / m_LastDeltaTime;
            }

            m_AimPosition = m_TargetTransform.position + m_CameraOffset;

            if (Vector3.Distance(m_AimPosition, m_CachedTransform.position) < m_LazyThDistance)
            {
                m_CachedTransform.position = m_AimPosition;
                return;
            }

            m_CachedTransform.position = Vector3.SmoothDamp(m_CachedTransform.position, m_AimPosition, ref velocity, m_ChaseUpTime, m_SpeedLimit);
        }

        private void FollowImmediately_Internal()
        {
            if (m_TargetTransform != null)
            {
                m_CachedTransform.position = m_TargetTransform.position + m_CameraOffset;
                UpdateLookAt();
            }

            m_LastCameraPos = m_CachedTransform.position;
        }

        private void Init(Transform cameraTransform)
        {
            m_CachedTransform = gameObject.transform;
            m_CachedTransform.parent = cameraTransform.parent;
            m_CachedTransform.localPosition = cameraTransform.localPosition;
            m_CachedTransform.localRotation = cameraTransform.localRotation;
            cameraTransform.parent = m_CachedTransform;
            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localRotation = Quaternion.identity;
            var shaker = cameraTransform.gameObject.AddComponent<CameraShaker>();
            shaker.SetShouldSkipShakingDelegate(ShouldPreventShakerFromShaking);
            cameraTransform.gameObject.AddComponent<CameraPostEffectController>();

            var controllerData = cameraTransform.gameObject.GetComponent<CameraControllerConfig>();
            if (controllerData != null)
            {
                m_CameraOffset = controllerData.m_CameraOffset;
                m_CenterOffset = controllerData.m_CenterOffset;
                m_SpeedLimit = controllerData.m_SpeedLimit;
                m_ChaseUpTime = controllerData.m_ChaseUpTime;
                m_LazyThDistance = controllerData.m_LazyThDistance;

            }
        }

        private bool ShouldPreventShakerFromShaking()
        {
            return CurrentStateId != StateId.Following;
        }

        private void UpdateLookAt()
        {
            if (m_TargetTransform)
            {
                m_CachedTransform.LookAt(m_TargetTransform.position + Vector3.up * m_CenterOffset);
            }
        }

        private void UpdateOcclusionRaycast()
        {
            var delta = m_TargetLastPosition - m_CachedTransform.position;
            var hits = Physics.RaycastAll(m_CachedTransform.position, delta.normalized, delta.magnitude, 1 << Constant.Layer.OcclusionColliderLayerId);

            var hitCaches = new bool[m_CachedAntiOcclusionControllers.Count];

            for (int i = 0; i < hits.Length; ++i)
            {
                var hit = hits[i];
                var antiOcclusionCtl = hit.collider.GetComponent<AntiOcclusionController>();

                if (antiOcclusionCtl == null)
                {
                    continue;
                }

                bool found = false;
                for (int j = 0; j < m_CachedAntiOcclusionControllers.Count; ++j)
                {
                    if (m_CachedAntiOcclusionControllers[j] == antiOcclusionCtl)
                    {
                        hitCaches[j] = true;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    antiOcclusionCtl.StartAntiOcclusion();
                    m_CachedAntiOcclusionControllers.Add(antiOcclusionCtl);
                }
            }

            for (int i = 0, j = 0; i < hitCaches.Length; ++i)
            {
                if (!hitCaches[i])
                {
                    var antiOcclusionCtl = m_CachedAntiOcclusionControllers[j];
                    if (antiOcclusionCtl != null)
                    {
                        antiOcclusionCtl.StartRecover();
                    }

                    m_CachedAntiOcclusionControllers.RemoveAt(j);
                }
                else
                {
                    ++j;
                }
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_Fsm = GameEntry.Fsm.CreateFsm(GetType().Name, this,
                new StateInit(),
                new StateFollowing(),
                new StateLoadingAnimation(),
                new StatePlayingAnimation(),
                new StatePreparePortage(),
                new StatePortaging());
            m_Fsm.Start<StateInit>();
        }

        private void Start()
        {
            (m_Fsm.CurrentState as StateBase).OnOwnerStart(m_Fsm);
        }

        private void OnDestroy()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_Fsm = null;
        }

        #endregion MonoBehaviour
    }
}
