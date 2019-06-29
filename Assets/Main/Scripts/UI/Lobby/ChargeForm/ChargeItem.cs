using UnityEngine;
using System.Collections;

namespace Genesis.GameClient
{
    /// <summary>
    /// 充值物品
    /// </summary>
    public class ChargeItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_ItemName = null;//商品名

        [SerializeField]
        private UILabel m_Description = null;//商品描述

        [SerializeField]
        private UILabel m_BadgeMsg = null;//角标信息

        [SerializeField]
        private UILabel m_ItemAbstract = null;//商品简介（月卡）

        [SerializeField]
        private UISprite m_Icon = null;//图标

        [SerializeField]
        private UILabel m_Price = null;//售价

        [SerializeField]
        private UISprite m_Recommend = null;//推荐

        [SerializeField]
        private UILabel m_RecommendText = null;//推荐

        [SerializeField]
        private UISprite m_PayMent = null;
        public UISprite PayMent { get { return m_PayMent; } }

        [SerializeField]
        private UIButton m_ButtonIcon = null;
        public UIButton ButtonIcon { get { return m_ButtonIcon; } }

        [SerializeField]
        private UILabel m_RemainingTime;//剩余时间
        public UILabel RemainingTime { get { return m_RemainingTime; } }

        [SerializeField]
        private UIButton m_Help = null;//查看物品详情（月卡）

        private int m_ItemId = 0;
        private ChargeForm m_ChargeForm = null;
        private ChargeInfo m_ItemData = null;

        /// <summary>
        /// 刷新商品数据
        /// </summary>
        /// <param name="data">商品的数据</param>
        /// <param name="IsFirstCharge">是否为首冲</param>
        public void RefreshData(ChargeForm chargeForm, object data, bool IsFirstCharge = false)
        {
            m_ButtonIcon.onClick.Clear();
            m_ButtonIcon.onClick.Add(new EventDelegate(() => OnClickBuyButton()));

            m_ChargeForm = chargeForm;
            m_ItemData = data as ChargeInfo;
            m_ItemId = m_ItemData.Id;

            m_PayMent.gameObject.SetActive(true);
            m_RemainingTime.gameObject.SetActive(false);
            //月卡
            m_ItemAbstract.transform.parent.gameObject.SetActive(m_ItemData.Type == 1);
            m_Help.gameObject.SetActive(m_ItemData.Type == 1);
            m_ItemAbstract.text = GameEntry.Localization.GetString(m_ItemData.Abstract);
            if (m_ItemData.Type == 1)
            {
                m_BadgeMsg.gameObject.SetActive(true);
                m_BadgeMsg.text = GameEntry.Localization.GetString("UI_TEXT_CHARGE_4", m_ItemData.GainCount);
            }
            else
            {
                m_BadgeMsg.gameObject.SetActive(true);
                m_BadgeMsg.text = GameEntry.Localization.GetString("UI_TEXT_CHARGE_5");
            }

            m_ItemName.text = GameEntry.Localization.GetString(m_ItemData.Name);

            //是否推荐
            m_Recommend.gameObject.SetActive(m_ItemData.Recommend);
            m_RecommendText.text = GameEntry.Localization.GetString("UI_TEXT_CHARGE_9");
            m_Icon.LoadAsync(m_ItemData.IconId);
            m_Price.text = GameEntry.Localization.GetString("UI_TEXT_CHARGE_6", m_ItemData.Price);

        }
        public void UpdateRewardsLabel(bool firstcharge)
        {
            if (firstcharge)
            {
                m_BadgeMsg.gameObject.SetActive(true);
                m_BadgeMsg.text = GameEntry.Localization.GetString("UI_TEXT_CHARGE_5");
            }
            else
            {
                if (m_ItemData.Extra != 0)
                {
                    m_BadgeMsg.gameObject.SetActive(true);
                    m_BadgeMsg.text = GameEntry.Localization.GetString("UI_TEXT_CHARGE_4", m_ItemData.GainCount * m_ItemData.Extra * 0.01);
                }
                else
                {
                    m_BadgeMsg.transform.parent.gameObject.SetActive(false);
                }
            }
        }


        public void OnClickHelpButton()
        {
            m_ChargeForm.OnClickHelpButton(transform, m_ItemId);
        }
        public void OnClickBuyButton()
        {
            m_ChargeForm.OnClickBuyButton(m_ItemId);
        }
    }
}

