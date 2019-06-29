using GameFramework;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 物品的简单视图。
    /// </summary>
    public class GeneralItemView : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_ItemIcon = null;

        [SerializeField]
        private UISprite m_QualityBg = null;

        [SerializeField]
        private UISprite m_BadgeBg = null;

        [SerializeField]
        private UILabel m_CountLabel = null;

        [SerializeField]
        private bool m_ResizeOnInit = false;

        [SerializeField]
        private Vector2 m_ItemIconSize = new Vector2(114f, 114f);

        [SerializeField]
        private Vector2 m_ItemIconOffset = Vector2.zero;

        [SerializeField]
        private Vector2 m_GenericBadgeIconSize = new Vector2(72f, 78f);

        [SerializeField]
        private Vector2 m_GenericBadgeIconOffset = Vector2.zero;

        [SerializeField]
        private Vector2 m_GenericBadgeBgSize = new Vector2(114f, 114f);

        [SerializeField]
        private Vector2 m_GenericBadgeBgOffset = Vector2.zero;

        [SerializeField]
        private Vector2 m_SpecificBadgeIconSize = new Vector2(60f, 60f);

        [SerializeField]
        private Vector2 m_SpecificBadgeIconOffset = Vector2.zero;

        [SerializeField]
        private Vector2 m_SpecificBadgeBgSize = new Vector2(114f, 114f);

        [SerializeField]
        private Vector2 m_SpecificBadgeBgOffset = Vector2.zero;

        [SerializeField]
        private int m_MaxCountForShow = 999999;

        [SerializeField]
        private bool m_UseDefaultOnClickCallback = true;

        private Scenario m_Scenario = Scenario.GeneralItem;

        private UIWidget[] m_AllWidgets = null;

        private bool m_NeedSetColor = false;

        private Color m_Color = Color.white;

        public UISprite ItemIcon
        {
            get
            {
                return m_ItemIcon;
            }
        }

        public UISprite BadgeBg
        {
            get
            {
                return m_BadgeBg;
            }
        }

        /// <summary>
        /// 数量（用于道具）。
        /// </summary>
        public int Count
        {
            get;
            private set;
        }

        /// <summary>
        /// 物品编号。
        /// </summary>
        public int GeneralItemId
        {
            get;
            private set;
        }

        /// <summary>
        /// 装备编号。
        /// </summary>
        public int GearId
        {
            get;
            private set;
        }

        /// <summary>
        /// 徽章编号。
        /// </summary>
        public int BadgeId
        {
            get;
            private set;
        }

        /// <summary>
        /// 徽章等级。
        /// </summary>
        public int BadgeLevel
        {
            get;
            private set;
        }

        private QualityType m_Quality;
        private GameFrameworkAction m_OnClick;

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

        /// <summary>
        /// 设置颜色。
        /// </summary>
        public void SetColor(Color color)
        {
            if (m_AllWidgets == null || m_AllWidgets.Length == 0)
            {
                m_NeedSetColor = true;
                m_Color = color;
                return;
            }
            m_NeedSetColor = false;
            for (int i = 0; i < m_AllWidgets.Length; i++)
            {
                m_AllWidgets[i].color = color;
            }
        }

        /// <summary>
        /// 初始化道具。
        /// </summary>
        /// <param name="itemId">道具编号。</param>
        /// <param name="count">数量。</param>
        public void InitItem(int itemId, int count = 0)
        {
            QualityType quality = (QualityType)GeneralItemUtility.GetGeneralItemQuality(itemId);
            InitItem(itemId, quality, count);
        }

        public void InitGeneralItem(int itemId, int count = 0)
        {
            var type = GeneralItemUtility.GetGeneralItemType(itemId);
            switch (type)
            {
                case GeneralItemType.Item:
                case GeneralItemType.QualityItem:
                    InitItem(itemId, count);
                    break;
                case GeneralItemType.SkillBadge:
                    InitSkillBadge(itemId, count);
                    break;
            }
        }

        /// <summary>
        /// 初始化道具。
        /// </summary>
        /// <param name="itemId">道具编号。</param>
        /// <param name="quality">品质。</param>
        /// <param name="count">数量。</param>
        public void InitItem(int itemId, QualityType quality, int count = 0)
        {
            m_Scenario = Scenario.GeneralItem;
            Count = count;
            GeneralItemId = itemId;
            Quality = quality;

            if (m_BadgeBg != null) m_BadgeBg.gameObject.SetActive(false);

            if (m_ItemIcon != null)
            {
                m_ItemIcon.gameObject.SetActive(true);
                if (itemId > 0)
                {
                    m_ItemIcon.LoadAsync(GeneralItemUtility.GetGeneralItemIconId(itemId), OnLoadSpriteSuccess);
                }
                else
                {
                    m_ItemIcon.LoadAsync(Constant.EmptyItemIconId, OnLoadSpriteSuccess);
                }

                if (m_ResizeOnInit)
                {
                    ResizeItemIconForGeneralItem();
                }
            }

            UpdateCount();
        }

        private void ResizeItemIconForGeneralItem()
        {
            if (m_ItemIcon == null) return;

            m_ItemIcon.gameObject.SetActive(true);
            m_ItemIcon.width = Mathf.RoundToInt(m_ItemIconSize.x);
            m_ItemIcon.height = Mathf.RoundToInt(m_ItemIconSize.y);
            m_ItemIcon.cachedTransform.SetLocalPositionX(m_ItemIconOffset.x);
            m_ItemIcon.cachedTransform.SetLocalPositionY(m_ItemIconOffset.y);
        }

        private void UpdateCount()
        {
            if (m_CountLabel != null)
            {
                if (Count > 1)
                {
                    int showCount = Mathf.Min(m_MaxCountForShow, Count);
                    m_CountLabel.text = GameEntry.Localization.GetString("UI_TEXT_ITEMNUMBER_WITHOUTPLUS", showCount.ToString());
                }
                else
                {
                    m_CountLabel.text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 初始化装备。
        /// </summary>
        /// <param name="gearId">装备编号。</param>
        /// <param name="quality">品质。</param>
        public void InitNewGear(int gearId, QualityType quality)
        {
            m_Scenario = Scenario.NewGear;
            GearId = gearId;
            Quality = quality;

            if (m_BadgeBg != null) m_BadgeBg.gameObject.SetActive(false);

            if (m_ItemIcon != null)
            {
                m_ItemIcon.gameObject.SetActive(true);
                m_ItemIcon.LoadAsync(NewGearUtility.GetIconId(gearId), OnLoadSpriteSuccess);
            }

            if (m_CountLabel != null)
            {
                m_CountLabel.text = string.Empty;
            }

            if (m_ResizeOnInit)
            {
                ResizeItemIconForGeneralItem();
            }
        }

        /// <summary>
        /// 初始化技能徽章。
        /// </summary>
        /// <param name="badgeId">徽章编号。</param>
        /// <param name="count">数量。</param>
        public void InitSkillBadge(int badgeId, int count = 0)
        {
            m_Scenario = Scenario.SkillBadge;
            GeneralItemId = BadgeId = badgeId;
            Count = count;
            if (m_QualityBg != null)
            {
                m_QualityBg.gameObject.SetActive(false);
            }

            var dr = GeneralItemUtility.GetSkillBadgeDataRow(badgeId);
            if (dr == null)
            {
                Log.Warning("Badge '{0}' not found in data table.", badgeId.ToString());
                return;
            }

            BadgeLevel = dr.Level;

            if (m_ItemIcon != null)
            {
                m_ItemIcon.gameObject.SetActive(true);
                m_ItemIcon.LoadAsync(GeneralItemUtility.GetGeneralItemIconId(badgeId), OnLoadSpriteSuccess);
            }

            var category = GeneralItemUtility.GetSkillBadgeCateogry(badgeId);
            if (m_BadgeBg != null)
            {
                m_BadgeBg.gameObject.SetActive(true);
                m_BadgeBg.spriteName = GeneralItemUtility.GetSkillBadgeBgSpriteName(category, BadgeLevel);
            }

            if (m_ResizeOnInit)
            {
                ResizeItemIconForBadge(category);
                ResizeBadgeBg(category);
            }
            UpdateCount();
        }

        private void ResizeItemIconForBadge(SkillBadgeSlotCategory category)
        {
            if (m_ItemIcon == null) return;

            m_ItemIcon.width = Mathf.RoundToInt((category == SkillBadgeSlotCategory.Specific ? m_SpecificBadgeIconSize : m_GenericBadgeIconSize).x);
            m_ItemIcon.height = Mathf.RoundToInt((category == SkillBadgeSlotCategory.Specific ? m_SpecificBadgeIconSize : m_GenericBadgeIconSize).y);
            m_ItemIcon.cachedTransform.SetLocalPositionX((category == SkillBadgeSlotCategory.Specific ? m_SpecificBadgeIconOffset : m_GenericBadgeIconOffset).x);
            m_ItemIcon.cachedTransform.SetLocalPositionY((category == SkillBadgeSlotCategory.Specific ? m_SpecificBadgeIconOffset : m_GenericBadgeIconOffset).y);
        }

        private void ResizeBadgeBg(SkillBadgeSlotCategory category)
        {
            if (m_BadgeBg == null) return;

            m_BadgeBg.width = Mathf.RoundToInt((category == SkillBadgeSlotCategory.Specific ? m_SpecificBadgeBgSize : m_GenericBadgeBgSize).x);
            m_BadgeBg.height = Mathf.RoundToInt((category == SkillBadgeSlotCategory.Specific ? m_SpecificBadgeBgSize : m_GenericBadgeBgSize).y);
            m_BadgeBg.cachedTransform.SetLocalPositionX((category == SkillBadgeSlotCategory.Specific ? m_SpecificBadgeBgOffset : m_GenericBadgeBgOffset).x);
            m_BadgeBg.cachedTransform.SetLocalPositionY((category == SkillBadgeSlotCategory.Specific ? m_SpecificBadgeBgOffset : m_GenericBadgeBgOffset).y);
        }

        /// <summary>
        /// 设置点击回调。
        /// </summary>
        /// <param name="onClick">回调函数。</param>
        public void SetOnClickDelegate(GameFrameworkAction onClick)
        {
            m_OnClick = onClick;
        }

        /// <summary>
        /// 重置点击回调。
        /// </summary>
        public void ResetOnClickDelegate()
        {
            m_OnClick = OnClick_Default;
        }

        // Called via reflection by NGUI.
        public void OnClick()
        {
            if (m_OnClick == null)
            {
                return;
            }
            m_OnClick();
        }

        private void OnLoadSpriteSuccess(UISprite sprite, string spriteName, object userData)
        {
            var button = sprite.GetComponent<UIButton>();
            if (button != null)
            {
                button.normalSprite = sprite.spriteName;
            }
        }

        private void UpdateQualityBg()
        {
            if (m_QualityBg == null)
            {
                return;
            }

            if (m_Scenario != Scenario.GeneralItem && m_Scenario != Scenario.NewGear)
            {
                m_QualityBg.gameObject.SetActive(false);
            }

            m_QualityBg.gameObject.SetActive(true);
            int index = (int)Quality;
            if (index >= 1 && index <= Constant.Quality.ItemBorderSpriteNames.Length - 1)
            {
                m_QualityBg.spriteName = Constant.Quality.ItemBorderSpriteNames[index];
            }
            else
            {
                m_QualityBg.spriteName = Constant.Quality.ItemBorderSpriteNames[0];
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            if (m_UseDefaultOnClickCallback)
            {
                ResetOnClickDelegate();
            }
        }

        private void Start()
        {
            m_AllWidgets = gameObject.GetComponentsInChildren<UIWidget>();

            if (m_NeedSetColor)
            {
                SetColor(m_Color);
            }
        }

        #endregion MonoBehaviour

        private void OnClick_Default()
        {
            if (GeneralItemId <= 0 || m_Scenario == Scenario.NewGear)
            {
                return;
            }

            GameEntry.UI.OpenUIForm(UIFormId.TipsForm, new TipsFormDisplayData { RefTransform = transform, GeneralItemId = GeneralItemId });
        }

        private enum Scenario
        {
            GeneralItem,
            NewGear,
            SkillBadge,
        }
    }
}
