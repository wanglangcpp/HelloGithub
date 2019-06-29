using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 摄像机逐帧渲染控制器。
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class PerCameraRenderController : MonoBehaviour
    {
        [SerializeField]
        private bool m_EnableFog = false;

        [SerializeField]
        private float m_AmbientIntensity = 0f;

        private bool m_CachedFogState;
        private float m_CachedAmbientLightIntensity;

        #region MonoBehaviour

        private void OnPreRender()
        {
            m_CachedFogState = RenderSettings.fog;
            RenderSettings.fog = m_EnableFog;

            m_CachedAmbientLightIntensity = RenderSettings.ambientIntensity;
            RenderSettings.ambientIntensity = m_AmbientIntensity;
        }

        private void OnPostRender()
        {
            RenderSettings.fog = m_CachedFogState;
            RenderSettings.ambientIntensity = m_CachedAmbientLightIntensity;
        }

        #endregion MonoBehaviour
    }
}
