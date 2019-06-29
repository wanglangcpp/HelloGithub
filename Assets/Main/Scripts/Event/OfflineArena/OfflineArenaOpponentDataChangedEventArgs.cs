using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技获取到对手数据的事件。
    /// </summary>
    public class OfflineArenaOpponentDataChangedEventArgs : GameEventArgs
    {
        public OfflineArenaOpponentDataChangedEventArgs()
        {

        }

        public override int Id
        {
            get { return (int)(EventId.OfflineArenaOpponentDataChanged); }
        }
    }
}
