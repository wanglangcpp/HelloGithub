using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.IO;

namespace Genesis.GameClient
{
    public partial class NetworkHelper
    {
        private class StrategyRoom : StrategyBase
        {
            private RoomConnection m_RoomConnection = null;

            public StrategyRoom(NetworkHelper owner) : base(owner)
            {
                GameEntry.Event.Subscribe(EventId.ConnectRoom, OnConnectRoom);
                GameEntry.Event.Subscribe(EventId.ServerError, OnServerError);
            }

            public override bool SendHeartBeat(INetworkChannel networkChannel)
            {
                if (!GameEntry.Data.Room.InRoom || GameEntry.Data.Room.HasReconnected)
                {
                    // 未登录
                    return false;
                }

                CRHeartBeat pbHeartBeat = new CRHeartBeat();
                pbHeartBeat.ClientTime = DateTime.UtcNow.Ticks;
                networkChannel.Send(pbHeartBeat);
                return true;
            }

            public override void Serialize<T>(INetworkChannel networkChannel, Stream destination, T packet, PacketBase packetImpl)
            {
                CRPacketHead crPacketHead = new CRPacketHead();
                crPacketHead.MsgId = ++s_MessageSerialId;
                crPacketHead.ActionId = packetImpl.PacketActionId;
                crPacketHead.SessionId = string.Empty;
                crPacketHead.UserId = 0;
                Serializer.SerializeWithLengthPrefix(destination, crPacketHead, PrefixStyle.Fixed32);
                Serializer.Serialize(destination, packet);
            }

            public override Packet Deserialize(INetworkChannel networkChannel, Stream source, out object customErrorData)
            {
                RCPacketHead rcPacketHead = Serializer.DeserializeWithLengthPrefix<RCPacketHead>(source, PrefixStyle.Fixed32);
                if (rcPacketHead == null)
                {
                    throw new GameFrameworkException("Can not deserialize packet header for room.");
                }
                customErrorData = new NetworkCustomErrorData(rcPacketHead.ErrorCode, rcPacketHead.ErrorMessage, rcPacketHead.PacketType, rcPacketHead.PacketActionId);
                if (rcPacketHead.ErrorCode != 0)
                {
                    return null;
                }
                int rcOpCode = GetOpCode(rcPacketHead.PacketType, rcPacketHead.PacketActionId);
                Type rcPacketType = m_Owner.GetPacketType(rcOpCode);
                if (rcPacketType == null)
                {
                    PacketType pt = PacketType.Undefined;
                    int aid = 0;
                    ParseOpCode(rcOpCode, out pt, out aid);
                    throw new GameFrameworkException(string.Format("Can not deserialize packet for packet type '{0}', action id '{1}'.", pt.ToString(), aid.ToString()));
                }
                else
                {
                    source = CheckIfNeedDecompressStream(source);
                    return (PacketBase)RuntimeTypeModel.Default.Deserialize(source, null, rcPacketType);
                }
            }

            public override void OnNetworkConnected(object sender, UnityGameFramework.Runtime.NetworkConnectedEventArgs ne)
            {
                base.OnNetworkConnected(sender, ne);
                GameEntry.Time.ResetRoomServerTime();
                if (m_RoomConnection != null)
                {
                    m_RoomConnection.OnNetworkConnected(sender, ne);
                }
            }

            public override void OnNetworkClosed(object sender, UnityGameFramework.Runtime.NetworkClosedEventArgs ne)
            {
                base.OnNetworkClosed(sender, ne);

                if (GameEntry.Data.Room.State == RoomStateType.Finish)
                {
                    GameEntry.Waiting.ClearWaitingOfType(WaitingType.RoomBreak);
                    m_RoomConnection.Shutdown();
                    m_RoomConnection = null;
                    GameEntry.Event.Fire(this, new RoomClosedEventArgs());
                    return;
                }

                if (!GameEntry.Network.GetNetworkChannel(Constant.Network.LobbyNetworkChannelName).Connected)
                {
                    //突发性断网
                    GameEntry.Restart();
                    return;
                }

                GameEntry.Waiting.ClearWaitingOfType(WaitingType.RoomBreak);
                if (m_RoomConnection != null && !GameEntry.Data.Room.HasReconnected)
                {
                    m_Owner.RoomWaiting(true);
                    m_RoomConnection.OnNetworkClosed(sender, ne);
                    UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_NETWORK_RECONNECTTING"));
                    m_RoomConnection.Connect(ConnectFinished);
                }
            }

            private void ConnectFinished()
            {
                m_Owner.RoomWaiting(false);
                GameEntry.Data.Room.HasReconnected = false;
            }

            private void OnConnectRoom(object sender, GameEventArgs e)
            {
                if (!GameEntry.Network.GetNetworkChannel(Constant.Network.LobbyNetworkChannelName).Connected)
                {
                    return;
                }

                GameEntry.Waiting.ClearWaitingOfType(WaitingType.RoomBreak);
                m_Owner.RoomWaiting(true);
                if (m_RoomConnection == null)
                {
                    m_RoomConnection = new RoomConnection();
                    m_RoomConnection.ConnectFinished = ConnectFinished;
                }
                else if (!GameEntry.Network.GetNetworkChannel(Constant.Network.RoomNetworkChannelName).Connected)
                {
                    m_RoomConnection.Connect(ConnectFinished);
                }
            }

            private void OnServerError(object sender, GameEventArgs e)
            {
                ServerErrorEventArgs networkCustomErrorData = e as ServerErrorEventArgs;

                if ((ServerErrorCode)networkCustomErrorData.NetworkErrorData.ErrorCode == ServerErrorCode.RoomStatusError)
                {
                    GameEntry.Waiting.ClearWaitingOfType(WaitingType.RoomBreak);
                    m_RoomConnection.Shutdown();
                    m_RoomConnection = null;
                    GameEntry.Event.Fire(this, new RoomClosedEventArgs());
                }
            }

            public override void OnNetworkError(object sender, UnityGameFramework.Runtime.NetworkErrorEventArgs ne)
            {
                base.OnNetworkError(sender, ne);

                GameEntry.SceneLogic.GoBackToLobby(true);
            }
        }
    }
}
