using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ItemPaketItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_ItemCountText = null;

        [SerializeField]
        private UILabel m_ItemNameText = null;

        [SerializeField]
        private UILabel m_ItemDescText = null;

        [SerializeField]
        private GameObject m_MultiSelectSlot = null;

        [SerializeField]
        private GameObject m_MultiSelectIcon = null;

#pragma warning disable 0414

        [SerializeField]
        private GameObject m_CountLabel = null;

#pragma warning restore 0414
        private ItemData m_Item = null;
        private ItemPacketPanel m_ParentPanel = null;

        public ItemData ItemInfo
        {
            get { return m_Item; }
            private set { m_Item = value; }
        }

        public bool IsMultiSelected
        {
            get { return m_MultiSelectIcon.activeSelf; }
        }

        public void InitData(ItemData item, ItemPacketPanel parent, bool IsMultiChoice)
        {
            m_Item = item;
            m_ParentPanel = parent;
            m_ItemCountText.text = item.Count.ToString();
            DRItem dataRow = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(item.Type);
            if (dataRow == null)
            {
                Log.Error("wrong Item Id");
                return;
            }
            m_ItemNameText.text = GameEntry.Localization.GetString(dataRow.Name);
            m_ItemDescText.text = GameEntry.Localization.GetString(dataRow.Description);
            if (IsMultiChoice)
            {
                m_MultiSelectSlot.SetActive(true);
            }
        }

        public void OnClickMe()
        {
            m_ParentPanel.OnSelectItem(this);
        }
    }
}
