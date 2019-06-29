using GameFramework;
using GameFramework.Event;
using GameFramework.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ImpactComponent : MonoBehaviour
    {
        [Serializable]
        private partial class HudTexts
        {
            [SerializeField]
            private Transform m_HudTextInstanceRoot = null;

            [SerializeField]
            private string m_TemplateName = null;

            [SerializeField]
            private ImpactHudTextConfig m_Config = null;

            [SerializeField]
            private int m_InstancePoolCapacity = 16;

            private HudText m_Template = null;
            private IObjectPool<HudTextObject> m_HudTextObjects;
            private IList<HudText> m_ActiveHudTexts = new List<HudText>();

            public bool PreloadComplete
            {
                get
                {
                    return m_Template != null;
                }
            }

            public void Init()
            {
                GameEntry.Event.Subscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);

                if (m_HudTextInstanceRoot == null)
                {
                    Log.Error("You must set hud text instance root first.");
                    return;
                }

                m_HudTextObjects = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<HudTextObject>("HudText", m_InstancePoolCapacity);
            }

            public void Shutdown()
            {
                Clear();

                if (!GameEntry.IsAvailable)
                {
                    return;
                }

                GameEntry.Event.Unsubscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);
            }

            public void Preload()
            {
                PreloadUtility.LoadPreloadResource(m_TemplateName);
            }

            public void Update()
            {
                float time = Time.time;
                for (int i = m_ActiveHudTexts.Count - 1; i >= 0; i--)
                {
                    HudText hudText = m_ActiveHudTexts[i];

                    Keyframe[] offsetXs = m_Config[hudText.Type].OffsetXCurve.keys;
                    Keyframe[] offsetYs = m_Config[hudText.Type].OffsetYCurve.keys;
                    Keyframe[] alphas = m_Config[hudText.Type].AlphaCurve.keys;
                    Keyframe[] scales = m_Config[hudText.Type].ScaleCurve.keys;
                    float offsetXEnd = offsetXs[offsetXs.Length - 1].time;
                    float offsetYEnd = offsetYs[offsetYs.Length - 1].time;
                    float alphaEnd = alphas[alphas.Length - 1].time;
                    float scaleEnd = scales[scales.Length - 1].time;
                    float totalEnd = Mathf.Max(offsetXEnd, offsetYEnd, alphaEnd, scaleEnd);

                    float currentTime = time - hudText.StartTime;
                    hudText.TextLabel.gameObject.SetActive(true);
                    hudText.TextLabel.cachedTransform.localPosition = new Vector3(m_Config[hudText.Type].OffsetXCurve.Evaluate(currentTime), m_Config[hudText.Type].OffsetYCurve.Evaluate(currentTime), 0f);
                    Vector3 uiPoint;
                    if (UIUtility.WorldToUIPoint(hudText.WorldOffset, out uiPoint))
                    {
                        hudText.TextLabel.cachedTransform.position += uiPoint;
                    }

                    hudText.TextLabel.alpha = m_Config[hudText.Type].AlphaCurve.Evaluate(currentTime);

                    float scale = m_Config[hudText.Type].ScaleCurve.Evaluate(currentTime);
                    hudText.TextLabel.cachedTransform.localScale = Vector3.one * (scale < 0.001f ? 0.001f : scale);

                    if (currentTime > totalEnd)
                    {
                        DestroyHudText(i);
                    }
                }
            }

            public void Clear()
            {
                while (m_ActiveHudTexts.Count > 0)
                {
                    DestroyHudText(0);
                }
            }

            public HudText CreateHudText(int type, Vector3 worldOffset, string content)
            {
                HudText hudText = null;
                HudTextObject hudTextObject = m_HudTextObjects.Spawn();
                if (hudTextObject != null)
                {
                    hudText = hudTextObject.Target as HudText;
                    hudText.gameObject.SetActive(true);
                }
                else
                {
                    hudText = Instantiate(m_Template);
                    Transform transform = hudText.GetComponent<Transform>();
                    transform.SetParent(m_HudTextInstanceRoot);
                    transform.localScale = Vector3.one;
                    m_HudTextObjects.Register(new HudTextObject(hudText), true);
                }

                var config = m_Config[type];
                hudText.Type = type;
                hudText.StartTime = Time.time;
                hudText.WorldOffset = worldOffset;
                hudText.TextLabel.bitmapFont = config.Font;
                hudText.TextLabel.fontSize = config.FontSize;
                hudText.TextLabel.fontStyle = config.FontStyle;

                hudText.TextLabel.applyGradient = config.UseGradient;
                if (config.UseGradient)
                {
                    hudText.TextLabel.color = config.GradientTop;
                    hudText.TextLabel.gradientTop = config.GradientTop;
                    hudText.TextLabel.gradientBottom = config.GradienBottom;
                }
                else
                {
                    hudText.TextLabel.color = config.GradientTop;
                }

                hudText.TextLabel.effectStyle = config.Effect;
                hudText.TextLabel.effectColor = config.EffectColor;
                hudText.TextLabel.text = content;
                hudText.TextLabel.gameObject.SetActive(false);
                m_ActiveHudTexts.Add(hudText);

                return hudText;
            }

            private void DestroyHudText(int index)
            {
                var hudText = m_ActiveHudTexts[index];
                m_ActiveHudTexts.RemoveAt(index);

                if (hudText != null && hudText.gameObject)
                {
                    hudText.gameObject.SetActive(false);
                    m_HudTextObjects.Unspawn(hudText);
                }
            }

            private void OnLoadPreloadResourceSuccess(object sender, GameEventArgs e)
            {
                LoadPreloadResourceSuccessEventArgs ne = e as LoadPreloadResourceSuccessEventArgs;
                if (ne.Name != m_TemplateName)
                {
                    return;
                }

                m_Template = (ne.Resource as GameObject).GetComponent<HudText>();
            }
        }
    }
}
