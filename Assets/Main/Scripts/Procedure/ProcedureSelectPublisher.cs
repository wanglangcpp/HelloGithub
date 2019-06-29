using GameFramework;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedureSelectPublisher : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {

            base.OnEnter(procedureOwner);

            GameEntry.UIBackground.ShowResourceBg();
            //GameEntry.Waiting.StartWaiting(WaitingType.Default, GetType().Name);

            if (GameEntry.Base.EditorResourceMode
                || Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.OSXPlayer)
            {
                SelectPublisher(procedureOwner, string.Empty);
                return;
            }

            if (!GameEntry.BuildInfo.InnerPublisherEnabled)
            {
                SelectPublisher(procedureOwner, GameEntry.BuildInfo.Publisher);
                return;
            }
            if (Debug.isDebugBuild)
            {
                string msg = GameEntry.Localization.GetString("UI_TEXT_SELECT_PUBLISHER");
                string confirmText = GameEntry.Localization.GetString("UI_TEXT_OUTER_SERVER");
                string cancelText = GameEntry.Localization.GetString("UI_TEXT_INNER_SERVER");
                EventDelegate outerAction = new EventDelegate(delegate ()
                {
                    SelectPublisher(procedureOwner, GameEntry.BuildInfo.Publisher);
                    GameEntry.Waiting.StartWaiting(WaitingType.Default, GetType().Name);
                });
                EventDelegate innerAction = new EventDelegate(delegate ()
                {
                    SelectPublisher(procedureOwner, string.Empty);
                    GameEntry.Waiting.StartWaiting(WaitingType.Default, GetType().Name);
                });

                SelectPubliser.OpenSelect(msg, confirmText, cancelText, outerAction, innerAction);
            }
            else
            {
                SelectPublisher(procedureOwner, GameEntry.BuildInfo.Publisher);
                GameEntry.Waiting.StartWaiting(WaitingType.Default, GetType().Name);
            }


            //GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            //{
            //    Mode = 2,
            //    Message = GameEntry.Localization.GetString("UI_TEXT_SELECT_PUBLISHER"),
            //    ConfirmText = GameEntry.Localization.GetString("UI_TEXT_OUTER_SERVER"),
            //    CancelText = GameEntry.Localization.GetString("UI_TEXT_INNER_SERVER"),
            //    OnClickConfirm = delegate (object userData) { SelectPublisher(procedureOwner, GameEntry.BuildInfo.Publisher); },
            //    OnClickCancel = delegate (object userData) { SelectPublisher(procedureOwner, string.Empty); },
            //});

        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Waiting.StopWaiting(WaitingType.Default, GetType().Name);
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        private void SelectPublisher(ProcedureOwner procedureOwner, string publisher)
        {
            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.Publisher, publisher);
            ChangeState(procedureOwner, GameEntry.Base.EditorResourceMode ? typeof(ProcedurePreload) : typeof(ProcedureCheckVersion));
        }
    }
}
