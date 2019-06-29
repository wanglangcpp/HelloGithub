using UnityEngine;
using System.Collections;
using GameFramework;

namespace Genesis.GameClient
{
    public class EveryDayGiftItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_ItemName = null;

        [SerializeField]
        private UISprite m_Icon = null;

        [SerializeField]
        private UILabel m_RewardContent = null;

        [SerializeField]
        private UIButton m_BuyBtn = null;

        [SerializeField]
        private UIButton m_GetBtn = null;

        [SerializeField]
        private UILabel m_BuyCount = null;

        [SerializeField]
        private Transform BuyOver = null;

        [SerializeField]
        private UILabel m_BuyText = null;

        [SerializeField]
        private UILabel m_GetText = null;

        private int GiftId = 0;//礼包编号

        private void Awake()
        {
            m_ItemName = transform.Find("Item bg/Name").GetComponent<UILabel>();
            m_Icon = transform.Find("Item bg/Icon").GetComponent<UISprite>();
            m_RewardContent = transform.Find("Description bg/Reward Content").GetComponent<UILabel>();
            m_BuyBtn = transform.Find("Btn Buy").GetComponent<UIButton>();
            m_GetBtn = transform.Find("Btn Get").GetComponent<UIButton>();
            m_BuyCount = transform.Find("Btn Buy/Restrictive").GetComponent<UILabel>();
            BuyOver = transform.Find("Buy Over Text");
            m_BuyText = m_BuyBtn.transform.GetComponentInChildren<UILabel>();
            m_BuyText.text = GameEntry.Localization.GetString(m_BuyText.text);
            m_GetText = m_GetBtn.transform.GetComponentInChildren<UILabel>();
            m_GetText.text = GameEntry.Localization.GetString(m_GetText.text);
        }

        /// <summary>
        /// 刷新Item数据
        /// </summary>
        public void RefreshData(DRGfitBag data)
        {
            GiftId = data.Id;
            m_BuyBtn.gameObject.SetActive(true);
            m_GetBtn.gameObject.SetActive(false);
            BuyOver.gameObject.SetActive(false);
            m_ItemName.text = GameEntry.Localization.GetString(data.Name, data.Price);
            m_Icon.LoadAsync(data.IconId);
            m_RewardContent.text = GameEntry.Localization.GetString(data.Desc);
            m_BuyCount.text = GameEntry.Localization.GetString("UI_WELFARE_BAG_PURCHASE", data.Count);
        }
        /// <summary>
        /// 刷新Item状态
        /// </summary>
        /// <param name="status">(0-未购买/1-未领取/2-已领取)</param>
        public void RefreshItemStatus(int status)
        {
            if (status == 0)
            {
                m_BuyBtn.gameObject.SetActive(false);
                m_GetBtn.gameObject.SetActive(true);
                BuyOver.gameObject.SetActive(false);
            }
            else if (status == 1)
            {
                m_BuyBtn.gameObject.SetActive(false);
                m_GetBtn.gameObject.SetActive(false);
                BuyOver.gameObject.SetActive(true);
            }
            else
            {
                m_BuyBtn.gameObject.SetActive(true);
                m_GetBtn.gameObject.SetActive(false);
                BuyOver.gameObject.SetActive(false);
                m_BuyCount.gameObject.SetActive(true);
            }
        }
        public void OnClickBuyBtn()
        {
            WelfareCenterBaseTabContent.onPaySuccess = WelfareCenterBaseTabContent.OnPaySuccess;
            //进入支付
            DRGfitBag drc = GameEntry.DataTable.GetDataTable<DRGfitBag>().GetDataRow(GiftId);

            string id = drc.Id.ToString();
            WelfareCenterBaseTabContent.Itemdata = drc.Id.ToString() + "," + drc.Price.ToString();
            string name = GameEntry.Localization.GetString(drc.Name, drc.Price);
            string desc = "";
            if (!string.IsNullOrEmpty(drc.Desc) && !drc.Desc.Equals("null"))
            {
                desc = GameEntry.Localization.GetString(drc.Desc);
            }
            else desc = name;
            int price = drc.Price;
            PayInfos payInfos = new PayInfos(id, name, desc, price);
            Log.Debug("Enter the recharge interface.....");
            SDKManager.Instance.helper.Pay(payInfos);
        }
        public void OnClickGetBtn()
        {
            CLGetGift cLGetGift = new CLGetGift() { giftId = GiftId };
            GameEntry.Network.Send(cLGetGift);
        }
    }
}

