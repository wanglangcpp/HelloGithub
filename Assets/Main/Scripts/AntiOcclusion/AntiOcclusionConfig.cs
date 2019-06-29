using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 主角防遮挡配置数据。
    /// </summary>
    public class AntiOcclusionConfig : ScriptableObject
    {
        [SerializeField]
        private float m_BeginAlpha = 1f;

        /// <summary>
        /// 起始不透明度。
        /// </summary>
        public float BeginAlpha
        {
            get
            {
                return m_BeginAlpha;
            }
        }

        [SerializeField]
        private float m_TargetAlpha = .3f;

        /// <summary>
        /// 目标不透明度。
        /// </summary>
        public float TargetAlpha
        {
            get
            {
                return m_TargetAlpha;
            }
        }

        [SerializeField]
        private float m_TransitionDuration = .3f;

        /// <summary>
        /// 过渡时长。
        /// </summary>
        public float TransitionDuration
        {
            get
            {
                return m_TransitionDuration;
            }
        }

        /// <summary>
        /// 配置数据。
        /// </summary>
        [Serializable]
        public class Config
        {
            [SerializeField]
            private Shader m_OriginalShader = null;

            [SerializeField]
            private Shader m_ReplacingShader = null;

            [SerializeField]
            private int m_RenderQueueDelta = 0;

            [SerializeField]
            private string m_ColorTintPropertyName = "_TintColor";

            /// <summary>
            /// 原始着色器。
            /// </summary>
            public Shader OriginalShader
            {
                get
                {
                    return m_OriginalShader;
                }
            }

            /// <summary>
            /// 替换着色器。
            /// </summary>
            public Shader ReplacingShader
            {
                get
                {
                    return m_ReplacingShader;
                }
            }

            /// <summary>
            /// 渲染队列增量。
            /// </summary>
            public int RenderQueueDelta
            {
                get
                {
                    return m_RenderQueueDelta;
                }
            }

            /// <summary>
            /// 调整着色器透明度使用的颜色属性名称。
            /// </summary>
            public string ColorTintPropertyName
            {
                get
                {
                    return m_ColorTintPropertyName;
                }
            }
        }

        [SerializeField]
        private Config[] m_Configs = null;

        /// <summary>
        /// 获取配置数据列表。
        /// </summary>
        /// <returns></returns>
        public IList<Config> GetConfigs()
        {
            return m_Configs;
        }
    }
}
