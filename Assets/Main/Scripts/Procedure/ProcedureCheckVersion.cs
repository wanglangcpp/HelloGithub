using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using System;
using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedureCheckVersion : ProcedureBase
    {
        private ProcedureConfig.ProcedureCheckVersionConfig m_Config = null;
        private VersionData m_VersionData = null;
        private bool m_LatestVersionListOK = false;
        private string publisher;
        private bool hadRequestVersion = false;

        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            
            m_Config = GameEntry.ClientConfig.ProcedureConfig.CheckVersionConfig;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_VersionData = null;
            m_LatestVersionListOK = false;

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.WebRequestSuccess, OnRequestVersionResponse);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.WebRequestFailure, OnRequestVersionError);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.VersionListUpdateSuccess, OnVersionListUpdateSuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.VersionListUpdateFailure, OnVersionListUpdateFailure);
            publisher = GameEntry.Data.GetTempData<string>(Constant.TempData.Publisher);
            SDKManager.Instance.helper.InitConfig(string.IsNullOrEmpty(publisher));
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.WebRequestSuccess, OnRequestVersionResponse);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.WebRequestFailure, OnRequestVersionError);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.VersionListUpdateSuccess, OnVersionListUpdateSuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.VersionListUpdateFailure, OnVersionListUpdateFailure);
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

#if !UNITY_EDITOR
            if (!SDKManager.Instance.SDKData.HadConfig()) {
                return;
            }
