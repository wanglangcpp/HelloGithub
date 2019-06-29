using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 摄像机动画加载失败事件。
    /// </summary>
    public class CameraAnimLoadFailureEventArgs : GameEventArgs
    {
        public CameraAnimLoadFailureEventArgs(string animName)
        {
            AnimName = animName;
        }

        public string AnimName { get; private set; }

        public override int Id
        {
            get
            {
                return (int)EventId.CameraAnimLoadFailure;
            }
        }
    }
}
