using UnityEngine;
using System.Collections;

namespace Genesis.GameClient
{
    public class DailyLoginItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_VipDoubleObject = null;

        [SerializeField]
        private UILabel m_VipDoubleLabel = null;

        [SerializeField]
        private GameObject m_TodaySelectionObject = null;

        [SerializeField]
        private GameObject m_ClaimedRewardMarkObject = null;

        [SerializeField]
        private GeneralItemView m_RewardItemView = null;

        [SerializeField]
        private UIEffectsController m_Effect;

        private DRDailyLogin m_CachedDataRow = null;

        [HideInInspector]
        public bool IsToday = false;

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
            if (GameEntry.Data.DailyLogin.IsLoginedToday)
            {
                m_TodaySelectionObject.SetActive(false);
                IsToday = false;
                if (m_CachedDataRow.Index < GameEntry.Data.DailyLogin.LoginedDayCount)
                {
                    // 已签到的天数
                    m_ClaimedRewardMarkObject.SetActive(true);
                    m_RewardItemView.ItemIcon.color = Color.grey;
                }
                else
                {
                    // 还未到达天数
                    m_ClaimedRewardMarkObject.SetActive(false);
                    m_RewardItemView.ItemIcon.color = Color.white;
                }
            }
            else
            {
                if (m_CachedDataRow.Index < GameEntry.Data.DailyLogin.LoginedDayCount)
                {
                    // 已签到的天数
                    m_ClaimedRewardMarkObject.SetActive(true);
                    m_RewardItemView.ItemIcon.color = Color.grey;
                    m_TodaySelectionObject.SetActive(false);
                    IsToday = false;
                }
                else if (m_CachedDataRow.Index == GameEntry.Data.DailyLogin.LoginedDayCount)
                {
                    // 今天
                    m_TodaySelectionObject.SetActive(true);
                    m_ClaimedRewardMarkObject.SetActive(false);
                    IsToday = true;

                    ShowEffect(true);
                }
                else
                {
                    // 还未到达天数
                    m_TodaySelectionObject.SetActive(false);
                    m_ClaimedRewardMarkObject.SetActive(false);
                    m_RewardItemView.ItemIcon.color = Color.white;
                    IsToday = false;
                }
            }
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