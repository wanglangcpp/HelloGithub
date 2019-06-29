using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 摄像机动画取消事件。
    /// </summary>
    public class CameraAnimCancelEventArgs : GameEventArgs
    {
        public CameraAnimCancelEventArgs(string animName)
        {
            AnimName = animName;
        }

        public string AnimName { get; private set; }

        public override int Id
        {
            get
            {
                return (int)EventId.CameraAnimCancel;
            }
        }
    }
}
