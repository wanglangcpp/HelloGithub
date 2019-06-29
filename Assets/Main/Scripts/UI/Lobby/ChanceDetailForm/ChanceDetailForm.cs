using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 抽奖详情界面。
    /// </summary>
    public partial class ChanceDetailForm : NGUIForm
    {
        private const int HeroItemFunctionId = 99;

        private const string OpenTimeKey = "ChanceDetailForm.OpenTime";
        private const string OpenTimeValueFormat = "{0}_{1}_{2}";

        [SerializeField]
        private UILabel m_NextRefreshTime = null;

        [SerializeField]
        private GameObject m_PreviewItemTemplate = null;

        [SerializeField]
        private Transform m_RefreshAnimationParent = null;

        [SerializeField]
        private Animation m_RefreshAnimationComponent = null;

        [SerializeField]
        private Transform[] m_PreviewItemParents = null;

        [SerializeField]
        private UILabel m_BuyOneCost = null;

        [SerializeField]
        private UILabel m_RefreshCost = null;

        [SerializeField]
        private UILabel m_BuyAllCost = null;

        [SerializeField]
        private UIScrollView m_CardScrollView = null;

        [SerializeField]
        private UIGrid m_CardListView = null;

        [SerializeField]
        private UICenterOnChild m_CardCenterOnChild = null;

        [SerializeField]
        private CardScrollViewCache m_CardScrollViewCache = null;

        [SerializeField]
        private UISprite[] m_CoinIcons = null;

        [SerializeField]
        private UISprite[] m_MoneyIcons = null;

        [SerializeField]
        private GameObject m_CoinCardTemplate = null;

        [SerializeField]
        private GameObject m_MoneyCardTemplate = null;

        [SerializeField]
        private Transform m_AnimatedCardsParent = null;

        [SerializeField]
        private float m_CardScaleFactor = 1f;

        [SerializeField]
        private float m_CardMinScale = .7f;

        [SerializeField]
        private float m_CardAlphaFactor = 1f;

        [SerializeField]
        private float m_CardMaxAlpha = .3f;

        [SerializeField]
        private int m_AnimatedCardCount = 4;

        [SerializeField]
        private float m_AnimatedCardTimeInterval = 2f / 30f;

        [SerializeField]
        private float m_AnimatedCardMovingTime = 5f / 30f;

        [SerializeField]
        private int m_AnimatedPreviewItemDepthChange = 10;

        [SerializeField]
        private int m_AnimatedCardItemDepthChange = 20;

        [SerializeField]
        private float m_NumberTweenDuration = .5f;

        private IDataTable<DRChanceRefresh> m_ChanceRefresh = null;

        private Transform m_CardListViewTransform = null;
        private Transform m_CardScrollViewTransform = null;

        /// <summary>
        /// 预览图标。
        /// </summary>
        private List<ChanceDetailPreviewItem> m_PreviewItems = new List<ChanceDetailPreviewItem>();

        /// <summary>
        /// 卡牌。
        /// </summary>
        private List<ChanceDetailCardItem> m_CardItems = new List<ChanceDetailCardItem>();

        /// <summary>
        /// 用于动画的卡牌副本。
        /// </summary>
        private List<ChanceDetailCardItem> m_AnimatedCards = new List<ChanceDetailCardItem>();

        private ChanceType m_CurrentChanceType = ChanceType.Coin;
        private ChanceData m_CurrentChanceData = null;

        /// <summary>
        /// 是否可以免费抽奖。
        /// </summary>
        private bool m_CanUseFree = false;

        /// <summary>
        /// 当前居中的卡牌。
        /// </summary>
        private ChanceDetailCardItem m_CenteredCard = null;

        /// <summary>
        /// 单次抽奖价格变化使用的 <see cref="Genesis.GameClient.TweenNumber"/> 对象。
        /// </summary>
        private TweenNumber m_BuyOneCostTweenNumber = null;

        /// <summary>
        /// 上次更新单次抽奖花费时是否显示为免费。
        /// </summary>
        private bool m_LastBuyOneCostIsFree = false;

        private StrategyBase m_Strategy = null;
        private IFsm<ChanceDetailForm> m_Fsm = null;
        private IDataTable<DRItem> m_DTItem = null;
        private bool m_ScrollViewIsMoving = false;

        private static readonly List<ServerErrorCode> s_BuyFailureErrorCodes = new List<ServerErrorCode>
        {

        };

        #region Async loading related

        private HashSet<int> m_LoadingPreviewItemTaskIds = new HashSet<int>();
        private int m_LoadingPreviewItemTaskSerialId = 0;
        private HashSet<int> m_LoadingCardItemTaskIds = new HashSet<int>();
        private int m_LoadingCardItemTaskSerialId = 0;

        #endregion Async loading related

        #region NGUIForm

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_DTItem = GameEntry.DataTable.GetDataTable<DRItem>();
            m_BuyOneCostTweenNumber = m_BuyOneCost.GetComponent<TweenNumber>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ChanceRefresh = GameEntry.DataTable.GetDataTable<DRChanceRefresh>();
            GameEntry.Event.Subscribe(EventId.ChanceDataChanged, OnChanceDataChanged);

            var myUserData = userData as ChanceDetailDisplayData;

            if (myUserData == null)
            {
                Log.Error("User data is invalid.");
                return;
            }

            m_CurrentChanceType = myUserData.ChanceType;
            m_Strategy = CreateStrategy(m_CurrentChanceType);
            m_Strategy.Init(this);
            m_LoadingPreviewItemTaskIds.Clear();
            m_CardScrollViewTransform = m_CardScrollView.transform;
            m_CardListViewTransform = m_CardListView.transform;
            m_CardCenterOnChild.onCenter = OnCenterOnCard;
            m_CardCenterOnChild.onFinished = OnCardScrollViewSpringPanelFinished;
            m_CardScrollView.onStoppedMoving = OnCardScrollViewStoppedMoving;
            m_CardScrollView.onDragStarted = OnCardScrollViewDragStarted;
            m_CenteredCard = null;
            m_LastBuyOneCostIsFree = false;
            m_ScrollViewIsMoving = false;

            m_Fsm = GameEntry.Fsm.CreateFsm(this,
                new StateInit(),
                new StateBuyAll(),
                new StateBuyOne(),
                new StateManualRefresh(),
                new StateNewCardsLayingOut(),
                new StateNewContentsFadingIn(),
                new StateNewContentsSpreading(),
                new StateNormal(),
                new StateOldContentsFadingOut(),
                new StateRequestData());
            m_Fsm.Start<StateInit>();
        }

        protected override void OnPostOpen(object data)
        {
            base.OnPostOpen(data);
            (m_Fsm.CurrentState as StateBase).OnPostOpen();
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Fsm.DestroyFsm(m_Fsm);

            DestroyPreviewItems();
            DestroyCardItems();
            DestroyAnimatedCards();
            m_LoadingPreviewItemTaskIds.Clear();
            m_LoadingCardItemTaskIds.Clear();
            m_CardScrollViewTransform = null;
            m_CardListViewTransform = null;

            m_CardCenterOnChild.onCenter = null;
            m_CardCenterOnChild.onFinished = null;
            m_CardScrollView.onDragStarted = null;
            m_CenteredCard = null;
            m_Strategy = null;

            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.ChanceDataChanged, OnChanceDataChanged);
            }

            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        #endregion NGUIForm

        public void OnClickRefreshButton()
        {
            (m_Fsm.CurrentState as StateBase).OnClickRefreshButton(m_Fsm);
        }

        public void OnClickBuyAllButton()
        {
            (m_Fsm.CurrentState as StateBase).OnClickBuyAllButton(m_Fsm);
        }

        private void OnClickOpenChanceButton(ChanceDetailCardItem cardScript)
        {
            int index = cardScript.IntKey.Key;
            (m_Fsm.CurrentState as StateBase).OnClickOpenChanceButton(m_Fsm, index);
        }

        private float GetCardAlphaForDistance(float distance)
        {
            return Mathf.Min(1f - Mathf.Exp(-m_CardAlphaFactor * Mathf.Abs(distance)), m_CardMaxAlpha);
        }

        private float GetCardScaleForDistance(float distance)
        {
            return Mathf.Max(Mathf.Exp(-m_CardScaleFactor * Mathf.Abs(distance)), m_CardMinScale);
        }

        private void OnOpenInternal()
        {
            m_CardScrollViewCache.ItemTemplate = CardTemplate;
            m_CardScrollViewCache.SetActive(false);
            m_CardScrollView.enabled = false;

            m_CurrentChanceData = GameEntry.Data.Chances.GetChanceData(m_CurrentChanceType);
            m_CanUseFree = false;

            m_Strategy.InitCurrencyIcons();

            m_RefreshCost.text = GameEntry.Localization.GetString("UI_TEXT_INTEGER", CostForManualRefresh);
            RefreshCost(false);
        }

        private int CostForManualRefresh
        {
            get
            {
                if (!GameEntry.IsAvailable)
                {
                    return 0;
                }

                var chanceRefresh = m_ChanceRefresh.GetDataRow((int)m_CurrentChanceType);
                return chanceRefresh.RefreshCostCurrencyPrice;
            }
        }

        private void UpdateNextFreeTime()
        {
            var chanceRefresh = m_ChanceRefresh.GetDataRow((int)m_CurrentChanceType);
            bool isFree = false;
            if (m_CurrentChanceType == ChanceType.Coin)
            {
                if (GameEntry.Data.Chances.GetChanceData(ChanceType.Coin).FreeChancedTimes < chanceRefresh.GiveFreeCount)
                {
                    DateTime nextFreeTime = m_CurrentChanceData.FreeTime.AddSeconds(chanceRefresh.UseFreeInterval);
                    isFree = nextFreeTime - GameEntry.Time.LobbyServerUtcTime <= TimeSpan.Zero;
                }
                else
                {
                    DateTime nextFreeTime = m_CurrentChanceData.FreeTime.AddSeconds(chanceRefresh.GiveFreeInterval);
                    isFree = nextFreeTime - GameEntry.Time.LobbyServerUtcTime <= TimeSpan.Zero;
                }
            }
            else
            {
                DateTime nextFreeTime = m_CurrentChanceData.FreeTime.AddSeconds(chanceRefresh.GiveFreeInterval);
                isFree = nextFreeTime - GameEntry.Time.LobbyServerUtcTime <= TimeSpan.Zero;
            }
            if (m_CanUseFree != isFree)
            {
                m_CanUseFree = isFree;
                RefreshCost();
            }
        }

        private void UpdateNextRefreshTime()
        {
            m_NextRefreshTime.text = GameEntry.Localization.GetString("UI_TEXT_FEFRESH_TIME", UIUtility.GetTimeSpanStringHms(TimeUtility.GetNearestUtcTime(m_CurrentChanceData.NextRefreshTime) - GameEntry.Time.LobbyServerUtcTime));
        }

        private void UpdateCardScalesAndAlphas()
        {
            var offset = m_CardScrollViewTransform.localPosition.x + m_CardListViewTransform.localPosition.x;
            for (int i = 0; i < m_CardItems.Count; ++i)
            {
                var cardItem = m_CardItems[i];
                if (cardItem == null)
                {
                    continue;
                }

                var distance = cardItem.CachedTransform.localPosition.x + offset;
                cardItem.MaskAlpha = GetCardAlphaForDistance(distance);
                cardItem.CachedTransform.localScale = GetCardScaleForDistance(distance) * Vector3.one;
            }
        }

        private void RefreshCost(bool shouldAnimate = true)
        {
            TweenNumber.Begin(m_BuyAllCost.gameObject, shouldAnimate ? m_NumberTweenDuration : 0f, BuyAllCost);

            if (m_CanUseFree)
            {
                m_BuyOneCostTweenNumber.enabled = false;
                m_BuyOneCost.text = GameEntry.Localization.GetString("UI_BUTTON_FREEGIVE");
                m_LastBuyOneCostIsFree = true;
            }
            else
            {
                m_BuyOneCostTweenNumber.enabled = true;
                if (m_LastBuyOneCostIsFree)
                {
                    TweenNumber.Begin(m_BuyOneCost.gameObject, 0f, BuyOneCost);
                    m_LastBuyOneCostIsFree = false;
                }
                else
                {
                    TweenNumber.Begin(m_BuyOneCost.gameObject, shouldAnimate ? m_NumberTweenDuration : 0f, BuyOneCost);
                }
            }
        }

        private int BuyAllCost
        {
            get
            {
                return m_Strategy.BuyAllCost;
            }
        }

        private int BuyOneCost
        {
            get
            {
                return m_Strategy.BuyOneCost;
            }
        }

        private string SpendCurrencyNoticeKey
        {
            get
            {
                return m_Strategy.SpendCurrencyNoticeKey;
            }
        }

        private CurrencyType CurrencyType
        {
            get
            {
                return m_Strategy.CurrencyType;
            }
        }

        private GameObject CardTemplate
        {
            get
            {
                return m_Strategy.CardTemplate;
            }
        }

        private void CreatePreviewItems()
        {
            for (int i = 0; i < m_PreviewItemParents.Length; ++i)
            {
                var parent = m_PreviewItemParents[i];
                var go = NGUITools.AddChild(parent.gameObject, m_PreviewItemTemplate);
                m_PreviewItems.Add(go.GetComponent<ChanceDetailPreviewItem>());
                var panels = m_PreviewItems[i].GetComponentsInChildren<UIPanel>(true);
                for (int j = 0; j < panels.Length; ++j)
                {
                    panels[j].depth += Depth - OriginalDepth;
                }
            }
        }

        private void CreateCardItems()
        {
            for (int i = 0; i < m_PreviewItemParents.Length; ++i)
            {
                var cardScript = m_CardScrollViewCache.GetOrCreateItem(i, (go) => { go.name = go.name + i; }, (go) => { go.name = go.name + i; });
                cardScript.IntKey.Key = i;
                cardScript.Button.isEnabled = true;
                var eventDelegate = new EventDelegate(this, "OnClickOpenChanceButton");
                eventDelegate.parameters[0].value = cardScript;
                cardScript.Button.onClick.Add(eventDelegate);
                m_CardItems.Add(cardScript);
            }
        }

        private void DestroyCardItems()
        {
            m_CardScrollViewCache.DestroyAllItems();
            m_CardItems.Clear();
        }

        private void DestroyAnimatedCards()
        {
            for (int i = 0; i < m_AnimatedCards.Count; ++i)
            {
                if (m_AnimatedCards[i] != null)
                {
                    Destroy(m_AnimatedCards[i].gameObject);
                }
            }

            m_AnimatedCards.Clear();
        }

        private void DestroyPreviewItems()
        {
            for (int i = 0; i < m_PreviewItems.Count; ++i)
            {
                if (m_PreviewItems[i] != null)
                {
                    Destroy(m_PreviewItems[i].gameObject);
                }
            }

            m_PreviewItems.Clear();
        }

        private void RefreshPreviewItems()
        {
            var viewItems = m_CurrentChanceData.GoodsForView.Data;

            if (m_CurrentChanceData.DummyIndex.Count == Constant.MaxChancedCardCount)
            {
                for (int i = 0; i < m_CurrentChanceData.DummyIndex.Count; i++)
                {
                    var previewItem = m_PreviewItems[i];
                    previewItem.RefreshItemBgGet(m_CurrentChanceData.DummyIndex[i]);
                }
            }

            for (int i = 0; i < m_PreviewItems.Count; i++)
            {
                var previewItem = m_PreviewItems[i];

                if (i >= viewItems.Count)
                {
                    previewItem.gameObject.SetActive(false);
                    continue;
                }

                var itemData = viewItems[i];
                previewItem.gameObject.SetActive(true);

                int taskSerialId = m_LoadingPreviewItemTaskSerialId++;
                m_LoadingPreviewItemTaskIds.Add(taskSerialId);

                previewItem.RefreshAsGeneralItem(itemData.Type, itemData.Count, OnPreviewItemLoadSuccess, OnPreviewItemLoadFailure, OnPreviewItemLoadAbort, taskSerialId);
            }
        }

        private void RefreshCardItems()
        {
            List<int> openedIndeces = m_CurrentChanceData.DummyIndex;
            List<ItemData> openedItems = m_CurrentChanceData.GoodsForView.Data;
            for (int i = 0; i < m_CardItems.Count; ++i)
            {
                var cardItem = m_CardItems[i];
                int index = openedIndeces.IndexOf(i);

                if (index < 0)
                {
                    cardItem.Back.SetActive(true);
                    cardItem.Front.SetActive(false);
                    continue;
                }

                cardItem.Back.SetActive(false);
                cardItem.Front.SetActive(true);

                ItemData itemData = openedItems[index];
                LoadCardItemWithData(cardItem, itemData);
            }
        }

        private void LoadCardItemWithData(ChanceDetailCardItem cardItem, ItemData itemData)
        {
            int taskSerialId = m_LoadingCardItemTaskSerialId++;
            int heroId = 0;
            if (IsHeroItem(itemData.Type, out heroId))
            {
                cardItem.RefreshAsHero(heroId, OnCardItemLoadSuccessForHero, OnCardItemLoadFailureForHero, OnCardItemLoadAbortForHero, taskSerialId);
            }
            else
            {
                cardItem.RefreshAsGeneralItem(itemData.Type, itemData.Count, OnCardItemLoadSuccess, OnCardItemLoadFailure, OnCardItemLoadAbort, taskSerialId);
            }
        }

        private void OnPreviewHeroLoadSuccess(UITexture sprite, object userData)
        {
            OnPreviewItemLoadCommon((int)userData);
        }

        private void OnPreviewItemLoadSuccess(UISprite sprite, string spriteName, object userData)
        {
            OnPreviewItemLoadCommon((int)userData);
        }

        private void OnPreviewHeroLoadFailure(UITexture sprite, object userData)
        {
            if (sprite != null)
            {
                Log.Warning("Cannot load sprite '{0}' in Hero item.", sprite.name);
            }

            OnPreviewItemLoadCommon((int)userData);
        }

        private void OnPreviewItemLoadFailure(UISprite sprite, object userData)
        {
            if (sprite != null)
            {
                Log.Warning("Cannot load sprite '{0}' in preview item.", sprite.name);
            }

            OnPreviewItemLoadCommon((int)userData);
        }

        private void OnPreviewHeroLoadAbort(UITexture sprite, object userData)
        {
            OnPreviewItemLoadCommon((int)userData);
        }

        private void OnPreviewItemLoadAbort(UISprite sprite, object userData)
        {
            OnPreviewItemLoadCommon((int)userData);
        }

        private void OnPreviewItemLoadCommon(int taskSerialId)
        {
            m_LoadingPreviewItemTaskIds.Remove(taskSerialId);
        }

        private void OnCardItemLoadSuccessForHero(UITexture texture, object userData)
        {
            OnCardItemLoadCommon((int)userData);
        }

        private void OnCardItemLoadFailureForHero(UITexture texture, object userData)
        {
            if (texture != null)
            {
                Log.Warning("Cannot load texture '{0}' for hero in card item.", texture.name);
            }

            OnCardItemLoadCommon((int)userData);
        }

        private void OnCardItemLoadAbortForHero(UITexture texture, object userData)
        {
            OnCardItemLoadCommon((int)userData);
        }

        private void OnCardItemLoadSuccess(UISprite sprite, string spriteName, object userData)
        {
            OnCardItemLoadCommon((int)userData);
        }

        private void OnCardItemLoadFailure(UISprite sprite, object userData)
        {
            OnCardItemLoadCommon((int)userData);
        }

        private void OnCardItemLoadAbort(UISprite sprite, object userData)
        {
            OnCardItemLoadCommon((int)userData);
        }

        private void OnCardItemLoadCommon(int taskSerialId)
        {
            m_LoadingPreviewItemTaskIds.Remove(taskSerialId);
        }

        private void OnCardScrollViewDragStarted()
        {
            //Log.Info("[ChanceDetailForm OnCardScrollViewDragStarted]");
            m_ScrollViewIsMoving = true;
            (m_Fsm.CurrentState as StateBase).OnCardScrollViewDragStarted(m_Fsm);
        }

        private void OnCardScrollViewStoppedMoving()
        {
            //Log.Info("[ChanceDetailForm OnCardScrollViewStoppedMoving]");
            m_ScrollViewIsMoving = false;
            (m_Fsm.CurrentState as StateBase).OnCardScrollViewStoppedMoving(m_Fsm);
        }

        private void OnCenterOnCard(GameObject centeredObject)
        {
            //Log.Info("[ChanceDetailForm OnCenterOnCard]");
            m_ScrollViewIsMoving = true;
            m_CenteredCard = centeredObject ? centeredObject.GetComponent<ChanceDetailCardItem>() : null;
            (m_Fsm.CurrentState as StateBase).OnCenterOnCard(m_Fsm);
        }

        private void OnCardScrollViewSpringPanelFinished()
        {
            //Log.Info("[ChanceDetailForm OnSpringPanelFinished]");
            m_ScrollViewIsMoving = false;
            (m_Fsm.CurrentState as StateBase).OnCardScrollViewSpringPanelFinished(m_Fsm);
        }

        private bool IsLoadingPreviewItems
        {
            get
            {
                return m_LoadingPreviewItemTaskIds.Count > 0;
            }
        }

        private bool IsLoadingCardItems
        {
            get
            {
                return m_LoadingCardItemTaskIds.Count > 0;
            }
        }

        private bool HasOpenedThisFormToday
        {
            get
            {
                if (!GameEntry.IsAvailable || GameEntry.OfflineMode.OfflineModeEnabled || !GameEntry.Data.Account.IsAvailable)
                {
                    return false;
                }

                string val = GameEntry.Setting.GetString(OpenTimeKey);
                if (string.IsNullOrEmpty(val))
                {
                    return false;
                }

                string[] splits = val.Split('_');
                if (splits.Length != 3)
                {
                    return false;
                }

                int serverId;
                if (!int.TryParse(splits[0], out serverId))
                {
                    return false;
                }

                int playerId;
                if (!int.TryParse(splits[1], out playerId))
                {
                    return false;
                }

                long openTimeTick;
                if (!long.TryParse(splits[2], out openTimeTick))
                {
                    return false;
                }

                var dataCom = GameEntry.Data;
                return (dataCom.Account.ServerData.Id == serverId && dataCom.Player.Id == playerId && GameEntry.Time.ClientTime.Date <= new DateTime(openTimeTick).Date);
            }
        }

        private void UpdateOpenTime()
        {
            if (!GameEntry.IsAvailable || GameEntry.OfflineMode.OfflineModeEnabled || !GameEntry.Data.Account.IsAvailable)
            {
                return;
            }

            var dataCom = GameEntry.Data;
            GameEntry.Setting.SetString(OpenTimeKey, string.Format(OpenTimeValueFormat, dataCom.Account.ServerData.Id.ToString(),
                dataCom.Player.Id.ToString(), GameEntry.Time.ClientTime.Ticks.ToString()));
            GameEntry.Setting.Save();
        }

        private bool HasBoughtAny
        {
            get
            {
                return m_CurrentChanceData.DummyIndex.Count > 0;
            }
        }

        private void OnChanceDataChanged(object sender, GameEventArgs e)
        {
            var ne = e as ChanceDataChangedEventArgs;

            if (ne.ChanceType != (int)(m_CurrentChanceType))
            {
                return;
            }
            RefreshPreviewItems();
            (m_Fsm.CurrentState as StateBase).OnChanceDataChanged(ne);
        }

        private bool IsHeroItem(int itemId, out int heroId)
        {
            DRItem dr = m_DTItem.GetDataRow(itemId);
            if (dr != null && dr.FunctionId == HeroItemFunctionId)
            {
                heroId = int.Parse(dr.FunctionParams);
                return true;
            }

            heroId = 0;
            return false;
        }

        [Serializable]
        private class CardScrollViewCache : UIScrollViewCache<ChanceDetailCardItem>
        {

        }
    }
}
