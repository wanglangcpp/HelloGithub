using UnityEngine;
using System.Collections;
using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class OrderRequestData
    {
        public int d = 0;//DEVICE_CODE
        public string app = string.Empty;//app_code
        public string iId = string.Empty;//itemId
        public string uId = string.Empty;//userId
        public int a = 0;//Amount
        public int p = 0;//TruthPrice
        public int pp = 0;//OrgPrice
        public string CC = string.Empty;//ChannelID
        public string gsId = string.Empty;//GameServerId
        public string an = "";//accountNumber

    }
}
