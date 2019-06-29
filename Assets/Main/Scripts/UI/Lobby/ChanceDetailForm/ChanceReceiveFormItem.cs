using UnityEngine;
using System.Collections;

namespace Genesis.GameClient
{
    public class ChanceReceiveFormItem : MonoBehaviour
    {
        [SerializeField]
        private GeneralItemView m_ItemView = null;

        [SerializeField]
        private UILabel m_ItemName = null;

        [SerializeField]
        private Animation m_ItemAnimation = null;

        public Animation ItemAnimation
        {
            get { return m_ItemAnimation; }
        }

        public void PlayAnimation()
        {
            m_ItemAnimation.Sample();
            m_ItemAnimation.Play();
            m_ItemAnimation.playAutomatically = false;
        }

        public void RefreshData(PBItemInfo itemInfo)
        {
            m_ItemView.InitGeneralItem(itemInfo.Type, itemInfo.Count);
            var goodsNameKey = GeneralItemUtility.GetGeneralItemName(itemInfo.Type);
            m_ItemName.color = ColorUtility.GetColorForQuality(GeneralItemUtility.GetGeneralItemQuality(itemInfo.Type));
            m_ItemName.text = GameEntry.Localization.GetString("UI_TEXT_ITEMNAME_COUNT", string.IsNullOrEmpty(goodsNameKey) ? string.Empty : GameEntry.Localization.GetString(goodsNameKey));
        }
    }
}