using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 资源副本数据改变事件。
    /// </summary>
    public class InstancesForResourceDataChangedEventArgs : GameEventArgs
    {
        public InstancesForResourceDataChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.InstancesForResourceDataChanged;
            }
        }
    }
}
