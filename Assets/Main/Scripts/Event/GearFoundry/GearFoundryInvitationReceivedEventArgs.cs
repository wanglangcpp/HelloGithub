using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备锻造活动 -- 收到邀请事件。
    /// </summary>
    public class GearFoundryInvitationReceivedEventArgs : GameEventArgs
    {
        public override int Id
        {
            get { return (int)EventId.GearFoundryInvitationReceived; }
        }
    }
}
