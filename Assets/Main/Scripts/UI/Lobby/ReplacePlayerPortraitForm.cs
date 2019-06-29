using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ReplacePlayerPortraitForm : NGUIForm
    {
        [SerializeField]
        private PortraitScrollViewCache m_CardScrollViewCache = null;

        [SerializeField]
        private UIGrid m_Grid = null;

        private int m_SelectPortraitId = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.ChangePlayerPortrait, OnPlayerPortraitDataChanged);
            RefreshPortraits();
        }

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.ChangePlayerPortrait, OnPlayerPortraitDataChanged);
            m_CardScrollViewCache.DestroyAllItems();
            base.OnClose(userData);
        }

        private void OnPlayerPortraitDataChanged(object sender, GameEventArgs e)
        {
        }

        private void RefreshPortraits()
        {
            m_SelectPortraitId = GameEntry.Data.Player.PortraitType;

            var dt = GameEntry.DataTable.GetDataTable<DRPlayerPortrait>();
            var allRows = dt.GetAllDataRows();
            for (int i = 0; i < allRows.Length; i++)
            {
                if (allRows[i].HeroId != -1 && GameEntry.Data.LobbyHeros.GetData(allRows[i].HeroId) == null)
                {
                    continue;
                }
                var script = m_CardScrollViewCache.CreateItem();
                script.InitPlayerHead(allRows[i].Id, OnClickPlayerPortrait);
                script.Select = allRows[i].Id == m_SelectPortraitId;
            }
            m_Grid.Reposition();
            m_CardScrollViewCache.ResetPosition();
        }

        private void OnClickPlayerPortrait(int portraitId)
        {
            m_SelectPortraitId = portraitId;
            for (int i = 0; i < m_CardScrollViewCache.Count; i++)
            {
                var item = m_CardScrollViewCache.GetItem(i);
                item.Select = item.PortraitId == m_SelectPortraitId;
            }
        }

        public void OnConfirmButton()
        {
            if (GameEntry.Data.Player.PortraitType != m_SelectPortraitId)
            {
                GameEntry.LobbyLogic.ChangePlayerPortrait(m_SelectPortraitId);
            }
            CloseSelf();
        }

        public void OnCancleButton()
        {
            CloseSelf();
        }

        [Serializable]
        private class PortraitScrollViewCache : UIScrollViewCache<PlayerHeadView>
        {

        }

    }
}
