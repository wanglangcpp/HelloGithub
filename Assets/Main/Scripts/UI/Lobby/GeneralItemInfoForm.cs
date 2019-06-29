using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 物品详情界面。
    /// </summary>
    public class GeneralItemInfoForm : NGUIForm
    {
        [SerializeField]
        private UISprite m_ItemIcon = null;

        [SerializeField]
        private UILabel m_ItemNameLabel = null;

        [SerializeField]
        private UILabel m_ItemTypeLabel = null;

        [SerializeField]
        private UILabel m_ItemLevelLabel = null;

        [SerializeField]
        private StarPanel m_StarPanelWithLevel = null;

        [SerializeField]
        private StarPanel m_StarPanelWithoutLevel = null;

        [SerializeField]
        private GameObject m_AttributeTitleRoot = null;

        [SerializeField]
        private GameObject m_ItemTitleRoot = null;

        [SerializeField]
        private UILabel m_ItemDescriptionLabel = null;

        [SerializeField]
        private AttributeScrollViewCache m_AttributeScrollViewCache = null;

        private GeneralItemInfoDisplayData m_CachedUserData = null;
        private BaseStrategy m_Strategy = null;
        private bool m_FirstTime = true;

        #region NGUIForm

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_CachedUserData = userData as GeneralItemInfoDisplayData;
            if (m_CachedUserData == null)
            {
                Log.Error("User data is invalid.");
                return;
            }

            InitStrategy();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            UpdateStrategy();
        }

        protected override void OnClose(object userData)
        {
            DeinitStrategy();
            base.OnClose(userData);
        }

        #endregion NGUIForm

        private void InitStrategy()
        {
            switch (GeneralItemUtility.GetGeneralItemType(m_CachedUserData.TypeId))
            {
                case GeneralItemType.Gear:
                    m_Strategy = new GearStrategy();
                    break;
                case GeneralItemType.Soul:
                    m_Strategy = new SoulStrategy();
                    break;
                case GeneralItemType.Epigraph:
                    m_Strategy = new EpigraphStrategy();
                    break;
                case GeneralItemType.Item:
                default:
                    m_Strategy = new ItemStrategy();
                    break;
            }

            m_Strategy.Init(this);
        }

        private void DeinitStrategy()
        {
            if (m_Strategy == null)
            {
                return;
            }

            m_Strategy.Shutdown();
            m_Strategy = null;
        }

        private void UpdateStrategy()
        {
            if (m_Strategy != null)
            {
                m_Strategy.OnUpdate();
            }
        }

        private void AddAttribute(string key, string value, int index)
        {
            var script = m_AttributeScrollViewCache.GetOrCreateItem(index);
            script.Name = GameEntry.Localization.GetString(key);
            script.Value = value;
        }

        private void AddAttributePercentage(string key, float value, int index)
        {
            AddAttribute(key, GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", value), index);
        }

        [Serializable]
        private class StarPanel
        {
            public GameObject Root = null;
            public UISprite[] Stars = null;
        }

        [Serializable]
        private class AttributeScrollViewCache : UIScrollViewCache<GearInfoAttributeItem>
        {
            // Empty.
        }

        private abstract class BaseStrategy
        {
            protected GeneralItemInfoForm m_Form = null;
            protected bool m_ShouldResetPosition = false;

            public virtual void Init(GeneralItemInfoForm form)
            {
                m_Form = form;

                m_Form.m_ItemNameLabel.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(m_Form.m_CachedUserData.TypeId));
                m_Form.m_ItemIcon.LoadAsync(GeneralItemUtility.GetGeneralItemIconId(m_Form.m_CachedUserData.TypeId));
            }

            public virtual void Shutdown()
            {
                m_Form = null;
            }

            public virtual void OnUpdate()
            {
                if (m_ShouldResetPosition)
                {
                    m_ShouldResetPosition = false;
                    m_Form.m_AttributeScrollViewCache.Reposition();
                }
            }

            protected void AddAttribute(string key, string value, int index)
            {
                m_Form.AddAttribute(key, value, index);
            }

            protected void AddAttributePercentage(string key, float value, int index)
            {
                m_Form.AddAttributePercentage(key, value, index);
            }

            protected void Reposition()
            {
                if (m_Form.m_FirstTime)
                {
                    m_Form.m_FirstTime = false;
                    m_ShouldResetPosition = true;
                }
                else
                {
                    m_Form.m_AttributeScrollViewCache.Reposition();
                }
            }
        }

        private class ItemStrategy : BaseStrategy
        {
            public override void Init(GeneralItemInfoForm form)
            {
                base.Init(form);
                m_Form.m_ItemLevelLabel.gameObject.SetActive(false);
                m_Form.m_ItemTitleRoot.SetActive(true);
                m_Form.m_AttributeTitleRoot.SetActive(false);
                m_Form.m_StarPanelWithLevel.Root.SetActive(false);
                m_Form.m_StarPanelWithoutLevel.Root.SetActive(false);
                m_Form.m_AttributeScrollViewCache.SetActive(false);

                m_Form.m_ItemDescriptionLabel.text = GetDesc();
                m_Form.m_ItemTypeLabel.text = GetTypeText();
            }

            private string GetDesc()
            {
                var key = GeneralItemUtility.GetGeneralItemDescription(m_Form.m_CachedUserData.TypeId);
                var baseDesc = string.IsNullOrEmpty(key) ? string.Empty : GameEntry.Localization.GetString(key);
                if (m_Form.m_CachedUserData.Qty <= 0 || GeneralItemUtility.GetItemType(m_Form.m_CachedUserData.TypeId) == ItemType.DummyItem)
                {
                    return baseDesc;
                }

                var qtyText = GameEntry.Localization.GetString("UI_TEXT_QTY_FORMAT", m_Form.m_CachedUserData.Qty);
                return GameEntry.Localization.GetString("UI_TEXT_ITEM_DESC_WITH_QTY_FORMAT", baseDesc, qtyText);
            }

            private string GetTypeText()
            {
                var itemType = GeneralItemUtility.GetItemType(m_Form.m_CachedUserData.TypeId);
                string typeTextKey;
                switch (itemType)
                {
                    case ItemType.HeroPieceItem:
                        typeTextKey = "UI_BUTTON_HEROPIECE";
                        break;
                    case ItemType.HeroExpItem:
                    case ItemType.NormalItem:
                    case ItemType.StrengthenItem:
                        typeTextKey = "UI_TEXT_ITEM";
                        break;
                    case ItemType.DummyItem:
                    case ItemType.OtherItem:
                    default:
                        typeTextKey = string.Empty;
                        break;
                }

                return string.IsNullOrEmpty(typeTextKey) ? string.Empty : GameEntry.Localization.GetString(typeTextKey);
            }
        }

        private class GearStrategy : BaseStrategy
        {
            public override void Init(GeneralItemInfoForm form)
            {
                base.Init(form);
                m_Form.m_ItemLevelLabel.gameObject.SetActive(true);
                m_Form.m_ItemTitleRoot.SetActive(false);
                m_Form.m_AttributeTitleRoot.SetActive(true);
                m_Form.m_StarPanelWithLevel.Root.SetActive(true);
                m_Form.m_StarPanelWithoutLevel.Root.SetActive(false);
                m_Form.m_AttributeScrollViewCache.SetActive(true);

                m_Form.m_ItemDescriptionLabel.text = string.Empty;
                m_Form.m_ItemTypeLabel.text = GetTypeText();

                m_Form.m_ItemLevelLabel.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", Mathf.Max(1, m_Form.m_CachedUserData.Level));
                UIUtility.SetStarLevel(m_Form.m_StarPanelWithLevel.Stars, m_Form.m_CachedUserData.StarLevel);

                RefreshAttributes(FakeGearData());
            }

            private string GetTypeText()
            {
                var gearType = GeneralItemUtility.GetGearPosition(m_Form.m_CachedUserData.TypeId);
                string typeTextKey = Constant.Gear.GearTypeNameDics[(int)gearType];
                return string.IsNullOrEmpty(typeTextKey) ? string.Empty : GameEntry.Localization.GetString(typeTextKey);
            }

            private GearData FakeGearData()
            {
                var pbGearInfo = new PBGearInfo
                {
                    Id = 1, // No use
                    Level = m_Form.m_CachedUserData.Level,
                    StrengthenLevel = m_Form.m_CachedUserData.StarLevel,
                    Type = m_Form.m_CachedUserData.TypeId,
                };

                var gearData = new GearData();
                gearData.UpdateData(pbGearInfo);
                return gearData;
            }

            private void RefreshAttributes(GearData gearData)
            {
                var displayer = new GearAttributeDisplayer<GearInfoAttributeItem>(gearData, GearAttributeNewValueMask.Default);
                displayer.GetItemDelegate += GetOrCreateItem;
                displayer.SetNameAndCurrentValueDelegate += SetAttribute;
                int attrCount = displayer.Run();
                m_Form.m_AttributeScrollViewCache.RecycleItemsAtAndAfter(attrCount);
                Reposition();
            }

            private GearInfoAttributeItem GetOrCreateItem(int index)
            {
                return m_Form.m_AttributeScrollViewCache.GetOrCreateItem(index);
            }

            private void SetAttribute(GearInfoAttributeItem script, string name, string value)
            {
                script.Name = name;
                script.Value = value;
            }
        }

        private class SoulStrategy : BaseStrategy
        {
            public override void Init(GeneralItemInfoForm form)
            {
                base.Init(form);

                m_Form.m_ItemLevelLabel.gameObject.SetActive(true);
                m_Form.m_ItemTitleRoot.SetActive(false);
                m_Form.m_AttributeTitleRoot.SetActive(true);
                m_Form.m_StarPanelWithLevel.Root.SetActive(false);
                m_Form.m_StarPanelWithoutLevel.Root.SetActive(true);
                m_Form.m_AttributeScrollViewCache.SetActive(true);

                m_Form.m_ItemDescriptionLabel.text = string.Empty;
                m_Form.m_ItemTypeLabel.text = GameEntry.Localization.GetString("UI_BUTTON_HEROSOUL");
                m_Form.m_ItemLevelLabel.text = string.Empty;

                var soulData = FakeSoulData();
                UIUtility.SetStarLevel(m_Form.m_StarPanelWithoutLevel.Stars, soulData.Quality);
                RefreshAttributes(soulData);
            }

            private void RefreshAttributes(SoulData soulData)
            {
                int index = 0;
                var key = UIUtility.GetSoulAttributeNameKey(soulData.EffectId);
                AddAttribute(key, UIUtility.GetSoulEffectValueText(soulData), index++);
                m_Form.m_AttributeScrollViewCache.RecycleItemsAtAndAfter(index);
                Reposition();
            }

            private SoulData FakeSoulData()
            {
                var pb = new PBSoulInfo
                {
                    Id = 1, // No use.
                    Type = m_Form.m_CachedUserData.TypeId,
                };

                var soulData = new SoulData();
                soulData.UpdateData(pb);
                return soulData;
            }
        }

        private class EpigraphStrategy : BaseStrategy
        {
            public override void Init(GeneralItemInfoForm form)
            {
                m_Form.m_ItemLevelLabel.gameObject.SetActive(true);
                m_Form.m_ItemTitleRoot.SetActive(false);
                m_Form.m_AttributeTitleRoot.SetActive(true);
                m_Form.m_StarPanelWithLevel.Root.SetActive(false);
                m_Form.m_StarPanelWithoutLevel.Root.SetActive(true);
                m_Form.m_AttributeScrollViewCache.SetActive(true);

                m_Form.m_ItemDescriptionLabel.text = string.Empty;
                m_Form.m_ItemTypeLabel.text = string.Empty;
                m_Form.m_ItemLevelLabel.text = string.Empty;

                var data = FakeEpigraphData();
                UIUtility.SetStarLevel(m_Form.m_StarPanelWithoutLevel.Stars, data.DTQuality);
                RefreshAttributes(data);
                base.Init(form);
            }

            private void RefreshAttributes(EpigraphData data)
            {
                int index = 0;
                var key = Constant.AttributeName.AttributeNameDics[data.DTAttributeType];
                AddAttribute(key, data.DTAttributeValue.ToString(), index++);
                m_Form.m_AttributeScrollViewCache.RecycleItemsAtAndAfter(index);
                Reposition();
            }

            private EpigraphData FakeEpigraphData()
            {
                var pb = new PBEpigraphInfo
                {
                    Type = m_Form.m_CachedUserData.TypeId,
                };

                var epigraphData = new EpigraphData();
                epigraphData.UpdateData(pb);
                return epigraphData;
            }
        }
    }
}
