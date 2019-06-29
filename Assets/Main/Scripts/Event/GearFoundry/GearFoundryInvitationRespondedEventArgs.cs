using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备锻造活动 -- 回应邀请事件。
    /// </summary>
    public class GearFoundryInvitationRespondedEventArgs : GameEventArgs
    {
        public GearFoundryInvitationRespondedEventArgs(bool accept, int inviterPlayerId, int teamId)
        {
            Accept = accept;
            InviterPlayerId = inviterPlayerId;
            TeamId = teamId;
        }

        public override int Id
        {
            get { return (int)EventId.GearFoundryInvitationResponded; }
        }

        public bool Accept { get; private set; }
        public int InviterPlayerId { get; private set; }
        public int TeamId { get; private set; }
    }
}
