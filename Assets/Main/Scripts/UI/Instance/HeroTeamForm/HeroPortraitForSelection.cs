using UnityEngine;

namespace Genesis.GameClient
{
    public class HeroPortraitForSelection : MonoBehaviour
    {
        [SerializeField]
        protected int m_HeroId = -1;

        [SerializeField]
        protected UITexture m_Portrait = null;

        [SerializeField]
        protected UISprite m_IntoBattleIcon = null;

        [SerializeField]
        protected UISprite m_Background = null;

        [SerializeField]
        protected float m_AlphaFactorWhenDrag = .7f;

        [SerializeField]
        protected int m_IndexInTeam = -1;

        [SerializeField]
        protected UISprite m_ElementIcon = null;

        [SerializeField]
        protected UISprite[] m_Stars = null;

        [SerializeField]
        protected UILabel m_QualityLevelLabel = null;

        [SerializeField]
        protected UISprite m_QualityIcon = null;

        [SerializeField]
        private Shader m_NormalShader = null;

        [SerializeField]
        private Shader m_GreyShader = null;

        [SerializeField]
        private UISprite m_QualityCornerIcon = null;

        private HeroTeamForm m_Receiver = null;

        private const string HeroTeamIndexIconName = "icon_";

        protected HeroTeamForm Receiver
        {
            get
            {
                if (m_Receiver == null)
                {
                    m_Receiver = GetComponentInParent<HeroTeamForm>();
                }

                return m_Receiver;
            }
        }

        protected Transform m_CachedTransform = null;

        public Transform CachedTransform
        {
            get
            {
                if (m_CachedTransform == null)
                {
                    m_CachedTransform = transform;
                }

                return m_CachedTransform;
            }
        }

        public int HeroId
        {
            get
            {
                return m_HeroId;
            }
            set
            {
                m_HeroId = value;
            }
        }

        public UITexture Portrait
        {
            get
            {
                return m_Portrait;
            }
        }

        public UISprite[] Stars
        {
            get
            {
                return m_Stars;
            }
        }

        public UILabel QualityLevelLabel
        {
            get
            {
                return m_QualityLevelLabel;
            }
        }

        public UISprite QualityIcon
        {
            get
            {
                return m_QualityIcon;
            }
        }

        public UISprite ElementIcon
        {
            get
            {
                return m_ElementIcon;
            }
        }

        public UISprite QualityCornerIcon
        {
            get
            {
                return m_QualityCornerIcon;
            }
        }

        public bool IsSelectedForBattle
        {
            get
            {
                return m_IntoBattleIcon.gameObject.activeSelf;
            }
        }

        public int IndexInTeam
        {
            get
            {
                return m_IndexInTeam;
            }

            set
            {
                m_IndexInTeam = value;
                m_IntoBattleIcon.gameObject.SetActive(m_IndexInTeam >= 0);
                m_IntoBattleIcon.spriteName = HeroTeamIndexIconName + (m_IndexInTeam + 1).ToString();
            }
        }

        public int Width
        {
            get
            {
                return m_Background.width;
            }
        }

        public int Height
        {
            get
            {
                return m_Background.height;
            }
        }

        private UIWidget[] m_CachedWidgets = null;

        protected UIWidget[] CachedWidgets
        {
            get
            {
                if (m_CachedWidgets == null)
                {
                    m_CachedWidgets = GetComponentsInChildren<UIWidget>(true);
                }

                return m_CachedWidgets;
            }
        }

        protected virtual void OnClickButton()
        {
            Receiver.OnSelectHeroPortrait(this);
        }

        public bool IsOnline
        {
            set
            {
                GetComponent<UIButton>().onClick.Clear();
                if (value)
                {
                    GetComponent<UIButton>().onClick.Add(new EventDelegate(OnClickButton));
                }
                Stars[0].transform.parent.gameObject.SetActive(value);
                QualityLevelLabel.transform.parent.gameObject.SetActive(value);
                QualityIcon.spriteName = Constant.Quality.HeroSquareBorderSpriteNames[1];
                var shader = value ? m_NormalShader : m_GreyShader;
                Portrait.shader = shader;
                QualityCornerIcon.gameObject.SetActive(value);
            }
        }


        #region MonoBehaivour

        private void Awake()
        {

        }

        #endregion
    }
}
