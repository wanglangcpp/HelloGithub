using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本中本对手准备完毕事件
    /// </summary>
    public class InstanceOppPreparedEventArgs : GameEventArgs
    {
        public InstanceOppPreparedEventArgs(int playerId)
        {
            PlayerId = playerId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.InstanceOppPrepared;
            }
        }

        public int PlayerId { get; private set; }
    }
}
