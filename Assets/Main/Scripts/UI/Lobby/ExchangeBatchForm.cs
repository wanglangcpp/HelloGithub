using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 批量兑换界面。
    /// </summary>
    /// <remarks>目前仅支持英雄碎片兑换魂魄。</remarks>
    public class ExchangeBatchForm : NGUIForm
    {
        [SerializeField]
        private UIInput m_Number = null;

        [SerializeField]
        private UIButton m_MinusButton = null;

        [SerializeField]
        private UIButton m_PlusButton = null;

        [SerializeField]
        private UIButton m_OkayButton = null;

        [SerializeField]
        private UILabel m_ExchangeText = null;

        private int m_ToUseCount;
        private ExchangeBatchDisplayData m_DisplayData;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ParseUserData(userData);
            ResetNumber();
            RefreshPassiveContents();
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        // Called by NGUI via reflection.
        public void OnClickOkayButton()
        {
            if (!m_OkayButton.isEnabled)
            {
                return;
            }

            //GameEntry.LobbyLogic.InventoryUseItem(m_DisplayData.HeroPieceTypeId, m_ToUseCount);
            CloseSelf();
        }

        // Called by NGUI via reflection.
        public void OnClickCancelButton()
        {
            CloseSelf();
        }

        // Called by NGUI via reflection.
        public void OnClickMinusButton()
        {
            m_Number.value = (m_ToUseCount - 1).ToString();
        }

        // Called by NGUI via reflection.
        public void OnClickPlusButton()
        {
            m_Number.value = (m_ToUseCount + 1).ToString();
        }

        // Called by NGUI via reflection.
        public void OnInputNumberChange()
        {
            if (!int.TryParse(m_Number.value, out m_ToUseCount))
            {
                ResetNumber();
            }

            bool invalid = false;
            if (m_ToUseCount < 1)
            {
                m_ToUseCount = 1;
                invalid = true;
            }
            else if (m_ToUseCount > m_DisplayData.OwnedCount)
            {
                m_ToUseCount = m_DisplayData.OwnedCount;
                invalid = true;
            }

            if (invalid)
            {
                m_Number.value = m_ToUseCount.ToString();
            }

            RefreshPassiveContents();
        }

        private void ParseUserData(object userData)
        {
            m_DisplayData = userData as ExchangeBatchDisplayData;

            if (m_DisplayData == null)
            {
                Log.Error("User data is invalid.");
                return;
            }
        }

        private void RefreshPassiveContents()
        {
            int spiritsPerPiece = 1; // GameEntry.ServerConfig.GetInt(Constant.ServerConfig.SpiritsPerPiece, 1);
            m_ExchangeText.text = GameEntry.Localization.GetString("UI_TEXT_EXCHANGETOTAL_NUMBER", (m_ToUseCount * spiritsPerPiece).ToString());

            m_MinusButton.isEnabled = m_ToUseCount > 1;
            m_PlusButton.isEnabled = m_ToUseCount < m_DisplayData.OwnedCount;
            m_OkayButton.isEnabled = m_ToUseCount > 0 && m_ToUseCount <= m_DisplayData.OwnedCount;
        }

        private void ResetNumber()
        {
            m_ToUseCount = 1;
            m_Number.value = "1";
        }
    }
}
