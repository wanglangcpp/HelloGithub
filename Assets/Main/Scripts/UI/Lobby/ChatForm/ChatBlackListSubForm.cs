using System;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ChatBlackListSubForm : NGUISubForm
    {
        [SerializeField]
        private ScrollViewCache m_BlackListItems = null;

        [SerializeField]
        private UILabel m_Title = null;

        protected internal override void OnOpen()
        {
            base.OnOpen();
            RefreshBlackList();
        }

        protected internal override void OnClose()
        {
            m_BlackListItems.DestroyAllItems();
            base.OnClose();
        }

        private void RefreshBlackList()
        {
            m_BlackListItems.RecycleAllItems();
            var blackList = GameEntry.Data.Chat.BlackList;
            m_Title.text = GameEntry.Localization.GetString("UI_TEXT_CHAT_SCREEN_NUMBER_DISPLAY", blackList.Count.ToString());
            for (int i = 0; i < blackList.Count; i++)
            {
                var item = m_BlackListItems.GetOrCreateItem(i);
                item.RefreshBlackItem(blackList[i], RefreshBlackList);
            }

            StartCoroutine(RepositionBlackList());
        }

        private IEnumerator RepositionBlackList()
        {
            yield return null;
            m_BlackListItems.ResetPosition();
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<ChatBlackListItem>
        {

        }
    }
}
