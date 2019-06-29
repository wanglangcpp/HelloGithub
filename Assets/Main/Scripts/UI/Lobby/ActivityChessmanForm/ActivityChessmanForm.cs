using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 活动界面 -- 翻翻棋
    /// </summary>
    public class ActivityChessmanForm : NGUIForm
    {
        private IChessManager m_ChessManager = null;

        [SerializeField]
        private GameObject[] m_ChessFieldTemplates = null;

        [SerializeField]
        private UIScrollView m_ChessBoardScrollView = null;

        [SerializeField]
        private UIGrid m_ChessBoardGrid = null;

        [SerializeField]
        private UISprite m_ChessBoardBg = null;

        [SerializeField]
        private UILabel m_RemainingChanceCount = null;

        [SerializeField]
        private UISprite m_Navigator = null;

        [SerializeField]
        private UIWidget m_NavigatorDelimiter = null;

        [SerializeField]
        private UIButton m_ResetButton = null;

        [SerializeField]
        private UILabel m_ResetButtonText = null;

        [SerializeField]
        private UILabel m_CoinObtained = null;

        [SerializeField]
        private UILabel m_MoneyObtained = null;

        [SerializeField]
        private UILabel m_StarEnergyObtained = null;

        [SerializeField]
        private UIButton m_BombButton = null;

        [SerializeField]
        private UIButton m_QuickOpenButton = null;

        [SerializeField]
        private GameObject m_WinPanelTemplate = null;

        [SerializeField]
        private Transform m_WinPanelParent = null;

        [SerializeField]
        private float m_RewardNumberTweenDuration = 1.5f;

        [SerializeField]
        private GameObject m_ObtainedGoodsTemplate = null;

        [SerializeField]
        private UIGrid m_GoodsListView = null;

        [SerializeField]
        private UIScrollView m_GoodsScrollView = null;

        [SerializeField]
        private float m_BombEffectDuration = .8f;

        private GameObject m_WinPanel = null;

        private List<ChessFieldBaseController> m_ChessFieldControllers = new List<ChessFieldBaseController>();
        private Vector2 m_CachedDragAmount;
        private bool m_ChessBoardScrollViewAvailable = false;
        private bool m_ChessBoardIsResetting = false;
        private bool m_IsPlayingBombingEffect = false;
        private float m_BombEffectTime = 0f;

        private const string CachedDragAmountX = "ActivityChessmanForm.CachedDragAmountX";
        private const string CachedDragAmountY = "ActivityChessmanForm.CachedDragAmountY";

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            LoadCachedDragAmount();
            InitChessManager();
            RefreshResetButton(0);
            RefreshData();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            UpdateNavigator();
            UpdateBombEffect(realElapseSeconds);
        }

        protected override void OnClose(object userData)
        {
            ClearChessBoard();
            DeinitChessManager();
            HideNavigator();
            SaveCachedDragAmount();
            ClearWinPanel();
            m_ChessBoardScrollViewAvailable = false;
            m_ChessBoardIsResetting = false;
            ClearGoods();

            base.OnClose(userData);
        }

        // Called by NGUI via reflection
        public void OnClickHelpButton()
        {
            if (m_IsPlayingBombingEffect)
            {
                return;
            }
        }

        // Called by NGUI via reflection
        public void OnClickBombButton()
        {
            if (m_IsPlayingBombingEffect)
            {
                return;
            }

            int bombCost = CalcBombCost();
            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Mode = 2,
                Message = GameEntry.Localization.GetString("UI_TEXT_CHESS_BOMB_CONFIRM", bombCost.ToString()),
                OnClickConfirm = DoBomb,
                UserData = bombCost,
            });
        }

        // Called by NGUI via reflection
        public void OnClickQuickButton()
        {
            if (m_IsPlayingBombingEffect)
            {
                return;
            }

            m_ChessManager.PerformQuickOpen();
        }

        // Called by NGUI via reflection
        public void OnClickResetButton()
        {
            if (m_IsPlayingBombingEffect)
            {
                return;
            }

            if (m_ChessManager.RemainingResetCount < 0)
            {
                Log.Info("[ActivityChessmanForm OnClickResetButton] You cannot reset the chess board.");
                return;
            }

            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Mode = 2,
                Message = GameEntry.Localization.GetString("UI_TEXT_CHESS_RESET_CONFIRM"),
                OnClickConfirm = DoReset,
            });
        }

        public void OnClickChessField(int index)
        {
            if (m_IsPlayingBombingEffect)
            {
                return;
            }

            var chessFieldData = m_ChessManager[index];
            if (chessFieldData.IsOpened)
            {
                Log.Warning("Chess field already opened at '{0}'.", index);
                return;
            }

            var normalChessField = chessFieldData as NormalChessField;
            var battleChessField = chessFieldData as BattleChessField;

            if (m_ChessManager.RemainingChanceCount <= 0)
            {
                if (battleChessField != null || !normalChessField.IsFree)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_CHESS_CHANGE_RUN_OUT") });
                    return;
                }
            }

            if (battleChessField != null)
            {
                if (!GameEntry.Data.ChessBattleMe.AnyHeroIsAlive)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_CHESS_OUT_OF_HEROES") });
                    return;
                }

                m_ChessManager.GetEnemyData(index);
                return;
            }

            if (normalChessField != null && !normalChessField.IsFree)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_CONFIRM_OPEN_CHESS"),
                    OnClickConfirm = OnConfirmOpenChessField,
                    UserData = index,
                });
                return;
            }

            DoOpenChessField(index);
        }

        private void OnConfirmOpenChessField(object indexObj)
        {
            DoOpenChessField((int)indexObj);
        }

        private int CalcBombCost()
        {
            int costPeriodCount = 5; // GameEntry.ServerConfig.GetInt(Constant.ServerConfig.ChessBombPeriodCount, 5);

            if (costPeriodCount <= 0)
            {
                return 0;
            }

            int[] costPeriods = new int[costPeriodCount];
            int[] costPerField = new int[costPeriodCount];
            for (int i = 0; i < costPeriodCount; ++i)
            {
                costPeriods[i] = (i + 1) * 20; // GameEntry.ServerConfig.GetInt(string.Format(Constant.ServerConfig.ChessBombPeriodFormat, i + 1), (i + 1) * 20);
                costPerField[i] = 1; // GameEntry.ServerConfig.GetInt(string.Format(Constant.ServerConfig.ChessBombCostFormat, i + 1), 1);
            }

            int cost = 0;
            int unopenedGreyFieldCount = 0;
            int currentCostPeriod = 0;
            for (int i = 0; i < m_ChessManager.ChessBoardCount; ++i)
            {
                var chessField = m_ChessManager[i];
                if (chessField.Color != ChessFieldColor.Gray || chessField.IsOpened)
                {
                    continue;
                }

                unopenedGreyFieldCount++;
                if (currentCostPeriod >= costPeriodCount)
                {
                    cost += 0;
                    continue;
                }

                cost += costPerField[currentCostPeriod];
                if (unopenedGreyFieldCount >= costPeriods[currentCostPeriod])
                {
                    ++currentCostPeriod;
                }
            }

            return cost;
        }

        private void DoBomb(object userData)
        {
            int cost = (int)userData;
            if (UIUtility.CheckCurrency(CurrencyType.Money, cost))
            {
                m_ChessManager.Bomb();
            }
        }

        private void DoReset(object userData)
        {
            m_ChessBoardIsResetting = true;
            m_ChessManager.ResetChessBoard();
        }

        private void DoOpenChessField(int index)
        {
            m_ChessManager.OpenNormalChessField(index);
        }

        private void InitChessManager()
        {
            m_ChessManager = GameEntry.ChessManager;
            m_ChessManager.OnChessBoardRefreshSuccess += OnChessBoardRefreshSuccess;
            m_ChessManager.OnChessBoardRefreshFailure += OnChessBoardRefreshFailure;
            m_ChessManager.OnChessFieldOpenSuccess += OnChessFieldOpenSuccess;
            m_ChessManager.OnChessFieldOpenFailure += OnChessFieldOpenFailure;
            m_ChessManager.OnBombSuccess += OnBombSuccess;
            m_ChessManager.OnBombFailure += OnBombFailure;
            m_ChessManager.OnGetEnemyDataSuccess += OnGetEnemyDataSuccess;
            m_ChessManager.OnGetEnemyDataFailure += OnGetEnemyDataFailure;
        }

        private void DeinitChessManager()
        {
            m_ChessManager.OnChessBoardRefreshSuccess -= OnChessBoardRefreshSuccess;
            m_ChessManager.OnChessBoardRefreshFailure -= OnChessBoardRefreshFailure;
            m_ChessManager.OnChessFieldOpenSuccess -= OnChessFieldOpenSuccess;
            m_ChessManager.OnChessFieldOpenFailure -= OnChessFieldOpenFailure;
            m_ChessManager.OnBombSuccess -= OnBombSuccess;
            m_ChessManager.OnBombFailure -= OnBombFailure;
            m_ChessManager.OnGetEnemyDataSuccess -= OnGetEnemyDataSuccess;
            m_ChessManager.OnGetEnemyDataFailure -= OnGetEnemyDataFailure;
            m_ChessManager = null;
        }

        private void RefreshData()
        {
            m_RemainingChanceCount.text = string.Empty;
            m_ChessManager.RefreshChessBoard();
        }

        private void OnChessBoardRefreshSuccess()
        {
            if (m_ChessBoardIsResetting)
            {
                ResetCachedDragAmount();
                m_ChessBoardIsResetting = false;
            }

            CheckAndShowChessBoard();
        }

        private void OnChessBoardRefreshFailure()
        {
            Log.Info("[ActivityChessmanForm OnChessBoardRefreshFailure]");
            ResetCachedDragAmount();
            ShowDataCorruptionDialog();
        }

        private void OnGetEnemyDataSuccess(int chessFieldIndex)
        {
            GameEntry.UI.OpenUIForm(UIFormId.PvpActivityChessmanForm);
        }

        private void OnGetEnemyDataFailure(int chessFieldIndex)
        {
            Log.Info("[ActivityChessmanForm OnGetEnemyDataFailure]");
            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_CHESS_CANNOT_GET_ENEMY_DATA") });
        }

        private void OnChessFieldOpenSuccess(IList<int> indices, ReceivedGeneralItemsViewData receivedGeneralItemsViewData)
        {
            RefreshFieldDatas(indices, true);
            RefreshRewards(true);
            GameEntry.RewardViewer.RequestShowRewards(receivedGeneralItemsViewData, false, delegate (object o) { RefreshMiscDisplay(true); }, null);
        }

        private void OnChessFieldOpenFailure()
        {
            Log.Info("[ActivityChessmanForm OnChessFieldOpenFailure]");
            ResetCachedDragAmount();
            ShowDataCorruptionDialog();
        }

        private void OnBombSuccess(IList<int> indices)
        {
            RefreshFieldDatas(indices, false);
            PlayBombEffect();
        }

        private void PlayBombEffect()
        {
            m_EffectsController.ShowEffect("EffectBombAll");
            m_IsPlayingBombingEffect = true;
            m_BombEffectTime = 0f;
        }

        private void UpdateBombEffect(float realElapseSeconds)
        {
            if (!m_IsPlayingBombingEffect)
            {
                return;
            }

            m_BombEffectTime += realElapseSeconds;
            if (m_BombEffectTime >= m_BombEffectDuration)
            {
                m_BombEffectTime = 0f;
                m_IsPlayingBombingEffect = false;
                RefreshRewards(true);
                RefreshMiscDisplay(true);
            }
        }

        private void OnBombFailure()
        {
            Log.Info("[ActivityChessmanForm OnBombFailure]");
            ResetCachedDragAmount();
            ShowDataCorruptionDialog();
        }

        private void ShowDataCorruptionDialog()
        {
            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Message = GameEntry.Localization.GetString("UI_TEXT_CHESS_DATA_CORRUPT"),
                OnClickConfirm = o => { CloseSelf(); },
            });
        }

        private void RefreshFieldDatas(IList<int> indices, bool checkEffect)
        {
            int openedGrayChessFieldCount = 0;
            int openedGrayChessFieldIndex = -1;

            for (int i = 0; i < indices.Count; ++i)
            {
                var index = indices[i];
                var controller = m_ChessFieldControllers[index];
                controller.RefreshData(m_ChessManager[index]);

                if (m_ChessManager[index].Color == ChessFieldColor.Gray && m_ChessManager[index].IsOpened)
                {
                    ++openedGrayChessFieldCount;
                    openedGrayChessFieldIndex = index;
                }
            }

            if (openedGrayChessFieldCount == 1)
            {
                (m_ChessFieldControllers[openedGrayChessFieldIndex] as ChessFieldGrayController).PlayOpenEffect();
            }
        }

        private void CheckAndShowChessBoard()
        {
            if (this == null || gameObject == null || !gameObject.activeInHierarchy)
            {
                return;
            }

            StartCoroutine(ClearAndShowChessBoardCo());
        }

        private IEnumerator ClearAndShowChessBoardCo()
        {
            GameEntry.Waiting.StartWaiting(WaitingType.Default, GetType().Name);

            HideNavigator();
            m_ChessBoardGrid.Reposition();
            m_ChessBoardScrollView.ResetPosition();
            ClearChessBoard();
            yield return null;
            m_ChessBoardGrid.maxPerLine = m_ChessManager.ChessBoardColCount;

            for (int i = 0; i < m_ChessManager.ChessBoardCount; ++i)
            {
                int row = i / m_ChessManager.ChessBoardColCount;
                int col = i % m_ChessManager.ChessBoardColCount;

                var chessFieldData = m_ChessManager[i];
                var colorId = (int)(chessFieldData.Color);
                var chessFieldView = NGUITools.AddChild(m_ChessBoardGrid.gameObject, m_ChessFieldTemplates[colorId]);
                chessFieldView.gameObject.name = string.Format("Chess Field {0} ({1}, {2})", i, row, col);
                var script = chessFieldView.GetComponent<ChessFieldBaseController>();
                script.Init(i, row);
                script.RefreshData(chessFieldData);
                m_ChessFieldControllers.Add(script);

                var panels = script.GetPanels();
                for (int j = 0; j < panels.Count; ++j)
                {
                    panels[j].depth += Depth - OriginalDepth;
                }
            }

            m_ChessBoardGrid.Reposition();
            m_ChessBoardScrollView.ResetPosition();
            m_ChessBoardScrollView.SetDragAmount(m_CachedDragAmount.x, m_CachedDragAmount.y, false);
            m_ChessBoardScrollViewAvailable = true;

            RefreshResetButton(m_ChessManager.RemainingResetCount);
            InitAndShowNavigator();
            RefreshRewards(false);
            RefreshMiscDisplay(false);
            GameEntry.Waiting.StopWaiting(WaitingType.Default, GetType().Name);
            yield break;
        }

        private void ClearChessBoard()
        {
            m_ChessBoardScrollViewAvailable = false;
            for (int i = 0; i < m_ChessFieldControllers.Count; ++i)
            {
                Destroy(m_ChessFieldControllers[i].gameObject);
            }

            m_ChessFieldControllers.Clear();
        }

        private void RefreshRewards(bool animated)
        {
            float duration = animated ? m_RewardNumberTweenDuration : 0f;
            if (m_StarEnergyObtained.gameObject.activeInHierarchy)
            {
                TweenNumber.Begin(m_StarEnergyObtained.gameObject, duration, m_ChessManager.StarEnergyObtained);
            }

            TweenNumber.Begin(m_MoneyObtained.gameObject, duration, m_ChessManager.MoneyObtained);
            TweenNumber.Begin(m_CoinObtained.gameObject, duration, m_ChessManager.CoinObtained);

            StartCoroutine(RefreshRewardsCo());
        }

        private IEnumerator RefreshRewardsCo()
        {
            if (ClearGoods() > 0)
            {
                yield return null;
            }

            var goods = new List<KeyValuePair<int, int>>(m_ChessManager.GoodsObtained);
            goods.Sort(delegate (KeyValuePair<int, int> a, KeyValuePair<int, int> b) { return CompareGoods(a.Key, b.Key); });

            for (int i = 0; i < goods.Count; ++i)
            {
                var go = NGUITools.AddChild(m_GoodsListView.gameObject, m_ObtainedGoodsTemplate);
                var script = go.GetComponent<ChessBoardObtainedGoods>();
                var qty = goods[i].Value;
                script.Refresh(goods[i].Key, qty);
            }

            m_GoodsListView.Reposition();
            m_GoodsScrollView.ResetPosition();
            yield break;
        }

        private int CompareGoods(int a, int b)
        {
            int aQuality = GeneralItemUtility.GetGeneralItemQuality(a);
            int bQuality = GeneralItemUtility.GetGeneralItemQuality(b);

            if (aQuality != bQuality)
            {
                return bQuality.CompareTo(aQuality);
            }

            GeneralItemType aType = GeneralItemUtility.GetGeneralItemType(a);
            GeneralItemType bType = GeneralItemUtility.GetGeneralItemType(b);

            if (aType != bType)
            {
                var typePriority = new Dictionary<GeneralItemType, int>
                {
                    { GeneralItemType.Gear, 0 },
                    { GeneralItemType.Soul, 1 },
                    { GeneralItemType.Epigraph, 2 },
                    { GeneralItemType.Item, 3 },
                };

                return typePriority[aType].CompareTo(typePriority[bType]);
            }

            return b.CompareTo(a);
        }

        private int ClearGoods()
        {
            var removedCount = 0;
            var existingChildren = m_GoodsListView.GetChildList().ToArray();
            for (int i = 0; i < existingChildren.Length; ++i)
            {
                Destroy(existingChildren[i].gameObject);
                ++removedCount;
            }

            return removedCount;
        }

        private void RefreshMiscDisplay(bool showCompleteEffect)
        {
            m_RemainingChanceCount.text = m_ChessManager.RemainingChanceCount.ToString();
            bool complete = m_ChessManager.IsComplete;
            m_QuickOpenButton.isEnabled = m_BombButton.isEnabled = !complete;

            if (!complete)
            {
                ClearWinPanel();
                return;
            }

            if (m_WinPanel == null)
            {
                m_WinPanel = NGUITools.AddChild(m_WinPanelParent.gameObject, m_WinPanelTemplate);
                var panel = m_WinPanel.GetComponent<UIPanel>();
                panel.depth += Depth - OriginalDepth;
            }

            if (showCompleteEffect)
            {
                m_WinPanel.GetComponent<ChessBoardWinPanel>().ShowWinEffect();
            }
        }

        private void ClearWinPanel()
        {
            if (m_WinPanel != null)
            {
                Destroy(m_WinPanel);
                m_WinPanel = null;
            }
        }

        private void HideNavigator()
        {
            m_Navigator.gameObject.SetActive(false);
        }

        private void InitAndShowNavigator()
        {
            m_Navigator.height = Mathf.CeilToInt(m_NavigatorDelimiter.height * m_ChessBoardScrollView.panel.height / m_ChessBoardBg.height);
            m_Navigator.width = Mathf.CeilToInt(m_NavigatorDelimiter.width * m_ChessBoardScrollView.panel.width / m_ChessBoardBg.width);
            UpdateNavigator();
            m_Navigator.gameObject.SetActive(true);
        }

        private void UpdateNavigator()
        {
            Vector2 dragAmount = m_ChessBoardScrollView.GetDragAmount();
            dragAmount = new Vector2(Mathf.Clamp01(dragAmount.x), Mathf.Clamp01(dragAmount.y));

            if (m_ChessBoardScrollViewAvailable)
            {
                m_CachedDragAmount = dragAmount;
            }

            m_Navigator.cachedTransform.localPosition = new Vector3(
                (dragAmount.x - .5f) * (m_NavigatorDelimiter.width - m_Navigator.width),
                (.5f - dragAmount.y) * (m_NavigatorDelimiter.height - m_Navigator.height),
                m_Navigator.cachedTransform.localPosition.z);
        }

        private void RefreshResetButton(int remaingingResetCount)
        {
            m_ResetButton.isEnabled = remaingingResetCount > 0;
            m_ResetButtonText.text = GameEntry.Localization.GetString("UI_BUTTON_RESET_WITH_TIMES", remaingingResetCount);
        }

        private void LoadCachedDragAmount()
        {
            m_CachedDragAmount = new Vector2(
                GameEntry.Setting.GetFloat(CachedDragAmountX, 0f),
                GameEntry.Setting.GetFloat(CachedDragAmountY, 0f));
        }

        private void SaveCachedDragAmount()
        {
            GameEntry.Setting.SetFloat(CachedDragAmountX, m_CachedDragAmount.x);
            GameEntry.Setting.SetFloat(CachedDragAmountY, m_CachedDragAmount.y);
            GameEntry.Setting.Save();
        }

        private void ResetCachedDragAmount()
        {
            m_CachedDragAmount = Vector2.zero;
            GameEntry.Setting.RemoveKey(CachedDragAmountX);
            GameEntry.Setting.RemoveKey(CachedDragAmountY);
            GameEntry.Setting.Save();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                SaveCachedDragAmount();
            }
        }
    }
}
