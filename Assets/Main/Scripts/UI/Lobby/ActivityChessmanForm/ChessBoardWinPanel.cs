using UnityEngine;

namespace Genesis.GameClient
{
    public class ChessBoardWinPanel : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_Text = null;

        private UIEffectsController m_EffectController = null;

        private bool m_ShouldShowWinEffect = false;
        private bool m_Started = false;

        public void ShowWinEffect()
        {
            if (!m_Started)
            {
                m_ShouldShowWinEffect = true;
                return;
            }

            if (m_EffectController != null)
            {
                m_EffectController.ShowEffect("Effect Win");
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_Text.text = GameEntry.Localization.GetString("UI_TEXT_CHESS_COMPLETE");
            m_EffectController = GetComponent<UIEffectsController>();
        }

        private void Start()
        {
            m_Started = true;

            if (m_ShouldShowWinEffect)
            {
                m_ShouldShowWinEffect = false;
                ShowWinEffect();
            }
        }

        private void OnEnable()
        {
            if (m_EffectController != null)
            {
                m_EffectController.Resume();
            }
        }

        private void OnDisable()
        {
            if (m_EffectController != null)
            {
                m_EffectController.Pause();
            }
        }

        #endregion MonoBehaviour
    }
}
