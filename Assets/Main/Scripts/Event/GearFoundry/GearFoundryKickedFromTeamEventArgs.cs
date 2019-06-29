using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备锻造活动 -- 被踢出团队事件。
    /// </summary>
    public class GearFoundryKickedFromTeamEventArgs : GameEventArgs
    {
        public override int Id
        {
            get { return (int)EventId.GearFoundryKickedFromTeam; }
        }
    }
}
