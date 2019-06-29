using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本喊话发生事件。
    /// </summary>
    public class InstancePropagandaBeginEventArgs : GameEventArgs
    {
        public InstancePropagandaBeginEventArgs(InstancePropagandaData data)
        {
            Data = data;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.InstancePropagandaBegin;
            }
        }

        public InstancePropagandaData Data
        {
            get;
            private set;
        }
    }
}
