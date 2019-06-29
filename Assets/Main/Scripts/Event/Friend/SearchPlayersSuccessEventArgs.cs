using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 搜索玩家成功的事件。
    /// </summary>
    public class SearchPlayersSuccessEventArgs : GameEventArgs
    {
        public SearchPlayersSuccessEventArgs(LCSearchPlayers packet)
        {
            Packet = packet;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.SearchPlayersSuccess;
            }
        }

        public LCSearchPlayers Packet { get; private set; }
    }
}
