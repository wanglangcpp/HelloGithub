using GameFramework.Fsm;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技战斗副本成功界面。
    /// </summary>
    public class ArenaBattleResultForm : BaseArenaBattleResultForm
    {
        protected override bool OnRankStateUpdate(ref bool myAnimationHasStarted, ref bool oppAnimationHasStarted, ref bool oppAnimationHasStopped, ref float oppAnimationStopTime, IFsm<BaseArenaBattleResultForm> fsm)
        {
            if (!myAnimationHasStarted && !oppAnimationHasStarted)
            {
                myAnimationHasStarted = true;
                m_Me.Animation.Play();
                oppAnimationHasStarted = true;
                m_Opponent.Animation.Play();
                m_EffectsController.ShowEffect("EffectOpticalCross");
                m_EffectsController.ShowEffect("EffectPlayerLight");
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
            var myUserData = userData as ArenaBattleResultDisplayData;
            if (myUserData == null)
            {
                return null;
            }

            return myUserData.EventArgs;
        }
    }
}
