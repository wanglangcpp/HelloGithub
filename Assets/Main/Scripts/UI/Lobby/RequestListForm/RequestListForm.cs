using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 请求处理界面。
    /// </summary>
    public class RequestListForm : NGUIForm
    {
        [SerializeField]
        private UIScrollView m_ScrollView = null;

        [SerializeField]
        private UIGrid m_ListView = null;

        [SerializeField]
        private GameObject m_ItemTemplate = null;

        private List<RequestListRequestItem> m_CachedItems = new List<RequestListRequestItem>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(EventId.GearFoundryInvitationResponded, OnInvitationResponded);

            InitList();
        }

        protected override void OnClose(object userData)
        {
            DeinitList();

            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.GearFoundryInvitationResponded, OnInvitationResponded);

            base.OnClose(userData);
        }

        private void OnInvitationResponded(object sender, GameEventArgs e)
        {
            var ne = e as GearFoundryInvitationRespondedEventArgs;

            int index = -1;
            for (int i = 0; i < m_CachedItems.Count; ++i)
            {
                if (m_CachedItems[i].PlayerId == ne.InviterPlayerId && (int)(m_CachedItems[i].UserData) == ne.TeamId)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            {
                var go = m_CachedItems[index].gameObject;
                m_ListView.RemoveChild(m_CachedItems[index].transform);
                m_CachedItems.RemoveAt(index);
                m_ScrollView.InvalidateBounds();
                Destroy(go);
            }
        }

        private void InitList()
        {
            var requests = GameEntry.Data.GearFoundry.Invitations.Data;

            for (int i = requests.Count - 1; i >= 0; --i)
            {
                var go = NGUITools.AddChild(m_ListView.gameObject, m_ItemTemplate);
                var script = go.GetComponent<RequestListRequestItem>();
                script.Refresh(requests[i].Inviter, requests[i].TeamId);
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
    }
}
