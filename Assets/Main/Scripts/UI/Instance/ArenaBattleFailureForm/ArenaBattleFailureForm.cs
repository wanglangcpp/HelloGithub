using GameFramework.Fsm;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技战斗副本失败界面。
    /// </summary>
    public class ArenaBattleFailureForm : BaseArenaBattleResultForm
    {
        protected override bool OnRankStateUpdate(ref bool myAnimationHasStarted, ref bool oppAnimationHasStarted, ref bool oppAnimationHasStopped, ref float oppAnimationStopTime, IFsm<BaseArenaBattleResultForm> fsm)
        {
            if (!oppAnimationHasStarted)
            {
                myAnimationHasStarted = true;
                oppAnimationHasStarted = true;
                m_Me.Animation.Play();
                m_Opponent.Animation.Play();
                return true;
            }

            if (!oppAnimationHasStopped && !m_Opponent.Animation.isPlaying)
            {
                oppAnimationHasStopped = true;
                oppAnimationStopTime = fsm.CurrentStateTime;
                return true;
            }

            return false;
        }

        protected override OfflineArenaBattleResultDataObtainedEventArgs ParseUserData(object userData)
        {
            var myUserData = userData as ArenaBattleFailureDisplayData;
            if (myUserData == null)
            {
                return null;
            }

            return myUserData.EventArgs;
        }
    }
}
