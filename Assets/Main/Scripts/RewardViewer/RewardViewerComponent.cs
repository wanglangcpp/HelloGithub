using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 领奖视图组件。
    /// </summary>
    public class RewardViewerComponent : MonoBehaviour
    {
        private Queue<InternalData> m_InternalDatas = new Queue<InternalData>();

        private InternalData m_CurrentInternalData = null;

        private IFsm<RewardViewerComponent> m_Fsm = null;

        public void RequestShowRewards(ReceivedGeneralItemsViewData rewardViewData, bool useFakeItems)
        {
            RequestShowRewards(rewardViewData, useFakeItems, null, null);
        }

        public void RequestShowRewards(ReceivedGeneralItemsViewData rewardViewData, bool useFakeItems, GameFrameworkAction<object> onComplete, object userData)
        {
            m_InternalDatas.Enqueue(new InternalData { RewardViewData = rewardViewData, UseFakeItems = useFakeItems, OnComplete = onComplete, UserData = userData });
        }

        #region MonoBehaviour

        private void Start()
        {
            m_Fsm = GameEntry.Fsm.CreateFsm(this, new StateIdle(), new StateShowHeroes(), new StateShowOtherItems());
            m_Fsm.Start<StateIdle>();
        }

        private void Update()
        {

        }

        private void OnDestroy()
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Fsm.DestroyFsm(m_Fsm);
            }

            m_Fsm = null;
        }

        #endregion MonoBehaviour

        private class InternalData
        {
            public ReceivedGeneralItemsViewData RewardViewData;
            public GameFrameworkAction<object> OnComplete;
            public object UserData;
            public bool UseFakeItems;
        }

        private abstract class StateBase : FsmState<RewardViewerComponent>
        {
            protected IFsm<RewardViewerComponent> m_Fsm = null;
            protected RewardViewerComponent m_Owner = null;
            protected InternalData CurrentInternalData { get { return m_Owner.m_CurrentInternalData; } }

            protected override void OnInit(IFsm<RewardViewerComponent> fsm)
            {
                base.OnInit(fsm);
                m_Fsm = fsm;
                m_Owner = fsm.Owner;
            }

            protected override void OnEnter(IFsm<RewardViewerComponent> fsm)
            {
                base.OnEnter(fsm);
            }

            protected override void OnLeave(IFsm<RewardViewerComponent> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }

            protected abstract void Next(IFsm<RewardViewerComponent> fsm);

            protected void Finish()
            {
                if (CurrentInternalData.OnComplete != null)
                {
                    CurrentInternalData.OnComplete(CurrentInternalData.UserData);
                }

                m_Owner.m_CurrentInternalData = null;
            }
        }

        private class StateIdle : StateBase
        {
            protected override void OnUpdate(IFsm<RewardViewerComponent> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (m_Owner.m_InternalDatas.Count <= 0)
                {
                    return;
                }

                m_Owner.m_CurrentInternalData = m_Owner.m_InternalDatas.Dequeue();

                if (CurrentInternalData.RewardViewData == null)
                {
                    Finish();
                    return;
                }

                Next(fsm);
            }

            protected override void Next(IFsm<RewardViewerComponent> fsm)
            {
                ChangeState<StateShowHeroes>(fsm);
            }
        }

        private class StateShowHeroes : StateBase
        {
            protected override void OnEnter(IFsm<RewardViewerComponent> fsm)
            {
                base.OnEnter(fsm);
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.CloseUIFormComplete, OnCloseUIFormComplete);

                var displayData = CurrentInternalData.RewardViewData.GetShowHeroData();
                if (displayData.Count <= 0)
                {
                    Next(fsm);
                    return;
                }

                GameEntry.UI.OpenUIForm(UIFormId.ReceiveHeroForm, displayData);
            }

            protected void OnCloseUIFormComplete(object sender, GameEventArgs e)
            {
                var ne = e as UnityGameFramework.Runtime.CloseUIFormCompleteEventArgs;
                if (ne.UIFormTypeId == (int)UIFormId.ReceiveHeroForm)
                {
                    Next(m_Fsm);
                }
            }

            protected override void OnLeave(IFsm<RewardViewerComponent> fsm, bool isShutdown)
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.CloseUIFormComplete, OnCloseUIFormComplete);
                }

                base.OnLeave(fsm, isShutdown);
            }

            protected override void Next(IFsm<RewardViewerComponent> fsm)
            {
                ChangeState<StateShowOtherItems>(fsm);
            }
        }

        private class StateShowOtherItems : StateBase
        {
            protected override void OnEnter(IFsm<RewardViewerComponent> fsm)
            {
                base.OnEnter(fsm);
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.CloseUIFormComplete, OnCloseUIFormComplete);

                var displayData = CurrentInternalData.RewardViewData.GetShowItemData();
                if (displayData.Count <= 0)
                {
                    Next(fsm);
                    return;
                }

                GameEntry.UI.OpenUIForm(UIFormId.ReceiveItemForm, displayData);
            }

            protected void OnCloseUIFormComplete(object sender, GameEventArgs e)
            {
                var ne = e as UnityGameFramework.Runtime.CloseUIFormCompleteEventArgs;
                if (ne.UIFormTypeId == (int)UIFormId.ReceiveItemForm)
                {
                    Next(m_Fsm);
                }
            }

            protected override void OnLeave(IFsm<RewardViewerComponent> fsm, bool isShutdown)
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.CloseUIFormComplete, OnCloseUIFormComplete);
                }

                base.OnLeave(fsm, isShutdown);
            }

            protected override void Next(IFsm<RewardViewerComponent> fsm)
            {
                Finish();
                ChangeState<StateIdle>(fsm);
            }
        }
    }
}
