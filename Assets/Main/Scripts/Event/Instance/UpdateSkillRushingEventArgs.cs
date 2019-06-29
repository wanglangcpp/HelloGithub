using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 更新技能冲刺事件。
    /// </summary>
    public class UpdateSkillRushingEventArgs : GameEventArgs
    {
        public UpdateSkillRushingEventArgs(int entityId, int skillId, Vector2 position, float rotation, bool justStarted)
        {
            EntityId = entityId;
            SkillId = skillId;
            Position = position;
            Rotation = rotation;
            HasJustStarted = justStarted;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.UpdateSkillRushing;
            }
        }

        public int EntityId { get; private set; }
        public Vector2 Position { get; private set; }
        public float Rotation { get; private set; }
        public int SkillId { get; private set; }
        public bool HasJustStarted { get; internal set; }
    }
}
