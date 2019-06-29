using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 英雄技能数据变化事件。
    /// </summary>
    public class HeroSkillDataChangedEventArgs : GameEventArgs
    {
        public HeroSkillDataChangedEventArgs(SkillChangeType changeType)
        {
            ChangeType = changeType;
        }

        public SkillChangeType ChangeType { get; private set; }

        public enum SkillChangeType
        {
            OpenSlot,
            RemoveBadge,
            LayInBadge,
            SkillLevelUp
        }

        public override int Id
        {
            get
            {
                return (int)EventId.HeroSkillDataChanged;
            }
        }
    }
}
