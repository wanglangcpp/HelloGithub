using UnityEngine;

namespace Genesis.GameClient
{
    public class CosmosCrackInstanceFailureForm : NGUIForm
    {
        [SerializeField]
        private GameObject m_TimeOutPanel = null;

        [SerializeField]
        private GameObject m_FailurePanel = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            var myUserData = userData as InstanceFailureData;
            m_TimeOutPanel.SetActive(myUserData.FailureReason == InstanceFailureReason.TimeOut);
            m_FailurePanel.SetActive(myUserData.FailureReason != InstanceFailureReason.TimeOut);
        }

        public void OnClickReturn()
        {
            if (GameEntry.SceneLogic.BaseInstanceLogic.HasResult)
            {
                GameEntry.SceneLogic.GoBackToLobby(true);
                return;
            }

            GameEntry.SceneLogic.BaseInstanceLogic.SetInstanceFailure(InstanceFailureReason.AbandonedByUser, false,
                delegate () { GameEntry.SceneLogic.GoBackToLobby(true); });
        }
    }
}
