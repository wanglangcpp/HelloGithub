using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技获取到竞技场数据的事件。
    /// </summary>
    public class OfflineArenaDataChangedEventArgs : GameEventArgs
    {
        public OfflineArenaDataChangedEventArgs()
        {

        }

        public override int Id
        {
            get { return (int)(EventId.OfflineArenaDataChanged); }
        }
    }
}
