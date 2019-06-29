using UnityEngine;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class ChestRewardSubForm : NGUIForm
    {
        [SerializeField]
        private UILabel m_InstructionLabel = null;

        [SerializeField]
        private UILabel m_TitleLabel = null;

        [SerializeField]
        private List<GeneralItemView> m_RewardList = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            ChestRewardDisplayData displayData = userData as ChestRewardDisplayData;
            if (displayData == null)
            {
                CloseSelf(true);
                return;
            }

            for (int i = 0; i < m_RewardList.Count; i++)
            {
                if (i < displayData.Rewards.Count)
                {
                    m_RewardList[i].InitGeneralItem(displayData.Rewards[i].Id, displayData.Rewards[i].Count);
                    m_RewardList[i].gameObject.SetActive(true);
                }
                else
                {
                    m_RewardList[i].gameObject.SetActive(false);
                }
            }

            m_InstructionLabel.text = displayData.InstructionLabelString;
            m_TitleLabel.text = displayData.TitleLabelString;
        }

    }
}
