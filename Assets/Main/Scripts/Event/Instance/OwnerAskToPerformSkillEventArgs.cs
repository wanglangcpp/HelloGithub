using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 实体要求其发出的子弹释放技能的消息。
    /// </summary>
    public class OwnerAskToPerformSkillEventArgs : GameEventArgs
    {
        public OwnerAskToPerformSkillEventArgs(int ownerEntityId, int skillId, int targetBulletTypeId)
        {
            OwnerEntityId = ownerEntityId;
            SkillId = skillId;
            TargetBulletTypeId = targetBulletTypeId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.OwnerAskToPerformSkill;
            }
        }

        /// <summary>
        /// 拥有者实体编号。
        /// </summary>
        public int OwnerEntityId { get; private set; }

        /// <summary>
        /// 要求子弹释放的技能编号。
        /// </summary>
        public int SkillId { get; private set; }

        /// <summary>
        /// 目标子弹种类编号。
        /// </summary>
        public int TargetBulletTypeId { get; private set; }
    }
}
