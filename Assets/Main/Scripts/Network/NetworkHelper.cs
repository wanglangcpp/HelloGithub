using GameFramework;
using GameFramework.Event;
using GameFramework.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    /// <summary>
    /// 网络辅助器。
    /// </summary>
    public partial class NetworkHelper : NetworkHelperBase
    {
        private static int s_MessageSerialId = 0;
        private readonly IDictionary<int, Type> m_PacketTypes = new Dictionary<int, Type>();
        private bool m_StartRoomWaiting = false;

        private void Start()
        {
            // 反射注册包和包行为。
            Type packetBaseType = typeof(PacketBase);
            Type packetHandlerBaseType = typeof(IPacketHandler);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (!types[i].IsClass || types[i].IsAbstract)
                {
                    continue;
                }

                if (types[i].BaseType == packetBaseType)
                {
                    PacketBase packetBase = (PacketBase)Activator.CreateInstance(types[i]);
                    if (m_PacketTypes.ContainsKey(packetBase.Id))
                    {
                        Log.Warning("Already exist packet type '{0}', '{1}' or '{2}'?.", packetBase.Id.ToString(), m_PacketTypes[packetBase.Id].Name, packetBase.GetType().Name);
                        continue;
                    }

                    m_PacketTypes.Add(packetBase.Id, types[i]);
                }
                else if (packetHandlerBaseType.IsAssignableFrom(types[i]))
                {
                    IPacketHandler packetHandler = (IPacketHandler)Activator.CreateInstance(types[i]);
                    GameEntry.Network.RegisterHandler(packetHandler);
                }
            }

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.NetworkConnected, OnNetworkConnected);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.NetworkClosed, OnNetworkClosed);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.NetworkSendPacket, OnNetworkSendPacket);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.NetworkMissHeartBeat, OnNetworkMissHeartBeat);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.NetworkError, OnNetworkError);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.NetworkCustomError, OnNetworkCustomError);

            m_Strategies[Constant.Network.LobbyNetworkChannelName] = new StrategyLobby(this);
            m_Strategies[Constant.Network.RoomNetworkChannelName] = new StrategyRoom(this);
        }

        private void OnDestroy()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.NetworkConnected, OnNetworkConnected);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.NetworkClosed, OnNetworkClosed);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.NetworkSendPacket, OnNetworkSendPacket);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.NetworkMissHeartBeat, OnNetworkMissHeartBeat);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.NetworkError, OnNetworkError);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.NetworkCustomError, OnNetworkCustomError);
        }

        /// <summary>
        /// 发送心跳协议包。
        /// </summary>
        public override bool SendHeartBeat(INetworkChannel networkChannel)
        {
            return GetStrategy(networkChannel.Name).SendHeartBeat(networkChannel);
        }

        /// <summary>
        /// 序列化协议包。
        /// </summary>
        /// <typeparam name="T">协议包类型。</typeparam>
        /// <param name="destination">要序列化的目标流。</param>
        /// <param name="packet">要序列化的协议包。</param>
        public override void Serialize<T>(INetworkChannel networkChannel, Stream destination, T packet)
        {
            PacketBase packetImpl = packet as PacketBase;
            if (packetImpl == null)
            {
                throw new GameFrameworkException("Packet is invalid.");
            }

            GetStrategy(networkChannel.Name).Serialize(networkChannel, destination, packet, packetImpl);
        }

        /// <summary>
        /// 反序列化协议包。
        /// </summary>
        /// <param name="source">要反序列化的来源流。</param>
        /// <param name="customErrorData">用户自定义网络错误数据。</param>
        /// <returns>反序列化后的协议包。</returns>
        public override Packet Deserialize(INetworkChannel networkChannel, Stream source, out object customErrorData)
        {
            // 注意：此函数并不在主线程调用！
            return GetStrategy(networkChannel.Name).Deserialize(networkChannel, source, out customErrorData);
        }

        public static int GetOpCode(PacketType packetType, int packetActionId)
        {
            if (packetActionId < 0 || packetActionId > 0x00ffffff)
            {
                throw new GameFrameworkException("Packet action id is invalid.");
            }

            return ((int)packetType << 24) | packetActionId;
        }

        public static void ParseOpCode(int opCode, out PacketType packetType, out int packetActionId)
        {
            packetType = (PacketType)(opCode >> 24);
            packetActionId = opCode & 0x00ffffff;
        }

        private Type GetPacketType(int opCode)
        {
            Type packetType = null;
            if (m_PacketTypes.TryGetValue(opCode, out packetType))
            {
                return packetType;
            }

            return null;
        }

        private static bool IsCompressedStream(Stream stream)
        {
            if (stream == null)
            {
                return false;
            }

            long currentPosition = stream.Position;
            if (stream.Length - currentPosition < 14)
            {
                return false;
            }

            bool compressedFlag = (stream.ReadByte() == 0 && stream.ReadByte() == 0 && stream.ReadByte() == 0 && stream.ReadByte() == 0 && stream.ReadByte() == 0x1F && stream.ReadByte() == 0x8B);
            stream.Position = currentPosition;
            return compressedFlag;
        }

        private static Stream CheckIfNeedDecompressStream(Stream source)
        {
            if (IsCompressedStream(source))
            {
                for (int i = 0; i < 4; i++)
                {
                    source.ReadByte();
                }
                long length = source.Length - source.Position;
                byte[] bytes = new byte[length];
                source.Read(bytes, 0, (int)length);
                bytes = Utility.Zip.Decompress(bytes);
                source = new MemoryStream(bytes);
            }

            return source;
        }

        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkConnectedEventArgs ne = e as UnityGameFramework.Runtime.NetworkConnectedEventArgs;
            Log.Info("Network channel '{0}' connected, local address '{1}:{2}', remote address '{3}:{4}'.", ne.NetworkChannel.Name, ne.NetworkChannel.LocalIPAddress, ne.NetworkChannel.LocalPort.ToString(), ne.NetworkChannel.RemoteIPAddress, ne.NetworkChannel.RemotePort.ToString());
            GetStrategy(ne.NetworkChannel.Name).OnNetworkConnected(sender, ne);
        }

        private void OnNetworkClosed(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkClosedEventArgs ne = e as UnityGameFramework.Runtime.NetworkClosedEventArgs;
            Log.Info("Network channel '{0}' closed.", ne.NetworkChannel.Name);
            GetStrategy(ne.NetworkChannel.Name).OnNetworkClosed(sender, ne);
        }

        private void OnNetworkSendPacket(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkSendPacketEventArgs ne = e as UnityGameFramework.Runtime.NetworkSendPacketEventArgs;
            GetStrategy(ne.NetworkChannel.Name).OnNetworkSendPacket(sender, ne);
        }

        private void OnNetworkMissHeartBeat(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs ne = e as UnityGameFramework.Runtime.NetworkMissHeartBeatEventArgs;
            Log.Info("Network channel '{0}' miss heart beat '{1}' times.", ne.NetworkChannel.Name, ne.MissCount.ToString());
            GetStrategy(ne.NetworkChannel.Name).OnNetworkMissHeartBeat(sender, ne);
        }

        private void OnNetworkError(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkErrorEventArgs ne = e as UnityGameFramework.Runtime.NetworkErrorEventArgs;
            Log.Info("Network channel '{0}' error, error code is '{1}', error message is '{2}'.", ne.NetworkChannel.Name, ne.ErrorCode.ToString(), ne.ErrorMessage);
            GetStrategy(ne.NetworkChannel.Name).OnNetworkError(sender, ne);
        }

        private void OnNetworkCustomError(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.NetworkCustomErrorEventArgs ne = e as UnityGameFramework.Runtime.NetworkCustomErrorEventArgs;
            NetworkCustomErrorData networkCustomErrorData = ne.CustomErrorData as NetworkCustomErrorData;
            Log.Info("Network channel '{0}' custom error, error code is '{1}', error message is '{2}'.", ne.NetworkChannel.Name, networkCustomErrorData.ErrorCode.ToString(), networkCustomErrorData.ErrorMessage);
            GetStrategy(ne.NetworkChannel.Name).OnNetworkCustomError(sender, ne);
        }

        private void RoomWaiting(bool startRoomWaiting)
        {
            m_StartRoomWaiting = startRoomWaiting;
            if (startRoomWaiting)
            {
                GameEntry.Waiting.StartWaiting(WaitingType.RoomBreak, Constant.Network.RoomNetworkChannelName);
            }
            else
            {
                GameEntry.Waiting.StopWaiting(WaitingType.RoomBreak, Constant.Network.RoomNetworkChannelName);
            }
        }
    }
}
