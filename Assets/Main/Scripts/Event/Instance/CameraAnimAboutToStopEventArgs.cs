using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 摄像机动画即将结束事件。
    /// </summary>
    public class CameraAnimAboutToStopEventArgs : GameEventArgs
    {
        public CameraAnimAboutToStopEventArgs(string animName)
        {
            AnimName = animName;
        }

        public string AnimName { get; private set; }

        public override int Id
        {
            get
            {
                return (int)EventId.CameraAnimAboutToStop;
            }
        }
    }
}
