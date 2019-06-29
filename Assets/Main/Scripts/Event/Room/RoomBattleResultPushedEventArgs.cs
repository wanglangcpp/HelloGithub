using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 房间连接关闭事件。
    /// </summary>
    public class RoomBattleResultPushedEventArgs : GameEventArgs
    {
        private int m_Reason;

        public RoomBattleResultPushedEventArgs(int result, int reason)
        {
            Result = (InstanceResultType)result;
            m_Reason = reason;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.RoomBattleResultPushed;
            }
        }

        public InstanceResultType Result { get; private set; }

        public InstanceSuccessReason SuccessReason
        {
            get
            {
                return (InstanceSuccessReason)m_Reason;
            }
        }

        public InstanceFailureReason FailureReason
        {
            get
            {
                return (InstanceFailureReason)m_Reason;
            }
        }

        public InstanceDrawReason DrawReason
        {
            get
            {
                return (InstanceDrawReason)m_Reason;
            }
        }
    }
}
