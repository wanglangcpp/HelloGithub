using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时间缩放任务结束事件。
    /// </summary>
    public class TimeScaleTaskFinishEventArgs : GameEventArgs
    {
        public TimeScaleTaskFinishEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.TimeScaleTaskFinish;
            }
        }
    }
}
