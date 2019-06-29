using System;
using System.Globalization;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 对 <see cref="UILabel"/> 显示的数字进行变化的类。
    /// </summary>
    [RequireComponent(typeof(UILabel))]
    public class TweenNumber : UITweener
    {
        private UILabel m_CachedLabel = null;
        private int? m_CachedValue = null;
        private string m_FormatKey = string.Empty;
        private int m_From = 0;
        private int m_To = 0;

        public int Value
        {
            get
            {
                if (m_CachedValue == null)
                {
                    try
                    {
                        m_CachedValue = int.Parse(m_CachedLabel.text, NumberStyles.Integer | NumberStyles.AllowThousands);
                    }
                    catch (FormatException)
                    {
                        m_CachedValue = 0;
                    }
                }

                return m_CachedValue.Value;
            }

            private set
            {
                m_CachedValue = value;
                m_CachedLabel.text = MakeString(m_CachedValue.Value);
            }
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            Value = Mathf.RoundToInt(Mathf.Lerp(m_From, m_To, factor));
        }

        private string MakeString(int value)
        {
            if (string.IsNullOrEmpty(m_FormatKey))
            {
                return value.ToString();
            }

            return GameEntry.Localization.GetString(m_FormatKey, value);
        }

        private void Awake()
        {
            m_CachedLabel = GetComponent<UILabel>();
        }

        private void OnDisable()
        {
            m_CachedValue = null;
        }

        public static void Begin(GameObject go, float duration, int targetValue, AnimationCurve curve = null, string formatKey = "UI_TEXT_INTEGER")
        {
            TweenNumber comp = Begin<TweenNumber>(go, duration);
            comp.m_CachedValue = null;
            comp.m_From = comp.Value;
            comp.m_To = targetValue;
            comp.m_FormatKey = formatKey;

            if (duration <= 0f)
            {
                comp.Sample(1f, true);
                comp.enabled = false;
            }
            else
            {
                if (curve != null)
                {
                    comp.animationCurve = curve;
                }
            }
        }
    }
}
