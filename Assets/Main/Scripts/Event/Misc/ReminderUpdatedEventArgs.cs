using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 提醒器更新事件。
    /// </summary>
    public class ReminderUpdatedEventArgs : GameEventArgs
    {
        public ReminderUpdatedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ReminderUpdated;
            }
        }
    }
}
