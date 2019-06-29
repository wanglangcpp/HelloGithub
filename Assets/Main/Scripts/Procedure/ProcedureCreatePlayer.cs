using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public partial class ProcedureCreatePlayer : ProcedureBase
    {
        private RandomName m_RandomName = null;
        private bool m_CreatePlayerOK = false;

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
            m_RandomName = new RandomName();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(EventId.CreatePlayer, OnCreatePlayer);

            m_CreatePlayerOK = false;
            GameEntry.UIBackground.ShowDefault();
            GameEntry.UI.OpenUIForm(UIFormId.CreatePlayer);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
                GameEntry.Event.Unsubscribe(EventId.CreatePlayer, OnCreatePlayer);
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_CreatePlayerOK)
            {
                ChangeState<ProcedureSignIn>(procedureOwner);
                return;
            }
        }

        public void CreatePlayer(string playerName, int heroType)
        {
            CLCreatePlayer request = new CLCreatePlayer();
            request.Name = playerName;
            request.IsFemale = false;
            request.FirstHeroType = heroType;
            GameEntry.Network.Send(request);
        }

        public string GetRandomName()
        {
            return m_RandomName.GetRandomName();
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {

        }

        private void OnCreatePlayer(object sender, GameEventArgs e)
        {
            m_CreatePlayerOK = true;
        }
    }
}
