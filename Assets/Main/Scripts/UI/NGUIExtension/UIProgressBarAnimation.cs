using UnityEngine;

namespace Genesis.GameClient
{
    public class UIProgressBarAnimation : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_TargetIcon = null;

        [SerializeField]
        private UIProgressBar m_TargetProgressBar = null;

        [SerializeField]
        private float m_AnimTime = 0.2f;

        private float m_LastValue = 1.0f;

        private float m_AimValue = 1.0f;

        private float m_CurValue = 1.0f;

        private float m_AnimSpeed = 0.0f;

        public bool IsPlaying
        {
            get { return m_CurValue == m_AimValue; }
        }

        private void Start()
        {
            if (m_AnimTime == 0)
            {
                m_AnimTime = 0.2f;
            }
            SetAnimData();
            SetTargetValue();
        }

        private void Update()
        {
            if (m_CurValue > m_AimValue)
            {
                SetTargetValue();
            }
        }

        public void ProgressValueChanged()
        {
            if (m_AimValue < m_TargetProgressBar.value)
            {
                //m_CurValue = 1.0f;
                //m_LastValue = 1.0f;
            }
            else
            {
                m_LastValue = m_CurValue;
            }

            SetAnimData();
        }

        private void SetAnimData()
        {
            m_AimValue = m_TargetProgressBar.value;
            m_AnimSpeed = (m_LastValue - m_AimValue) / (m_AnimTime > 0f ? m_AnimTime : 0.2f);
        }

        private void SetTargetValue()
        {
            if (m_CurValue <= m_AimValue)
            {
                m_CurValue = m_AimValue;
            }
            else
            {
                m_CurValue = m_CurValue - m_AnimSpeed * Time.deltaTime;
            }

            m_TargetIcon.fillAmount = m_CurValue;
        }
    }
}
