using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedureCheckServerList : ProcedureBase
    {
        private ProcedureConfig.ProcedureCheckServerListConfig m_Config = null;
        private bool m_ServerListOK = false;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            //SDKManager.Instance.helper.InitSDK();
            m_Config = GameEntry.ClientConfig.ProcedureConfig.CheckServerListConfig;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.WebRequestSuccess, OnRequestServerListResponse);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.WebRequestFailure, OnRequestServerListError);

            m_ServerListOK = false;

            RequestServerList();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.WebRequestSuccess, OnRequestServerListResponse);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.WebRequestFailure, OnRequestServerListError);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_ServerListOK)
            {
                ChangeProcedure(procedureOwner);
            }
        }

        private void RequestServerList()
        {
            string publisher = GameEntry.Data.GetTempData<string>(Constant.TempData.Publisher);
            Log.Debug("Unity.SDKManager.HasConfig:" + SDKManager.HasConfig);
            if (SDKManager.HasConfig)
            {
                m_Config.CheckServerListUri = SDKManager.Instance.SDKData.ServerListURL;
                GameEntry.BuildInfo.CheckServerListUri = SDKManager.Instance.SDKData.ServerListURLOut;
            }
            GameEntry.WebRequest.AddWebRequest(string.IsNullOrEmpty(publisher) ? m_Config.CheckServerListUri : GameEntry.BuildInfo.CheckServerListUri);

            Log.Debug("SDKManager.RequestServerList."+ (string.IsNullOrEmpty(publisher) ? m_Config.CheckServerListUri : GameEntry.BuildInfo.CheckServerListUri));
            //Log.Debug("SDKManager.RequestServerList.out." + GameEntry.BuildInfo.CheckServerListUri);
            //Log.Debug("SDKManager.RequestServerList.in." + m_Config.CheckServerListUri);
        }

        private void OnRequestServerListResponse(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = e as WebRequestSuccessEventArgs;
            string jsonList = Utility.Converter.GetStringFromBytes(ne.GetWebResponseBytes());
            SDKManager.Debug("OnRequestServerListResponse:" + jsonList);

#if UNITY_EDITOR
            jsonList = ParseHtmlBodyTag(jsonList);
#else
            //if (SDKManager.Instance.SDKData.ChannelCode=="0" && !string.IsNullOrEmpty(GameEntry.Data.GetTempData<string>(Constant.TempData.Publisher))){
                //外网&&母包
                //jsonList = AESMgrTool.Decrypt(jsonList, SDKManager.Instance.SDKData.AesKey);
            //}else{
                jsonList = ParseHtmlBodyTag(jsonList);
            //}
#endif
            SDKManager.Debug("OnRequestServerListResponse.jsonList:" + jsonList);
            jsonList = jsonList.Replace("\n", string.Empty).Replace("\r", string.Empty);

            ServerListData data = Utility.Json.ToObject<ServerListData>(jsonList);
            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.ServerList, data);
            GameEntry.Data.ServerNames.UpdateData(data);



            m_ServerListOK = true;
        }

        private void OnRequestServerListError(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = e as WebRequestFailureEventArgs;
            OnError("Request server list failed with error message '{0}'.", ne.ErrorMessage);
        }

        private void ChangeProcedure(ProcedureOwner procedureOwner)
        {
            string accountName = GameEntry.Data.Account.AccountName;
            string loginKey = GameEntry.Data.Account.LoginKey;
            bool needLogin = string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(loginKey);
            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.NeedShowAccountAgreement, needLogin);
            ChangeState(procedureOwner, needLogin ? typeof(ProcedureLogin) : typeof(ProcedureSelectServer));

        }

        private string ParseHtmlBodyTag(string html, string startTag = "<body>", string lastTag = "</body>")
        {
            string msg = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(html) && html.IndexOf(startTag) > 0 && html.IndexOf(lastTag) > 0 && html.IndexOf(lastTag) > html.IndexOf(startTag))
                {
                    int a = html.IndexOf(startTag);
                    int b = html.IndexOf(lastTag);
                    var len1 = a + startTag.Length;
                    msg = html.Substring(len1, b - len1);
                }
            }
            catch
            {
                Log.Debug("ParseHtmlBodyTag Fail");
            }
            if (startTag.Equals("<body>") && lastTag.Equals("</body>"))
            {
                msg = "{\"ServerList\":" + msg + "}";
            }
            return msg;
        }
    }
}
