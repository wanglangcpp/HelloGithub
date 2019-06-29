using UnityEngine;

namespace Genesis.GameClient
{
    public class MeridianItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_SelectObj = null;

        [SerializeField]
        private GameObject m_LockObj = null;

        [SerializeField]
        private GameObject m_UnLockObj = null;

        [SerializeField]
        private UISprite m_Progress = null;

        private float m_StarCount = -1;

        public float StarCount
        {
            get { return m_StarCount; }
            set { m_StarCount = value; }
        }

        public void InitItem(bool active, bool selected = false)
        {
            Select = selected;
            m_LockObj.SetActive(!active);
            m_UnLockObj.SetActive(active);
            float allCount = Constant.MaxMeridianAstrolabe;
            if (m_StarCount == -1)
            {
                m_StarCount = 0;
            }
            m_Progress.fillAmount = m_StarCount / allCount;
        }

        public bool Select
        {
            set
            {
                m_SelectObj.SetActive(value);
            }
        }
    }
}
