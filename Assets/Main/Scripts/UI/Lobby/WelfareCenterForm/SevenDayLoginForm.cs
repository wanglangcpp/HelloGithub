using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 七日登录
    /// </summary>
    public class SevenDayLoginForm : WelfareCenterBaseTabContent
    {
        private Dictionary<int, DRSevenDayLogin> dicSevenDayLoginData = null;

        private Dictionary<int, SevenDayLoginItem> allSevenDayLoginItem = new Dictionary<int, SevenDayLoginItem>();

        [SerializeField]
        private GameObject m_ItemTemplate_Novice = null;//新手模板

        protected override void OnOpen(GameObject obj)
        {
            dicSevenDayLoginData = GameEntry.Data.SevenDayLoginData.GetDatas();
            base.OnOpen(obj);
        }
        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            GameEntry.Event.Subscribe(EventId.SevenDayLoginDataChange, SevenDayLoginDataChange);
        }
        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            GameEntry.Event.Unsubscribe(EventId.SevenDayLoginDataChange, SevenDayLoginDataChange);
        }

        private void SevenDayLoginDataChange(object sender, GameEventArgs e)
        {
            int id = GameEntry.Data.SevenDayLoginData.ClaimId;
            if (id != 0 && allSevenDayLoginItem.ContainsKey(id))
            {
                allSevenDayLoginItem[id].SetRewardStatus();
                GameEntry.Data.SevenDayLoginData.ClaimId = 0;
            }
        }
        protected override IEnumerator RefreshData()
        {
            foreach (var item in dicSevenDayLoginData)
            {
                GameObject go = null;
                if (GameEntry.Data.SevenDayLoginData.Type == 1)
                {
                    go = NGUITools.AddChild(m_Grid.gameObject, m_ItemTemplate_Novice);
                }
                else
                {
                    go = NGUITools.AddChild(m_Grid.gameObject, m_ItemTemplate);
                }
                SevenDayLoginItem script = go.GetComponent<SevenDayLoginItem>();
                script.RefreshItem(item.Value);
                if (!allSevenDayLoginItem.ContainsKey(item.Key))
                {
                    allSevenDayLoginItem.Add(item.Key, script);
                }
            }
            yield return null;
        }


    }
}

