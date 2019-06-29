using UnityEngine;
using System.Collections;
using GameFramework.Event;
using System.Collections.Generic;
using System;

namespace Genesis.GameClient
{
    public class EveryDayGiftForm : WelfareCenterBaseTabContent
    {
        private Dictionary<int, EveryDayGiftItem> m_CachedChargeItems = new Dictionary<int, EveryDayGiftItem>();
        private DRGfitBag[] m_GiftData = null;
        
        protected override void OnOpen(GameObject obj)
        {
            m_GiftData = UIUtility.GetGiftType(GiftType.EveryDayGift).ToArray();
            base.OnOpen(this.gameObject);
        }

        /// <summary>
        /// 读表初始化物品信息
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator RefreshData()
        {
            for (int i = 0; i < m_GiftData.Length; i++)
            {
                GameObject go = NGUITools.AddChild(m_Grid.gameObject, m_ItemTemplate);
                go.name = string.Format("Gift Item {0}", i.ToString());
                var script = go.GetComponent<EveryDayGiftItem>();
                script.RefreshData(m_GiftData[i]);
                m_CachedChargeItems.Add(m_GiftData[i].Id, script);
            }
            m_Grid.enabled = true;
            yield return null;
        }
        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }

        /// <summary>
        /// 礼包状态回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void GetItemSuatus(object sender, GameEventArgs e)
        {
            ChargeStatusData ne = GameEntry.Data.ChargeStatusData;
            if (ne.StatusData != null)
            {
                UpdataItemStatus(ne.StatusData);
            }
        }
        protected override void UpdataItemStatus(ChargeStatus ne)
        {
            foreach (var item in m_CachedChargeItems)
            {
                if (ne.GiftStatus.ContainsKey(item.Key))
                {
                    ItemStatus(item.Key, ne.GiftStatus[item.Key]);
                }
            }
        }
        private void ItemStatus(int id, int status)
        {
            m_CachedChargeItems[id].RefreshItemStatus(status);
        }
    }
}

