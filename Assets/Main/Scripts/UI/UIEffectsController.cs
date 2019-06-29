using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 界面特效控制器。
    /// </summary>
    public class UIEffectsController : MonoBehaviour
    {
        [Serializable]
        private class Data
        {
            public string Key = string.Empty;
            public GameObject Template = null;
            public Transform Parent = null;

            [Tooltip("Override Parent if not empty.")]
            public string ParentPath = null;

            public Vector3 LocalPosition = Vector3.zero;
            public Vector3 LocalRotation = Vector3.zero;
            public int StartingDepth = 0;
            public bool IsPersistent = false;
            public int Width = 0;
            public int Height = 0;
            public bool DeactivateOutOfPanel = false;
            public bool Hiden = false;
        }

        private class RuntimeCache
        {
            public int Id;
            public float TotalTimeElapsed;
            public UIEffect EffectScript = null;
            public int Width;
            public int Height;
            public bool DeactivateOutOfPanel;
            public string Key;
        }

        [SerializeField]
        private Data[] m_Datas = null;

        private Dictionary<string, Data> m_RuntimeDatas = new Dictionary<string, Data>();
        private LinkedList<RuntimeCache> m_RuntimeCaches = new LinkedList<RuntimeCache>();
        private bool m_IsPaused = true;
        private int m_CurrentId = 1;
        private Transform m_CachedTransform = null;

        public void Resume()
        {
            if (!m_IsPaused)
            {
                return;
            }

            m_IsPaused = false;

            foreach (var kvPairs in m_RuntimeDatas)
            {
                var key = kvPairs.Key;
                if (kvPairs.Value.IsPersistent)
                {
                    ShowEffect(key);
                }
            }
        }

        public void Pause()
        {
            if (m_IsPaused)
            {
                return;
            }

            m_IsPaused = true;

            for (var node = m_RuntimeCaches.First; node != null; node = node.Next)
            {
                if (node.Value.EffectScript == null || node.Value.EffectScript.gameObject == null)
                {
                    continue;
                }

                Destroy(node.Value.EffectScript.gameObject);
            }

            m_RuntimeCaches.Clear();
        }

        public bool IsPaused
        {
            get { return m_IsPaused; }
        }

        public bool HasEffect(string key)
        {
            return m_RuntimeDatas.ContainsKey(key);
        }

        public bool EffectIsShowing(string key)
        {
            for (var node = m_RuntimeCaches.First; node != null; node = node.Next)
            {
                if (node.Value.EffectScript == null || node.Value.EffectScript.gameObject == null)
                {
                    continue;
                }

                if (node.Value.Key == key)
                {
                    return true;
                }
            }

            return false;
        }

        public int ShowEffect(string key)
        {
            if (m_IsPaused)
            {
                return 0;
            }

            Data data;
            if (!m_RuntimeDatas.TryGetValue(key, out data))
            {
                Log.Warning("Effect for key '{0}' cannot be found.", key);
                return 0;
            }

            return ShowEffect(data);
        }

        public bool DestroyEffect(int id)
        {
            for (var node = m_RuntimeCaches.First; node != null; node = node.Next)
            {
                if (node.Value.Id == id)
                {
                    Destroy(node.Value.EffectScript.gameObject);
                    m_RuntimeCaches.Remove(node);
                    return true;
                }
            }

            return false;
        }

        public int DestroyEffect(string key)
        {
            int destroyCount = 0;
            for (var node = m_RuntimeCaches.First; node != null; node = node.Next)
            {
                if (node.Value.Key == key)
                {
                    Destroy(node.Value.EffectScript.gameObject);
                    m_RuntimeCaches.Remove(node);
                }
            }

            return destroyCount;
        }

        private int ShowEffect(Data data)
        {
            if (data.Hiden)
            {
                return 0;
            }
            if (data.Template == null)
            {
                Log.Warning("Effect template for key '{0}' cannot be found.", data.Key);
                return 0;
            }

            var root = new GameObject(string.Format("UIEffect - {0}", data.Key));
            root.layer = Constant.Layer.UILayerId;
            var rootTransform = root.transform;

            if (string.IsNullOrEmpty(data.ParentPath))
            {
                rootTransform.parent = data.Parent;
            }
            else
            {
                rootTransform.parent = m_CachedTransform.Find(data.ParentPath);
            }

            rootTransform.localPosition = Vector3.zero;
            rootTransform.localRotation = Quaternion.identity;
            rootTransform.localScale = Vector3.one;

            var effectScript = UIEffect.Create(root, data.Template, data.LocalPosition, Quaternion.Euler(data.LocalRotation), data.StartingDepth, data.Width, data.Height);

            if (effectScript == null)
            {
                Log.Warning("Cannot instantiate effect for key '{0}'", data.Key);
                Destroy(root);
                return 0;
            }

            int id = m_CurrentId++;
            m_RuntimeCaches.AddLast(new RuntimeCache
            {
                Id = id,
                EffectScript = effectScript,
                TotalTimeElapsed = 0f,
                DeactivateOutOfPanel = data.DeactivateOutOfPanel,
                Width = data.Width,
                Height = data.Height,
                Key = data.Key,
            });

            return id;
        }

        private void CheckOutOfPanel(RuntimeCache rc)
        {
            var panel = rc.EffectScript.Panel;
            if (panel == null || panel.clipping == UIDrawCall.Clipping.None)
            {
                return;
            }

            var panelTransform = panel.cachedTransform;
            var t = rc.EffectScript.CachedTransform;
            if (t == null || panelTransform == null)
            {
                return;
            }

            var bounds = NGUIMath.CalculateRelativeWidgetBounds(panelTransform, t, true);
            var effectRect = new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);
            var clipRegion = panel.finalClipRegion;
            var panelRect = new Rect(clipRegion.x - panel.width * .5f, clipRegion.y - panel.height * .5f, clipRegion.z, clipRegion.w);
            var contained = panelRect.Contains(new Vector2(effectRect.xMin, effectRect.yMin)) && panelRect.Contains(new Vector2(effectRect.xMax, effectRect.yMax));

            var go = rc.EffectScript.gameObject;
            if (contained && !go.activeSelf)
            {
                go.SetActive(true);
            }
            else if (!contained && go.activeSelf)
            {
                go.SetActive(false);
            }
        }

        #region MonoBahviour

        private void Awake()
        {
            for (int i = 0; i < m_Datas.Length; ++i)
            {
                var data = m_Datas[i];
                m_RuntimeDatas.Add(data.Key, data);
            }

            m_CachedTransform = transform;
        }

        private void Update()
        {
            for (var node = m_RuntimeCaches.First; node != null; /* Empty */)
            {
                var nodeValue = node.Value;
                var effectScript = nodeValue.EffectScript;

                if (nodeValue.DeactivateOutOfPanel)
                {
                    CheckOutOfPanel(nodeValue);
                }

                if (effectScript.IsForever)
                {
                    node = node.Next;
                    continue;
                }

                nodeValue.TotalTimeElapsed += Time.deltaTime;
                if (nodeValue.TotalTimeElapsed >= effectScript.Duration)
                {
                    Destroy(effectScript.gameObject);
                    var next = node.Next;
                    m_RuntimeCaches.Remove(node);
                    node = next;
                }
                else
                {
                    node = node.Next;
                }
            }
        }

        private void OnDestroy()
        {
            m_RuntimeDatas.Clear();
            m_RuntimeCaches.Clear();
        }

        #endregion MonoBahviour
    }
}
