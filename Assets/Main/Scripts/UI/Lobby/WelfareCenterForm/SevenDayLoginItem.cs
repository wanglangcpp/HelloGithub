using UnityEngine;
using System.Collections;
using System;

namespace Genesis.GameClient
{
    public class SevenDayLoginItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_Days = null;
        [SerializeField]
        private GameObject m_LabelHint = null;
        [SerializeField]
        private GameObject m_IconClaim = null;
        [SerializeField]
        private GameObject m_HasClaimMask = null;
        [SerializeField]
        private GeneralItemView[] m_Rewards = null;

        private DRSevenDayLogin m_Data = null;

        public void RefreshItem(DRSevenDayLogin data)
        {
            m_Data = data;
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
            UIEventListener.Get(m_IconClaim).onClick = OnClickClaimRewardsBtn;
            m_Days.text = GameEntry.Localization.GetString("UI_TEXT_OPERATION_DAY_NUMBER", m_Data.Id);
            for (int i = 0; i < m_Rewards.Length; i++)
            {
                if (i > m_Data.Rewards.Count)
                {
                    m_Rewards[i].gameObject.SetActive(false);
                    continue;
                }
                m_Rewards[i].InitItem(m_Data.Rewards[i].Type, m_Data.Rewards[i].Count);
            }
            SetRewardStatus();
        }
        public void SetRewardStatus()
        {
            m_LabelHint.SetActive(false);
            m_IconClaim.SetActive(false);
            m_HasClaimMask.SetActive(false);
            bool isNoCliam = (GameEntry.Data.SevenDayLoginData.RewardState & (1 << m_Data.Id)) == 0;
            //奖励状态（0-不可领取，1-可领取，2-已领取）
            int status = isNoCliam ? GameEntry.Data.SevenDayLoginData.LoginCount < m_Data.Id ? 0 : 1 : 2;
            if (status == 0 && GameEntry.Data.SevenDayLoginData.LoginCount + 1 == m_Data.Id)
            {
                m_LabelHint.SetActive(true);
            }
            else if (status == 1)
            {
                m_IconClaim.SetActive(true);
            }
            else if (status == 2)
            {
                m_HasClaimMask.SetActive(true);
            }
        }
        private void OnClickClaimRewardsBtn(GameObject go)
        {
            CLClaimSevenDayLoginReward sender = new CLClaimSevenDayLoginReward();
            sender.RewardDay = m_Data.Id;
            GameEntry.Network.Send(sender);
            GameEntry.Data.SevenDayLoginData.ClaimId = m_Data.Id;
        }
    }
}

