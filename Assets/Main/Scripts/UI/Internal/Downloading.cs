using UnityEngine;

namespace Genesis.GameClient
{
    public class Downloading : MonoBehaviour
    {
        [SerializeField]
        private float m_FadeInDuration = 0.3f;

        [SerializeField]
        private float m_FadeOutDuration = 0.3f;

        [SerializeField]
        private UISprite m_MainPanel = null;

        [SerializeField]
        private UILabel m_ProgressText = null;

        [SerializeField]
        private UIProgressBar m_ProgressBar = null;

        private UIEffectsController m_EffectsController = null;

        public string ProgressText
        {
            get
            {
                return m_ProgressText.text;
            }
            set
            {
                m_ProgressText.text = value;
            }
        }

        public float Progress
        {
            get
            {
                return m_ProgressBar.value;
            }
            set
            {
                m_ProgressBar.value = value;
            }
        }

        private void Awake()
        {
            m_EffectsController = GetComponent<UIEffectsController>();
        }

        public void FadeIn()
        {
            if (m_FadeInDuration <= 0f)
            {
                m_MainPanel.alpha = 1f;
            }
            else
            {
                var tween = TweenAlpha.Begin(m_MainPanel.gameObject, m_FadeInDuration, 1f);
                tween.SetOnFinished(() => { });
            }

            if (m_EffectsController != null)
            {
                m_EffectsController.Resume();
            }
        }

        public void FadeOut()
        {
            if (m_EffectsController != null)
            {
                m_EffectsController.Pause();
            }

            if (m_FadeOutDuration <= 0f)
            {
                Destroy(gameObject);
            }
            else
            {
                var tween = TweenAlpha.Begin(m_MainPanel.gameObject, m_FadeOutDuration, 0f);
                tween.SetOnFinished(() => { Destroy(gameObject); });
            }
        }
    }
}
