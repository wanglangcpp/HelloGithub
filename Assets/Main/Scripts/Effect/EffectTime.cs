using UnityEngine;

namespace Genesis.GameClient
{
    public class EffectTime : MonoBehaviour
    {
        [SerializeField]
        private bool m_Forever = false;

        [SerializeField]
        private float m_Duration = 1f;

        public bool IsForever
        {
            get
            {
                return m_Forever;
            }
        }

        public float Duration
        {
            get
            {
                return m_Duration;
            }
        }
    }
}
