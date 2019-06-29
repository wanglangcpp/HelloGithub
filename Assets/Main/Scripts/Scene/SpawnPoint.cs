//------------------------------------------------------------
// Game Framework v2.x
// Copyright © 2014-2016 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 场景出生点。
    /// </summary>
    public sealed class SpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private Color m_Color = Color.red;

        /// <summary>
        /// 获取或设置场景出生点的颜色。
        /// </summary>
        public Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                m_Color = value;
            }
        }

        private void OnDrawGizmos()
        {
            Color cachedColor = Gizmos.color;
            Gizmos.color = new Color(m_Color.r, m_Color.g, m_Color.b, 0.3f);
            Gizmos.DrawSphere(transform.position, 0.3f);
            Gizmos.color = m_Color;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
            Gizmos.color = cachedColor;
        }
    }
}
