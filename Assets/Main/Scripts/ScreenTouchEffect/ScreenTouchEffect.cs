using UnityEngine;

namespace Genesis.GameClient
{
    public class ScreenTouchEffect : MonoBehaviour
    {
        private ParticleSystem[] m_CachedParticleSystems = null;

        public float StartTime
        {
            get;
            set;
        }

        private void Start()
        {
            m_CachedParticleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_CachedParticleSystems.Length; ++i)
            {
                m_CachedParticleSystems[i].Pause();
            }
        }

        private void Update()
        {
            for (int i = 0; i < m_CachedParticleSystems.Length; ++i)
            {
                m_CachedParticleSystems[i].Simulate(Time.unscaledDeltaTime, false, false);
            }
        }
    }
}
