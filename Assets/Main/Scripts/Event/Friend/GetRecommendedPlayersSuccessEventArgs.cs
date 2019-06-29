using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 获取推荐玩家数据成功的事件。
    /// </summary>
    public class GetRecommendedPlayersSuccessEventArgs : GameEventArgs
    {
        public GetRecommendedPlayersSuccessEventArgs(LCGetRecommendedPlayers packet)
        {
            Packet = packet;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.GetRecommendedPlayersSuccess;
            }
        }

        public LCGetRecommendedPlayers Packet { get; private set; }
    }
}
