using UnityEngine;

namespace Genesis.GameClient
{
    public class UIBackground : MonoBehaviour
    {

        [SerializeField]
        public int m_Depth = 0;

        [SerializeField]
        private bool m_MaskBackground = false;

        [SerializeField]
        private GameObject m_BackgroundTemplate = null;

        private GameObject m_Mask = null;
        private GameObject m_Background = null;

        public void OnOpen()
        {
            if (m_MaskBackground)
            {
                GameEntry.UIBackground.ShowMask(m_Mask);
            }

            if (m_Background != null)
            {
                m_Background.SetActive(true);
            }
        }

        public void OnClose()
        {
            if (m_MaskBackground)
            {
                GameEntry.UIBackground.HideMask(m_Mask);
            }

            if (m_Background != null)
            {
                m_Background.SetActive(false);
            }
        }

        public void OnPause()
        {
            if (m_MaskBackground)
            {
                GameEntry.UIBackground.HideMask(m_Mask);
            }
        }

        public void OnResume()
        {
            if (m_MaskBackground)
            {
                GameEntry.UIBackground.ShowMask(m_Mask);
            }
        }

        public void OnRefocus()
        {
            if (m_MaskBackground)
            {
                GameEntry.UIBackground.ShowMask(m_Mask);
            }
        }

        private void Awake()
        {
            CreateMaskIfNeeded();
            CreateBackgroundIfNeeded();
        }

        private void OnDestroy()
        {
            DestroyMask();
            DestroyBackground();
        }

        private void CreateMaskIfNeeded()
        {
            if (!m_MaskBackground || m_Mask != null)
            {
                return;
            }

            m_Mask = NGUITools.AddChild(gameObject, GameEntry.UIBackground.MaskBackgroundTemplate);
            m_Mask.GetComponent<UIPanel>().depth += m_Depth;
        }

        private void CreateBackgroundIfNeeded()
        {
            if (m_MaskBackground || m_BackgroundTemplate == null || m_Background != null)
            {
                return;
            }

            m_Background = NGUITools.AddChild(gameObject, m_BackgroundTemplate);
        }

        private void DestroyMask()
        {
            if (m_Mask == null)
            {
                return;
            }

            Destroy(m_Mask);
            m_Mask = null;
        }

        private void DestroyBackground()
        {
            if (m_Background == null)
            {
                return;
            }

            Destroy(m_Background);
            m_Background = null;
        }
    }
}
