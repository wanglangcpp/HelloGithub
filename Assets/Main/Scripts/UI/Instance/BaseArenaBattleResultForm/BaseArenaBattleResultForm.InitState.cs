using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class BaseArenaBattleResultForm
    {
        protected class InitState : StateBase
        {
            protected override void OnEnter(IFsm<BaseArenaBattleResultForm> fsm)
            {
                base.OnEnter(fsm);
                fsm.Owner.Reset();
                if (fsm.Owner.m_UserData.MyNewRank == fsm.Owner.m_UserData.MyOldRank)
                {
                    ChangeState<RewardState>(fsm);
                }
                else
                {
                    ChangeState<RankState>(fsm);
                }
            }
        }
    }
}
