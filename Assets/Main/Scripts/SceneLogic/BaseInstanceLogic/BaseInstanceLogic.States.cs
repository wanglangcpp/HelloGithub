using GameFramework;
using GameFramework.Fsm;
using System;

namespace Genesis.GameClient
{
    public abstract partial class BaseInstanceLogic
    {
        private abstract class StateBase : FsmState<BaseInstanceLogic>
        {
            private IFsm<BaseInstanceLogic> m_Fsm = null;

            protected BaseInstanceLogic InstanceLogic
            {
                get
                {
                    return m_Fsm.Owner;
                }
            }

            protected override void OnInit(IFsm<BaseInstanceLogic> fsm)
            {
                base.OnInit(fsm);
                m_Fsm = fsm;
            }

            protected override void OnEnter(IFsm<BaseInstanceLogic> fsm)
            {
                base.OnEnter(fsm);
                Log.Info("[BaseInstanceLogic.{0} OnEnter]", GetType().Name);
            }

            protected override void OnLeave(IFsm<BaseInstanceLogic> fsm, bool isShutdown)
            {
                Log.Info("[BaseInstanceLogic.{0} OnLeave]", GetType().Name);
                base.OnLeave(fsm, isShutdown);
            }

            public virtual void StartInstance(IFsm<BaseInstanceLogic> m_Fsm)
            {
                throw new NotSupportedException();
            }

