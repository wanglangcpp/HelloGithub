using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 连续点击技更新输入事件。
    /// </summary>
    public class SkillContinualTapUpdateInputEventArgs : GameEventArgs
    {
        public SkillContinualTapUpdateInputEventArgs(int ownerEntityId, bool success)
        {
            OwnerEntityId = ownerEntityId;
            Success = success;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.SkillContinualTapUpdateInput;
            }
        }

        public int OwnerEntityId { get; private set; }

        public bool Success { get; private set; }
    }
}
