using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        /// <summary>
        /// 无队伍向有队伍过渡的状态。
        /// </summary>
        private class StateNoTeamToHasTeam : StateBase
        {
            private const float m_AnimSeqInterval = 4f / 30f;

            private Animation m_ButtonListAnim = null;

            private float m_CachedTime;

            private Member[] m_Members;

            protected override void OnEnter(IFsm<ActivityFoundryForm> fsm)
            {
                base.OnEnter(fsm);
                var owner = fsm.Owner;
                var srcData = owner.m_SrcData;

                m_Members = owner.m_Members;

                owner.m_ButtonListNoTeam.gameObject.SetActive(false);
                owner.m_ButtonListHasTeam.gameObject.SetActive(true);

                int bgLevel = owner.RewardLevel >= 0 ? owner.RewardLevel : srcData.Progress.CurrentLevel;
                for (int i = 0; i < owner.m_LevelIndicators.Length; ++i)
                {
                    owner.m_LevelIndicators[i].gameObject.SetActive(i == bgLevel);
                }

                m_ButtonListAnim = owner.m_ButtonListHasTeam.gameObject.GetComponent<Animation>();
                m_ButtonListAnim.Rewind();
                m_ButtonListAnim.Play();
                m_CachedTime = -1f;

                owner.RefreshPlayers();

                owner.m_ProgressText.gameObject.SetActive(true);
                owner.RefreshProgress();
            }

            protected override void OnLeave(IFsm<ActivityFoundryForm> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<ActivityFoundryForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                for (int i = 0; i < m_Members.Length; ++i)
                {
                    if (m_CachedTime < i * m_AnimSeqInterval && fsm.CurrentStateTime >= i * m_AnimSeqInterval)
                    {
                        var member = m_Members[i];
                        if (!member.Root.gameObject.activeSelf)
                        {
                            member.Root.gameObject.SetActive(true);
                            if (member.KickButton != null)
                            {
                                member.KickButton.isEnabled = false;
                            }

                            member.Animation.enabled = true;
                            member.Animation.clip.SampleAnimation(member.Root.gameObject, 0f);
                            member.Animation.Play();
                        }
                    }
                }

                if (AllAnimationsAreComplete)
                {
                    CheckDataAndChangeState(fsm);
                }

                m_CachedTime = fsm.CurrentStateTime;
            }

            private bool AllAnimationsAreComplete
            {
                get
                {
                    if (m_ButtonListAnim.isPlaying)
                    {
                        return false;
                    }

                    for (int i = 0; i < m_Members.Length; ++i)
                    {
                        var member = m_Members[i];
                        if (!member.Root.gameObject.activeSelf || member.Animation.isPlaying)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            private void CheckDataAndChangeState(IFsm<ActivityFoundryForm> fsm)
            {
                if (!fsm.Owner.m_SrcData.HasTeam)
                {
                    ChangeState<StateNoTeam>(fsm);
                }
                else
                {
                    ChangeState<StateHasTeam>(fsm);
                }
            }
        }
    }
}
