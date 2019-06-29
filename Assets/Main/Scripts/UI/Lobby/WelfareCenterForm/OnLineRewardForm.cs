using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 在线奖励
    /// </summary>
    public class OnLineRewardForm : WelfareCenterBaseTabContent
    {
        private Dictionary<int, OnLineRewardItem> allOnLineItem = new Dictionary<int, OnLineRewardItem>();

        protected override void OnOpen(GameObject obj)
        {
            base.OnOpen(obj);
        }

        protected override IEnumerator RefreshData()
        {
            var allTabelData = GameEntry.DataTable.GetDataTable<DROnlineRewards>().GetAllDataRows();
            foreach (var item in allTabelData)
            {
                GameObject go = NGUITools.AddChild(m_Grid.gameObject, m_ItemTemplate);
                OnLineRewardItem script = go.GetComponent<OnLineRewardItem>();
                if (script != null)
                {
                    script.RefreshItem(item);
                    allOnLineItem.Add(item.Id, script);
                }

            }
            yield return null;
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            GameEntry.Event.Subscribe(EventId.OnlineRewardsDataChange, OnlineRewardsDataChange);

        }
        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            GameEntry.Event.Unsubscribe(EventId.OnlineRewardsDataChange, OnlineRewardsDataChange);
        }

        private void OnlineRewardsDataChange(object sender, GameEventArgs e)
        {
            foreach (var item in allOnLineItem)
            {
                item.Value.SetItemStatus();
            }
        }
    }
}

