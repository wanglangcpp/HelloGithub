using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 伤害记录到统计信息事件。
    /// </summary>
    public class StatDamageRecordedEventArgs : GameEventArgs
    {
        public StatDamageRecordedEventArgs(int entityId, int damageValue)
        {
            EntityId = entityId;
            DamageValue = damageValue;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.StatDamageRecorded;
            }
        }

        public int EntityId { get; private set; }

        public int DamageValue { get; private set; }
    }
}
