using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class BuildingMotion
    {
        private class EnteringState : StateBase
        {
            protected override string AnimClipAlias
            {
                get
                {
                    return "Entering";
                }
            }

            protected override void OnEnter(IFsm<BuildingMotion> fsm)
            {
                fsm.Owner.BuildingOwner.EnableNavMeshObstacles(false);
                base.OnEnter(fsm);
                PlayAnimation(fsm, m_AnimInfo);
            }

            protected override void OnUpdate(IFsm<BuildingMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (fsm.CurrentStateTime > m_AnimInfo.AnimMaxLength - Constant.DefaultAnimCrossFadeDuration)
                {
                    ChangeState<DefaultState>(fsm);
                }
            }
        }
    }
}
