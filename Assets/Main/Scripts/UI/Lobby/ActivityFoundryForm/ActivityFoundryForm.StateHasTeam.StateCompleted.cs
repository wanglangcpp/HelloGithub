using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        private partial class StateHasTeam
        {
            /// <summary>
            /// 已通关状态。
            /// </summary>
            private class StateCompleted : StateBase
            {
                protected override void OnEnter(IFsm<StateHasTeam> fsm)
                {
                    base.OnEnter(fsm);
                    m_OuterFsm.Owner.CloseCDMaskDoor(false);

                    var levelIndicators = m_OuterFsm.Owner.m_LevelIndicators;
                    for (int i = 0; i < levelIndicators.Length; ++i)
                    {
                        levelIndicators[i].gameObject.SetActive(i == levelIndicators.Length - 1);
                        levelIndicators[i].GetComponent<Animation>().Stop();
                    }

                    m_OuterFsm.Owner.m_ProgressText.text = GameEntry.Localization.GetString("UI_TEXT_FOUNDRY_COMPLETE");
                }
            }
        }
    }
}
