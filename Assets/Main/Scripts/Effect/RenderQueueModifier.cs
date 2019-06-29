using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 渲染顺序修改器。
    /// </summary>
    public class RenderQueueModifier : MonoBehaviour
    {
        [SerializeField, Tooltip("Renderers whose queues are to be modified.")]
        private Renderer[] m_Renderers = null;

        [SerializeField, Tooltip("The delta value of the render queues.")]
        private int m_RenderQueueDelta = 0;

        private void OnEnable()
        {
            IncreaseRenderQueue(m_RenderQueueDelta);
        }

        private void OnDisable()
        {
            IncreaseRenderQueue(-m_RenderQueueDelta);
        }

        private void IncreaseRenderQueue(int delta)
        {
            for (int i = 0; i < m_Renderers.Length; ++i)
            {
                var renderer = m_Renderers[i];
                var materials = renderer.materials;
                for (int j = 0; j < materials.Length; ++j)
                {
                    materials[j].renderQueue += delta;
                }
            }
        }
    }
}
