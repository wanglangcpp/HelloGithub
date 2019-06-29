using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        /// <summary>
        /// 有队伍向无队伍过渡的状态。
        /// </summary>
        private class StateHasTeamToNoTeam : StateBase
        {
            private Animation m_ButtonListAnim = null;

            protected override void OnEnter(IFsm<ActivityFoundryForm> fsm)
            {
                base.OnEnter(fsm);
                var owner = fsm.Owner;
                owner.ResetNoTeamDisplay();
                m_ButtonListAnim = owner.m_ButtonListNoTeam.gameObject.GetComponent<Animation>();
                m_ButtonListAnim.Rewind();
                m_ButtonListAnim.Play();

                if (owner.m_CDMaskDoorStatus != CDMaskDoorStatus.Closed)
                {
                    owner.CloseCDMaskDoor(true);
                }
            }

            protected override void OnLeave(IFsm<ActivityFoundryForm> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<ActivityFoundryForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (!m_ButtonListAnim.isPlaying && !fsm.Owner.IsCDMaskDoorAnimating)
                {
                    CheckDataAndChangeState(fsm);
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
