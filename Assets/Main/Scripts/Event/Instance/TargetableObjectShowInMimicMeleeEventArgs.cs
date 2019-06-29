using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 模拟乱斗中可被攻击实体显示事件。
    /// </summary>
    public class TargetableObjectShowInMimicMeleeEventArgs : GameEventArgs
    {
        public TargetableObjectShowInMimicMeleeEventArgs(int entityId)
        {
            EntityId = entityId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.TargetableObjectShowInMimicMelee;
            }
        }

        public int EntityId { get; private set; }

    }
}
