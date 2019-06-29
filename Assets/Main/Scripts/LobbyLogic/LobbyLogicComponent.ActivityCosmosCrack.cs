using UnityEngine;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        private ActivityCosmosCrack m_CosmosCrack = new ActivityCosmosCrack();

        /// <summary>
        /// 获取时空裂缝活动对象。
        /// </summary>
        public ActivityCosmosCrack CosmosCrack
        {
            get
            {
                return m_CosmosCrack;
            }
        }

        /// <summary>
        /// 时空裂缝活动类。
        /// </summary>
        public class ActivityCosmosCrack
        {
            /// <summary>
            /// 请求数据。
            /// </summary>
            public void RequestData()
            {
                if (!GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    var request = new CLGetCosmosCrackInfo();
                    GameEntry.Network.Send(request);
                    return;
                }

                var response = new LCGetCosmosCrackInfo
                {
                    UsedRoundCount = 5,
                };

                for (int i = 0; i < 3; ++i)
                {
                    var cosmosInstanceInfo = new PBCosmosCrackInstanceInfo
                    {
                        InstanceType = i + 1,
                        RewardLevel = i + 1,
                    };

                    for (int j = 0; j < Random.Range(1, 4); ++j)
                    {
                        var reward = new PBItemInfo
                        {
                            Type = 202101 + j * 101,
                            Count = j + 1,
                        };

                        cosmosInstanceInfo.Rewards.Add(reward);
                    }

                    response.InstanceInfos.Add(cosmosInstanceInfo);
                }

                LCGetCosmosCrackInfoHandler.Handle(this, response);
            }

            /// <summary>
            /// 请求进入副本。
            /// </summary>
            /// <param name="instanceId">副本编号。</param>
            public void RequestEnterInstance(int instanceId)
            {
                if (!GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    var request = new CLEnterCosmosCrackInstance
                    {
                        InstanceType = instanceId,
                    };

                    GameEntry.Network.Send(request);
                    return;
                }

                var response = new LCEnterCosmosCrackInstance
                {
                    InstanceType = instanceId,
                    NpcLevel = 10,
                };

                LCEnterCosmosCrackInstanceHandler.Handle(this, response);
            }

            /// <summary>
            /// 请求离开副本。
            /// </summary>
            /// <param name="instanceId">副本编号。</param>
            /// <param name="win">是否获胜。</param>
            public void RequestLeaveInstance(int instanceId, bool win)
            {
                if (!GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    var request = new CLLeaveCosmosCrackInstance
                    {
                        InstanceType = instanceId,
                        Win = win,
                    };

                    GameEntry.Network.Send(request);
                    return;
                }

                var response = new LCLeaveCosmosCrackInstance
                {
                    InstanceType = instanceId,
                    RewardLevel = 2,
                };

                response.ReceivedItems.ItemInfo.Add(new PBItemInfo { Type = 202101, Count = 3, });
                response.ReceivedItems.GearInfo.Add(new PBGearInfo { Type = 111001, Id = 1, });
                response.ReceivedItems.SoulInfo.Add(new PBSoulInfo { Type = 171001, Id = 4444444, });
                LCLeaveCosmosCrackInstanceHandler.Handle(this, response);
            }
        }
    }
}
