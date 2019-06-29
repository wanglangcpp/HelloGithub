using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 房间连接关闭事件。
    /// </summary>
    public class RoomClosedEventArgs : GameEventArgs
    {
        public RoomClosedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.RoomClosed;
            }
        }
    }
}
