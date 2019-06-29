using UnityEngine;
using System.Collections;
using System;

namespace Genesis.GameClient
{
    public partial class MimicMeleeInstanceLogic
    {
        protected override AbstractInstanceFailure CreateFailureResult(bool shouldOpenUI)
        {
            return new InstanceResultFailure(shouldOpenUI);
        }

        protected override AbstractInstanceSuccess CreateSuccessResult()
        {
            return new InstanceResultSuccess();
        }

        private class InstanceResultSuccess : BaseSinglePlayerInstanceSuccess
        {
            protected override void SendLeaveInstanceRequest()
            {
                if (!GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    if (GameEntry.Data.GetTempData<bool>(Constant.TempData.NeedCreatePlayer))
                    {
                        GameEntry.Data.AddOrUpdateTempData(Constant.TempData.NeedCreatePlayer, false);
                    }
                }
                GameEntry.UI.OpenUIForm(UIFormId.ActivityMeleeBattleOverForm);
            }

            protected override void ShowResultUI(UIFormBaseUserData userData)
            {
                if (!GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    if (GameEntry.Data.GetTempData<bool>(Constant.TempData.NeedCreatePlayer))
                    {
                        GameEntry.Data.AddOrUpdateTempData(Constant.TempData.NeedCreatePlayer, false);
                    }
                }
                GameEntry.UI.OpenUIForm(UIFormId.ActivityMeleeBattleOverForm);
            }
        }

        private class InstanceResultFailure : BaseSinglePlayerInstanceFailure
        {
            public InstanceResultFailure(bool shouldOpenUI) : base(shouldOpenUI)
            {

            }

            protected override void SendLeaveInstanceRequest()
            {
                if (!GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    if (GameEntry.Data.GetTempData<bool>(Constant.TempData.NeedCreatePlayer))
                    {
                        GameEntry.Data.AddOrUpdateTempData(Constant.TempData.NeedCreatePlayer, false);
                    }
                }
                GameEntry.SceneLogic.GoBackToLobby();
            }

            protected override void ShowResultUI(UIFormBaseUserData userData)
            {

            }
        }
    }
}
