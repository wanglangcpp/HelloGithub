using UnityEngine;
using System.Collections;
using GameFramework;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChanceReceiveForm : NGUIForm
    {
        [SerializeField]
        private ChanceReceiveFormItem[] m_ChanceReceiveFormItems = null;

        [SerializeField]
        private GameObject m_WholeScreenBtn = null;

        private ReceivedGeneralItemsViewData m_ReceivedItemsData = null;

        private GameFrameworkAction<object> OnComplete = null;

        private int m_ItemIndex = 0;

        private int m_HeroId = 0;

        private bool m_ItemCanShow = false;

        private bool m_ItemIsHero = false;

        private bool m_ItemIndexCanPlus = true;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.CloseUIFormComplete, OnCloseUIFormComplete);
            var myUserData = userData as ChanceReceiveDisplayData;
            m_ReceivedItemsData = myUserData.ReceiveGoodsData;
            OnComplete = myUserData.OnComplete;
            Init();
        }
        protected override void OnPostOpen(object data)
        {
            base.OnPostOpen(data);
            m_EffectsController.ShowEffect("EffectBgBomb");
        }

        private void Init()
        {
            m_ItemIndex = 0;
            m_ItemCanShow = true;
            m_ItemIsHero = true;
            m_WholeScreenBtn.SetActive(false);

            for (int i = 0; i < m_ChanceReceiveFormItems.Length; i++)
            {
                m_ChanceReceiveFormItems[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < m_ReceivedItemsData.Items.Count; i++)
            {
                m_ChanceReceiveFormItems[i].RefreshData(m_ReceivedItemsData.Items[i]);
            }
        }

        private bool ShouldShowHero()
        {
            if (m_ReceivedItemsData.IsGeneralItemsHero(m_ReceivedItemsData.Items[m_ItemIndex].Type, out m_HeroId))
            {
                return true;
            }
            return false;
        }

        private void ShowHero()
        {
            m_ItemIndexCanPlus = false;
            m_ItemIsHero = false;
            m_ItemCanShow = false;
            var displayData = m_ReceivedItemsData.GetOneHeroDataById(m_HeroId);
            GameEntry.UI.OpenUIForm(UIFormId.ReceiveHeroForm, displayData);
        }

        private void ShowReceiveItem()
        {
            m_ChanceReceiveFormItems[m_ItemIndex].gameObject.SetActive(true);
            m_ChanceReceiveFormItems[m_ItemIndex].PlayAnimation();
            m_ItemIndexCanPlus = true;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (ShouldShowHero() && m_ItemIsHero)
            {
                ShowHero();
                return;
            }

            if (m_ItemCanShow)
            {
                ShowReceiveItem();
                m_ItemCanShow = false;
            }

            if (m_ItemIndex == m_ReceivedItemsData.Items.Count - 1)
            {
                m_ItemCanShow = false;
                m_WholeScreenBtn.SetActive(true);
                return;
            }

            if (!m_ChanceReceiveFormItems[m_ItemIndex].ItemAnimation.isPlaying && m_ItemIndexCanPlus)
            {
                m_ItemIndex++;
                m_ItemIsHero = true;
                m_ItemCanShow = true;
            }
        }

        protected void OnCloseUIFormComplete(object sender, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.CloseUIFormCompleteEventArgs;
            if (ne.UIFormTypeId == (int)UIFormId.ReceiveHeroForm)
            {
                if (m_ItemIndex == m_ReceivedItemsData.Items.Count)
                {
                    m_ItemCanShow = false;
                    m_WholeScreenBtn.SetActive(true);
                }
                else
                {
                    m_ItemCanShow = true;
                    m_ItemIsHero = false;
                }
            }
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);

            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.CloseUIFormComplete, OnCloseUIFormComplete);
            }
        }

        public void OnClickScreenBtn()
        {
            CloseSelf();
            OnComplete(null);
        }
    }
}