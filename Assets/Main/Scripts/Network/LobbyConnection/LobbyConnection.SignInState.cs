using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Network;

namespace Genesis.GameClient
{
    public partial class LobbyConnection
    {
        private class SignInState : StateBase
        {
            private int m_SignInPrepareCount = 0;

            protected override void OnEnter(IFsm<LobbyConnection> fsm)
            {
                base.OnEnter(fsm);
                GameEntry.Event.Subscribe(EventId.SignInPrepare, OnSignInPrepare);

                GameEntry.Waiting.StartWaiting(WaitingType.Network, "SignInState");
                
                m_SignInPrepareCount = 0;
                SendSignInPreparePacket(new CLSignIn());
            }

            protected override void OnLeave(IFsm<LobbyConnection> fsm, bool isShutdown)
            {
                if (GameEntry.IsAvailable && GameEntry.Event != null)
                {
                    GameEntry.Event.Unsubscribe(EventId.SignInPrepare, OnSignInPrepare);
                }

                if (GameEntry.IsAvailable && GameEntry.Waiting != null)
                {
                    GameEntry.Waiting.StopWaiting(WaitingType.Network, "SignInState");
                }

                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<LobbyConnection> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (m_SignInPrepareCount > 0)
                {
                    return;
                }

                GameEntry.Event.Fire(this, new ResponseSignInLobbyServerEventArgs());
                ChangeState<CheckingState>(fsm);
            }

            private void OnSignInPrepare(object sender, GameEventArgs e)
            {
                --m_SignInPrepareCount;
            }

            private void SendSignInPreparePacket<T>(T packet) where T : Packet
            {
                ++m_SignInPrepareCount;
                GameEntry.Network.Send(packet);
            }
        }
    }
}
