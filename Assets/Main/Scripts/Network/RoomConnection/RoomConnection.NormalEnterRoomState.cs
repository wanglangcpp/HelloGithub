using GameFramework.Event;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class RoomConnection
    {
        private class NormalEnterRoom : StateBase
        {
            private IFsm<RoomConnection> m_Fsm = null;

            protected override void OnEnter(IFsm<RoomConnection> fsm)
            {
                base.OnEnter(fsm);
                m_Fsm = fsm;
                //GameEntry.Event.Subscribe(EventId.RoomReady, OnRoomReady);
                GameEntry.Event.Subscribe(EventId.RegistPlayerToRoom, OnRoomReady);

                GameEntry.RoomLogic.AddLog("BasePvpInstanceLogic GetRoomInfo", string.Empty);
                //CRGetRoomInfo request = new CRGetRoomInfo();

                //request.PlayerId = GameEntry.Data.Player.Id;
                //request.RoomId = fsm.Owner.m_RoomId;
                //request.Token = fsm.Owner.m_RoomServerToken;
                
            }

            protected override void OnLeave(IFsm<RoomConnection> fsm, bool isShutdown)
            {
                if (GameEntry.IsAvailable)
                {
                    //GameEntry.Event.Unsubscribe(EventId.RoomReady, OnRoomReady);
                    GameEntry.Event.Unsubscribe(EventId.RegistPlayerToRoom, OnRoomReady);
                }
                base.OnLeave(fsm, isShutdown);
            }

            private void OnRoomReady(object sender, GameEventArgs e)
            {
                ChangeState<CheckingState>(m_Fsm);
            }

            private void OnRoomDataChanged(object sender, GameEventArgs e)
            {
            }
        }
    }
}
