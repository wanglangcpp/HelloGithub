using GameFramework;
using GameFramework.Event;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedureSignIn : ProcedureBase
    {
        private bool m_SignInOK = false;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(EventId.ResponseSignInLobbyServer, OnResponseSignInLobbyServer);

            m_SignInOK = false;

            GameEntry.Data.RemoveTempData(Constant.TempData.ServerList);
            GameEntry.Event.Fire(this, new RequestSignInLobbyServerEventArgs());
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(EventId.ResponseSignInLobbyServer, OnResponseSignInLobbyServer);
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_SignInOK)
            {
                return;
            }

            if (GameEntry.Data.Room.InRoom)
            {
                GameEntry.Event.FireNow(this, new WillChangeSceneEventArgs(InstanceLogicType.SinglePvp, GameEntry.Data.Room.InstanceId, true));
                ProcedureChangeScene procedureChangeScene = procedureOwner.GetState<ProcedureChangeScene>();
                procedureChangeScene.ChangeScene(procedureOwner, InstanceLogicType.SinglePvp, true);
            }
            else
            {
                GameEntry.Event.FireNow(this, new WillChangeSceneEventArgs(InstanceLogicType.NonInstance, Constant.LobbySceneId, true)); // Dummy event for refresh scene logic.
                ProcedureChangeScene procedureChangeScene = procedureOwner.GetState<ProcedureChangeScene>();
                procedureChangeScene.ChangeScene(procedureOwner, InstanceLogicType.NonInstance, true);
            }
        }

        private void OnResponseSignInLobbyServer(object sender, GameEventArgs e)
        {
            m_SignInOK = true;
            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.SignInShowId, 2);
            if (SDKManager.HasConfig && SDKManager.Instance.isSDKLogin)
            {
                SDKManager.Instance.TalkingData.SetAccountName(GameEntry.Data.Player.Id.ToString());

                SDKManager.Instance.helper.UploadData("Loading");
            }
        }
    }
}
