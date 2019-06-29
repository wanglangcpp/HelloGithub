using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class NetworkHelper
    {
        private static readonly Dictionary<PacketType, HashSet<int>> s_LobbyNotNeedWaitingPackets = new Dictionary<PacketType, HashSet<int>>
        {
            {
                PacketType.ClientToLobbyServer, new HashSet<int>
                {
                    999,
                    1001,
                    1006,
                    1008,
                    1051,
                    1060,
                    1061,
                    1070,
                    2305,
                    2306,
                    2307,
                    2602,
                    3000,
                    3001,
                    3002,
                    3107,
                    3108,
                    3200,
                    4001, // TODO: 从此开始为未定。
                    4002,
                    4003,
                    4004,
                    4005,
                    4006,
                    5016,
                    1072,
                }
            },
        };

        private static readonly Dictionary<PacketType, HashSet<int>> s_RoomNeedWaitingPackets = new Dictionary<PacketType, HashSet<int>>
        {
            {
                PacketType.ClientToRoomServer, new HashSet<int>
                {
                    5003,
                }
            },
        };

        public static bool NeedWaiting(PacketType packetType, int packetActionId)
        {
            if (packetType == PacketType.ClientToLobbyServer || packetType == PacketType.LobbyServerToClient)
            {
                HashSet<int> s;
                if (!s_LobbyNotNeedWaitingPackets.TryGetValue(PacketType.ClientToLobbyServer, out s) || s == null)
                {
                    return true;
                }

                return !s.Contains(packetActionId);
            }

            if (packetType == PacketType.ClientToRoomServer || packetType == PacketType.RoomServerToClient)
            {
                HashSet<int> s;
                if (s_RoomNeedWaitingPackets.TryGetValue(PacketType.ClientToRoomServer, out s) || s == null)
                {
                    return false;
                }

                return s.Contains(packetActionId);
            }

            return false;
        }
    }
}
