using GameFramework.Event;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework;
using System;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class ProcedureMain : ProcedureBase
    {
        private ProcedureConfig.ProcedureChargeTableConfig m_Config = null;
        private bool m_HasRequiredToGoToCreatePlayer = false;
        private ChangeSceneRequestData m_ChangeSceneRequestData = null;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void GoToCreatePlayer()
        {
            m_HasRequiredToGoToCreatePlayer = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            m_Config = GameEntry.ClientConfig.ProcedureConfig.ChargeTableConfig;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_ChangeSceneRequestData = null;
            m_HasRequiredToGoToCreatePlayer = false;
            GameEntry.UIBackground.HideDefault();

            GameEntry.Event.Subscribe(EventId.WillChangeScene, OnWillChangeScene);
            GameEntry.Event.Subscribe(EventId.ResponseConnectLobbyServer, OnResponseConnectLobbyServer);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.WebRequestSuccess, OnRequestChargeTableResponse);
            RequestChargeTable();

            SDKManager.Instance.helper.UploadData("EntreGame");
        }



        private void RequestChargeTable()
        {
            string publisher = GameEntry.Data.GetTempData<string>(Constant.TempData.Publisher);
            Log.Debug("Unity.SDKManager.HasConfig:" + SDKManager.HasConfig);
            if (SDKManager.HasConfig)
            {
                m_Config.ChargeTableUri = SDKManager.Instance.SDKData.ChargeTableURL;
                GameEntry.BuildInfo.ChargeTableUrl = SDKManager.Instance.SDKData.ChargeTableURLOut;

            }
            Log.Debug("SDKManager.RequestChargeTable." + (string.IsNullOrEmpty(publisher) ? m_Config.ChargeTableUri : GameEntry.BuildInfo.ChargeTableUrl));

            GameEntry.WebRequest.AddWebRequest(string.IsNullOrEmpty(publisher) ? m_Config.ChargeTableUri : GameEntry.BuildInfo.ChargeTableUrl);
        }
        private void OnRequestChargeTableResponse(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = e as WebRequestSuccessEventArgs;
            string jsonList = Utility.Converter.GetStringFromBytes(ne.GetWebResponseBytes());
            SDKManager.Debug("OnRequestChargeTableResponse:" + jsonList);

            jsonList = ParseHtmlBodyTag(jsonList);

            SDKManager.Debug("OnRequestChargeTableResponse.jsonList:" + jsonList);
            jsonList = jsonList.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);

            SDKManager.Debug(jsonList);

            ChargeListInfo data = Utility.Json.ToObject<ChargeListInfo>(jsonList);

            GameEntry.Data.ChargeTable = data.ChargeTable;

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
                msg = "{\"ChargeTable\":" + msg + "}";
            }
            return msg;
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (!GameEntry.IsAvailable)
            {
                base.OnLeave(procedureOwner, isShutdown);
                return;
            }

            GameEntry.Event.Unsubscribe(EventId.WillChangeScene, OnWillChangeScene);
            GameEntry.Event.Unsubscribe(EventId.ResponseConnectLobbyServer, OnResponseConnectLobbyServer);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.WebRequestSuccess, OnRequestChargeTableResponse);

            GameEntry.Base.ResetNormalGameSpeed();
            m_HasRequiredToGoToCreatePlayer = false;
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_ChangeSceneRequestData != null)
            {
                ProcedureChangeScene procedureChangeScene = procedureOwner.GetState<ProcedureChangeScene>();
                procedureChangeScene.ChangeScene(procedureOwner, m_ChangeSceneRequestData.InstanceLogicType, m_ChangeSceneRequestData.AutoHideLoading);
                m_ChangeSceneRequestData = null;
                return;
            }

            if (m_HasRequiredToGoToCreatePlayer)
            {
                ChangeState<ProcedureCreatePlayer>(procedureOwner);
                return;
            }
        }

        private void OnWillChangeScene(object sender, GameEventArgs e)
        {
            var ne = e as WillChangeSceneEventArgs;
            m_ChangeSceneRequestData = new ChangeSceneRequestData(ne.SceneOrInstanceId, ne.InstanceLogicType, ne.AutoHideLoading);

            // 清理和副本相关的数据。TODO: 移出 Procedure 模块。
            GameEntry.CameraShaking.Reset();
            GameEntry.TimeLine.ClearAllTimeInstances();
        }

        private void OnResponseConnectLobbyServer(object sender, GameEventArgs e)
        {
            GameEntry.Event.Fire(this, new RequestSignInLobbyServerEventArgs());
        }
    }
}
