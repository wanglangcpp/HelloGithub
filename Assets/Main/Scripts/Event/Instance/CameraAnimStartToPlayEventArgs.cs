using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 摄像机动画开始播放事件。
    /// </summary>
    public class CameraAnimStartToPlayEventArgs : GameEventArgs
    {
        public CameraAnimStartToPlayEventArgs(string animName)
        {
            AnimName = animName;
        }

        public string AnimName { get; private set; }

        public override int Id
        {
            get
            {
                return (int)EventId.CameraAnimStartToPlay;
            }
        }
    }
}
