using GameFramework;
using GameFramework.Network;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class NetworkHelper
    {
        private Dictionary<string, StrategyBase> m_Strategies = new Dictionary<string, StrategyBase>();

        private StrategyBase GetStrategy(string name)
        {
            StrategyBase strategy = null;
            lock (m_Strategies)
            {
                if (!m_Strategies.TryGetValue(name, out strategy))
                {
                    Log.Error("Network helper strategy with name '{0}' cannot be found.", name);
                }
            }

            return strategy;
        }

        private abstract class StrategyBase
        {
            protected readonly NetworkHelper m_Owner;
            protected bool autoReconnect = true;
            public StrategyBase(NetworkHelper owner)
            {
                m_Owner = owner;
            }

            public abstract bool SendHeartBeat(INetworkChannel networkChannel);

            public abstract void Serialize<T>(INetworkChannel networkChannel, Stream destination, T packet, PacketBase packetImpl);

            public abstract Packet Deserialize(INetworkChannel networkChannel, Stream source, out object customErrorData);

            public virtual void OnNetworkConnected(object sender, UnityGameFramework.Runtime.NetworkConnectedEventArgs ne)
            {

            }

            public virtual void OnNetworkClosed(object sender, UnityGameFramework.Runtime.NetworkClosedEventArgs ne)
            {

            }

            public virtual void OnNetworkSendPacket(object sender, UnityGameFramework.Runtime.NetworkSendPacketEventArgs ne)
            {

            }

            public virtual void OnNetworkMissHeartBeat(object sender, UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs ne)
            {
                if (ne.MissCount < 2)
                {
                    return;
                }

                ne.NetworkChannel.Close();
            }

            public virtual void OnNetworkError(object sender, UnityGameFramework.Runtime.NetworkErrorEventArgs ne)
            {
                if (Debug.isDebugBuild)
                {
                    //Debug.LogError(ne.Id + "------------"+ne.NetworkChannel.Name+"---------------"+ne.ErrorCode+"-------------" + ne.ErrorMessage);
                }
                ne.NetworkChannel.Close();
            }

            public virtual void OnNetworkCustomError(object sender, UnityGameFramework.Runtime.NetworkCustomErrorEventArgs ne)
            {
                NetworkCustomErrorData networkCustomErrorData = ne.CustomErrorData as NetworkCustomErrorData;
                string debugErrorInfo = string.Format("Channel: {0}\nError Code: {1}\nError Message: {2}\nPacket Type: {3}\nAction Id: {4}",
                    ne.NetworkChannel.Name,
                    networkCustomErrorData.ErrorCode.ToString(),
                    networkCustomErrorData.ErrorMessage,
                    networkCustomErrorData.PacketType.ToString(),
                    networkCustomErrorData.PacketActionId.ToString());
                Log.Info(debugErrorInfo);

                if (NeedWaiting(networkCustomErrorData.PacketType, networkCustomErrorData.PacketActionId))
                {
                    GameEntry.Waiting.StopWaiting(WaitingType.Network, networkCustomErrorData.PacketActionId.ToString());
                }

                const int ServerException = (int)ServerErrorCode.ServerException;
                string dialogKey = string.Format("ERROR_MESSAGE_{0}", ((ServerErrorCode)networkCustomErrorData.ErrorCode).ToString().ToUpper());
                string dialogMessage = GameEntry.Localization.GetString(dialogKey);
                string toastMessage = dialogMessage;
                if (Debug.isDebugBuild)
                {
                    dialogMessage += "\n" + debugErrorInfo;
                }

                GameEntry.Event.Fire(this, new NetworkCustomErrorEventArgs((ServerErrorCode)networkCustomErrorData.ErrorCode));
                //断开网络，防止心跳包爆炸
                if (networkCustomErrorData.ErrorCode >= ServerException)
                {
                    var nc = GameEntry.Network.GetNetworkChannel(ne.NetworkChannel.Name);
                    if (nc == null)
                    {
                        return;
                    }
                    autoReconnect = false;
                    nc.Close();
                    //if (((ServerErrorCode)networkCustomErrorData.ErrorCode)== ServerErrorCode.DuplicateLogin)
                    //{
                    //    GameEntry.Restart();
                    //}
                    UIUtility.ShowRestartDialog(dialogMessage);
                    return;
                }

                ServerErrorHandlingType errorHandlingType;
                if (!s_ErrorHandlingTypes.TryGetValue((ServerErrorCode)networkCustomErrorData.ErrorCode, out errorHandlingType))
                {
                    errorHandlingType = ServerErrorHandlingType.Toast;
                }

                switch (errorHandlingType)
                {
                    case ServerErrorHandlingType.Toast:
                        UIUtility.ShowToast(toastMessage);
                        break;
                    case ServerErrorHandlingType.DialogWithOkayButton:
                        UIUtility.ShowOkayButtonDialog(dialogMessage);
                        break;
                    case ServerErrorHandlingType.DialogWithRestartButton:
                        UIUtility.ShowRestartDialog(dialogMessage);
                        break;
                    case ServerErrorHandlingType.SendServerErrorEventMessage:
                        GameEntry.Event.Fire(this, new ServerErrorEventArgs(networkCustomErrorData));
                        break;
                    case ServerErrorHandlingType.DialogWithRestartAndCancelButton:
                    default:
                        UIUtility.ShowRestartAndCancelDialog(dialogMessage);
                        break;
                }
            }
        }
    }
}
