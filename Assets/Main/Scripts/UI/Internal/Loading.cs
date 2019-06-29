using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class Loading : MonoBehaviour
    {
        [SerializeField]
        private float m_FadeInDuration = 0.3f;

        [SerializeField]
        private float m_FadeOutDuration = 0.3f;

        [SerializeField]
        private UISprite m_BackgroundMask = null;

        [SerializeField]
        private UISprite m_MainPanel = null;

        [SerializeField]
        private UITexture m_Background = null;

        [SerializeField]
        private UILabel m_ProgressText = null;

        [SerializeField]
        private UIProgressBar m_ProgressBar = null;

        [SerializeField]
        private UILabel m_TipsText = null;

        [SerializeField]
        private UILabel m_VersionText = null;

        private UIEffectsController m_EffectsController = null;

        public UITexture Background
        {
            get
            {
                return m_Background;
            }
        }

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

        public string TipsText
        {
            get
            {
                return m_TipsText.text;
            }
            set
            {
                m_TipsText.text = value;
            }
        }

        public string VersionText
        {
            get
            {
                return m_VersionText.text;
            }
            set
            {
                m_VersionText.text = value;
            }
        }

        private void Awake()
        {
            m_EffectsController = GetComponent<UIEffectsController>();
        }

        public void ResizeBackground()
        {
            var uiRoot = GetComponentInParent<UIRoot>();
            m_Background.width = uiRoot.manualHeight * Screen.width / Screen.height + 4;
        }

        public void ShowBackgroundMask()
        {
            gameObject.SetActive(true);
            m_BackgroundMask.alpha = 1f;
        }

        public void FadeInMainPanel()
        {
            gameObject.SetActive(true);
            GameEntry.Waiting.StartWaiting(WaitingType.Default, "Loading");
            if (m_FadeInDuration <= 0f)
            {
                m_MainPanel.alpha = 1f;
            }
            else
            {
                TweenAlpha.Begin(m_MainPanel.gameObject, m_FadeInDuration, 1f);
            }

            if (m_EffectsController != null)
            {
                m_EffectsController.Resume();
            }
        }

        public void FadeOut(GameFrameworkAction onComplete)
        {
            if (this == null || gameObject == null)
            {
                return;
            }

            if (m_EffectsController != null)
            {
                m_EffectsController.Pause();
            }

            GameEntry.Waiting.StopWaiting(WaitingType.Default, "Loading");
            if (m_FadeOutDuration <= 0f)
            {
                if (onComplete != null) onComplete();
            }
            else
            {
                TweenAlpha.Begin(m_MainPanel.gameObject, m_FadeOutDuration, 0f);
                var tween = TweenAlpha.Begin(m_BackgroundMask.gameObject, m_FadeOutDuration, 0f);
                tween.SetOnFinished(() =>
                {
                    if (onComplete != null) onComplete();
                });
            }
        }
    }
}
