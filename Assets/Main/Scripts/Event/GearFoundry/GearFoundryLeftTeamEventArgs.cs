using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备锻造活动 -- 退出团队事件。
    /// </summary>
    public class GearFoundryLeftTeamEventArgs : GameEventArgs
    {
        public override int Id
        {
            get { return (int)EventId.GearFoundryLeftTeam; }
        }
    }
}
