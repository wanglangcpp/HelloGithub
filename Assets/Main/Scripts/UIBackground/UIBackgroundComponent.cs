using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 界面背景组件。
    /// </summary>
    public class UIBackgroundComponent : MonoBehaviour
    {
        [SerializeField]
        private string m_TitleTemplateName = null;

        [SerializeField]
        private string m_MaskBackgroundTemplateName = null;
        [SerializeField]
        private GameObject m_ResourceBackgroundTemplate = null;
        [SerializeField]
        private GameObject m_DefaultBackgroundTemplate = null;

        [SerializeField]
        private GameObject m_BlankBackgroundTemplate = null;

        [SerializeField]
        private GameObject m_ParentNode = null;

        [SerializeField]
        private string m_NodeName = "Background";

        private GameObject m_TitleTemplate = null;
        private GameObject m_MaskBackgroundTemplate = null;

        private GameObject m_Background = null;
        private LinkedList<GameObject> m_Masks = new LinkedList<GameObject>();

        public GameObject TitleTemplate
        {
            get
            {
                return m_TitleTemplate;
            }
        }
        public GameObject ResourceBackgroundTemplate
        {
            get { return m_ResourceBackgroundTemplate; }
        }
        public GameObject DefaultBackgroundTemplate
        {
            get
            {
                return m_DefaultBackgroundTemplate;
            }
        }

        public GameObject MaskBackgroundTemplate
        {
            get
            {
                return m_MaskBackgroundTemplate;
            }
        }

        public bool PreloadComplete
        {
            get
            {
                return m_TitleTemplate != null && m_MaskBackgroundTemplate != null;
            }
        }

        private void Start()
        {
            GameEntry.Event.Subscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);
        }

        private void OnDestroy()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Event.Unsubscribe(EventId.LoadPreloadResourceSuccess, OnLoadPreloadResourceSuccess);
        }

        public void Preload()
        {
            PreloadUtility.LoadPreloadResource(m_TitleTemplateName);
            PreloadUtility.LoadPreloadResource(m_MaskBackgroundTemplateName);
        }

        public void ShowBlank()
        {
            DestroyBackground();
            m_Background = NGUITools.AddChild(m_ParentNode, m_BlankBackgroundTemplate);
            m_Background.name = m_NodeName;
            m_Background.SetActive(true);
        }

        public void HideBlank()
        {
            DestroyBackground();
        }

        public void ShowDefault()
        {
            DestroyBackground();
            m_Background = NGUITools.AddChild(m_ParentNode, m_DefaultBackgroundTemplate);
            m_Background.name = m_NodeName;
            m_Background.SetActive(true);
        }

        public void HideDefault()
        {
            DestroyBackground();
        }
        public void ShowResourceBg()
        {
            DestroyBackground();
            m_Background = NGUITools.AddChild(m_ParentNode, m_ResourceBackgroundTemplate);
            m_Background.name = m_NodeName;
            m_Background.SetActive(true);
        }
        public void HideResourceBg()
        {
            DestroyBackground();
        }
        public void ShowMask(GameObject mask)
        {
            LinkedListNode<GameObject> node = m_Masks.Find(mask);
            if (node == null)
            {
                node = m_Masks.AddFirst(mask);
            }
            else
            {
                m_Masks.Remove(node);
                m_Masks.AddFirst(node);
            }

            RefreshMasks();
        }

        public void HideMask(GameObject mask)
        {
            m_Masks.Remove(mask);
            RefreshMasks();
        }

        private void RefreshMasks()
        {
            LinkedListNode<GameObject> current = m_Masks.First;
            if (current != null)
            {
                current.Value.SetActive(true);
                current = current.Next;
            }

            while (current != null)
            {
                current.Value.SetActive(false);
                current = current.Next;
            }
        }

        private void DestroyBackground()
        {
            if (m_Background != null)
            {
                Destroy(m_Background);
                m_Background = null;
                GameEntry.Resource.ForceUnloadUnusedAssets(true);
            }
        }

        private void OnLoadPreloadResourceSuccess(object sender, GameEventArgs e)
        {
            LoadPreloadResourceSuccessEventArgs ne = e as LoadPreloadResourceSuccessEventArgs;
            if (ne.Name == m_TitleTemplateName)
            {
                m_TitleTemplate = ne.Resource as GameObject;
            }
            else if (ne.Name == m_MaskBackgroundTemplateName)
            {
                m_MaskBackgroundTemplate = ne.Resource as GameObject;
            }
        }
    }
}
