using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 实体死亡保留时间已到事件。
    /// </summary>
    public class DeadKeepTimeReachedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="targetable">死亡实体。</param>
        public DeadKeepTimeReachedEventArgs(ITargetable targetable)
        {
            Targetable = targetable;
        }

        /// <summary>
        /// 事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.DeadKeepTimeReached;
            }
        }

        /// <summary>
        /// 死亡实体。
        /// </summary>
        public ITargetable Targetable { get; private set; }
    }
}
