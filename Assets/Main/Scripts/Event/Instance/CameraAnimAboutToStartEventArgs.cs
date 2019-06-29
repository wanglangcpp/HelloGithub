using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    ///
    /// </summary>
    public class CameraAnimAboutToStartEventArgs : GameEventArgs
    {
        public CameraAnimAboutToStartEventArgs(string animName)
        {
            AnimName = animName;
        }

        public string AnimName { get; private set; }

        public override int Id
        {
            get
            {
                return (int)EventId.CameraAnimAboutToStart;
            }
        }
    }
}
