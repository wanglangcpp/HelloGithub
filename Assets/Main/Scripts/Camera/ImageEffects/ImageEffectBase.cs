using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    [RequireComponent(typeof(Camera))]
    public abstract class ImageEffectBase : MonoBehaviour
    {
        protected abstract string ShaderDisplayName
        {
            get;
        }

        private Shader m_Shader = null;

        private Material m_Material = null;

        protected bool m_IsSupported = false;

        protected virtual void Awake()
        {
            m_Shader = Shader.Find(ShaderDisplayName);

            if (m_Shader != null)
            {
                m_Material = new Material(m_Shader);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
            }
        }

        protected virtual void Start()
        {
            if (!SystemInfo.supportsImageEffects)
            {
                enabled = false;
                Log.Warning("Image effects are not supported.");
                return;
            }

            if (!m_Shader)
            {
                enabled = false;
                Log.Warning("Failed to create shader: {0}", ShaderDisplayName);
                return;
            }

            if (!m_Shader.isSupported)
            {
                enabled = false;
                Log.Warning("This shader is not supported: {0}", ShaderDisplayName);
                return;
            }

            m_IsSupported = true;
        }

        protected Material Material
        {
            get
            {
                return m_Material;
            }
        }

        protected virtual void OnDestroy()
        {
            if (null != m_Material)
                DestroyImmediate(m_Material);

            m_Material = null;
            m_Shader = null;
        }
    }
}
