using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ImpactHudTextConfig : ScriptableObject
    {
        [SerializeField]
        private Config[] m_Configs = null;

        public Config this[int index]
        {
            get
            {
                return m_Configs[index];
            }
        }

        public int Length
        {
            get
            {
                return m_Configs.Length;
            }
        }

        [Serializable]
        public class Config
        {
            [SerializeField]
            private UIFont m_Font = null;

            [SerializeField]
            private int m_FontSize = 20;

            [SerializeField]
            private FontStyle m_FontStyle = FontStyle.Normal;

            [SerializeField]
            private bool m_UseGradient = false;

            [SerializeField]
            private Color m_GradientTop = Color.white;

            [SerializeField]
            private Color m_GradienBottom = new Color(0.7f, 0.7f, 0.7f);

            [SerializeField]
            private UILabel.Effect m_Effect = UILabel.Effect.None;

            [SerializeField]
            private Color m_EffectColor = Color.black;

            [SerializeField]
            private AnimationCurve m_OffsetXCurve = new AnimationCurve();

            [SerializeField]
            private AnimationCurve m_OffsetYCurve = new AnimationCurve();

            [SerializeField]
            private AnimationCurve m_ScaleCurve = new AnimationCurve();

            [SerializeField]
            private AnimationCurve m_AlphaCurve = new AnimationCurve();

            public UIFont Font
            {
                get
                {
                    return m_Font;
                }
                set
                {
                    m_Font = value;
                }
            }

            public int FontSize
            {
                get
                {
                    return m_FontSize;
                }
                set
                {
                    m_FontSize = value;
                }
            }

            public FontStyle FontStyle
            {
                get
                {
                    return m_FontStyle;
                }
                set
                {
                    m_FontStyle = value;
                }
            }

            public bool UseGradient
            {
                get
                {
                    return m_UseGradient;
                }
                set
                {
                    m_UseGradient = value;
                }
            }

            public Color GradientTop
            {
                get
                {
                    return m_GradientTop;
                }
                set
                {
                    m_GradientTop = value;
                }
            }

            public Color GradienBottom
            {
                get
                {
                    return m_GradienBottom;
                }
                set
                {
                    m_GradienBottom = value;
                }
            }

            public UILabel.Effect Effect
            {
                get
                {
                    return m_Effect;
                }
                set
                {
                    m_Effect = value;
                }
            }

            public Color EffectColor
            {
                get
                {
                    return m_EffectColor;
                }
                set
                {
                    m_EffectColor = value;
                }
            }

            public AnimationCurve OffsetXCurve
            {
                get
                {
                    return m_OffsetXCurve;
                }
                set
                {
                    m_OffsetXCurve = value;
                }
            }

            public AnimationCurve OffsetYCurve
            {
                get
                {
                    return m_OffsetYCurve;
                }
                set
                {
                    m_OffsetYCurve = value;
                }
            }

            public AnimationCurve ScaleCurve
            {
                get
                {
                    return m_ScaleCurve;
                }
                set
                {
                    m_ScaleCurve = value;
                }
            }

            public AnimationCurve AlphaCurve
            {
                get
                {
                    return m_AlphaCurve;
                }
                set
                {
                    m_AlphaCurve = value;
                }
            }
        }
    }
}
