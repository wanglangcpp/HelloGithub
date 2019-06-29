using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class RoomConnection
    {
        private class CheckingState : StateBase
        {
            protected override void OnEnter(IFsm<RoomConnection> fsm)
            {
                base.OnEnter(fsm);
                if (fsm.Owner.m_ConnectFinished != null)
                {
                    fsm.Owner.m_ConnectFinished();
                    fsm.Owner.m_ConnectFinished = null;
                }
            }
        }
    }
}
