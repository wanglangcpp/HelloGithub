using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技刷新玩家列表成功的事件。
    /// </summary>
    public class OfflineArenaPlayerListChangedEventArgs : GameEventArgs
    {
        public OfflineArenaPlayerListChangedEventArgs()
        {

        }

        public override int Id
        {
            get { return (int)(EventId.OfflineArenaPlayerListChanged); }
        }
    }
}
