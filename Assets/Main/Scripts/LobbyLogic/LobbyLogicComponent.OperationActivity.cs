using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        private static readonly Dictionary<string, string> DailyLoginGetDataResponse = new Dictionary<string, string>
        {
            { "DayCnt", "7" },
            { "ClaimedCnt", "4" },
            { "HasClaimed", "false" }, // 今日是否已经领奖。
            { "StartDate", "2016.01.01" },
            { "EndDate", "2016.12.31" },
            { "Reward.0.ItemId", "202101" },
            { "Reward.0.ItemCnt", "10" },
            { "Reward.1.ItemId", "202202" },
            { "Reward.1.ItemCnt", "5" },
            { "Reward.2.ItemId", "202303" },
            { "Reward.2.ItemCnt", "2" },
            { "Reward.3.ItemId", "202404" },
            { "Reward.3.ItemCnt", "1" },
            { "Reward.4.ItemId", "113001" },
            { "Reward.4.ItemCnt", "1" },
            { "Reward.5.ItemId", "113002" },
            { "Reward.5.ItemCnt", "1" },
            { "Reward.6.ItemId", "113003" },
            { "Reward.6.ItemCnt", "1" },
        };

        private static readonly Dictionary<string, string> DailyLoginClaimRewardResponse = new Dictionary<string, string>
        {
            { "ClaimedCnt", "5" },
            { "HasClaimed", "true" },
        };

        /// <summary>
        /// 请求运营活动的信息。
        /// </summary>
        /// <param name="requestData">请求数据。</param>
        public void OperationActivityRequest(IDictionary<string, string> requestData)
        {
            if (requestData == null)
            {
                requestData = new Dictionary<string, string>();
            }

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                var request = new CLOperationActivity { };

                foreach (var kv in requestData)
                {
                    request.Params.Add(new PBKeyValuePair { Key = kv.Key, Value = kv.Value });
                }

                GameEntry.Network.Send(request);
                return;
            }

            var responseData = new Dictionary<string, string>();
            responseData["AId"] = requestData["AId"];
            responseData["Op"] = requestData["Op"];

            IDictionary<string, string> appendingData = null;
            switch (int.Parse(requestData["AId"]))
            {
                case 1: // Daily Login
                    appendingData = GetAppendingDataForDailyLogin(requestData["Op"]);
                    break;
                default:
                    appendingData = new Dictionary<string, string>();
                    break;
            }

            var response = new LCOperationActivity();

            foreach (var kv in responseData)
            {
                response.Params.Add(new PBKeyValuePair { Key = kv.Key, Value = kv.Value });
            }

            foreach (var kv in appendingData)
            {
                response.Params.Add(new PBKeyValuePair { Key = kv.Key, Value = kv.Value });
            }

            if (responseData["Op"] == "ClaimReward")
            {
                response.ReceivedItems = new PBReceivedItems();
                int itemId = 202101;
                int itemCount;
                var itemData = GameEntry.Data.Items.GetData(itemId);
                if (itemData == null)
                {
                    itemCount = 0;
                }
                else
                {
                    itemCount = itemData.Count;
                }

                response.ReceivedItems.ItemInfo.Add(new PBItemInfo { Type = 202101, Count = itemCount + 5 });
            }

            LCOperationActivityHandler.Handle(this, response);
        }

        private static IDictionary<string, string> GetAppendingDataForDailyLogin(string op)
        {
            return op == "GetData" ? DailyLoginGetDataResponse
                : op == "ClaimReward" ? DailyLoginClaimRewardResponse
                : new Dictionary<string, string>();
        }
    }
}
