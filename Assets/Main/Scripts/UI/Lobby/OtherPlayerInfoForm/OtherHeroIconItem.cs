using UnityEngine;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 玩家详细信息界面的英雄Item
    /// </summary>
    public class OtherHeroIconItem : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_HeroIcon = null;

        [SerializeField]
        private UISprite m_HeroQuality = null;

        [SerializeField]
        private GameObject m_FightObj = null;

        [SerializeField]
        private GameObject m_SelectObj = null;

        private bool m_IsInTeam = false;
        private Action<int> m_OnClickHeroViewReturn = null;
        private int m_Index = 0;

        public bool IsSelect
        {
            set
            {
                m_SelectObj.SetActive(value);
            }
        }

        public void InitHeroItem(LobbyHeroData heroData, bool isInTeam, int index, Action<int> onClickHeroViewReturn)
        {
            m_Index = index;
            m_IsInTeam = isInTeam;
            m_OnClickHeroViewReturn = onClickHeroViewReturn;
            m_FightObj.SetActive(m_IsInTeam);
            m_HeroIcon.LoadAsync(heroData.IconId);
            int quality = (int)heroData.Quality;
            if (quality >= 1 && quality <= Constant.Quality.HeroBorderSpriteNames.Length - 1)
            {
                m_HeroQuality.spriteName = Constant.Quality.HeroBorderSpriteNames[quality];
            }
            else
            {
                m_HeroQuality.spriteName = Constant.Quality.HeroBorderSpriteNames[0];
            }
            IsSelect = m_Index == 0;
        }

        public void OnClickHeroView()
        {
            if (m_OnClickHeroViewReturn != null)
            {
                m_OnClickHeroViewReturn(m_Index);
            }
        }
    }
}
