using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 停止 PVP 匹配的事件。
    /// </summary>
    public class StopPvpMatchingEventArgs : GameEventArgs
    {
        public StopPvpMatchingEventArgs(PvpType pvpType)
        {
            PvpType = pvpType;
        }

        public override int Id
        {
            get { return (int)(EventId.StopPvpMatchingEventArgs); }
        }

        public PvpType PvpType
        {
            get;
            private set;
        }
    }
}
