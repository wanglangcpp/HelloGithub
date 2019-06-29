namespace Genesis.GameClient
{
    /// <summary>
    /// 网络用户自定义错误数据。
    /// </summary>
    public class NetworkCustomErrorData
    {
        public NetworkCustomErrorData(int errorCode, string errorMessage, PacketType packetType, int packetActionId)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            PacketType = packetType;
            PacketActionId = packetActionId;
        }

        public int ErrorCode { get; private set; }

        public string ErrorMessage { get; private set; }

        public PacketType PacketType { get; private set; }

        public int PacketActionId { get; private set; }
    }
}
