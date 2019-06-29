using GameFramework;
using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BasePvpInstanceLogic
    {
        private class Waiter : AbstractInstanceWaiter
        {
            public override void Init(BaseInstanceLogic instanceLogic)
            {
                base.Init(instanceLogic);
                GameEntry.Event.Subscribe(EventId.GetRoomStatus, OnGetRoomStatus);
            }

            public override void Shutdown(bool isExternalShutdown)
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.GetRoomStatus, OnGetRoomStatus);
                }

                base.Shutdown(isExternalShutdown);
            }

            public override void OnDraw(InstanceDrawReason reason, GameFrameworkAction onComplete)
            {
                GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenActivitySinglePvpMainForm, true);
                GameEntry.SceneLogic.GoBackToLobby(false);
                FireShouldGoToResult();
            }

            public override void OnLose(InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
            {
                GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenActivitySinglePvpMainForm, true);
                GameEntry.SceneLogic.GoBackToLobby(false);
                FireShouldGoToResult();
            }
            public override void OnWin(InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenActivitySinglePvpMainForm, true);
                GameEntry.SceneLogic.GoBackToLobby(false);
                FireShouldGoToResult();
            }
            public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(elapseSeconds, realElapseSeconds);

                var pvp = m_InstanceLogic as BasePvpInstanceLogic;
                if (Time.time >= pvp.InstanceStartTime)
                {
                    pvp.DoStartInstance();
                    FireShouldGoToRunning();
                }
            }
            private void OnGetRoomStatus(object sender, GameEventArgs e)
            {
                var msg = e as GetRoomStatusEventArgs;
                if (msg.RoomStatus == (int)ServerErrorCode.RoomStatusError || msg.RoomStatus == (int)RoomStateType.Finish)
                {
                    (m_InstanceLogic as BasePvpInstanceLogic).EnforceGoBackToLobby();
                    return;
                }
            }
        }
    }
}
