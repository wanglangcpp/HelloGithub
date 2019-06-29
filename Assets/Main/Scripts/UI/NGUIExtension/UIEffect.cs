using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// UI 特效。
    /// </summary>
    public class UIEffect : MonoBehaviour
    {
        private GameObject m_EffectGO = null;
        private EffectTime m_EffectTime = null;
        private UIFakeWidget[] m_FakeWidgets = null;
        private UIPanel m_CachedPanel = null;
        private Transform m_CachedTransform = null;

        public bool IsForever
        {
            get
            {
                return m_EffectTime.IsForever;
            }
        }

        public float Duration
        {
            get
            {
                return m_EffectTime.Duration;
            }
        }

        public UIPanel Panel
        {
            get
            {
                return m_CachedPanel;
            }
        }

        public Transform CachedTransform
        {
            get
            {
                return m_CachedTransform;
            }
        }

        public static UIEffect Create(GameObject go, GameObject effectPrefab, Vector3 localPosition, Quaternion localRotation, int startingDepth)
        {
            return Create(go, effectPrefab, localPosition, localRotation, startingDepth, 0, 0);
        }

        public static UIEffect Create(GameObject go, GameObject effectPrefab, Vector3 localPosition, Quaternion localRotation, int startingDepth, int width, int height)
        {
            if (effectPrefab == null)
            {
                Log.Warning("Effect prefab is invalid.");
                return null;
            }

            if (effectPrefab.GetComponent<EffectTime>() == null)
            {
                Log.Warning("There is no EffectTime component on the effect prefab.");
                return null;
            }

            var script = go.AddComponent<UIEffect>();
            script.Init(effectPrefab, localPosition, localRotation, startingDepth, width, height);
            return script;
        }

        private void Init(GameObject effectPrefab, Vector3 localPosition, Quaternion localRotation, int startingDepth, int width, int height)
        {
            m_EffectGO = NGUITools.AddChild(gameObject, effectPrefab);
            m_EffectTime = m_EffectGO.GetComponent<EffectTime>();

            var t = m_EffectTime.transform;
            t.localPosition = localPosition;
            t.localRotation = localRotation;
            t.localScale = Vector3.one;

            var renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            var materialDatas = new List<MaterialData>();
            for (int i = 0; i < renderers.Length; ++i)
            {
                var renderer = renderers[i];
                for (int j = 0; j < renderer.materials.Length; ++j)
                {
                    materialDatas.Add(new MaterialData { Renderer = renderer, MaterialIndex = j });
                }
            }

            materialDatas.Sort(CompareMaterialDatas);

            m_FakeWidgets = new UIFakeWidget[materialDatas.Count];
            int lastRenderQueue = -1;
            int depth = startingDepth;
            for (int i = 0; i < materialDatas.Count; ++i)
            {
                var materialData = materialDatas[i];
                if (materialData.RendererQueue != lastRenderQueue)
                {
                    ++depth;
                    lastRenderQueue = materialData.RendererQueue;
                }

                var fakeWidget = NGUITools.AddChild<UIFakeWidget>(gameObject);
                fakeWidget.gameObject.name = string.Format("Fake Widget - {0:D2}", i);
                fakeWidget.Init(materialData.Renderer, materialData.MaterialIndex, depth, width, height);
                m_FakeWidgets[i] = fakeWidget;
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_CachedTransform = transform;
        }

        private void Start()
        {
            for (var t = m_CachedTransform; t != null; t = t.parent)
            {
                var panel = t.GetComponent<UIPanel>();
                if (panel != null && panel.clipping == UIDrawCall.Clipping.SoftClip)
                {
                    m_CachedPanel = panel;
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            if (m_FakeWidgets == null)
            {
                return;
            }

            for (int i = 0; i < m_FakeWidgets.Length; ++i)
            {
                m_FakeWidgets[i] = null;
            }

            m_FakeWidgets = null;
        }

        #endregion MonoBehaviour

        private int CompareMaterialDatas(MaterialData a, MaterialData b)
        {
            return a.RendererQueue.CompareTo(b.RendererQueue);
        }

        private class MaterialData
        {
            public Renderer Renderer;
            public int MaterialIndex;

            public int RendererQueue
            {
                get
                {
                    return Renderer.materials[MaterialIndex].renderQueue;
                }
            }
        }
    }
}
