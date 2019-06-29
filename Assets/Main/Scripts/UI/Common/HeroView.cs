using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class HeroView : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_HeroIcon = null;

        [SerializeField]
        private UISprite m_QualityBg = null;

        [SerializeField]
        private UISprite m_HeroElement = null;

        [SerializeField]
        private GameObject m_Select = null;

        [SerializeField]
        private BoxCollider m_Collider = null;

        [SerializeField]
        private int m_HeroId = 0;

        [SerializeField]
        private UISprite[] m_Stars = null;

        private Action<int> m_OnClickReturn = null;
        private QualityType m_Quality;

        public void InitHeroView(int heroId, Action<int> onClickReturn = null, LobbyHeroData lobbyHeroData = null)
        {
            m_HeroId = heroId;
            m_OnClickReturn = onClickReturn;
            if (m_Collider != null)
            {
                m_Collider.enabled = false;
            }
            var dt = GameEntry.DataTable.GetDataTable<DRHero>();

            DRHero dr = dt.GetDataRow(heroId);
            if (dr == null)
            {
                Log.Warning("Hero Id '{0}' not found.", heroId);
                return;
            }
            if (m_HeroIcon != null)
            {
                m_HeroIcon.LoadAsync(dr.IconId, OnHeroIconLoadSuccess);
            }

            if (m_HeroElement != null)
            {
                m_HeroElement.spriteName = UIUtility.GetElementSpriteName(dr.ElementId);
            }
            var heroData = lobbyHeroData == null ? GameEntry.Data.LobbyHeros.GetData(heroId) : lobbyHeroData;
            if (heroData == null)
            {
                return;
            }
            Quality = heroData.Quality;
            if (m_Stars != null && m_Stars.Length > 0)
            {
                UIUtility.SetStarLevel(m_Stars, heroData.StarLevel);
            }
        }

        private void UpdateQualityBg()
        {
            if (m_QualityBg == null)
            {
                return;
            }

            int index = (int)Quality;
            if (index >= 1 && index <= Constant.Quality.HeroBorderSpriteNames.Length - 1)
            {
                m_QualityBg.spriteName = Constant.Quality.HeroBorderSpriteNames[index];
            }
            else
            {
                m_QualityBg.spriteName = Constant.Quality.HeroBorderSpriteNames[0];
            }
        }

        /// <summary>
        /// 品质。
        /// </summary>
        public QualityType Quality
        {
            get
            {
                return m_Quality;
            }

            set
            {
                m_Quality = value;
                UpdateQualityBg();
            }
        }

        public bool Select
        {
            set
            {
                if (m_Select == null)
                {
                    return;
                }
                m_Select.SetActive(value);
            }
        }

        public int HeroId
        {
            get
            {
                return m_HeroId;
            }
        }

        private void OnHeroIconLoadSuccess(UISprite sprite, string spriteName, object userData)
        {
            if (m_Collider != null)
            {
                m_Collider.enabled = true;
            }
        }

        public void OnClickView()
        {
            if (m_OnClickReturn != null)
            {
                m_OnClickReturn(m_HeroId);
            }
        }
    }
}
