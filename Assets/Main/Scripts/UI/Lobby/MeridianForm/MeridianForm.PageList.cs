using UnityEngine;

namespace Genesis.GameClient
{
    public partial class MeridianForm
    {
        [SerializeField]
        private MeridianItem[] m_MeridianList = null;

        private void RefreshPageList()
        {
            int maxMeridianProgress = GameEntry.Data.Meridian.MeridianProgress / Constant.MaxMeridianAstrolabe;
            int length = m_MeridianList.Length;
            for (int i = 0; i < maxMeridianProgress; i++)
            {
                m_MeridianList[i].StarCount = Constant.MaxMeridianAstrolabe;
            }
            m_CurrentMeridianIndex = maxMeridianProgress == 12 ? 11 : maxMeridianProgress;
            if (maxMeridianProgress < 12)
            {
                m_MeridianList[m_CurrentMeridianIndex].StarCount = GameEntry.Data.Meridian.MeridianProgress % Constant.MaxMeridianAstrolabe;
                for (int i = 0; i < length; i++)
                {
                    m_MeridianList[i].InitItem(i <= maxMeridianProgress, i == m_CurrentMeridianIndex);
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    m_MeridianList[i].InitItem(true, false);
                }
            }
        }
    }
}
