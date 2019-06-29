using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class PlatformDataPart1
    {
        public string HasConfig = "0";
        public string HasSDK = "0";
        public string ServerListURL = string.Empty;
        public string ServerListURLOut = string.Empty;//外网
        public string RequestOrderURL = string.Empty;
        public string RequestOrderURLOut = string.Empty;
        public string UpdateResourceURL = string.Empty;
        public string UpdateResourceURLOut = string.Empty;
        public string CheckVersionURL = string.Empty;//CheckVersion
        public string CheckVersionURLOut = string.Empty;//CheckVersion
    }

    [Serializable]
    public class PlatformDataPart2
    {
        public string ChannelCode = string.Empty;
        public string AesKey = string.Empty;
        public string AppCode = string.Empty;
        public string ExistFloating = "0";
        public string FloatingStatus = "0";
        public string SDKInitCompleted = "0";
        public string SDKSupportAutoLogin = "0";
        public string HasExitWindow = "0";
        public string HasLogout = "0";
        public string ChargeTableURL = string.Empty;//商品列表地址内网
        public string ChargeTableURLOut = string.Empty;//外网
    }

    [Serializable]
    public class PlatformData : SDKCallbackBase
    {
        public string HasConfig = "0";
        public string HasSDK = "0";
        public string ServerListURLOut = string.Empty;//外网
        public string ServerListURL = string.Empty;
        public string RequestOrderURL = string.Empty;
        public string RequestOrderURLOut = string.Empty;
        public string UpdateResourceURL = string.Empty;
        public string UpdateResourceURLOut = string.Empty;
        public string CheckVersionURL = string.Empty;//CheckVersion
        public string CheckVersionURLOut = string.Empty;//CheckVersion

        public string ChannelCode = string.Empty;
        public string AesKey = string.Empty;
        public string AppCode = string.Empty;
        public string ExistFloating = "0";
        public string FloatingStatus = "0";
        public string SDKInitCompleted = "0";
        public string SDKSupportAutoLogin = "0";
        public string HasExitWindow = "0";
        public string HasLogout = "0";
        public string ChargeTableURL = string.Empty;//商品列表地址内网
        public string ChargeTableURLOut = string.Empty;//外网

        public string AccountName = string.Empty;
        public string Token = string.Empty;
        public string Uid = string.Empty;
        public string Data = string.Empty;//after access web login
        public string LoginURL = string.Empty;
        public string LoginURLOut = string.Empty;

        public void SetFieldValue(PlatformDataPart1 obj)
        {
            HasConfig = obj.HasConfig;
            HasSDK = obj.HasSDK;
            ServerListURLOut = obj.ServerListURLOut;
            ServerListURL = obj.ServerListURL;
            RequestOrderURL = obj.RequestOrderURL;
            RequestOrderURLOut = obj.RequestOrderURLOut;
            UpdateResourceURL = obj.UpdateResourceURL;
            UpdateResourceURLOut = obj.UpdateResourceURLOut;
            CheckVersionURL = obj.CheckVersionURL;
            CheckVersionURLOut = obj.CheckVersionURLOut;
        }

        public void SetFieldValue(PlatformDataPart2 obj)
        {
            ChannelCode = obj.ChannelCode;
            AesKey = obj.AesKey;
            AppCode = obj.AppCode;
            ExistFloating = obj.ExistFloating;
            FloatingStatus = obj.FloatingStatus;
            SDKInitCompleted = obj.SDKInitCompleted;
            SDKSupportAutoLogin = obj.SDKSupportAutoLogin;
            HasExitWindow = obj.HasExitWindow;
            HasLogout = obj.HasLogout;
            ChargeTableURL = obj.ChargeTableURL;
            ChargeTableURLOut = obj.ChargeTableURLOut;
        }

        public bool HadConfig()
        {
            return HasConfig.Equals("1");
        }

        public bool HadSDK()
        {
            return HasSDK.Equals("1");
        }
        public bool HadExitWindow()
        {
            return HasExitWindow.Equals("1");
        }
        /// <summary>
        /// 是否有注销用户接口
        /// </summary>
        public bool HadLogout()
        {
            return HasLogout.Equals("1");
        }
        public bool IsExistFloating()
        {
            return ExistFloating.Equals("1");
        }

        public bool IsOpeningFloating()
        {
            return FloatingStatus.Equals("1");
        }

        public bool IsSDKInitCompleted()
        {
            return SDKInitCompleted.Equals("1");
        }

        public bool IsSDKSupportAutoLogin()
        {
            return SDKSupportAutoLogin.Equals("1");
        }

    }
}
