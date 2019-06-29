using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本准备完毕事件。
    /// </summary>
    public class InstanceReadyToStartEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.InstanceReadyToStart;
            }
        }
    }
}
