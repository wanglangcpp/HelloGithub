namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="ChatForm"/> 显示数据。
    /// </summary>
    public class ChatDisplayData : UIFormBaseUserData
    {
        public ChatType ChatType { get; set; }

        public PlayerData ChatPlayer { get; set; }
    }
}
