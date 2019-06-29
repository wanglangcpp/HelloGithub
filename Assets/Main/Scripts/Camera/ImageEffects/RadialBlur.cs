using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class RadialBlur : ImageEffectBase
    {
        protected override string ShaderDisplayName
        {
            get
            {
                return "Hidden/RadialBlur";
            }
        }

        private int m_BlurStrengthId = 0;
        private float m_BlurStrength = 0;

        private float m_BluringSpeed = 7f;

        public void FadeInRadialBlur()
        {
            if (!m_IsSupported)
            {
                Log.Warning("This image effect is not supported.");
                return;
            }

            m_BlurStrength = 0;
            m_BluringSpeed = 8f;
            enabled = true;
        }

        public void FadeOutRadialBlur()
        {
            m_BluringSpeed = -3f;
        }

        protected override void Awake()
        {
            base.Awake();

            m_BlurStrengthId = Shader.PropertyToID("_BlurStrength");
        }

        protected override void Start()
        {
            if (!SystemInfo.supportsRenderTextures)
            {
                enabled = false;
                Log.Warning("Render textures are not supported.");
                return;
            }

            base.Start();

            enabled = false;
        }

        private void FixedUpdate()
        {
            //m_BlurStrength += Time.fixedDeltaTime * m_BluringSpeed;

            //if (m_BlurStrength < 0.0f)
            //{
            //    enabled = false;
            //}

            //m_BlurStrength = Mathf.Clamp01(m_BlurStrength);
        }

        private Rect rect = new Rect(0, 0, 800, 800);

        private void OnGUI()
        {
            GUI.Label(rect, m_BlurStrength.ToString());
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            m_BlurStrength += Time.deltaTime * m_BluringSpeed;

            if (m_BlurStrength < 0.0f)
            {
                enabled = false;
            }

            m_BlurStrength = Mathf.Clamp01(m_BlurStrength);

            Material.SetFloat(m_BlurStrengthId, m_BlurStrength);
            Graphics.Blit(source, destination, Material);
        }
    }
}
