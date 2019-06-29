using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 开始 PVP 匹配的事件。
    /// </summary>
    public class StartPvpMatchingEventArgs : GameEventArgs
    {
        public StartPvpMatchingEventArgs(PvpType pvpType)
        {
            PvpType = pvpType;
        }

        public override int Id
        {
            get { return (int)(EventId.StartPvpMatchingEventArgs); }
        }

        public PvpType PvpType
        {
            get;
            private set;
        }
    }
}
