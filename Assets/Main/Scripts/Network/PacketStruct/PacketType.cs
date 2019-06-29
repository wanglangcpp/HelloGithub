namespace Genesis.GameClient
{
    public enum PacketType : byte
    {
        Undefined = 0,
        ClientToLobbyServer,
        LobbyServerToClient,
        ClientToRoomServer,
        RoomServerToClient,
    }
}
