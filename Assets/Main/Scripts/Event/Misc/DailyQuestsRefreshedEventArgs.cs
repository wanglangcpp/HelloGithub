using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 每日任务主动刷新事件。
    /// </summary>
    public class DailyQuestsRefreshedEventArgs : GameEventArgs
    {
        public DailyQuestsRefreshedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.DailyQuestsRefreshed;
            }
        }
    }
}
