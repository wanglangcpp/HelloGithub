using BehaviorDesigner.Runtime;
using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BasePvpInstanceLogic
    {
        private class Preparer : AbstractInstancePreparer
        {
            private const float WaitBeforeChangeToWaitingState = .5f;
            private float m_DelayWaitingStateTime = 0f;

            private bool m_BattleFormOpened = false;
            private bool m_MePrepared = false;
            private bool m_OppPrepared = false;

            private ResourcesStep m_ResourcesStep = ResourcesStep.LoadingScene;
            private RoomConnectStep m_RoomStep = RoomConnectStep.StartConnectingRoom;

            private List<BehaviorTree> m_BehaviorsToLoad = new List<BehaviorTree>();
            private float startTime;

            public override void Init(BaseInstanceLogic instanceLogic)
            {
                base.Init(instanceLogic);

                //GameEntry.Event.Subscribe(EventId.GetRoomStatus, OnGetRoomStatus);
                //GameEntry.Event.Subscribe(EventId.RoomReady, OnRoomReady);
                //GameEntry.Event.Subscribe(EventId.RoomDataChanged, OnRoomDataChanged);
                GameEntry.Event.Subscribe(EventId.InstanceMePrepared, OnMePrepared);
                GameEntry.Event.Subscribe(EventId.InstanceOppPrepared, OnOppPrepared);
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.NetworkConnected, OnNetworkConnected);
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.NetworkClosed, OnNetworkClosed);
                GameEntry.Event.Subscribe(EventId.ServerError, OnServerError);
                GameEntry.Event.Subscribe(EventId.RegistPlayerToRoom, OnRegisterPlayerSuccess);
                GameEntry.Event.Subscribe(EventId.PvpStart, OnReceivePvpStart);
                GameEntry.Event.Subscribe(EventId.GetRoomBattleStatus, OnGetBattleStatus);

                m_BattleFormOpened = false;
                m_MePrepared = false;
                m_OppPrepared = false;
                m_DelayWaitingStateTime = 0f;

                m_BehaviorsToLoad.Clear();

                GameEntry.Input.JoystickActivated = false;
                GameEntry.Input.SkillActivated = false;

                GameEntry.RoomLogic.Reset();
                GameEntry.RoomLogic.Init();
            }

            public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(elapseSeconds, realElapseSeconds);
                //Log.Info("RoomStep=> " + m_RoomStep.ToString() + "    ResourceStep=> " + m_ResourcesStep.ToString());
                if (m_RoomStep == RoomConnectStep.ServerError && GameEntry.Scene.GetLoadingSceneNames().Length == 0)
                {
                    m_RoomStep = RoomConnectStep.ServerErrorEvent;
                    GameEntry.Event.Fire(this, new InstanceReadyToStartEventArgs());
                    return;
                }

                if (m_RoomStep == RoomConnectStep.ServerErrorEvent && m_ResourcesStep >= ResourcesStep.StartPreparingResouces && (GameEntry.Procedure.CurrentProcedure is ProcedureMain))
                {
                    m_RoomStep = RoomConnectStep.ServerErrorEnd;
                    (m_InstanceLogic as BasePvpInstanceLogic).EnforceGoBackToLobby();
                    if (m_BattleFormOpened)
                    {
                        GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(UIFormId.BattleForm));
                    }
                    return;
                }

                if (m_ResourcesStep >= ResourcesStep.StartPreparingResouces && m_RoomStep == RoomConnectStep.StartConnectingRoom)
                {
                    m_RoomStep = RoomConnectStep.ConnectingRoom;
                    GameEntry.Event.Fire(this, new ConnectRoomEventArgs());
                    return;
                }

                if (m_ResourcesStep == ResourcesStep.StartPreparingResouces && m_RoomStep >= RoomConnectStep.StartRoomReady)
                {
                    m_ResourcesStep = ResourcesStep.PreparingResouces;
                    var pvpInstanceLogic = m_InstanceLogic as BasePvpInstanceLogic;
                    pvpInstanceLogic.PrepareData();
                    pvpInstanceLogic.PrepareAndShowMeHero();
                    pvpInstanceLogic.PrepareAndShowOppHero();
                    return;
                }

                if (m_ResourcesStep == ResourcesStep.PreparingResouces && ResourcesArePrepared)
                {
                    m_ResourcesStep = ResourcesStep.PreparingResoucesSuccess;
                    return;
                }

                if (m_ResourcesStep >= ResourcesStep.PreparingResoucesSuccess && m_RoomStep == RoomConnectStep.StartRoomReady)
                {
                    m_RoomStep = RoomConnectStep.RoomReady;
                    //GameEntry.RoomLogic.ClaimRoomReady();
                    return;
                }



                if (m_ResourcesStep >= ResourcesStep.PreparingResoucesSuccess && m_RoomStep == RoomConnectStep.RoomReadySuccess)
                {

                    if ((m_InstanceLogic as BasePvpInstanceLogic).m_RoomReconnect)
                    {
                        //m_ResourcesStep = ResourcesStep.StartingInstance;
                        GameEntry.Event.Fire(this, new InstanceReadyToStartEventArgs());

                        //如果是重新连接进来的，不发送ready，直接请求当前的副本数据，完全准备完毕后发送Ready

                        GameEntry.RoomLogic.GetRoomBattleInfo();
                    }
                    else
                    {
                        m_RoomStep = RoomConnectStep.RoomStartRequest;
                        //正常进入战斗场景在准备完成后发送已经就绪的消息通知服务器
                        GameEntry.RoomLogic.ClaimRoomReady();

                        //CRReadySingleMatch request = new CRReadySingleMatch();
                        //GameEntry.Network.Send(request);
                    }
                    return;
                }
                if (m_ResourcesStep >= ResourcesStep.PreparingResoucesSuccess && m_RoomStep == RoomConnectStep.RoomReconnectSetStatus)
                {
                    //等待服务器返回重连时房间数据
                    m_RoomStep = RoomConnectStep.RoomStart;
                    return;
                }
                if (m_ResourcesStep >= ResourcesStep.PreparingResoucesSuccess && m_RoomStep == RoomConnectStep.RoomStartRequest)
                {
                    //等待服务器返回开始时间
                    return;
                }
                if (m_ResourcesStep == ResourcesStep.PreparingResoucesSuccess && m_RoomStep == RoomConnectStep.RoomStart)
                {
                    m_ResourcesStep = ResourcesStep.StartingInstance;
                    GameEntry.Event.Fire(this, new InstanceReadyToStartEventArgs());

                    return;
                }
                if (m_ResourcesStep == ResourcesStep.DelayWaitingState && m_RoomStep == RoomConnectStep.RoomStart)
                {
                    m_DelayWaitingStateTime += realElapseSeconds;
                    if (m_DelayWaitingStateTime >= WaitBeforeChangeToWaitingState)
                    {
                        if ((m_InstanceLogic as BasePvpInstanceLogic).m_RoomReconnect)
                        {
                            //GameEntry.Event.Fire(this, new InstanceReadyToStartEventArgs());
                            (m_InstanceLogic as BasePvpInstanceLogic).m_RoomReconnect = false;
                            FireShouldGoToWaiting();
                            return;
                        }
                        var pvp = m_InstanceLogic as BasePvpInstanceLogic;
                        pvp.InstanceStartTime = startTime;//Time.unscaledTime - GameEntry.Data.Room.StartTime + 3f; // GameEntry.ServerConfig.GetFloat(Constant.ServerConfig.RoomBattleStartProtectionTime, 3f);
                        FireShouldGoToWaiting();
                        return;
                    }
                }

            }

            public override void Shutdown(bool isExternalShutdown)
            {
                m_BehaviorsToLoad.Clear();

                if (GameEntry.IsAvailable)
                {
                    //GameEntry.Event.Unsubscribe(EventId.GetRoomStatus, OnGetRoomStatus);
                    //GameEntry.Event.Unsubscribe(EventId.RoomReady, OnRoomReady);
                    //GameEntry.Event.Unsubscribe(EventId.RoomDataChanged, OnRoomDataChanged);
                    GameEntry.Event.Unsubscribe(EventId.InstanceMePrepared, OnMePrepared);
                    GameEntry.Event.Unsubscribe(EventId.InstanceOppPrepared, OnOppPrepared);
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.NetworkConnected, OnNetworkConnected);
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.NetworkClosed, OnNetworkClosed);
                    GameEntry.Event.Unsubscribe(EventId.ServerError, OnServerError);
                    GameEntry.Event.Unsubscribe(EventId.RegistPlayerToRoom, OnRegisterPlayerSuccess);
                    GameEntry.Event.Unsubscribe(EventId.PvpStart, OnReceivePvpStart);
                    GameEntry.Event.Unsubscribe(EventId.GetRoomBattleStatus, OnGetBattleStatus);
                }

                base.Shutdown(isExternalShutdown);
            }

            public override void OnLoadSceneSuccess(UnityGameFramework.Runtime.LoadSceneSuccessEventArgs e)
            {
                m_ResourcesStep = ResourcesStep.LoadingBehaviors;
                StartLoadingBehavior();
            }

            public override void OnOpenUIFormSuccess(UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs e)
            {
                if (e.UIForm.TypeId != (int)UIFormId.BattleForm)
                {
                    return;
                }

                m_BattleFormOpened = true;
            }

            public override void OnLoadBehaviorSuccess(LoadBehaviorSuccessEventArgs e)
            {
                if (!m_BehaviorsToLoad.Contains(e.Behavior))
                {
                    return;
                }

                Log.Debug("Load behavior '{0}' OK.", e.BehaviorName);
                var instanceLogic = (m_InstanceLogic as BasePvpInstanceLogic);
                if (e.Behavior != instanceLogic.m_MyPlayerBehavior)
                {
                    instanceLogic.AddBehaviorTree(e.Behavior);
                }

                m_BehaviorsToLoad.Remove(e.Behavior);

                if (m_BehaviorsToLoad.Count > 0)
                {
                    return;
                }

                m_ResourcesStep = ResourcesStep.StartPreparingResouces;
            }

            public override void StartInstance()
            {
                (m_InstanceLogic as BasePvpInstanceLogic).DisableBehaviorTrees();
                GameEntry.Input.JoystickActivated = false;
                GameEntry.Input.SkillActivated = false;
                GameEntry.Loading.Hide();
                (m_InstanceLogic as BasePvpInstanceLogic).IsRunning = false;

                if (m_InstanceLogic.HasResult)
                {
                    FireShouldGoToResult();
                }
                else
                {
                    m_ResourcesStep = ResourcesStep.DelayWaitingState;
                }
            }

            private void StartLoadingBehavior()
            {
                var instanceLogic = m_InstanceLogic as BasePvpInstanceLogic;
                GameObject gameObject = new GameObject("Scene Logic");
                instanceLogic.LoadMyPlayerAI(gameObject);
                m_BehaviorsToLoad.Add(instanceLogic.m_MyPlayerBehavior);
            }

            private bool ResourcesArePrepared
            {
                get
                {
                    return m_BattleFormOpened && m_MePrepared && m_OppPrepared;
                }
            }

            //private void OnGetRoomStatus(object sender, GameEventArgs e)
            //{
            //    if (m_RoomStep == RoomConnectStep.ServerError)
            //    {
            //        return;
            //    }
            //    m_RoomStep = RoomConnectStep.ReconnectingRoom;
            //    var msg = e as GetRoomStatusEventArgs;
            //    if (msg.RoomStatus == (int)ServerErrorCode.RoomStatusError || msg.RoomStatus == (int)RoomStateType.Finish)
            //    {
            //        m_RoomStep = RoomConnectStep.ServerError;
            //        return;
            //    }
            //}

            //private void OnRoomReady(object sender, GameEventArgs e)
            //{
            //    if (m_RoomStep == RoomConnectStep.ServerError)
            //    {
            //        return;
            //    }
            //    m_RoomStep = RoomConnectStep.RoomReadySuccess;
            //}

            private void OnMePrepared(object sender, GameEventArgs e)
            {
                if (m_RoomStep == RoomConnectStep.ServerError)
                {
                    return;
                }
                m_MePrepared = true;
                GameEntry.SceneLogic.OpenBattleForm();
            }

            private void OnOppPrepared(object sender, GameEventArgs e)
            {
                if (m_RoomStep == RoomConnectStep.ServerError)
                {
                    return;
                }
                m_OppPrepared = true;
            }

            //private void OnRoomDataChanged(object sender, GameEventArgs e)
            //{
            //    if (m_RoomStep == RoomConnectStep.ServerError)
            //    {
            //        return;
            //    }
            //    if (m_RoomStep >= RoomConnectStep.StartRoomReady)
            //    {
            //        return;
            //    }

            //    if (m_RoomStep == RoomConnectStep.ReconnectingRoom)
            //    {
            //        var state = GameEntry.Data.Room.State;
            //        if (state == RoomStateType.Running)
            //        {
            //            m_RoomStep = RoomConnectStep.RoomReadySuccess;
            //        }
            //        else if (state == RoomStateType.Waiting)
            //        {
            //            m_RoomStep = RoomConnectStep.RoomReady;
            //        }
            //        else
            //        {
            //            m_RoomStep = RoomConnectStep.ServerError;
            //        }
            //    }
            //    else
            //    {
            //        m_RoomStep = RoomConnectStep.StartRoomReady;
            //    }
            //}

            private void OnNetworkConnected(object sender, GameEventArgs e)
            {
                if (m_RoomStep == RoomConnectStep.ServerError)
                {
                    return;
                }
                var ne = e as UnityGameFramework.Runtime.NetworkConnectedEventArgs;

                if (ne.NetworkChannel.Name != Constant.Network.RoomNetworkChannelName)
                {
                    return;
                }

                if (GameEntry.Data.Room.HasReconnected)
                {
                    var pvpInstanceLogic = m_InstanceLogic as BasePvpInstanceLogic;
                    pvpInstanceLogic.m_RoomReconnect = true;
                    //m_RoomStep = RoomConnectStep.ReconnectingRoom;
                    //return;
                }

                if (m_RoomStep == RoomConnectStep.ConnectingRoom)
                {
                    m_RoomStep = RoomConnectStep.ConnectingRoomSuccess;
                    CRPlayerRegister request = new CRPlayerRegister();
                    request.RoomId = GameEntry.Data.Room.Id;
                    request.Token = GameEntry.Data.Room.Token;
                    GameEntry.Network.Send(request);
                }
            }

            private void OnNetworkClosed(object sender, GameEventArgs e)
            {
                var pvpInstanceLogic = m_InstanceLogic as BasePvpInstanceLogic;

                pvpInstanceLogic.m_RoomClosed = true;

                if (m_RoomStep == RoomConnectStep.ServerError)
                {
                    return;
                }
                var ne = e as UnityGameFramework.Runtime.NetworkClosedEventArgs;

                if (ne.NetworkChannel.Name != Constant.Network.RoomNetworkChannelName)
                {
                    return;
                }

                m_RoomStep = RoomConnectStep.ReconnectingRoom;
            }
            private void OnRegisterPlayerSuccess(object sender, GameEventArgs e)
            {
                //if (m_RoomStep == RoomConnectStep.ConnectingRoomSuccess)
                //{
                //    m_RoomStep = RoomConnectStep.RoomReadySuccess;
                //}
                if (m_RoomStep == RoomConnectStep.ServerError)
                {
                    return;
                }
                m_RoomStep = RoomConnectStep.RoomReadySuccess;
            }
            private void OnReceivePvpStart(object sender, GameEventArgs args)
            {
                StartSinglePvpEventArgs e = args as StartSinglePvpEventArgs;
                if (e == null)
                {
                    return;
                }
                if (m_RoomStep >= RoomConnectStep.RoomReadySuccess)
                {
                    m_RoomStep = RoomConnectStep.RoomStart;
                }
                //System.DateTime serverTime = new System.DateTime(e.StartTime, System.DateTimeKind.Utc);
                startTime = 3f /*- (float)(System.DateTime.UtcNow - serverTime).TotalSeconds*/ + Time.time;
            }
            private void OnServerError(object sender, GameEventArgs e)
            {
                ServerErrorEventArgs networkCustomErrorData = e as ServerErrorEventArgs;

                if ((ServerErrorCode)networkCustomErrorData.NetworkErrorData.ErrorCode == ServerErrorCode.RoomStatusError)
                {
                    m_RoomStep = RoomConnectStep.ServerError;
                }
            }
            private void OnGetBattleStatus(object sender, GameEventArgs e)
            {

                RCGetRoomBattleStatus battleStatus = (e as GetRoomBattleStatusEventArgs).BattleStatus;
                var pvpInstanceLogic = m_InstanceLogic as BasePvpInstanceLogic;
                pvpInstanceLogic.SetRoomBattleStatus(battleStatus);
                m_RoomStep = RoomConnectStep.RoomReconnectSetStatus;
                var dataRow = pvpInstanceLogic.DRInstance;
                pvpInstanceLogic.InstanceStartTime =  Time.time - (dataRow.TimerDuration - battleStatus.LeftTime);


                GameEntry.Data.Room.HasReconnected = false;
                GameEntry.RoomLogic.ClaimRoomReconnectReady();
            }

            public override void OnLoadFail()
            {
                //GameEntry.Event.Fire(this, new InstanceReadyToStartEventArgs());
                //var pvpInstanceLogic = m_InstanceLogic as BasePvpInstanceLogic;
                //(m_InstanceLogic as BasePvpInstanceLogic).EnforceGoBackToLobby();
                //GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenActivitySinglePvpMainForm, true);
                //GameEntry.SceneLogic.GoBackToLobby(false);
                m_RoomStep = RoomConnectStep.ServerError;


            }

            private enum ResourcesStep
            {
                LoadingScene,

                LoadingBehaviors,

                /// <summary>
                /// Preparing my heroes, opponent's heroes and battle form.
                /// </summary>
                ///
                StartPreparingResouces,

                PreparingResouces,

                PreparingResoucesSuccess,

                StartingInstance,

                /// <summary>
                /// Wait a while before going to <see cref="StateWaiting"/> for better display on <see cref="BattleForm"/>.
                /// </summary>
                DelayWaitingState,
            }

            private enum RoomConnectStep
            {
                ServerError,

                ServerErrorEvent,

                ServerErrorEnd,

                ReconnectingRoom,

                StartConnectingRoom,

                ConnectingRoom,

                ConnectingRoomSuccess,

                StartRoomReady,

                RoomReady,
                /// <summary>
                /// room的资源准备完成
                /// </summary>
                RoomReadySuccess,
                /// <summary>
                /// 如果处在重连状态，设置当前场景的过程
                /// </summary>
                RoomReconnectSetStatus,

                RoomStartRequest,

                RoomStart,

                //WaitOtherPlayer,
            }

        }
    }
}
