using GameFramework;
using UnityEngine;

// This class implements simple ghosting type Motion Blur.
// If Extra Blur is selected, the scene will allways be a little blurred,
// as it is scaled to a smaller resolution.
// The effect works by accumulating the previous frames in an accumulation
// texture.
namespace Genesis.GameClient
{
    [RequireComponent(typeof(Camera))]
    public class MotionBlur : ImageEffectBase
    {
        protected override string ShaderDisplayName
        {
            get
            {
                return "Hidden/MotionBlur";
            }
        }

        [SerializeField, Range(0.0f, 0.92f)]
        private float m_BlurAmount = 0.8f;

        [SerializeField]
        private bool m_ExtraBlur = false;

        private RenderTexture m_AccumTexture;

        private int m_MainTexId = 0;
        private int m_AccumOrigId = 0;

        public void EnableMotionBlur()
        {
            if (!m_IsSupported)
            {
                Log.Warning("This image effect is not supported.");
                return;
            }

            enabled = true;
        }

        public void DisableMotionBlur()
        {
            enabled = false;
        }

        protected override void Awake()
        {
            base.Awake();

            m_MainTexId = Shader.PropertyToID("_MainTex");
            m_AccumOrigId = Shader.PropertyToID("_AccumOrig");
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DestroyImmediate(m_AccumTexture);
            m_AccumTexture = null;
        }

        // Called by camera to apply image effect
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // Create the accumulation texture
            if (m_AccumTexture == null || m_AccumTexture.width != source.width || m_AccumTexture.height != source.height)
            {
                DestroyImmediate(m_AccumTexture);
                m_AccumTexture = new RenderTexture(source.width, source.height, 0);
                m_AccumTexture.hideFlags = HideFlags.HideAndDontSave;
                Graphics.Blit(source, m_AccumTexture);
            }

            // If Extra Blur is selected, downscale the texture to 4x4 smaller resolution.
            if (m_ExtraBlur)
            {
                RenderTexture blurbuffer = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
                m_AccumTexture.MarkRestoreExpected();
                Graphics.Blit(m_AccumTexture, blurbuffer);
                Graphics.Blit(blurbuffer, m_AccumTexture);
                RenderTexture.ReleaseTemporary(blurbuffer);
            }

            // Clamp the motion blur variable, so it can never leave permanent trails in the image
            m_BlurAmount = Mathf.Clamp(m_BlurAmount, 0.0f, 0.92f);

            // Setup the texture and floating point values in the shader
            Material.SetTexture(m_MainTexId, m_AccumTexture);
            Material.SetFloat(m_AccumOrigId, 1.0F - m_BlurAmount);

            // We are accumulating motion over frames without clear/discard
            // by design, so silence any performance warnings from Unity
            m_AccumTexture.MarkRestoreExpected();

            // Render the image using the motion blur shader
            Graphics.Blit(source, m_AccumTexture, Material);
            Graphics.Blit(m_AccumTexture, destination);
        }
    }
}
