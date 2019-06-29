using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本中本玩家准备完毕事件
    /// </summary>
    public class InstanceMePreparedEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.InstanceMePrepared;
            }
        }
    }
}
