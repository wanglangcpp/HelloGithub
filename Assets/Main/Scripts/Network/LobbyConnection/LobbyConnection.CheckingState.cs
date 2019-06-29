using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class LobbyConnection
    {
        private class CheckingState : StateBase
        {
            protected override void OnEnter(IFsm<LobbyConnection> fsm)
            {
                base.OnEnter(fsm);

                fsm.Owner.m_IsReConnecting = false;
                if (GameEntry.Data.Room.InRoom && GameEntry.Data.Room.HasReconnected)
                {
                    GameEntry.Event.Fire(this, new ConnectRoomEventArgs());
                }
            }
        }
    }
}
