using GameFramework;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public static class NetworkExtension
    {
        public static void Reset()
        {

        }

        public static void Send<T>(this NetworkComponent networkComponent, T packet, object userData = null) where T : Packet
        {
            if (packet == null)
            {
                Log.Warning("Network packet is invalid.");
            }
            PacketType packetType = PacketType.Undefined;
            int actionId = 0;
            NetworkHelper.ParseOpCode(packet.Id, out packetType, out actionId);

            if (NetworkHelper.NeedWaiting(packetType, actionId))
            {
                GameEntry.Waiting.StartWaiting(WaitingType.Network, actionId.ToString());
            }

            if (packetType == PacketType.ClientToLobbyServer)
            {
                networkComponent.SendToLobby(packet, userData);
            }
            else if (packetType == PacketType.ClientToRoomServer)
            {
                networkComponent.SendToRoom(packet, userData);
            }
            else
            {
                Log.Warning("You can not send packet with opcode '{0}'.", packet.Id.ToString());
            }
#if CSF
            NetLog.LogPackage(packet);
#endif
        }

        private static void SendToLobby<T>(this NetworkComponent networkComponent, T packet, object userData) where T : Packet
        {
            var lobbyNetworkChannel = networkComponent.GetNetworkChannel(Constant.Network.LobbyNetworkChannelName);
            if (lobbyNetworkChannel == null)
            {
                Log.Warning("Can not find lobby network channel.");
                return;
            }

            if (!lobbyNetworkChannel.Connected)
            {
                return;
            }

            lobbyNetworkChannel.Send(packet, userData);
        }

        private static void SendToRoom<T>(this NetworkComponent networkComponent, T packet, object userData) where T : Packet
        {
            var roomNetworkChannel = networkComponent.GetNetworkChannel(Constant.Network.RoomNetworkChannelName);
            if (roomNetworkChannel == null)
            {
                Log.Warning("Can not find room network channel.");
                return;
            }

            if (!roomNetworkChannel.Connected)
            {
                return;
            }

            roomNetworkChannel.Send(packet, userData);
        }


    }
}
