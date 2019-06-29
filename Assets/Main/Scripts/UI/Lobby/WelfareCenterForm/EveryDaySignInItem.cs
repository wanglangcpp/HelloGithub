using UnityEngine;
using System.Collections;

namespace Genesis.GameClient
{
    public class EveryDaySignInItem : MonoBehaviour
    {
        /// <summary>
        /// VIP翻倍
        /// </summary>
        [SerializeField]
        private GameObject m_VipDoubleObject = null;
        [SerializeField]
        private UILabel m_VipDoubleLabel = null;
        /// <summary>
        /// 当天的签到背景
        /// </summary>
        [SerializeField]
        private GameObject m_TodaySelectionObject = null;
        /// <summary>
        /// 已签遮罩
        /// </summary>
        [SerializeField]
        private GameObject m_ClaimedRewardMarkObject = null;
        /// <summary>
        /// 补签遮罩
        /// </summary>
        [SerializeField]
        private GameObject m_RetroactiveMarkObject = null;
        /// <summary>
        /// 奖励物品
        /// </summary>
        [SerializeField]
        private GeneralItemView m_RewardItemView = null;

        [SerializeField]
        private UIEffectsController m_Effect;

        private DRDailyLogin m_CachedDataRow = null;

        [HideInInspector]
        public bool IsToday = false;
        /// <summary>
        /// 签到状态
        /// </summary>
        int status = 0;

        public void SetItemData(DRDailyLogin dailyLoginRow)
        {
            m_CachedDataRow = dailyLoginRow;

            if (m_CachedDataRow.NeedVipLevel < 0)
            {
                m_VipDoubleObject.SetActive(false);
            }
            else
            {
                m_VipDoubleObject.SetActive(true);
                m_VipDoubleLabel.text = GameEntry.Localization.GetString("UI_SIGN_VIP_DOUBLE", m_CachedDataRow.NeedVipLevel);
            }

            m_RewardItemView.InitItem(m_CachedDataRow.RewardType, m_CachedDataRow.RewardCount);

            RefreshIconState();
            UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate((go) => { m_RewardItemView.OnClick(); });
        }

        public void RefreshIconState()
        {
            m_RewardItemView.ResetOnClickDelegate();
            m_TodaySelectionObject.SetActive(false);
            m_ClaimedRewardMarkObject.SetActive(false);
            m_RetroactiveMarkObject.SetActive(false);
            IsToday = m_CachedDataRow.Id == System.DateTime.UtcNow.Day;

            var ClaimStatus = GameEntry.Data.EveryDaySignInData.ClaimStatus;
            if (ClaimStatus != null && ClaimStatus.Count != 0)
                status = ClaimStatus[m_CachedDataRow.Id];

            if (status == 2)
            {
                m_ClaimedRewardMarkObject.SetActive(true);
                m_RewardItemView.ItemIcon.color = Color.grey;
            }
            else
            {
                m_RewardItemView.ItemIcon.color = Color.white;
                if (m_CachedDataRow.Id == System.DateTime.UtcNow.Day)
                {
                    m_TodaySelectionObject.SetActive(true);
                    ShowEffect(true);
                    m_RewardItemView.SetOnClickDelegate(OnClickClaim);
                }
                //可以补签的
                else if (m_CachedDataRow.Id < System.DateTime.UtcNow.Day)
                {
                    m_RetroactiveMarkObject.SetActive(true);
                    m_RewardItemView.SetOnClickDelegate(OnClickClaim);
                }
                //还未到的
                else
                {

                }
            }
        }
        /// <summary>
        /// 点击签到
        /// </summary>
        private void OnClickClaim()
        {
            //补签
            if (m_CachedDataRow.Id < System.DateTime.UtcNow.Day)
            {
                //TODO:根据VIP等级限制补签次数
                //int count = GameEntry.Data.EveryDaySignInData.RetroactiveCount;
                //GameEntry.Data.VipsData.GetData((int)VipPrivilegeType.RetroactiveCount);
                CLRetroactiveDailyLogin request = new CLRetroactiveDailyLogin();
                request.day = m_CachedDataRow.Id;
                GameEntry.Network.Send(request);

                EveryDaySignInForm.ClaimDay = m_CachedDataRow.Id;
                return;
            }
            CLClaimDailyLogin sender = new CLClaimDailyLogin();
            GameEntry.Network.Send(sender);
        }
        public void ShowEffect(bool isShow)
        {
            m_Effect.Resume();
            if (isShow)
            {
                if (m_Effect.EffectIsShowing("EffectDailyLogIn") == false)
                    m_Effect.ShowEffect("EffectDailyLogIn");
            }
            else
            {
                m_Effect.DestroyEffect("EffectDailyLogIn");
            }
        }
    }

}
