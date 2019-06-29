using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技获取到战报数据的事件。
    /// </summary>
    public class OfflineArenaReportListChangedEventArgs : GameEventArgs
    {
        public OfflineArenaReportListChangedEventArgs()
        {

        }

        public override int Id
        {
            get { return (int)(EventId.OfflineArenaReportListChanged); }
        }
    }
}
