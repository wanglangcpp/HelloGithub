using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 玩家英雄释放技能结束事件。
    /// </summary>
    public class MyHeroPerformSkillEndEventArgs : GameEventArgs
    {
        public MyHeroPerformSkillEndEventArgs(int entityId, Vector3 position, float rotation, int skillId, SkillEndReasonType reason)
        {
            EntityId = entityId;
            Position = position;
            Rotation = rotation;
            SkillId = skillId;
            Reason = reason;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.MyHeroPerformSkillEnd;
            }
        }

        public int EntityId { get; private set; }

        public Vector3 Position { get; private set; }

        public float Rotation { get; private set; }

        public int SkillId { get; private set; }

        public SkillEndReasonType Reason { get; private set; }
    }
}
