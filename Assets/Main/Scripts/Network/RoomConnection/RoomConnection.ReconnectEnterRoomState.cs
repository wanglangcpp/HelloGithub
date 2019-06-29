using GameFramework.Event;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class RoomConnection
    {
        private class ReconnectEnterRoomState : StateBase
        {
            private IFsm<RoomConnection> m_Fsm = null;

            protected override void OnEnter(IFsm<RoomConnection> fsm)
            {
                base.OnEnter(fsm);

                GameEntry.Event.Subscribe(EventId.GetRoomStatus, OnGetRoomStatus);
                GameEntry.Event.Subscribe(EventId.RoomDataChanged, OnReconnectRoom);

                m_Fsm = fsm;
                CRGetRoomStatus request = new CRGetRoomStatus();
                GameEntry.RoomLogic.AddLog("BasePvpInstanceLogic GetRoomInfo", string.Empty);
                request.PlayerId = GameEntry.Data.Player.Id;
                request.RoomId = fsm.Owner.m_RoomId;
                request.Token = fsm.Owner.m_RoomServerToken;
                GameEntry.Network.Send(request);
            }

            protected override void OnLeave(IFsm<RoomConnection> fsm, bool isShutdown)
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.GetRoomStatus, OnGetRoomStatus);
                    GameEntry.Event.Unsubscribe(EventId.RoomDataChanged, OnReconnectRoom);
                }
                base.OnLeave(fsm, isShutdown);
            }

            private void OnGetRoomStatus(object sender, GameEventArgs e)
            {
                var msg = e as GetRoomStatusEventArgs;
                if (msg.RoomStatus == (int)ServerErrorCode.RoomStatusError || msg.RoomStatus == (int)RoomStateType.Finish)
                {
                    m_Fsm.Owner.PvpHasFinished = true;
                    ChangeState<CheckingState>(m_Fsm);
                    return;
                }

                CRReconnectRoom request = new CRReconnectRoom();
                GameEntry.Network.Send(request);
            }

            private void OnReconnectRoom(object sender, GameEventArgs e)
            {
                ChangeState<CheckingState>(m_Fsm);
            }
        }
    }
}
