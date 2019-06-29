using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class SDKLoginCallback : SDKCallbackBase
    {
        public string AccountName = string.Empty;
        public string Token = string.Empty;
        public string ChannelCode = string.Empty;
        public string Uid = string.Empty;
        public string AesKey = string.Empty;
        public string Data = string.Empty;//after access web login
        public string ServerList = string.Empty;//外网
    }
}
