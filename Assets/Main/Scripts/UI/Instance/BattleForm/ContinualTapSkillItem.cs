using UnityEngine;

namespace Genesis.GameClient
{
    public class ContinualTapSkillItem : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_Foreground = null;

        private UIEffectsController m_EffectsController = null;

        public void ShowForeground()
        {
            m_Foreground.gameObject.SetActive(true);
            m_EffectsController.ShowEffect("ShowForeground");
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_EffectsController = GetComponent<UIEffectsController>();
            m_Foreground.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            m_EffectsController.Resume();
        }

        private void OnDisable()
        {
            m_EffectsController.Pause();
        }

        #endregion MonoBehaviour
    }
}
