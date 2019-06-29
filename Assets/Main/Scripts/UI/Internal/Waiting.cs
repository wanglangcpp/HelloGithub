using UnityEngine;

namespace Genesis.GameClient
{
    public class Waiting : MonoBehaviour
    {
        [SerializeField]
        private float m_FadeInDuration = 0.3f;

        [SerializeField]
        private float m_FadeOutDuration = 0.3f;

        [SerializeField]
        private UISprite m_MainPanel = null;

        public void FadeIn()
        {
            gameObject.SetActive(true);
            if (m_FadeInDuration <= 0f)
            {
                m_MainPanel.alpha = 1f;
            }
            else
            {
                var tween = TweenAlpha.Begin(m_MainPanel.gameObject, m_FadeInDuration, 1f);
                tween.SetOnFinished(() => { });
            }
        }

        public void FadeOut()
        {
            if (this == null || gameObject == null)
            {
                return;
            }

            if (m_FadeOutDuration <= 0f)
            {
                gameObject.SetActive(false);
            }
            else
            {
                var tween = TweenAlpha.Begin(m_MainPanel.gameObject, m_FadeOutDuration, 0f);
                tween.SetOnFinished(() => { gameObject.SetActive(false); });
            }
        }
    }
}
