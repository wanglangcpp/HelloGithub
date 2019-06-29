using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 获取每日任务进度事件。
    /// </summary>
    public class UpdateDailyQuestEventArgs : GameEventArgs
    {
        public UpdateDailyQuestEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.UpdateDailyQuest;
            }
        }
    }
}
