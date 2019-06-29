using UnityEngine;

namespace Genesis.GameClient
{
    public class UIProgressCircle : MonoBehaviour
    {
        [SerializeField]
        private Transform[] m_Targets = null;

        private float m_Value = 0.0f;

        private const float RoundAngle = 360;

        public float Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
                if (m_Value > 1)
                {
                    m_Value = m_Value - Mathf.Round(m_Value);
                }
                SetProgressValue();
            }
        }

        private void SetProgressValue()
        {
            float angle = CalcCircleAngle();
            for (int i = 0; i < m_Targets.Length; i++)
            {
                m_Targets[i].localEulerAngles = new Vector3(m_Targets[i].localEulerAngles.x, m_Targets[i].localEulerAngles.y, angle);
            }
        }

        private float CalcCircleAngle()
        {
            float angle = 0;
            //0 -- -180 -- 180 --0
            if (m_Value >= 0 && m_Value <= 0.5f)
            {
                angle = -360 * (1 - m_Value);
            }
            else
            {
                angle = 360 * m_Value;
            }
            return angle;
        }

    }
}
