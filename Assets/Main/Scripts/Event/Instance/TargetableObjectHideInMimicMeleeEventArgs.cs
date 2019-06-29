using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 模拟乱斗中可作为目标的实体隐藏事件。
    /// </summary>
    public class TargetableObjectHideInMimicMeleeEventArgs : GameEventArgs
    {
        public TargetableObjectHideInMimicMeleeEventArgs(int entityId)
        {
            EntityId = entityId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.TargetableObjectHideInMimicMelee;
            }
        }

        public int EntityId { get; private set; }

    }
}
