using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class InstanceResultForm
    {
        private class SimpleState : StateBase
        {
            public override void GoToNextStep(IFsm<InstanceResultForm> fsm)
            {
                GameEntry.SceneLogic.GoBackToLobby();
            }

            public override void SkipAnimation(IFsm<InstanceResultForm> fsm)
            {

            }

            protected override void OnEnter(IFsm<InstanceResultForm> fsm)
            {
                fsm.Owner.m_ReturnLobbyButton.gameObject.SetActive(true);
            }

            protected override void OnLeave(IFsm<InstanceResultForm> fsm, bool isShutdown)
            {
                // Empty
            }
        }
    }
}
