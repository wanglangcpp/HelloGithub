using UnityEngine;
using System.Collections;

namespace Genesis.GameClient
{
    public class SDKInitData : SDKCallbackBase
    {
        public bool hasSDK = false;
        public string ServerListURL = string.Empty;
        public string ServerListURLOut = string.Empty;
        public string ChannelCode = string.Empty;
        public string AesKey = string.Empty;
    }
}