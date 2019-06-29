using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 抽奖详情界面 -- 卡牌项。
    /// </summary>
    public class ChanceDetailCardItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Back = null;

        public GameObject Back
        {
            get
            {
                return m_Back;
            }
        }

        [SerializeField]
        private GameObject m_Front = null;

        public GameObject Front
        {
            get
            {
                return m_Front;
            }
        }

        [SerializeField]
        private GameObject m_ItemPanel = null;

        [SerializeField]
        private GameObject m_HeroPanel = null;

        [SerializeField]
        private UISprite m_Mask = null;

        [SerializeField]
        private UISprite m_ItemIcon = null;

        [SerializeField]
        private UILabel m_ItemName = null;

        [SerializeField]
        private UITexture m_HeroPortrait = null;

        [SerializeField]
        private UILabel m_HeroName = null;

        [SerializeField]
        private UISprite[] m_HeroStars = null;

        [SerializeField]
        private UIIntKey m_IntKey = null;

        [SerializeField]
        private GeneralItemView m_ChanceItemView = null;

        public UIIntKey IntKey
        {
            get
            {
                return m_IntKey;
            }
        }

        [SerializeField]
        private UIButton m_Button = null;

        public UIButton Button
        {
            get
            {
                return m_Button;
            }
        }

        private Animation m_CachedAnimation = null;

        public Animation CachedAnimation
        {
            get
            {
                return m_CachedAnimation;
            }
        }

        private Transform m_CachedTransform = null;

        public Transform CachedTransform
        {
            get
            {
                return m_CachedTransform;
            }
        }

        private ChanceDetailForm m_Form = null;

        private ChanceDetailForm Form
        {
            get
            {
                if (m_Form == null)
                {
                    m_Form = GetComponentInParent<ChanceDetailForm>();
                }

                return m_Form;
            }
        }

        private UIWidget m_OuterWidget = null;

        public UIWidget OuterWidget
        {
            get
            {
                return m_OuterWidget;
            }
        }

        private UIWidget m_BackWidget = null;

        public UIWidget BackWidget
        {
            get
            {
                return m_BackWidget;
            }
        }

        private UIWidget m_FrontWidget = null;

        public UIWidget FrontWidget
        {
            get
            {
                return m_FrontWidget;
            }
        }

        public float MaskAlpha
        {
            set
            {
                m_Mask.alpha = value;
            }

            get
            {
                return m_Mask.alpha;
            }
        }

        public void RefreshAsHero(int heroId,
            GameFrameworkAction<UITexture, object> onSuccess = null,
            GameFrameworkAction<UITexture, object> onFailure = null,
            GameFrameworkAction<UITexture, object> onAbort = null,
            object userData = null)
        {
            m_ItemPanel.SetActive(false);
            m_HeroPanel.SetActive(true);

            var dtHero = GameEntry.DataTable.GetDataTable<DRHero>();
            DRHero drHero = dtHero.GetDataRow(heroId);
            if (drHero == null)
            {
                Log.Warning("Can not find hero type '{0}'.", heroId.ToString());
                UIUtility.SetStarLevel(m_HeroStars, 0);
                return;
            }

            m_HeroName.text = GameEntry.Localization.GetString(drHero.Name);
            UIUtility.SetStarLevel(m_HeroStars, drHero.DefaultStarLevel);

            m_HeroPortrait.LoadAsync(drHero.ProtraitTextureId, onSuccess, onFailure, onAbort, userData);
        }

        public void RefreshAsGeneralItem(int itemTypeId, int itemCount,
            GameFrameworkAction<UISprite, string, object> onSuccess = null,
            GameFrameworkAction<UISprite, object> onFailure = null,
            GameFrameworkAction<UISprite, object> onAbort = null,
            object userData = null)
        {
            m_ItemPanel.SetActive(true);
            m_HeroPanel.SetActive(false);

            m_ChanceItemView.InitGeneralItem(itemTypeId, itemCount);
            m_ItemName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(itemTypeId));
            m_ItemIcon.LoadAsync(GeneralItemUtility.GetGeneralItemIconId(itemTypeId), onSuccess, onFailure, onAbort, userData);
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_CachedAnimation = GetComponent<Animation>();
            m_CachedTransform = transform;
            m_OuterWidget = GetComponent<UIWidget>();
            m_FrontWidget = m_Front.GetComponent<UIWidget>();
            m_BackWidget = m_Back.GetComponent<UIWidget>();
        }

        private void OnDestroy()
        {
            m_ItemIcon.atlas = null;
            m_HeroPortrait.mainTexture = null;
        }

        #endregion MonoBehaviour
    }
}
