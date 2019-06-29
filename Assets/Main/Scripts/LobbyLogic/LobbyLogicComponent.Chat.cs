namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        public void SendChat(ChatType channel, int receiverPlayerId, string message)
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return;
            }
            if (channel == ChatType.World)
            {
                GameEntry.Data.Chat.LastSendWorldChatTime = UnityEngine.Time.time;//GameEntry.Time.LobbyServerUtcTime;
            }
            CLSendChat msg = new CLSendChat();
            msg.Channel = (int)channel;
            msg.Message = message;
            msg.ReceiverPlayerId = receiverPlayerId;
            GameEntry.Network.Send(msg);
        }
    }
}
