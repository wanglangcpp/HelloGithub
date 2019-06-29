using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class OfflineArenaRecordForm : NGUIForm
    {
        [SerializeField]
        private ScrollViewCache m_OfflineArenaRecordItem = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.OfflineArenaReportListChanged, OnOfflineArenaRecordChanged);
            RefreshArenaReportItems();
        }

        private void OnOfflineArenaRecordChanged(object sender, GameEventArgs e)
        {
            RefreshArenaReportItems();
        }

        private void RefreshArenaReportItems()
        {
            m_OfflineArenaRecordItem.RecycleAllItems();
            var datas = GameEntry.Data.OfflineArenaBattleReports.Data;
            for (int j = 0, i = datas.Count - 1; i >= 0; i--, j++)
            {
                var item = m_OfflineArenaRecordItem.GetOrCreateItem(j);
                item.RefreshData(datas[i]);
            }
            m_OfflineArenaRecordItem.RecycleItemsAtAndAfter(datas.Count);
            m_OfflineArenaRecordItem.ResetPosition();
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Event.Unsubscribe(EventId.OfflineArenaReportListChanged, OnOfflineArenaRecordChanged);
            base.OnClose(userData);
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<OfflineArenaRecordItem>
        {

        }
    }
}
