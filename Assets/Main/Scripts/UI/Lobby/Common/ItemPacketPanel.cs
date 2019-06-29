using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ItemPacketPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_ItemPacketItem = null;

        [SerializeField]
        private UITable m_ItemPacket = null;

        [SerializeField]
        private UIScrollView m_ItemPacketScrollView = null;

        //         [SerializeField]
        //         private UILabel m_FilterButtonText = null;
        //         [SerializeField]
        //         private GameObject m_ConfirmButton = null;
        //         [SerializeField]
        //         private UILabel m_ConfirmButtonText = null;

        private bool m_IsMultiChoice = false;
        private Predicate<DRItem> m_Filter = null;

        public void Init(Dictionary<string, object> userDict)
        {
            RefreshData(userDict);
            ShowItemPacket();
        }

        private void ShowItemPacket()
        {
            if (m_IsMultiChoice)
            {

            }
            else
            {
                foreach (ItemData item in GameEntry.Data.Items.Data)
                {
                    DRItem dataRow = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(item.Type);
                    if (dataRow != null)
                    {
                        if (m_Filter == null || m_Filter(dataRow))
                        {
                            AddItem(item);
                        }
                    }
                }
            }

            m_ItemPacket.Reposition();
            m_ItemPacketScrollView.ResetPosition();
            m_ItemPacketScrollView.enabled = (m_ItemPacketItem.GetComponent<UISprite>().height * m_ItemPacket.GetChildList().Count >= m_ItemPacketScrollView.GetComponent<UIPanel>().height);
        }

        public void OnSelectItem(ItemPaketItem item)
        {
            if (m_IsMultiChoice)
            {
                //Undo
            }
            else
            {
                GameEntry.Event.Fire(this, new ItemSelectedEventArgs(item.ItemInfo));
                Destroy(this.gameObject);
            }
        }

        public void OnClickWholeScreenButton(Transform transformToDeactivate)
        {
            Destroy(transformToDeactivate.gameObject);
        }

        private void RefreshData(Dictionary<string, object> userData)
        {
            m_IsMultiChoice = (bool)userData[Constant.UI.ItemIsMultiChoice];
            if (userData.ContainsKey(Constant.UI.ItemFilter))
            {
                m_Filter = userData[Constant.UI.ItemFilter] as Predicate<DRItem>;
            }
        }

        private ItemPaketItem AddItem(ItemData item)
        {
            var go = NGUITools.AddChild(m_ItemPacket.gameObject, m_ItemPacketItem);
            m_ItemPacketScrollView.GetComponent<UIPanel>().depth = this.GetComponent<UIPanel>().depth + 1;
            go.GetComponent<UISprite>().depth = 34;
            var script = go.GetComponent<ItemPaketItem>();
            script.InitData(item, this, m_IsMultiChoice);
            return script;
        }
    }
}
