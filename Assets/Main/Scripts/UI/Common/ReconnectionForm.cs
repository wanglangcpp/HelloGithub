using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ReconnectionForm : NGUIForm
    {
        [SerializeField]
        private UILabel m_MessageLabel = null;

        [SerializeField]
        private UILabel m_ButtonLabel = null;

        private GameFrameworkAction<object> m_OnClickConfirm = null;

        private object m_CachedUserData = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            var reconnectData = userData as ReconnectionDisplayData;
            if (reconnectData == null)
            {
                Log.Warning("reconnectData is invalid.");
                return;
            }
            m_ButtonLabel.text = reconnectData.ButtonMessage;
            m_OnClickConfirm = reconnectData.OnClickConfirm;
            m_MessageLabel.text = reconnectData.Message;
            m_CachedUserData = reconnectData.UserData;
        }

        protected override void OnClose(object userData)
        {
            m_CachedUserData = null;
            m_OnClickConfirm = null;
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

        protected override void OnChangeSceneStart(object sender, GameEventArgs e)
        {
            // Empty. SceneLogicComponent will close this page.
        }
    }
}
