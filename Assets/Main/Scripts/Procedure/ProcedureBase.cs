using GameFramework;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public abstract class ProcedureBase : GameFramework.Procedure.ProcedureBase
    {
        public abstract bool UseNativeDialog
        {
            get;
        }

        protected static string TempTextsRootPath
        {
            get
            {
                return Utility.Path.GetCombinePath(GameEntry.Resource.ReadWritePath, "temp");
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Log.Info("{0} OnEnter", GetType().Name);
            base.OnEnter(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            Log.Info("{0} OnLeave", GetType().Name);
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected void OnError(string format, params object[] args)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_NETWORK_NOT_REACHABLE"),
                    ConfirmText = GameEntry.Localization.GetString("UI_TEXT_RETRY"),
                    CancelText = GameEntry.Localization.GetString("UI_TEXT_QUIT_GAME"),
                    OnClickConfirm = RestartGame,
                    OnClickCancel = QuitGame,
                });
                return;
            }

            string logString = string.Format(format, args);
            BuglyAgent.ReportException("CustomError", logString, string.Empty);
            Log.Error(logString);
        }

        protected void RestartGame(object userData)
        {
            GameEntry.Restart();
        }

        protected void QuitGame(object userData)
        {
            GameEntry.Shutdown();
        }

        protected class ChangeSceneRequestData
        {
            public int SceneOrInstanceId { get; private set; }

            public InstanceLogicType InstanceLogicType { get; private set; }

            public bool AutoHideLoading { get; private set; }

            public ChangeSceneRequestData(int sceneOrInstanceId, InstanceLogicType instanceLogicType, bool autoHideLoading)
            {
                SceneOrInstanceId = sceneOrInstanceId;
                InstanceLogicType = instanceLogicType;
                AutoHideLoading = autoHideLoading;
            }
        }

        protected enum TextType
        {
            Dictionary,
            DataTable,
        }
    }
}
