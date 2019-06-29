using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class SoulPacketPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_SoulPacketItem = null;

        [SerializeField]
        private UITable m_SoulPacket = null;

        [SerializeField]
        private UIScrollView m_SoulPacketScrollView = null;

        private bool m_IsMultiChoice = false;
        private Predicate<SoulData> m_Filter = null;

        public void Init(Dictionary<string, object> userDict)
        {
            RefreshData(userDict);
            ShowSoulPacket();
        }

        private void ShowSoulPacket()
        {
            foreach (SoulData soul in GameEntry.Data.Souls.Data)
            {
                if (m_Filter == null || m_Filter(soul))
                {
                    AddSoul(soul);
                }
            }

            m_SoulPacket.Reposition();
            m_SoulPacketScrollView.ResetPosition();
            m_SoulPacketScrollView.enabled = (m_SoulPacketItem.GetComponent<UISprite>().height * m_SoulPacket.GetChildList().Count >= m_SoulPacketScrollView.GetComponent<UIPanel>().height);
        }

        public void OnSelectItem(SoulPacketItem soul)
        {
            if (m_IsMultiChoice)
            {
                //Undo
            }
            else
            {
                GameEntry.Event.Fire(this, new SoulSelectedEventArgs(soul.SoulInfo));
                //Destroy(this.gameObject);
            }
        }

        public void OnClickWholeScreenButton(Transform transformToDeactivate)
        {
            Destroy(transformToDeactivate.gameObject);
        }

        private void RefreshData(Dictionary<string, object> userData)
        {
            m_IsMultiChoice = (bool)userData[Constant.UI.IsMultiChoice];
            if (userData.ContainsKey(Constant.UI.ItemFilter))
            {
                m_Filter = userData[Constant.UI.ItemFilter] as Predicate<SoulData>;
            }
        }

        private SoulPacketItem AddSoul(SoulData soul)
        {
            var go = NGUITools.AddChild(m_SoulPacket.gameObject, m_SoulPacketItem);
            var script = go.GetComponent<SoulPacketItem>();
            script.InitData(soul, this, m_IsMultiChoice);
            return script;
        }
    }
}
