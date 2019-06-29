using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BaseArenaBattleResultForm
    {
        protected abstract class StateBase : FsmState<BaseArenaBattleResultForm>
        {
            protected Animation m_CachedAnimation = null;

            protected Transform m_CachedSubPanel = null;

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

            protected Transform m_LastSubPanel = null;

            protected Animation m_LastAnimation = null;

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

            public virtual void OnClickWholeScreenButton(IFsm<BaseArenaBattleResultForm> fsm)
            {
                if (m_CachedSubPanel != null)
                {
                    m_CachedSubPanel.gameObject.SetActive(false);
                }

                if (m_LastSubPanel != null)
                {
                    m_LastSubPanel.gameObject.SetActive(false);
                }
            }

            protected override void OnEnter(IFsm<BaseArenaBattleResultForm> fsm)
            {
                base.OnEnter(fsm);

                if (m_CachedSubPanel != null)
                {
                    m_CachedSubPanel.gameObject.SetActive(true);
                }

                RewindAndPlayAnimation(InwardClipName);
            }

            protected override void OnLeave(IFsm<BaseArenaBattleResultForm> fsm, bool isShutdown)
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

            protected void RewindAndPlayAnimation(string clipName)
            {
                if (CachedAnimation == null)
                {
                    return;
                }

                CachedAnimation.Rewind(clipName);
                CachedAnimation.Play(clipName);
            }
        }
    }
}
