using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 摄像机动画停止事件。
    /// </summary>
    public class CameraAnimStoppedEventArgs : GameEventArgs
    {
        public CameraAnimStoppedEventArgs(string animName)
        {
            AnimName = animName;
        }

        public string AnimName { get; private set; }

        public override int Id
        {
            get
            {
                return (int)EventId.CameraAnimStopped;
            }
        }
    }
}
