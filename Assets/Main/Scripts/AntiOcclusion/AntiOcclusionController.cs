using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 主角防遮挡控制器。
    /// </summary>
    /// <remarks>
    /// 根据 <see cref="Genesis.GameClient.AntiOcclusionComponent"/> 中的配置数据，替换着色器及相关内容的类。
    /// </remarks>
    [RequireComponent(typeof(Collider))]
    public class AntiOcclusionController : MonoBehaviour
    {
        private class MaterialCache
        {
            internal int Index;
            internal string ColorPropertyName;
            internal Material OriginalMaterial;
        }

        private enum State
        {
            Default,
            Replacing,
            Recovering,
        }

        [SerializeField]
        private Renderer[] m_TargetRenderers = null;

        private List<MaterialCache>[] m_MaterialCacheLists = null;

        private float m_BeginAlpha = 1f;
        private float m_TargetAlpha = 0f;
        private float m_CurrentAlpha = 0f;
        private float m_TransitionDuration = 0f;
        private State m_State = State.Default;

        /// <summary>
        /// 开始防遮挡动作。
        /// </summary>
        public void StartAntiOcclusion()
        {
            if (!enabled || !GameEntry.IsAvailable)
            {
                return;
            }

            if (m_State == State.Replacing)
            {
                return;
            }

            if (m_State == State.Recovering)
            {
                m_State = State.Replacing;
                return;
            }

            m_State = State.Replacing;

            if (m_MaterialCacheLists == null)
            {
                m_MaterialCacheLists = new List<MaterialCache>[m_TargetRenderers.Length];
            }

            for (int i = 0; i < m_TargetRenderers.Length; ++i)
            {
                var renderer = m_TargetRenderers[i];
                List<MaterialCache> cacheList = m_MaterialCacheLists[i];

                var materials = renderer.materials;
                bool materialsModified = false;
                for (int j = 0; j < materials.Length; ++j)
                {
                    var material = materials[j];
                    var config = GameEntry.AntiOcclusion.GetConfig(material.shader);
                    if (config == null)
                    {
                        continue;
                    }

                    if (cacheList == null)
                    {
                        cacheList = new List<MaterialCache>();
                        m_MaterialCacheLists[i] = cacheList;
                    }

                    cacheList.Add(new MaterialCache { Index = j, OriginalMaterial = material, ColorPropertyName = config.ColorTintPropertyName });
                    var newMat = new Material(material);

                    if (config.ReplacingShader != null)
                    {
                        newMat.shader = config.ReplacingShader;
                    }

                    newMat.renderQueue += config.RenderQueueDelta;
                    SetAlpha(newMat, config.ColorTintPropertyName, m_BeginAlpha);
                    materials[j] = newMat;
                    materialsModified = true;
                    m_CurrentAlpha = m_BeginAlpha;
                }

                if (materialsModified)
                {
                    renderer.materials = materials;
                }
            }
        }

        /// <summary>
        /// 开始复原动作。
        /// </summary>
        public void StartRecover()
        {
            if (!enabled)
            {
                return;
            }

            if (m_MaterialCacheLists == null || m_MaterialCacheLists.Length <= 0)
            {
                return;
            }

            m_State = State.Recovering;
        }

        #region MonoBehaviour

        private void Awake()
        {
            var component = GameEntry.AntiOcclusion;
            m_BeginAlpha = component.BeginAlpha;
            m_TargetAlpha = component.TargetAlpha;
            m_TransitionDuration = component.TransitionDuration;
        }

        private void OnEnable()
        {
            Reset();
        }

        private void OnDisable()
        {
            Reset();
        }

        private void Update()
        {
            switch (m_State)
            {
                case State.Recovering:
                    UpdateRecovering();
                    break;
                case State.Replacing:
                    UpdateReplacing();
                    break;
                default:
                    break;
            }
        }

        #endregion MonoBehaviour

        private void Reset()
        {
            m_State = State.Default;

            if (m_MaterialCacheLists == null)
            {
                return;
            }

            for (int i = 0; i < m_MaterialCacheLists.Length; ++i)
            {
                var renderer = m_TargetRenderers[i];
                var caches = m_MaterialCacheLists[i];

                if (caches == null)
                {
                    continue;
                }

                var materials = renderer.materials;
                bool materialsModified = false;

                for (int j = 0; j < caches.Count; ++j)
                {
                    var cache = caches[j];
                    materials[cache.Index] = cache.OriginalMaterial;
                    materialsModified = true;
                }

                if (materialsModified)
                {
                    renderer.materials = materials;
                }
            }
        }

        private void UpdateReplacing()
        {
            bool stop = false;
            if (m_TransitionDuration <= 0f)
            {
                stop = true;
                m_CurrentAlpha = m_BeginAlpha;
            }
            else
            {
                m_CurrentAlpha -= Time.deltaTime / m_TransitionDuration * (m_BeginAlpha - m_TargetAlpha);
                if (m_CurrentAlpha <= m_TargetAlpha)
                {
                    stop = true;
                    m_CurrentAlpha = m_TargetAlpha;
                }
            }

            UpdateAllAlphas();

            if (stop)
            {
                m_State = State.Default;
            }
        }

        private void UpdateRecovering()
        {
            bool stop = false;
            if (m_TransitionDuration <= 0f)
            {
                stop = true;
                m_CurrentAlpha = m_BeginAlpha;
            }
            else
            {
                m_CurrentAlpha += Time.deltaTime / m_TransitionDuration * (m_BeginAlpha - m_TargetAlpha);
                if (m_CurrentAlpha >= m_BeginAlpha)
                {
                    stop = true;
                    m_CurrentAlpha = m_TargetAlpha;
                }
            }

            UpdateAllAlphas();

            if (stop)
            {
                Reset();
            }
        }

        private void UpdateAllAlphas()
        {
            if (m_MaterialCacheLists == null)
            {
                return;
            }

            for (int i = 0; i < m_MaterialCacheLists.Length; ++i)
            {
                var renderer = m_TargetRenderers[i];
                var caches = m_MaterialCacheLists[i];

                if (caches == null)
                {
                    continue;
                }

                for (int j = 0; j < caches.Count; ++j)
                {
                    var cache = caches[j];
                    SetAlpha(renderer.materials[cache.Index], cache.ColorPropertyName, m_CurrentAlpha);
                }
            }
        }

        private static void SetAlpha(Material m, string colorPropertyName, float alpha)
        {
            var color = m.GetColor(colorPropertyName);
            color.a = alpha;
            m.SetColor(colorPropertyName, color);
        }
    }
}
