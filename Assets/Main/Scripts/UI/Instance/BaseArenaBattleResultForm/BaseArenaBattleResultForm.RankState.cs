using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class BaseArenaBattleResultForm
    {
        protected class RankState : StateBase
        {
            private Player m_Me = null;
            private Player m_Opponent = null;

            private bool m_MyAnimationHasStarted = false;
            private bool m_OppAnimationHasStarted = false;
            private bool m_OppAnimationHasStopped = false;
            private float m_OppAnimationStopTime = 0f;

            private bool m_WholeScreenButtonClicked = false;

            protected override void OnInit(IFsm<BaseArenaBattleResultForm> fsm)
            {
                base.OnInit(fsm);
                m_CachedSubPanel = fsm.Owner.m_RankSubPanel;

                m_Me = fsm.Owner.m_Me;
                m_Me.Rank.text = fsm.Owner.m_UserData.MyNewRank.ToString();
                m_Me.Portrait.SetPortrait(GameEntry.Data.Player);

                m_Opponent = fsm.Owner.m_Opponent;
                m_Opponent.Rank.text = fsm.Owner.m_UserData.MyOldRank.ToString();
                m_Opponent.Portrait.SetPortrait(GameEntry.Data.OfflineArenaOpponent.Player);
            }

            protected override void OnEnter(IFsm<BaseArenaBattleResultForm> fsm)
            {
                base.OnEnter(fsm);
                m_MyAnimationHasStarted = false;
                m_OppAnimationHasStarted = false;
                m_OppAnimationHasStopped = false;
                m_OppAnimationStopTime = 0f;
                m_WholeScreenButtonClicked = false;

                fsm.Owner.m_RankSubPanel.gameObject.SetActive(true);
                m_Me.Animation.Stop();
                m_Me.Animation.clip.SampleAnimation(m_Me.Self.gameObject, 0f);
                m_Opponent.Animation.Stop();
                m_Opponent.Animation.clip.SampleAnimation(m_Opponent.Self.gameObject, 0f);
            }

            protected override void OnUpdate(IFsm<BaseArenaBattleResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (m_WholeScreenButtonClicked)
                {
                    return;
                }

                if (CachedAnimation[InwardClipName].normalizedTime < 1f)
                {
                    return;
                }

                if (fsm.Owner.OnRankStateUpdate(ref m_MyAnimationHasStarted, ref m_OppAnimationHasStarted, ref m_OppAnimationHasStopped, ref m_OppAnimationStopTime, fsm))
                {
                    return;
                }

                if (fsm.Owner.HasGotReward && fsm.CurrentStateTime - m_OppAnimationStopTime > fsm.Owner.m_RankSubPanelDuration)
                {
                    GoToNextState(fsm);
                    return;
                }
            }

            public override void OnClickWholeScreenButton(IFsm<BaseArenaBattleResultForm> fsm)
            {
                m_WholeScreenButtonClicked = true;

                if (!fsm.Owner.HasGotReward)
                {
                    CachedAnimation.Stop();
                    CachedAnimation[InwardClipName].clip.SampleAnimation(m_CachedSubPanel.gameObject, 1f);
                    m_Me.Animation.Stop();
                    m_Me.Animation.clip.SampleAnimation(m_Me.Self.gameObject, 1f);
                    m_Opponent.Animation.Stop();
                    m_Opponent.Animation.clip.SampleAnimation(m_Opponent.Self.gameObject, 1f);
                    return;
                }

                base.OnClickWholeScreenButton(fsm);
                GoToNextState(fsm);
            }

            private void GoToNextState(IFsm<BaseArenaBattleResultForm> fsm)
            {
                ChangeState<RewardState>(fsm);
            }
        }
    }
}
