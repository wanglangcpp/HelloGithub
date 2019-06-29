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
        private class StrategyLobby : StrategyBase
        {
            private LobbyConnection m_LobbyConnection;

            public StrategyLobby(NetworkHelper owner) : base(owner)
            {
                m_LobbyConnection = new LobbyConnection();
                GameEntry.Event.Subscribe(EventId.RequestConnectLobbyServer, OnRequestConnectLobbyServer);
                GameEntry.Event.Subscribe(EventId.RequestSignInLobbyServer, OnRequestSignInLobbyServer);
            }

            public override bool SendHeartBeat(INetworkChannel networkChannel)
            {
                if (GameEntry.Data.Player.Id <= 0)
                {
                    // 未登录
                    return false;
                }

                CLHeartBeat pbHeartBeat = new CLHeartBeat();
                pbHeartBeat.ClientTime = DateTime.UtcNow.Ticks;
                networkChannel.Send(pbHeartBeat);
                return true;
            }

            public override void Serialize<T>(INetworkChannel networkChannel, Stream destination, T packet, PacketBase packetImpl)
            {
                CLPacketHead clPacketHead = new CLPacketHead();
                clPacketHead.MsgId = ++s_MessageSerialId;
                clPacketHead.ActionId = packetImpl.PacketActionId;
                clPacketHead.SessionId = string.Empty;
                clPacketHead.UserId = 0;
                Serializer.SerializeWithLengthPrefix(destination, clPacketHead, PrefixStyle.Fixed32);
                Serializer.Serialize(destination, packet);
            }

            public override Packet Deserialize(INetworkChannel networkChannel, Stream source, out object customErrorData)
            {
                LCPacketHead lcPacketHead = Serializer.DeserializeWithLengthPrefix<LCPacketHead>(source, PrefixStyle.Fixed32);
                if (lcPacketHead == null)
                {
                    throw new GameFrameworkException("Can not deserialize packet header for lobby.");
                }
                customErrorData = new NetworkCustomErrorData(lcPacketHead.ErrorCode, lcPacketHead.ErrorMessage, lcPacketHead.PacketType, lcPacketHead.PacketActionId);
                if (lcPacketHead.ErrorCode != 0)
                {
                    return null;
                }
                int lcOpCode = GetOpCode(lcPacketHead.PacketType, lcPacketHead.PacketActionId);
                Type lcPacketType = m_Owner.GetPacketType(lcOpCode);
                if (lcPacketType == null)
                {
                    PacketType pt = PacketType.Undefined;
                    int aid = 0;
                    ParseOpCode(lcOpCode, out pt, out aid);
                    throw new GameFrameworkException(string.Format("Can not deserialize packet for packet type '{0}', action id '{1}'.", pt.ToString(), aid.ToString()));
                }
                else
                {
                    source = CheckIfNeedDecompressStream(source);
                    return (PacketBase)RuntimeTypeModel.Default.Deserialize(source, null, lcPacketType);
                }
            }

            public override void OnNetworkConnected(object sender, UnityGameFramework.Runtime.NetworkConnectedEventArgs ne)
            {
                base.OnNetworkConnected(sender, ne);
                m_LobbyConnection.OnNetworkConnected(sender, ne);
                GameEntry.Time.ResetLobbyServerTime();
                autoReconnect = true;
            }

            public override void OnNetworkClosed(object sender, UnityGameFramework.Runtime.NetworkClosedEventArgs ne)
            {
                base.OnNetworkClosed(sender, ne);
                m_LobbyConnection.OnNetworkClosed(sender, ne);
                if (m_Owner.m_StartRoomWaiting)
                {
                    m_Owner.RoomWaiting(false);
                }
                INetworkChannel channelLobby = GameEntry.Network.GetNetworkChannel(Constant.Network.RoomNetworkChannelName);
                if (channelLobby!=null)
                {
                    channelLobby.Close();
                }
                if (autoReconnect == false)
                {
                    return;
                }
                if (!m_LobbyConnection.IsConnect)
                {
                    GameEntry.Data.Player.Id = 0;
                    Reconnect();
                }
            }

            public override void OnNetworkError(object sender, UnityGameFramework.Runtime.NetworkErrorEventArgs ne)
            {
                base.OnNetworkError(sender, ne);
                GameEntry.Restart();
            }

            private void OnRequestConnectLobbyServer(object sender, GameEventArgs e)
            {
                m_LobbyConnection.Connect();
            }

            private void OnRequestSignInLobbyServer(object sender, GameEventArgs e)
            {
                m_LobbyConnection.SignIn();
            }

            private void Reconnect()
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_NETWORK_RECONNECTTING"));
                m_LobbyConnection.Connect();
            }

        }
    }
}
