using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本喊话结束事件。
    /// </summary>
    public class InstancePropagandaEndEventArgs : GameEventArgs
    {
        public InstancePropagandaEndEventArgs()
        {
            // Empty
        }

        public override int Id
        {
            get
            {
                return (int)EventId.InstancePropagandaEnd;
            }
        }
    }
}
