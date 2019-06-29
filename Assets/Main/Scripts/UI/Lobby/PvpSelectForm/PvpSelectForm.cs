using UnityEngine;

namespace Genesis.GameClient
{
    public partial class PvpSelectForm : NGUIForm
    {
        private enum PvpType
        {
            OfflinePvp = 0,
            SinglePvp = 1,
            Other = 3,
        }

        [SerializeField]
        private GoodsView[] m_RewardList = null;

        [SerializeField]
        private PvpItem[] m_PvpItemList = null;

        [SerializeField]
        private UIScrollView m_CardScrollView = null;

        [SerializeField]
        private UIGrid m_CardListView = null;

        [SerializeField]
        private UICenterOnChild m_CardCenterOnChild = null;

        [SerializeField]
        private float m_CardScaleFactor = 0.0028f;

        [SerializeField]
        private float m_CardMinScale = 0.6f;

        [SerializeField]
        private float m_CardColorFactor = 0.0028f;

        [SerializeField]
        private Color m_CardLimitColor = new Color32(0xA9, 0xB6, 0xC3, 0xFF);

        private Transform m_CardListViewTransform = null;
        private Transform m_CardScrollViewTransform = null;

        /// <summary>
        /// 当前居中的卡牌。
        /// </summary>
        private GameObject m_CenteredCard = null;

        private int m_CenteredCardEffectId = -1;
        private string m_CenteredCardEffectKey = string.Empty;
        private const string CenteredCardEffectKeyPrefix = "EffectPvpSelect";
        private bool m_HasInitEffect = false;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_HasInitEffect = false;
            m_CardScrollViewTransform = m_CardScrollView.transform;
            m_CardListViewTransform = m_CardListView.transform;
            m_CardCenterOnChild.onCenter = OnCenterOnCard;
            m_CardCenterOnChild.onFinished = OnCardScrollViewSpringPanelFinished;
            m_CardScrollView.onStoppedMoving = OnCardScrollViewStoppedMoving;
            m_CardScrollView.onDragStarted = OnCardScrollViewDragStarted;
            m_CardCenterOnChild.CenterOnImmediately(m_PvpItemList[(int)PvpType.SinglePvp].CachedTransform, m_CardScrollView);
            ShowCenterReward();
        }

        protected override void OnClose(object userData)
        {
            m_CardScrollViewTransform = null;
            m_CardListViewTransform = null;

            m_CardCenterOnChild.onCenter = null;
            m_CardCenterOnChild.onFinished = null;
            m_CardScrollView.onDragStarted = null;
            m_CenteredCard = null;

            base.OnClose(userData);
        }

        protected override void OnResume()
        {
            base.OnResume();
            m_HasInitEffect = false;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            UpdateCardScalesAndColors();

            if (!m_HasInitEffect)
            {
                var key = GetCardEffectKey();
                m_CenteredCardEffectId = m_EffectsController.ShowEffect(key);
                if (m_CenteredCardEffectId > 0)
                {
                    m_HasInitEffect = true;
                    m_CenteredCardEffectKey = key;
                }
            }
        }

        private string GetCardEffectKey()
        {
            return CenteredCardEffectKeyPrefix + ((int)GetPvpType() + 1).ToString();
        }

        private void OnCenterOnCard(GameObject centeredObject)
        {
            m_CenteredCard = centeredObject;

            if (!m_HasInitEffect)
            {
                return;
            }

            var key = GetCardEffectKey();

            if (key == m_CenteredCardEffectKey)
            {
                return;
            }

            if (m_CenteredCardEffectId > 0)
            {
                m_EffectsController.DestroyEffect(m_CenteredCardEffectId);
            }

            m_CenteredCardEffectId = m_EffectsController.ShowEffect(key);
            m_CenteredCardEffectKey = m_CenteredCardEffectId > 0 ? key : string.Empty;
        }

        private void OnCardScrollViewSpringPanelFinished()
        {
        }

        private void OnCardScrollViewDragStarted()
        {
        }

        private void OnCardScrollViewStoppedMoving()
        {
            ShowCenterReward();
        }

        private void UpdateCardScalesAndColors()
        {
            var offset = m_CardScrollViewTransform.localPosition.x + m_CardListViewTransform.localPosition.x;
            for (int i = 0; i < m_PvpItemList.Length; ++i)
            {
                var cardItem = m_PvpItemList[i];
                if (cardItem == null)
                {
                    continue;
                }

                var distance = cardItem.CachedTransform.localPosition.x + offset;
                cardItem.CachedTransform.localScale = GetCardScaleForDistance(distance) * Vector3.one;

                var colorTint = GetCardColorForDistance(distance);
                cardItem.Button.defaultColor = cardItem.Button.disabledColor = cardItem.Button.hover = cardItem.Button.pressed = cardItem.Image.color = cardItem.NameIcon.color = colorTint;
            }
        }

        private float GetCardScaleForDistance(float distance)
        {
            return Mathf.Max(Mathf.Exp(-m_CardScaleFactor * Mathf.Abs(distance)), m_CardMinScale);
        }

        private Color GetCardColorForDistance(float distance)
        {
            var expValue = Mathf.Exp(-m_CardColorFactor * Mathf.Abs(distance));
            return Color.Lerp(m_CardLimitColor, Color.white, expValue);
        }

        public void OnClickPvpItem(int key)
        {
            var currentType = GetPvpType();
            if (key != (int)currentType)
            {
                m_CardCenterOnChild.CenterOn(m_PvpItemList[key].CachedTransform);
                return;
            }

            switch (currentType)
            {
                case PvpType.OfflinePvp:
                    GameEntry.UI.OpenUIForm(UIFormId.ActivityOfflineArenaForm);
                    break;
                case PvpType.SinglePvp:
                    GameEntry.UI.OpenUIForm(UIFormId.ActivitySinglePvpMainForm);
                    break;
                case PvpType.Other:
                default:
                    break;
            }
        }

        private void ShowCenterReward()
        {
            // TODO: 现在所有 PVP 全部没有奖励。
            for (int i = 0; i < m_RewardList.Length; i++)
            {
                m_RewardList[i].gameObject.SetActive(false);
            }
        }

        private PvpType GetPvpType()
        {
            PvpType type = PvpType.SinglePvp;
            for (int i = 0; i < m_PvpItemList.Length; i++)
            {
                if (m_CenteredCard == m_PvpItemList[i].CachedTransform.gameObject)
                {
                    return (PvpType)i;
                }
            }
            return type;
        }
    }
}
