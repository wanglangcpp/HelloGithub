using UnityEngine;

namespace Genesis.GameClient
{
    public abstract class HeroAlbumBaseCard : MonoBehaviour
    {
        protected int m_HeroId = -1;

        public int HeroId
        {
            get
            {
                return m_HeroId;
            }
        }

        [SerializeField]
        protected UISprite m_ElementIcon = null;

        [SerializeField]
        protected UITexture m_Portrait = null;

        [SerializeField]
        protected UILabel m_HeroName = null;

        [SerializeField]
        protected UILabel m_GoldenHeroName = null;

        [SerializeField]
        protected UISprite[] m_Stars = null;

        [SerializeField]
        protected GameObject m_CommonBG = null;

        [SerializeField]
        protected GameObject m_GoldenBG = null;
    }
}
