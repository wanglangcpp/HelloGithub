using UnityEngine;
using System.Collections;

namespace Genesis.GameClient
{
    public class MeleeResurrection : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_WhoKillMeText = null;

        [SerializeField]
        private UILabel m_ReviveWaitTimeText = null;

        [SerializeField]
        private UILabel m_TitleText = null;

        private float m_ReviveWaitTime = 0f;

        public void ShowUI(string killerName, float reviveWaitTime)
        {
            m_ReviveWaitTime = reviveWaitTime;
            m_TitleText.text = GameEntry.Localization.GetString("UI_TEXT_MELEE_WAS_KILLED");
            m_WhoKillMeText.text = GameEntry.Localization.GetString("UI_TEXT_MELEE_WHO_KILL_YOU", killerName);
            m_ReviveWaitTimeText.text = GameEntry.Localization.GetString("UI_TEXT_MELEE_RESURRECTION_TIME", m_ReviveWaitTime);
        }

        private void Update()
        {
            if (m_ReviveWaitTime >= 0)
            {
                m_ReviveWaitTime -= Time.deltaTime;
                m_ReviveWaitTimeText.text = GameEntry.Localization.GetString("UI_TEXT_MELEE_RESURRECTION_TIME", (int)m_ReviveWaitTime);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}