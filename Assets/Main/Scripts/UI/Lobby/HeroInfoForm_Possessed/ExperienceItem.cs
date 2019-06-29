using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ExperienceItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_SelectIcon = null;

        [SerializeField]
        private GeneralItemView m_ItemView = null;

        [SerializeField]
        private UILabel m_ExpPlus = null;

        [SerializeField]
        private UIEffectsController m_PressEffect = null;

        private readonly string PressEffectName = "PressEffect";

        private bool m_IsPressed = false;
        private float m_CurPressedTime = 0.0f;
        private const float MinUseExpTimeSpan = 0.1f;
        private const float MaxUseExpTimeSpan = 0.5f;
        private const int PerDescendingUseExpTims = 5;
        private const float UseExpDescendingTime = 0.1f;
        private int m_CurUseExpTimes = 0;
        private float m_UseExpTimeSpan = 0.5f;

        private BaseLobbyHeroData m_HeroData = null;
        private DRItem m_Item = null;
        private bool m_CanUseItem = true;

        public bool CanUseItem { get { return gameObject.activeSelf && m_CanUseItem; } }

        private bool IsUsingExp { get; set; }

        private void Start()
        {
            m_PressEffect.Resume();
        }

        private void OnEnable()
        {
            m_SelectIcon.SetActive(false);
            IsUsingExp = false;
            UIEventListener.Get(gameObject).onPress += OnExpItemPress;
        }

        public void RefreshAndUseItemFinished()
        {
            if (IsUsingExp)
            {
                GameEntry.Waiting.StartWaiting(WaitingType.Default, "ExperienceItem");
            }

            IsUsingExp = false;
            RefreshData(m_Item, m_HeroData);
        }

        private void OnDisable()
        {
            UIEventListener.Get(gameObject).onPress -= OnExpItemPress;
        }

        public void RefreshData(DRItem item, BaseLobbyHeroData heroData)
        {
            m_Item = item;
            var itemData = GameEntry.Data.Items.GetData(item.Id);
            m_HeroData = heroData;
            m_ItemView.InitItem(item.Id, (QualityType)item.Quality, itemData == null ? 0 : itemData.Count);
            m_ItemView.SetOnClickDelegate(null);
            m_ExpPlus.text = GameEntry.Localization.GetString("UI_TEXT_EXP_PLUS", item.FunctionParams);
            if (itemData == null || itemData.Count <= 0)
            {
                m_ItemView.GetComponent<UISprite>().color = Color.grey;
                m_ItemView.GetComponent<UISprite>().alpha = 0.5f;
                m_CanUseItem = false;
                m_SelectIcon.SetActive(false);
            }
            else
            {
                m_ItemView.GetComponent<UISprite>().color = Color.white;
                m_ItemView.GetComponent<UISprite>().alpha = 1.0f;
                m_CanUseItem = true;
            }
        }

        protected void Update()
        {
            if (m_IsPressed)
            {
                m_CurPressedTime += Time.deltaTime;
                if (m_CurPressedTime > m_UseExpTimeSpan * m_CurUseExpTimes)
                {
                    OnUseExp();
                }
            }
        }

        public void OnExpItemPress(GameObject go, bool pressed)
        {
            m_IsPressed = pressed;
            if (!m_CanUseItem)
            {
                m_SelectIcon.SetActive(false);
            }
            else
            {
                m_SelectIcon.SetActive(m_IsPressed);
            }

            if (!pressed)
            {
                m_CurPressedTime = 0.0f;
                m_CurUseExpTimes = 0;
                m_UseExpTimeSpan = MaxUseExpTimeSpan;
            }
        }

        private void OnUseExp()
        {
            if (IsUsingExp || m_HeroData == null || m_Item == null || !m_CanUseItem)
            {
                return;
            }

            var dataTable = GameEntry.DataTable.GetDataTable<DRHeroBase>();
            DRHeroBase heroBaseDataRow = dataTable.GetDataRow(m_HeroData.Level + 1);
            if (heroBaseDataRow == null)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_PLAYER_MAX_LEVEL"));
                return;
            }

            if (m_HeroData.Level >= GameEntry.Data.Player.Level)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_HERO_NOT_EXCEED_PLAYERS"));
                return;
            }

            m_PressEffect.ShowEffect(PressEffectName);

            m_CurUseExpTimes++;
            if ((m_CurUseExpTimes % PerDescendingUseExpTims == 0) && (m_UseExpTimeSpan - MinUseExpTimeSpan + 0.00001f >= MinUseExpTimeSpan))
            {
                m_CurPressedTime = 0.0f;
                m_UseExpTimeSpan -= UseExpDescendingTime;
                m_CurUseExpTimes = 1;
            }
            IsUsingExp = true;
            GameEntry.Waiting.StopWaiting(WaitingType.Default, "ExperienceItem");
            GameEntry.LobbyLogic.InventoryUseHeroExpItem(m_Item.Id, 1, m_HeroData.Type);
        }

        public void OnClickPlusButton()
        {
        }
    }
}
