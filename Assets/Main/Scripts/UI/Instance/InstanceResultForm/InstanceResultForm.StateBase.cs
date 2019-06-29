using GameFramework.Fsm;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class InstanceResultForm
    {
        private abstract class StateBase : FsmState<InstanceResultForm>
        {
            protected Type m_NextStateType = null;

            private Transform m_CachedSubPanel = null;

            private Animation m_CachedAnimation = null;

            protected Animation CachedAnimation
            {
                get
                {
                    if (m_CachedAnimation != null)
                    {
                        return m_CachedAnimation;
                    }

                    if (m_CachedSubPanel != null)
                    {
                        m_CachedAnimation = m_CachedSubPanel.GetComponent<Animation>();
                    }

                    return m_CachedAnimation;
                }
            }

            private Transform m_LastSubPanel = null;

            private Animation m_LastAnimation = null;

            protected Animation LastAnimation
            {
                get
                {
                    if (m_LastAnimation != null)
                    {
                        return m_LastAnimation;
                    }

                    if (m_LastSubPanel != null)
                    {
                        m_LastAnimation = m_LastSubPanel.GetComponent<Animation>();
                    }

                    return m_LastAnimation;
                }
            }

            protected const string InwardClipName = "InstanceResultSubPanelInward";
            protected const string OutwardClipName = "InstanceResultSubPanelOutward";
            protected const string StarsMoveAnimation = "StarMove";

            protected override void OnEnter(IFsm<InstanceResultForm> fsm)
            {
                base.OnEnter(fsm);

                if (m_CachedSubPanel != null)
                {
                    m_CachedSubPanel.gameObject.SetActive(true);
                }

                RewindAndPlayAnimation(InwardClipName);
            }

            protected override void OnLeave(IFsm<InstanceResultForm> fsm, bool isShutdown)
            {
                RewindAndPlayAnimation(OutwardClipName);

                if (LastAnimation != null)
                {
                    LastAnimation.Stop();
                }

                if (m_LastSubPanel != null)
                {
                    m_LastSubPanel.gameObject.SetActive(false);
                }

                base.OnLeave(fsm, isShutdown);
            }

            public abstract void GoToNextStep(IFsm<InstanceResultForm> fsm);

            protected void RewindAndPlayAnimation(string clipName)
            {
                if (CachedAnimation == null || CachedAnimation[clipName] == null)
                {
                    return;
                }

                CachedAnimation.Rewind(clipName);
                CachedAnimation.Play(clipName);
            }

            public abstract void SkipAnimation(IFsm<InstanceResultForm> fsm);

            public StateBase() : this(null, null, null)
            {

            }

            public StateBase(Type nextStateType, Transform currentSubPanel = null, Transform lastSubPanel = null) : base()
            {
                m_NextStateType = nextStateType;
                m_CachedSubPanel = currentSubPanel;
                m_LastSubPanel = lastSubPanel;
            }

            public virtual void GotoNextInstance(IFsm<InstanceResultForm> m_Fsm)
            {
            }
        }
    }
}
