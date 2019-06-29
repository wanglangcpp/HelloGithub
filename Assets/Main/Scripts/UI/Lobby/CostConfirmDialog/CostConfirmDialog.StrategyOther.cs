using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CostConfirmDialog
    {
        private class StrategyOther : StrategyBase
        {
            public override void RefreshData()
            {
                m_Form.m_ItemIcon.gameObject.SetActive(false);
                m_Form.m_PreMessage.gameObject.SetActive(true);
                m_Form.m_PreMessage.text = m_Form.m_Data.PreMessage;
                m_Form.m_Title.text = m_Form.m_Data.Title;
                m_Form.m_PostMessage.text = m_Form.m_Data.PostMessage;
                m_Form.m_ButtonCurrencyIcon.spriteName = IconNames[(int)m_Form.m_Data.UseCurrencyType];
                m_Form.m_ButtonCurrencyCount.text = m_Form.m_Data.CurrencyCount.ToString();
            }

            public override void OnClickBuyButton()
            {
                base.OnClickBuyButton();
                m_Form.CloseSelf();
            }
        }
    }
}
