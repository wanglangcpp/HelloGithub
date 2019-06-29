using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离开副本请求回包事件。
    /// </summary>
    public class LeaveInstanceResponseEventArgs : GameEventArgs
    {
        public LeaveInstanceResponseEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.LeaveInstanceResponse;
            }
        }
    }
}
