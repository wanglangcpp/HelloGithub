using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public class BattleRequestSwitch : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_RequestDescription = null;

        [SerializeField]
        private Animation m_SwitchAnimation = null;

        private bool m_IsClose = true;

        public void SwitchRequest()
        {
            m_IsClose = !m_IsClose;
            m_RequestDescription.SetActive(true);
            if (!m_IsClose)
            {
                m_SwitchAnimation[m_SwitchAnimation.clip.name].speed = -1.0f;
                m_SwitchAnimation[m_SwitchAnimation.clip.name].time = m_SwitchAnimation[m_SwitchAnimation.clip.name].length;
            }
            else
            {
                m_SwitchAnimation[m_SwitchAnimation.clip.name].speed = 1;
                m_SwitchAnimation[m_SwitchAnimation.clip.name].time = 0;
            }
            m_SwitchAnimation.Play();
            StartCoroutine(AnimFinishReturn(m_SwitchAnimation[m_SwitchAnimation.clip.name].length));
        }

        private IEnumerator AnimFinishReturn(float duration)
        {
            yield return new WaitForSeconds(duration);
            if (!m_IsClose)
            {
                m_RequestDescription.SetActive(false);
            }
            else
            {
                m_RequestDescription.SetActive(true);
            }
        }
    }
}
