using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备锻造活动 -- 完成锻造操作事件。
    /// </summary>
    public class GearFoundryPerformedEventArgs : GameEventArgs
    {
        public GearFoundryPerformedEventArgs(bool isMe)
            : base()
        {
            IsMe = isMe;
        }

        public override int Id
        {
            get { return (int)EventId.GearFoundryPerformed; }
        }

        /// <summary>
        /// 是否由当前玩家完成。
        /// </summary>
        public bool IsMe { get; private set; }
    }
}
