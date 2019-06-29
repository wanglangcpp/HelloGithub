using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class SelectServerItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_ServerName = null;

        [SerializeField]
        private GameObject m_MaintenanceIcon = null;

        [SerializeField]
        private GameObject m_FluencyIcon = null;

        [SerializeField]
        private GameObject m_CrowdedIcon = null;

        [SerializeField]
        private UISprite m_Recommend = null;

        [SerializeField]
        private UILabel m_StatusText = null;

        [SerializeField]
        private UILabel m_ServerConstituencyText = null;

        [SerializeField]
        private UIButton m_SelfButton = null;

        public int ServerId
        {
            get;
            set;
        }

        public string ServerName
        {
            get
            {
                return m_ServerName.text;
            }
            set
            {
                m_ServerName.text = value;
            }
        }

        private bool m_IsMaintenance = false;

        public void RefreshServerData(ServerData data)
        {
            ServerId = data.Id;
            ServerName = data.Name;
            m_ServerConstituencyText.text = GameEntry.Localization.GetString("UI_SIGN_CONSTITUENCY", ServerId);
            m_Recommend.gameObject.SetActive(data.CheckFlag(ServerFlag.Recommended));
            if (data.CheckFlag(ServerFlag.Maintenance) || data.Load == ServerLoad.OutOfService)
            {
                m_IsMaintenance = true;
                m_MaintenanceIcon.SetActive(true);
                m_FluencyIcon.SetActive(false);
                m_CrowdedIcon.SetActive(false);
                m_SelfButton.isEnabled = false;
                m_StatusText.text = GameEntry.Localization.GetString("UI_BUTTON_SERVER_MAINTENANCE");
            }
            else
            {
                m_IsMaintenance = false;
                m_MaintenanceIcon.SetActive(false);
                m_FluencyIcon.SetActive((ServerLoad)data.Load == ServerLoad.Good);
                m_CrowdedIcon.SetActive((ServerLoad)data.Load == ServerLoad.Full);
                m_SelfButton.isEnabled = true;
                m_StatusText.text = (ServerLoad)data.Load == ServerLoad.Full ? GameEntry.Localization.GetString("UI_BUTTON_SERVER_HOT") : GameEntry.Localization.GetString("UI_BUTTON_SERVER_FLUENT");
            }
        }

        public void OnClickButton()
        {
            if (m_IsMaintenance)
            {
                UIUtility.ShowOkayButtonDialog(GameEntry.Localization.GetString("UI_TEXT_SERVER_NOTE_MAINTENANCE"));
                return;
            }

            ProcedureSelectServer procedureSelectServer = GameEntry.Procedure.CurrentProcedure as ProcedureSelectServer;
            if (procedureSelectServer == null)
            {
                Log.Warning("Can not select server in '{0}' procedure.", GameEntry.Procedure.CurrentProcedure.GetType().Name);
                return;
            }

            procedureSelectServer.ConnectServer(ServerId);
        }
    }
}
