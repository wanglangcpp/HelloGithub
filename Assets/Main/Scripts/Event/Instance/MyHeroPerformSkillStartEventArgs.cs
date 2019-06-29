using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 玩家英雄释放技能开始事件。
    /// </summary>
    public class MyHeroPerformSkillStartEventArgs : GameEventArgs
    {
        public MyHeroPerformSkillStartEventArgs(int entityId, Vector3 position, float rotation, int skillId, int skillIndex)
        {
            EntityId = entityId;
            Position = position;
            Rotation = rotation;
            SkillId = skillId;
            SkillIndex = skillIndex;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.MyHeroPerformSkillStart;
            }
        }

        public int EntityId { get; private set; }

        public Vector3 Position { get; private set; }

        public float Rotation { get; private set; }

        public int SkillId { get; private set; }

        public int SkillIndex { get; private set; }
    }
}
