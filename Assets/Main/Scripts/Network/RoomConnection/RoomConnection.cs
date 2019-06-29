using GameFramework.Fsm;
using GameFramework.Event;
using System;

namespace Genesis.GameClient
{
    public partial class RoomConnection
    {
        private readonly IFsm<RoomConnection> m_Fsm;

        private int m_RoomId = 0;

        private string m_RoomServerToken = string.Empty;

        private Action m_ConnectFinished = null;

        public Action ConnectFinished
        {
            set
            {
                m_ConnectFinished = value;
            }
        }

        private bool PvpHasFinished
        {
            get;
            set;
        }

        public RoomConnection()
        {
            m_RoomId = GameEntry.Data.GetAndRemoveTempData<int>(Constant.TempData.RoomId);
            m_RoomServerToken = GameEntry.Data.GetAndRemoveTempData<string>(Constant.TempData.RoomServerToken);
            GameEntry.Data.Room.Id = m_RoomId;
            GameEntry.Data.Room.Token = m_RoomServerToken;

            PvpHasFinished = false;
            GameEntry.Event.Subscribe(EventId.RoomBattleResultPushed, OnRoomBattleResultPushed);
            m_Fsm = GameEntry.Fsm.CreateFsm(this,
                new ConnectState(),
                new NormalEnterRoom(),
                new ReconnectEnterRoomState(),
                new WaitForConnectState(),
                new CheckingState());
            m_Fsm.Start<ConnectState>();
        }

        public void Shutdown()
        {
            GameEntry.Event.Unsubscribe(EventId.RoomBattleResultPushed, OnRoomBattleResultPushed);
            GameEntry.Fsm.DestroyFsm(m_Fsm);
        }

        public void Connect(Action connectFinished)
        {
            m_ConnectFinished = connectFinished;
            (m_Fsm.CurrentState as StateBase).Connect(m_Fsm);
        }

        public void OnNetworkConnected(object sender, UnityGameFramework.Runtime.NetworkConnectedEventArgs ne)
        {
            (m_Fsm.CurrentState as StateBase).OnNetworkConnected(m_Fsm, sender, ne);
        }

        public void OnNetworkClosed(object sender, UnityGameFramework.Runtime.NetworkClosedEventArgs ne)
        {
            (m_Fsm.CurrentState as StateBase).OnNetworkClosed(m_Fsm, sender, ne);
        }

        private void OnRoomBattleResultPushed(object sender, GameEventArgs e)
        {
            PvpHasFinished = true;
        }
    }

    public class Aciton<T>
    {
    }
}