#endif
            if (!hadRequestVersion) {
                hadRequestVersion = true;
                RequestVersion();
            }

            if (m_LatestVersionListOK)
            {
                ChangeState<ProcedureUpdateResource>(procedureOwner);
            }
        }

        private void GotoUpdateApp(object userData)
        {
            string url = null;
#if UNITY_EDITOR
            url = "http://ota.antinc.cn/";
#elif UNITY_IOS
            url = GameEntry.BuildInfo.IosAppUrl;
#elif UNITY_ANDROID
            //url = GameEntry.BuildInfo.AndroidAppUrl;
            SDKManager.Instance.helper.LaunchAppDetail();
#else
            url = "http://ota.antinc.cn/";
#endif
            //Application.OpenURL(url);
        }

        private void RequestVersion()
        {
            
            string deviceId = SystemInfo.deviceUniqueIdentifier;
            string deviceName = SystemInfo.deviceName;
            string deviceModel = SystemInfo.deviceModel;
            string processorType = SystemInfo.processorType;
            string processorCount = SystemInfo.processorCount.ToString();
            string memorySize = SystemInfo.systemMemorySize.ToString();
            string operatingSystem = SystemInfo.operatingSystem;
            string iOSGeneration = string.Empty;
            string iOSSystemVersion = string.Empty;
            string iOSVendorIdentifier = string.Empty;
            string iOSAdvertisingIdentifier = string.Empty;
            string iOSAdvertisingTracking = string.Empty;
#if UNITY_IOS && !UNITY_EDITOR
            iOSGeneration = UnityEngine.iOS.Device.generation.ToString();
            iOSSystemVersion = UnityEngine.iOS.Device.systemVersion;
            iOSVendorIdentifier = UnityEngine.iOS.Device.vendorIdentifier ?? string.Empty;
            iOSAdvertisingIdentifier = UnityEngine.iOS.Device.advertisingIdentifier ?? string.Empty;
            iOSAdvertisingTracking = UnityEngine.iOS.Device.advertisingTrackingEnabled.ToString();
#endif
            string gameVersion = GameEntry.Base.GameVersion;
            string platform = Application.platform.ToString();
            string language = GameEntry.Localization.Language.ToString();
            string unityVersion = Application.unityVersion;
            string installMode = Application.installMode.ToString();
            string sandboxType = Application.sandboxType.ToString();
            string screenWidth = Screen.width.ToString();
            string screenHeight = Screen.height.ToString();
            string screenDpi = Screen.dpi.ToString();
            string screenOrientation = Screen.orientation.ToString();
            string screenResolution = string.Format("{0} x {1} @ {2}Hz", Screen.currentResolution.width.ToString(), Screen.currentResolution.height.ToString(), Screen.currentResolution.refreshRate.ToString());
            string useWifi = (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork).ToString();

            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("Publisher", WebUtility.EscapeString(publisher));
            wwwForm.AddField("DeviceId", WebUtility.EscapeString(deviceId));
            wwwForm.AddField("DeviceName", WebUtility.EscapeString(deviceName));
            wwwForm.AddField("DeviceModel", WebUtility.EscapeString(deviceModel));
            wwwForm.AddField("ProcessorType", WebUtility.EscapeString(processorType));
            wwwForm.AddField("ProcessorCount", WebUtility.EscapeString(processorCount));
            wwwForm.AddField("MemorySize", WebUtility.EscapeString(memorySize));
            wwwForm.AddField("OperatingSystem", WebUtility.EscapeString(operatingSystem));
            wwwForm.AddField("IOSGeneration", WebUtility.EscapeString(iOSGeneration));
            wwwForm.AddField("IOSSystemVersion", WebUtility.EscapeString(iOSSystemVersion));
            wwwForm.AddField("IOSVendorIdentifier", WebUtility.EscapeString(iOSVendorIdentifier));
            wwwForm.AddField("IOSAdvertisingIdentifier", WebUtility.EscapeString(iOSAdvertisingIdentifier));
            wwwForm.AddField("IOSAdvertisingTracking", WebUtility.EscapeString(iOSAdvertisingTracking));
            wwwForm.AddField("GameVersion", WebUtility.EscapeString(gameVersion));
            wwwForm.AddField("Platform", WebUtility.EscapeString(platform));
            wwwForm.AddField("Language", WebUtility.EscapeString(language));
            wwwForm.AddField("UnityVersion", WebUtility.EscapeString(unityVersion));
            wwwForm.AddField("InstallMode", WebUtility.EscapeString(installMode));
            wwwForm.AddField("SandboxType", WebUtility.EscapeString(sandboxType));
            wwwForm.AddField("ScreenWidth", WebUtility.EscapeString(screenWidth));
            wwwForm.AddField("ScreenHeight", WebUtility.EscapeString(screenHeight));
            wwwForm.AddField("ScreenDPI", WebUtility.EscapeString(screenDpi));
            wwwForm.AddField("ScreenOrientation", WebUtility.EscapeString(screenOrientation));
            wwwForm.AddField("ScreenResolution", WebUtility.EscapeString(screenResolution));
            wwwForm.AddField("UseWifi", WebUtility.EscapeString(useWifi));
            wwwForm.AddField("ChannelCode", WebUtility.EscapeString(SDKManager.Instance.SDKData.HadConfig() ? SDKManager.Instance.SDKData.ChannelCode : Constant.TempData.UnityEditorWindowsChannelCode));

            
            if (SDKManager.HasConfig)
            {
                m_Config.CheckVersionUri = SDKManager.Instance.SDKData.CheckVersionURL;
                GameEntry.BuildInfo.CheckVersionUri = SDKManager.Instance.SDKData.CheckVersionURLOut;
            }
            Log.Info("RequestUrl:" + (string.IsNullOrEmpty(publisher) ? m_Config.CheckVersionUri : GameEntry.BuildInfo.CheckVersionUri));
            GameEntry.WebRequest.AddWebRequest(string.IsNullOrEmpty(publisher) ? m_Config.CheckVersionUri : GameEntry.BuildInfo.CheckVersionUri, wwwForm);
        }

        private void UpdateVersion()
        {
            if (GameEntry.Resource.CheckVersionList(m_VersionData.InternalResourceVersion) == CheckVersionListResult.Updated)
            {
                m_LatestVersionListOK = true;
            }
            else
            {
                GameEntry.Resource.UpdateVersionList(m_VersionData.VersionListLength, m_VersionData.VersionListHashCode, m_VersionData.VersionListZipLength, m_VersionData.VersionListZipHashCode);
            }
        }

        private void OnRequestVersionResponse(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = e as WebRequestSuccessEventArgs;
            //string str = System.Text.Encoding.UTF8.GetString(ne.GetWebResponseBytes());
            //Log.Info("=="+ str);

           
            m_VersionData = Utility.Json.ToObject<VersionData>(ne.GetWebResponseBytes());
            Log.Info("OnRequestVersionResponse==== " + m_VersionData.ForceGameUpdate);
            Log.Info("Latest game version is '{0}', local game version is '{1}'.", m_VersionData.LatestGameVersion + ":" + m_VersionData.InternalApplicationVersion, GameEntry.Base.GameVersion + ":" + GameEntry.Base.InternalApplicationVersion);
            if (m_VersionData.ForceGameUpdate)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_FORCE_GAME_UPDATE"),
                    ConfirmText = GameEntry.Localization.GetString("UI_TEXT_GOTO_UPDATE"),
                    CancelText = GameEntry.Localization.GetString("UI_TEXT_QUIT_GAME"),
                    OnClickConfirm = GotoUpdateApp,
                    OnClickCancel = QuitGame,
                });

                return;
            }

            string publisher = GameEntry.Data.GetTempData<string>(Constant.TempData.Publisher);
            Log.Info("publisher=== " + publisher);//string.IsNullOrEmpty(publisher) && 
            if (GameEntry.Base.InternalApplicationVersion > 0 && m_VersionData.InternalApplicationVersion > 0 && GameEntry.Base.InternalApplicationVersion != m_VersionData.InternalApplicationVersion)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = "已有最新版本可供下载，请前往下载安装！",//GameEntry.Localization.GetString("UI_TEXT_VERIFY_INNER_VERSION_FAILED" + GameEntry.Base.InternalApplicationVersion, m_VersionData.InternalApplicationVersion.ToString()),
                    ConfirmText = GameEntry.Localization.GetString("UI_TEXT_GOTO_UPDATE"),
                    CancelText = GameEntry.Localization.GetString("UI_TEXT_QUIT_GAME"),
                    OnClickConfirm = GotoUpdateApp,
                    OnClickCancel = QuitGame,
                });

                return;
            }

            if (SDKManager.HasConfig)
            {
                GameEntry.BuildInfo.UpdateResourceUri = SDKManager.Instance.SDKData.UpdateResourceURLOut;
                m_Config.UpdateResourceUri = SDKManager.Instance.SDKData.UpdateResourceURL;
            }

            GameEntry.Resource.UpdatePrefixUri = Utility.Path.GetCombinePath(string.IsNullOrEmpty(publisher) ? m_Config.UpdateResourceUri : GameEntry.BuildInfo.UpdateResourceUri, GetResourceVersionName(), GetPlatformPath());
            Log.Info("UpdatePrefixUri:" + GameEntry.Resource.UpdatePrefixUri);
            UpdateVersion();
        }

        private void OnRequestVersionError(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = e as WebRequestFailureEventArgs;
            OnError("Request resource version failed with error message '{0}'.", ne.ErrorMessage);
        }

        private void OnVersionListUpdateSuccess(object sender, GameEventArgs e)
        {
            m_LatestVersionListOK = true;
        }

        private void OnVersionListUpdateFailure(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.VersionListUpdateFailureEventArgs ne = e as UnityGameFramework.Runtime.VersionListUpdateFailureEventArgs;
            OnError("Can not download latest resource version list from '{0}' with error message '{1}'.", ne.DownloadUri, ne.ErrorMessage);
        }

        private string GetResourceVersionName()
        {
            string[] splitApplicableGameVersion = GameEntry.Base.GameVersion.Split('.');
            if (splitApplicableGameVersion.Length != 3)
            {
                return string.Empty;
            }

            return string.Format("{0}_{1}_{2}_{3}", splitApplicableGameVersion[0], splitApplicableGameVersion[1], splitApplicableGameVersion[2], m_VersionData.InternalResourceVersion.ToString());
        }

        private string GetPlatformPath()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return "windows";
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return "osx";
                case RuntimePlatform.IPhonePlayer:
                    return "ios";
                case RuntimePlatform.Android:
                    return "android";
                case RuntimePlatform.WSAPlayerX86:
                case RuntimePlatform.WSAPlayerX64:
                case RuntimePlatform.WSAPlayerARM:
                    return "winstore";
                default:
                    return string.Empty;
            }
        }
    }
}
