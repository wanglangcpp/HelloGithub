using UnityEngine;
using System.Collections;
using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class OrderRspData
    {
        public Order data;
        public int retCode = -1;
        public string retMsg = string.Empty;
        //{"data":{"orderId":"DL20180127115551288010"},"retCode":0,"retMsg":"订单创建成功"}
        [Serializable]
        public class Order
        {
            public string orderId = string.Empty;
        }
    }


}