using UnityEngine;

namespace Genesis.GameClient
{
    public class UISound : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] m_OpenAudioClips = null;

        [SerializeField]
        private AudioClip[] m_CloseAudioClips = null;

        public void PlayOpenSounds()
        {
            for (int i = 0; i < m_OpenAudioClips.Length; ++i)
            {
                NGUITools.PlaySound(m_OpenAudioClips[i]);
            }
        }

        public void PlayCloseSounds()
        {
            for (int i = 0; i < m_CloseAudioClips.Length; ++i)
            {
                NGUITools.PlaySound(m_CloseAudioClips[i]);
            }
        }
    }
}
