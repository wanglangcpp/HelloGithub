using GameFramework;
using System;
using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class InstanceForResourceLogic
    {
        protected override AbstractInstanceFailure CreateFailureResult(bool shouldOpenUI)
        {
            return new InstanceResultFailure(shouldOpenUI);
        }

        protected override AbstractInstanceSuccess CreateSuccessResult()
        {
            return new InstanceResultSuccess();
        }

        private class InstanceResultSuccess : BaseSinglePlayerInstanceSuccess
        {
            protected override void SendLeaveInstanceRequest()
            {
                var instanceLogic = InstanceLogic as InstanceForResourceLogic;
                GameEntry.LobbyLogic.LeaveInstanceForResource(instanceLogic.m_InstanceId, instanceLogic.m_RewardLevel, instanceLogic.m_DeadDropCoins, true);
            }

            protected override UIFormBaseUserData PopulateData(GameEventArgs e)
            {
                var ne = e as LeaveInstanceForResourceResponseEventArgs;
                //TODO:csf 后期材料统一处理
                int exp = 0;
                List<PBItemInfo> items = new List<PBItemInfo>();
                for (int i = 0; i < ne.Packet.CompoundItemInfos.Count; i++)
                {
                    if (ne.Packet.CompoundItemInfos[i].AutoUseItemInfo.AutoUseItemInfo.Type == 2)
                    {
                        exp += ne.Packet.CompoundItemInfos[i].AutoUseItemInfo.AutoUseItemInfo.Count;
                    }
                }
                var ret = new InstanceForResourceResultData
                {
                    CoinObtained = ne.Packet.RewardCoin > 0 ? ne.Packet.RewardCoin : exp,
                    InstanceForResourceId = ne.Packet.InstanceForResourceId,
                };
                return ret;
            }

            protected override void ShowResultUI(UIFormBaseUserData userData)
            {
                GameEntry.UI.OpenUIForm(UIFormId.InstanceForResourceResult, userData);
            }

            public override void Init(BaseInstanceLogic instanceLogic, InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                base.Init(instanceLogic, reason, onComplete);
                GameEntry.Event.Subscribe(EventId.LeaveInstanceForResourceResponse, OnReceiveLeaveInstanceResponse);
            }

            public override void ShutDown()
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.LeaveInstanceForResourceResponse, OnReceiveLeaveInstanceResponse);
                }

                base.ShutDown();
            }
        }

        private class InstanceResultFailure : BaseSinglePlayerInstanceFailure
        {
            public InstanceResultFailure(bool shouldOpenUI) : base(shouldOpenUI)
            {

            }

            public override void Init(BaseInstanceLogic instanceLogic, InstanceFailureReason reason, GameFrameworkAction onComplete)
            {
                base.Init(instanceLogic, reason, onComplete);
                GameEntry.Event.Subscribe(EventId.LeaveInstanceForResourceResponse, OnReceiveLeaveInstanceResponse);
            }

            public override void ShutDown()
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.LeaveInstanceForResourceResponse, OnReceiveLeaveInstanceResponse);
                }

                base.ShutDown();
            }

            protected override void SendLeaveInstanceRequest()
            {
                var instanceLogic = InstanceLogic as InstanceForResourceLogic;
                GameEntry.LobbyLogic.LeaveInstanceForResource(instanceLogic.m_InstanceId, instanceLogic.m_RewardLevel, 0, false);
            }

            protected override void ShowResultUI(UIFormBaseUserData userData)
            {
                throw new NotImplementedException();
            }
        }
    }
}
