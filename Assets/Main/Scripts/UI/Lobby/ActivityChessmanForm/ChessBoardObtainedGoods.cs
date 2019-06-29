using UnityEngine;

namespace Genesis.GameClient
{
    public class ChessBoardObtainedGoods : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_Icon = null;

        [SerializeField]
        private UILabel m_Qty = null;

        public int GeneralItemId
        {
            get;
            private set;
        }

        public int Qty
        {
            get;
            private set;
        }

        public void Refresh(int generalItemId, int qty)
        {
            GeneralItemId = generalItemId;
            m_Icon.LoadAsync(GeneralItemUtility.GetGeneralItemIconId(GeneralItemId));

            Qty = qty;
            if (Qty <= 1)
            {
                m_Qty.enabled = false;
            }
            else
            {
                m_Qty.enabled = true;
                m_Qty.text = Qty.ToString();
            }
        }

        // Called via reflection by NGUI.
        private void OnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.GeneralItemInfoForm, new GeneralItemInfoDisplayData { TypeId = GeneralItemId, Qty = Qty });
        }
    }
}
