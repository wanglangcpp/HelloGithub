using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备锻造活动 -- 团队人员变化事件。
    /// </summary>
    public class GearFoundryTeamPlayersChangedEventArgs : GameEventArgs
    {
        public override int Id
        {
            get { return (int)EventId.GearFoundryTeamPlayersChanged; }
        }
    }
}
