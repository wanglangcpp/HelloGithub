using GameFramework;
using GameFramework.Event;
using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 屏幕触效组件。
    /// </summary>
    public class ScreenTouchEffectComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_ParentNode = null;

        [SerializeField]
        private string m_NodeName = "Screen Touch Effect Instances";

        [SerializeField]
        private GameObject m_BackgroundTemplate = null;

        [SerializeField]
        private string m_EffectTemplateName = null;

        [SerializeField]
        private int m_InstancePoolCapacity = 4;

        [SerializeField]
        private float m_EffectDuration = 1f;

        [SerializeField]
        private int m_EffectRenderQueue = 8999;

        private GameObject m_Background = null;
        private ScreenTouchEffect m_EffectTemplate = null;
        private IObjectPool<ScreenTouchEffectObject> m_ScreenTouchEffectObjects = null;
        private IList<ScreenTouchEffect> m_ActiveScreenTouchEffects = new List<ScreenTouchEffect>();

        public bool PreloadComplete
        {
            get
            {
                return m_EffectTemplate != null;
            }
        }

        private void Start()
        {
            GameEntry.Input.OnTouchBegan += OnInputTouchBegan;
            GameEntry.Event.Subscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);

            m_ScreenTouchEffectObjects = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<ScreenTouchEffectObject>("ScreenTouchEffect", m_InstancePoolCapacity);

            m_Background = NGUITools.AddChild(m_ParentNode, m_BackgroundTemplate);
            m_Background.name = m_NodeName;
            m_Background.SetActive(true);
        }

        private void OnDestroy()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Input.OnTouchBegan -= OnInputTouchBegan;
            GameEntry.Event.Unsubscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);
        }

        private void Update()
        {
            float time = Time.time;
            for (int i = m_ActiveScreenTouchEffects.Count - 1; i >= 0; i--)
            {
                ScreenTouchEffect screenTouchEffect = m_ActiveScreenTouchEffects[i];

                float currentTime = time - screenTouchEffect.StartTime;
                if (currentTime > m_EffectDuration)
                {
                    DestroyScreenTouchEffect(i);
                }
            }
        }

        public void Preload()
        {
            PreloadUtility.LoadPreloadResource(m_EffectTemplateName);
        }

        private ScreenTouchEffect CreateScreenTouchEffect(Vector3 position)
        {
            ScreenTouchEffect screenTouchEffect = null;
            ScreenTouchEffectObject screenTouchEffectObject = m_ScreenTouchEffectObjects.Spawn();
            if (screenTouchEffectObject != null)
            {
                screenTouchEffect = screenTouchEffectObject.Target as ScreenTouchEffect;
                screenTouchEffect.gameObject.SetActive(true);
            }
            else
            {
                screenTouchEffect = NGUITools.AddChild(m_Background, m_EffectTemplate.gameObject).GetComponent<ScreenTouchEffect>();
                Renderer[] renderers = screenTouchEffect.GetComponentsInChildren<Renderer>(true);
                for (int i = 0; i < renderers.Length; i++)
                {
                    for (int j = 0; j < renderers[i].materials.Length; j++)
                    {
                        renderers[i].materials[j].renderQueue = m_EffectRenderQueue;
                    }
                }

                Transform transform = screenTouchEffect.GetComponent<Transform>();
                transform.SetParent(m_Background.transform);
                transform.localScale = Vector3.one;

                m_ScreenTouchEffectObjects.Register(new ScreenTouchEffectObject(screenTouchEffect), true);
            }

            screenTouchEffect.transform.position = position;
            screenTouchEffect.StartTime = Time.time;
            m_ActiveScreenTouchEffects.Add(screenTouchEffect);

            return screenTouchEffect;
        }

        private void DestroyScreenTouchEffect(int index)
        {
            var hudText = m_ActiveScreenTouchEffects[index];
            m_ActiveScreenTouchEffects.RemoveAt(index);

            if (hudText != null && hudText.gameObject)
            {
                hudText.gameObject.SetActive(false);
                m_ScreenTouchEffectObjects.Unspawn(hudText);
            }
        }

        private void OnInputTouchBegan(Vector2 position)
        {
            if (m_EffectTemplate == null)
            {
                return;
            }

            var currentUICamera = UICamera.currentCamera;
            if (currentUICamera == null)
            {
                return;
            }

            Vector3 uiPoint = currentUICamera.ScreenToWorldPoint(position);
            CreateScreenTouchEffect(uiPoint);
        }

        private void OnLoadPreloadResourceSuccess(object sender, GameEventArgs e)
        {
            LoadPreloadResourceSuccessEventArgs ne = e as LoadPreloadResourceSuccessEventArgs;
            if (ne.Name == m_EffectTemplateName)
            {
                m_EffectTemplate = (ne.Resource as GameObject).GetComponent<ScreenTouchEffect>();
                if (m_EffectTemplate == null)
                {
                    Log.Warning("Screen touch effect is invalid.");
                }
            }
        }
    }
}
