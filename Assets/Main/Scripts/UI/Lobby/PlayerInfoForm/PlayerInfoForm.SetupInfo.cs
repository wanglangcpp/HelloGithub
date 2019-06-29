using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class PlayerInfoForm
    {
        [Serializable]
        private class SetupInfo
        {
            [SerializeField]
            private UILabel m_ServerName = null;

            [SerializeField]
            private UIToggle m_MusicIsUnmuted = null;

            [SerializeField]
            private UIProgressBar m_MusicVolume = null;

            [SerializeField]
            private UIToggle m_SoundIsUnmuted = null;

            [SerializeField]
            private UIProgressBar m_SoundVolume = null;

            [SerializeField]
            private UIToggle[] m_QualityLevels = null;

            public void InitData()
            {
                m_ServerName.text = GameEntry.Data.Account.ServerData.Name;
                m_MusicIsUnmuted.value = !GameEntry.Sound.MusicIsMuted();
                m_MusicVolume.value = GameEntry.Sound.GetMusicVolume();
                m_SoundIsUnmuted.value = !GameEntry.Sound.SoundIsMuted();
                m_SoundVolume.value = GameEntry.Sound.GetSoundVolume();

                ResetQuality();
            }

            public void ResetQuality()
            {
                int qualityLevel = GameEntry.Setting.GetInt(Constant.Setting.QualityLevel, QualitySettings.GetQualityLevel());
                if (qualityLevel >= m_QualityLevels.Length)
                {
                    qualityLevel = m_QualityLevels.Length - 1;
                }
                else if (qualityLevel < 0)
                {
                    qualityLevel = 0;
                }

                m_QualityLevels[qualityLevel].value = true;
            }
        }
    }
}
