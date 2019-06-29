using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备锻造活动中的好友列表子界面。
    /// </summary>
    public class ActivityFoundryFriendList : MonoBehaviour
    {
        [SerializeField]
        private UIScrollView m_ScrollView = null;

        [SerializeField]
        private UIGrid m_ListView = null;

        [SerializeField]
        private GameObject m_ItemTemplate = null;

        private List<ActivityFoundryFriendItem> m_CachedItems = new List<ActivityFoundryFriendItem>();

        // Called by NGUI via reflection.
        public void OnClickWholeScreenButton()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            InitList();
        }

        private void OnDisable()
        {
            DeinitList();
        }

        private void InitList()
        {
            var sorted = new List<FriendData>(GameEntry.Data.Friends.Data);
            sorted.Sort((a, b) => { return ComparePlayers(a.Player, b.Player); });

            for (int i = 0; i < sorted.Count; ++i)
            {
                var go = NGUITools.AddChild(m_ListView.gameObject, m_ItemTemplate);
                var script = go.GetComponent<ActivityFoundryFriendItem>();
                script.Refresh(sorted[i].Player, true);
                m_CachedItems.Add(script);
            }

            m_ListView.Reposition();
            m_ScrollView.ResetPosition();
        }

        private void DeinitList()
        {
            for (int i = 0; i < m_CachedItems.Count; ++i)
            {
                Destroy(m_CachedItems[i].gameObject);
            }

            m_CachedItems.Clear();
        }

        private int ComparePlayers(PlayerData a, PlayerData b)
        {
            if (a.Level != b.Level)
            {
                return b.Level.CompareTo(a.Level);
            }

            return b.VipLevel.CompareTo(a.VipLevel);
        }
    }
}
