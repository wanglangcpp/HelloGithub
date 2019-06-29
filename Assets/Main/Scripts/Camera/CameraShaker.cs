using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class CameraShaker : MonoBehaviour
    {
        private Transform m_CachedTransform;

        private GameFrameworkFunc<bool> m_ShouldSkipShakingDelegate;

        /// <summary>
        /// 设置屏蔽震动的委托。
        /// </summary>
        /// <param name="shouldSkipShakingDelegate">屏蔽震动的委托。</param>
        public void SetShouldSkipShakingDelegate(GameFrameworkFunc<bool> shouldSkipShakingDelegate)
        {
            m_ShouldSkipShakingDelegate = shouldSkipShakingDelegate;
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_CachedTransform = transform;
        }

        private void OnDestroy()
        {
            m_CachedTransform = null;
        }

        private void Update()
        {
            if (!GameEntry.IsAvailable) return;

            if (m_ShouldSkipShakingDelegate != null && m_ShouldSkipShakingDelegate())
            {
                m_CachedTransform.localPosition = Vector3.zero;
            }
            else
            {
                //m_CachedTransform.localPosition = GameEntry.CameraShaking.CurrentOffset;
                m_CachedTransform.localPosition = new Vector3(GameEntry.CameraShaking.CurrentOffset.x, GameEntry.CameraShaking.CurrentOffset.y, 0);
            }
        }

        #endregion MonoBehaviour
    }
}
