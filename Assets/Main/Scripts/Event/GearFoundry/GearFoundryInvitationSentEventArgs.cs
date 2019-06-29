using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备锻造活动 -- 发出邀请事件。
    /// </summary>
    public class GearFoundryInvitationSentEventArgs : GameEventArgs
    {
        public GearFoundryInvitationSentEventArgs(int inviteePlayerId)
        {
            InviteePlayerId = inviteePlayerId;
        }

        public override int Id
        {
            get { return (int)EventId.GearFoundryInvitationSent; }
        }

        public int InviteePlayerId { get; private set; }
    }
}
