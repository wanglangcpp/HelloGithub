using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class HeroExpUpItem : MonoBehaviour
    {
        private UIEffectsController m_EffectsController = null;

        [SerializeField]
        private UISprite m_Icon = null;

        [SerializeField]
        private GameObject m_Plus = null;

        [SerializeField]
        private UILabel m_Count = null;

        [SerializeField]
        private UILabel m_Exp = null;

        [SerializeField]
        private UIButton m_ItemBtn = null;

        private int m_ItemType = 0;

        private int m_HeroType = 0;

        private Action<GameObject> m_UseItemReturn = null;

        public DRItem DRExpData
        {
            get;
            set;
        }

        public ItemData ExpData
        {
            get;
            set;
        }

        public void RefreshData(DRItem itemData, int heroType, Action<GameObject> useItemReturn)
        {
            DRExpData = itemData;
            m_ItemType = itemData.Id;
            m_HeroType = heroType;
            m_UseItemReturn = useItemReturn;

            ItemData data = GameEntry.Data.Items.GetData(itemData.Id);
            ExpData = data;
            m_Icon.LoadAsync(itemData.IconId);
            m_ItemBtn.normalSprite = m_Icon.spriteName;
            m_Plus.SetActive(data == null);
            m_ItemBtn.isEnabled = data != null;
            m_Count.text = data == null ? string.Empty : data.Count.ToString();
            m_Exp.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", itemData.FunctionParams);
        }

        public void OnUseItem()
        {
            var heroData = GameEntry.Data.LobbyHeros.GetData(m_HeroType);
            if (heroData.Level >= GameEntry.Data.Player.Level && heroData.Exp >= heroData.LevelUpExp - 1)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_USEITEM_NOTICE_LEVELFULL") });
                return;
            }

            GameEntry.LobbyLogic.InventoryUseHeroExpItem(m_ItemType, 1, m_HeroType);
            if (m_UseItemReturn != null)
            {
                m_UseItemReturn(gameObject);
            }
            m_EffectsController.ShowEffect("EffectUseItem");
        }

        #region MonoBahaviour

        private void Awake()
        {
            m_EffectsController = GetComponent<UIEffectsController>();
        }

        private void OnEnable()
        {
            m_EffectsController.Resume();
        }

        private void OnDisable()
        {
            m_EffectsController.Pause();
        }

        #endregion MonoBahaviour
    }
}
