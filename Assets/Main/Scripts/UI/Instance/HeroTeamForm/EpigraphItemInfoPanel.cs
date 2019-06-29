using UnityEngine;

namespace Genesis.GameClient
{

    public class EpigraphItemInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_ItemName = null;

        [SerializeField]
        private UILabel m_Level = null;

        [SerializeField]
        private UILabel m_AttributeName = null;

        [SerializeField]
        private UILabel m_AttributeCount = null;

        [SerializeField]
        private UISprite m_ItemIcon = null;

        [SerializeField]
        private UISprite[] m_Stars = null;

        private EpigraphData m_EpigraphData = null;
        private int m_SlotIndex = 0;
        private int m_Id = 0;
        private bool m_Start = false;
        private bool m_Init = false;

        private void Start()
        {
            m_Start = true;
            if (m_Init)
            {
                ShowItemData();
            }
        }

        public void Init(int slotIndex, int id)
        {
            m_SlotIndex = slotIndex;
            m_Id = id;
            m_Init = true;
            gameObject.SetActive(true);
            if (m_Start)
            {
                ShowItemData();
            }
        }

        private void ShowItemData()
        {
            EpigraphData epData = null;

            foreach (EpigraphData data in GameEntry.Data.EpigraphSlots.Data)
            {
                if (m_Id == data.Id)
                {
                    epData = data;
                }
            }

            if (epData == null)
            {
                return;
            }

            m_EpigraphData = epData;

            m_AttributeCount.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", epData.DTAttributeValue);
            if (epData.DTAttributeType > 0 && Constant.AttributeName.AttributeNameDics.Length > epData.DTAttributeType)
            {
                m_AttributeName.text = GameEntry.Localization.GetString(Constant.AttributeName.AttributeNameDics[epData.DTAttributeType]);
            }
            m_ItemName.text = GameEntry.Localization.GetString(epData.DTName);
            m_Level.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", epData.Level);

            int iconId = GeneralItemUtility.GetGeneralItemIconId(epData.Id);
            m_ItemIcon.LoadAsync(iconId);
            UIUtility.SetStarLevel(m_Stars, m_EpigraphData.Level);
        }

        public void ClickUnloadBtn()
        {
//             gameObject.SetActive(false);
// 
//             if (m_EpigraphData == null)
//             {
//                 return;
//             }
// 
//             CLChangeEpigraph msg = new CLChangeEpigraph();
//             msg.Index = m_SlotIndex;
//             msg.UndressedEpigraph = new PBEpigraphInfo();
//             msg.UndressedEpigraph.Level = m_EpigraphData.Level;
//             msg.UndressedEpigraph.Type = m_EpigraphData.Id;
//             GameEntry.Network.Send(msg);
        }

        public void ClickChangeBtn()
        {
            GameEntry.UI.OpenUIForm(UIFormId.InventoryForm, new InventoryDisplayData
            {
                InventoryType = InventoryForm.InventoryType.EpigraphChange,
                EpigraphIndex = m_SlotIndex,
            });
        }
    }

}
