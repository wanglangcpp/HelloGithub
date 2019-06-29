using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// PVP 匹配成功的事件。
    /// </summary>
    public class PvpMatchingSuccessEventArgs : GameEventArgs
    {
        public PvpMatchingSuccessEventArgs(int instanceId)
        {
            InstanceId = instanceId;
        }

        public override int Id
        {
            get { return (int)(EventId.PvpMatchingSuccessEventArgs); }
        }

        public int InstanceId
        {
            get;
            set;
        }
    }
}
