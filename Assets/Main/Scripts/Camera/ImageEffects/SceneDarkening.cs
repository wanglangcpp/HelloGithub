using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    [RequireComponent(typeof(Camera))]
    public class SceneDarkening : ImageEffectBase
    {
        protected override string ShaderDisplayName
        {
            get
            {
                return "Game/Scene Darkening (Post Effect)";
            }
        }

        private class Data
        {
            internal float Attack = 0f;
            internal float Sustain = 0f;
            internal float Release = 0f;
            internal Color TargetColor = Color.white;

            internal float Duration
            {
                get
                {
                    return Attack + Sustain + Release;
                }
            }
        }

        private Data m_Data = null;

        /// <summary>
        /// 当次效果的持续时间。
        /// </summary>
        private float m_CurrentEffectTime = 0f;

        private int m_MainColorId = 0;

        public void ResetColorChange()
        {
            m_Data = null;
            m_CurrentEffectTime = 0f;
            enabled = false;
        }

        public void StartColorChange(Color targetColor, float attack, float sustain, float release)
        {
            if (!m_IsSupported)
            {
                Log.Warning("This image effect is not supported.");
                return;
            }

            if (attack < 0f || sustain < 0f || release < 0f)
            {
                Log.Warning("Invalid data for post effect color change.");
                return;
            }

            // 如果已经存在一个 m_Data，其中的内容和计时将被覆盖。
            m_Data = new Data { Attack = attack, Release = release, Sustain = sustain, TargetColor = targetColor };
            m_CurrentEffectTime = 0f;
            enabled = true;
        }

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();

            m_MainColorId = Shader.PropertyToID("_MainColor");
            Material.SetColor(m_MainColorId, Color.white);
        }

        protected override void Start()
        {
            base.Start();

            enabled = false;
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            m_CurrentEffectTime += Time.deltaTime;

            if (m_CurrentEffectTime > m_Data.Duration)
            {
                m_CurrentEffectTime = 0f;
                enabled = false;
                return;
            }

            if (m_CurrentEffectTime < m_Data.Attack)
            {
                var proportion = m_Data.Attack < 0.001f ? 1f : m_CurrentEffectTime / m_Data.Attack;
                Material.SetColor(m_MainColorId, proportion * m_Data.TargetColor + (1 - proportion) * Color.white);
            }
            else if (m_CurrentEffectTime < m_Data.Attack + m_Data.Sustain)
            {
                Material.SetColor(m_MainColorId, m_Data.TargetColor);
            }
            else
            {
                var proportion = m_Data.Release < 0.001f ? 1f : (m_Data.Duration - m_CurrentEffectTime) / m_Data.Release;
                Material.SetColor(m_MainColorId, proportion * m_Data.TargetColor + (1 - proportion) * Color.white);
            }

            Graphics.Blit(src, dst, Material);
        }

        #endregion MonoBehaviour
    }
}
