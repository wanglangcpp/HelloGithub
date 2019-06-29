using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public class Dialog : NGUIForm
    {
        private const int MinDialogWidth = 450;

        [SerializeField]
        private GameObject[] m_ModePanel = null;

        [SerializeField]
        private UILabel m_Message = null;

        [SerializeField]
        private UILabel[] m_ConfirmTexts = null;

        [SerializeField]
        private UILabel[] m_CancelTexts = null;

        [SerializeField]
        private UILabel[] m_OtherTexts = null;

        [SerializeField]
        private UIWidget[] m_UpdateAnchors = null;

        private int m_DialogMode = 1;
        private bool m_PauseGame = false;
        private int m_DialogWidth = MinDialogWidth;
        private GameFrameworkAction<object> m_OnClickConfirm = null;
        private GameFrameworkAction<object> m_OnClickCancel = null;
        private GameFrameworkAction<object> m_OnClickOther = null;
        private object m_CachedUserData = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            var dialogData = userData as DialogDisplayData;
            if (dialogData == null)
            {
                Log.Warning("dialogData is invalid.");
                return;
            }

            RefreshData(dialogData);

            for (int i = 0; i < m_UpdateAnchors.Length; i++)
            {
                m_UpdateAnchors[i].UpdateAnchors();
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(object userData)
        {
            if (m_PauseGame)
            {
                GameEntry.TimeScale.ResumeGame();
            }

            ResetData();

            base.OnClose(userData);
        }

        public void OnClickConfirm()
        {
            if (m_OnClickConfirm != null)
            {
                m_OnClickConfirm(m_CachedUserData);
            }

            CloseSelf();
        }

        public void OnClickCancel()
        {
            if (m_OnClickCancel != null)
            {
                m_OnClickCancel(m_CachedUserData);
            }

            CloseSelf();
        }

        public void OnClickOther()
        {
            if (m_OnClickOther != null)
            {
                m_OnClickOther(m_CachedUserData);
            }

            CloseSelf();
        }

        protected override void OnChangeSceneStart(object sender, GameEventArgs e)
        {
            // Do nothing!
        }

        private void RefreshDialogMode()
        {
            for (int i = 1; i <= m_ModePanel.Length; i++)
            {
                m_ModePanel[i - 1].SetActive(i == m_DialogMode);
            }
        }

        private void RefreshPauseGame()
        {
            if (m_PauseGame)
            {
                GameEntry.TimeScale.PauseGame();
            }
        }

        private void RefreshDialogWidth()
        {
            m_Message.width = m_DialogWidth;
        }

        private void RefreshConfirmText(string confirmText)
        {
            if (string.IsNullOrEmpty(confirmText))
            {
                confirmText = GameEntry.Localization.GetString("UI_BUTTON_CONFIRM");
            }

            for (int i = 0; i < m_ConfirmTexts.Length; i++)
            {
                m_ConfirmTexts[i].text = confirmText;
            }
        }

        private void RefreshCancelText(string cancelText)
        {
            if (string.IsNullOrEmpty(cancelText))
            {
                cancelText = GameEntry.Localization.GetString("UI_BUTTON_CANCEL");
            }

            for (int i = 0; i < m_CancelTexts.Length; i++)
            {
                m_CancelTexts[i].text = cancelText;
            }
        }

        private void RefreshOtherText(string otherText)
        {
            if (string.IsNullOrEmpty(otherText))
            {
                otherText = GameEntry.Localization.GetString("UI_BUTTON_OTHER");
            }

            for (int i = 0; i < m_OtherTexts.Length; i++)
            {
                m_OtherTexts[i].text = otherText;
            }
        }

        private void RefreshData(DialogDisplayData dialogData)
        {
            m_DialogMode = dialogData.Mode;
            RefreshDialogMode();
            m_PauseGame = dialogData.PauseGame;
            RefreshPauseGame();
            m_DialogWidth = Mathf.Max(dialogData.Width, MinDialogWidth);
            RefreshDialogWidth();
            m_Message.text = dialogData.Message;
            RefreshConfirmText(dialogData.ConfirmText);
            RefreshCancelText(dialogData.CancelText);
            RefreshOtherText(dialogData.OtherText);
            m_OnClickConfirm = dialogData.OnClickConfirm;
            m_OnClickCancel = dialogData.OnClickCancel;
            m_OnClickOther = dialogData.OnClickOther;
            m_CachedUserData = dialogData.UserData;
        }

        private void ResetData()
        {
            m_DialogMode = 1;
            m_PauseGame = false;
            m_DialogWidth = MinDialogWidth;
            m_OnClickConfirm = null;
            m_OnClickCancel = null;
            m_OnClickOther = null;
            m_CachedUserData = null;
        }
    }
}