            public virtual void OnInstanceSuccess(IFsm<BaseInstanceLogic> fsm, InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {

            }

            public virtual void OnInstanceFailure(IFsm<BaseInstanceLogic> fsm, InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
            {

            }

            public virtual void OnInstanceDraw(IFsm<BaseInstanceLogic> fsm, InstanceDrawReason reason, GameFrameworkAction onComplete)
            {

            }

            public virtual void OnLoadSceneSuccess(IFsm<BaseInstanceLogic> fsm, UnityGameFramework.Runtime.LoadSceneSuccessEventArgs e)
            {

            }

            public virtual void OnOpenUIFormSuccess(IFsm<BaseInstanceLogic> fsm, UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs e)
            {

            }

            public virtual void OnLoadBehaviorSuccess(IFsm<BaseInstanceLogic> fsm, LoadBehaviorSuccessEventArgs ne)
            {

            }
        }

        private class StatePrepare : StateBase
        {
            private GameFrameworkFunc<AbstractInstancePreparer> m_PreparerGetter = null;
            private AbstractInstancePreparer m_Preparer = null;

            public StatePrepare(GameFrameworkFunc<AbstractInstancePreparer> preparerGetter)
            {
                m_PreparerGetter = preparerGetter;
            }

            public override void OnLoadSceneSuccess(IFsm<BaseInstanceLogic> fsm, UnityGameFramework.Runtime.LoadSceneSuccessEventArgs e)
            {
                m_Preparer.OnLoadSceneSuccess(e);
            }

            public override void OnOpenUIFormSuccess(IFsm<BaseInstanceLogic> fsm, UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs e)
            {
                m_Preparer.OnOpenUIFormSuccess(e);
            }

            public override void StartInstance(IFsm<BaseInstanceLogic> fsm)
            {
                m_Preparer.StartInstance();
            }

            protected override void OnEnter(IFsm<BaseInstanceLogic> fsm)
            {
                base.OnEnter(fsm);
                m_Preparer = m_PreparerGetter();
                m_Preparer.Init(InstanceLogic);
                m_Preparer.OnShouldGoToWaiting += delegate () { ChangeState<StateWaiting>(fsm); };
                m_Preparer.OnShouldGoToResult += delegate () { ChangeState<StateResult>(fsm); };
            }

            protected override void OnLeave(IFsm<BaseInstanceLogic> fsm, bool isShutdown)
            {
                m_Preparer.Shutdown(isShutdown);
                m_Preparer = null;
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<BaseInstanceLogic> fsm, float elapseSeconds, float realElapseSeconds)
            {
                m_Preparer.OnUpdate(elapseSeconds, realElapseSeconds);
            }

            public override void OnLoadBehaviorSuccess(IFsm<BaseInstanceLogic> fsm, LoadBehaviorSuccessEventArgs ne)
            {
                m_Preparer.OnLoadBehaviorSuccess(ne);
            }

            public override void OnInstanceDraw(IFsm<BaseInstanceLogic> fsm, InstanceDrawReason reason, GameFrameworkAction onComplete)
            {
                m_Preparer.OnLoadFail();
            }

            public override void OnInstanceSuccess(IFsm<BaseInstanceLogic> fsm, InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                m_Preparer.OnLoadFail();
            }

            public override void OnInstanceFailure(IFsm<BaseInstanceLogic> fsm, InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
            {
                m_Preparer.OnLoadFail();
            }
        }

        private class StateWaiting : StateBase
        {
            private GameFrameworkFunc<AbstractInstanceWaiter> m_WaiterGetter;
            private AbstractInstanceWaiter m_Waiter;

            public StateWaiting(GameFrameworkFunc<AbstractInstanceWaiter> waiterGetter)
            {
                m_WaiterGetter = waiterGetter;
            }

            protected override void OnEnter(IFsm<BaseInstanceLogic> fsm)
            {
                base.OnEnter(fsm);
                m_Waiter = m_WaiterGetter();
                m_Waiter.OnShouldGoToRunning += delegate () { ChangeState<StateRunning>(fsm); };
                m_Waiter.Init(InstanceLogic);
            }

            protected override void OnLeave(IFsm<BaseInstanceLogic> fsm, bool isShutdown)
            {
                m_Waiter.Shutdown(isShutdown);
                m_Waiter = null;
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<BaseInstanceLogic> fsm, float elapseSeconds, float realElapseSeconds)
            {
                m_Waiter.OnUpdate(elapseSeconds, realElapseSeconds);
            }

            public override void OnInstanceDraw(IFsm<BaseInstanceLogic> fsm, InstanceDrawReason reason, GameFrameworkAction onComplete)
            {
                m_Waiter.OnDraw(InstanceDrawReason.Default, null);
            }

            public override void OnInstanceSuccess(IFsm<BaseInstanceLogic> fsm, InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                m_Waiter.OnWin(InstanceSuccessReason.Unknown, null);
            }

            public override void OnInstanceFailure(IFsm<BaseInstanceLogic> fsm, InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
            {
                m_Waiter.OnLose(InstanceFailureReason.Unknown, false, null);
            }
        }

        private class StateRunning : StateBase
        {
            private GameFrameworkFunc<AbstractInstanceRunner> m_RunnerGetter;
            private AbstractInstanceRunner m_Runner = null;

            public StateRunning(GameFrameworkFunc<AbstractInstanceRunner> runnerGetter)
            {
                m_RunnerGetter = runnerGetter;
            }

            protected override void OnEnter(IFsm<BaseInstanceLogic> fsm)
            {
                base.OnEnter(fsm);
                m_Runner = m_RunnerGetter();
                m_Runner.Init(InstanceLogic);
                m_Runner.OnShouldGoToResult += delegate () { ChangeState<StateResult>(fsm); };
            }

            protected override void OnUpdate(IFsm<BaseInstanceLogic> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                m_Runner.OnUpdate(elapseSeconds, realElapseSeconds);
            }

            protected override void OnLeave(IFsm<BaseInstanceLogic> fsm, bool isShutdown)
            {
                m_Runner.Shutdown(isShutdown);
                m_Runner = null;
                base.OnLeave(fsm, isShutdown);
            }

            public override void OnInstanceSuccess(IFsm<BaseInstanceLogic> fsm, InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                m_Runner.OnWin(reason, onComplete);
            }

            public override void OnInstanceFailure(IFsm<BaseInstanceLogic> fsm, InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
            {
                m_Runner.OnLose(reason, shouldOpenUI, onComplete);
            }
        }

        private class StateResult : StateBase
        {
            private GameFrameworkFunc<AbstractInstanceResult> m_ResultGetter;

            public StateResult(GameFrameworkFunc<AbstractInstanceResult> resultGetter)
            {
                m_ResultGetter = resultGetter;
            }

            protected override void OnEnter(IFsm<BaseInstanceLogic> fsm)
            {
                base.OnEnter(fsm);
                m_ResultGetter().Start();
            }

            public override void OnInstanceSuccess(IFsm<BaseInstanceLogic> fsm, InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                throw new NotSupportedException();
            }

            public override void OnInstanceFailure(IFsm<BaseInstanceLogic> fsm, InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
            {
                throw new NotSupportedException();
            }
        }
    }
}
