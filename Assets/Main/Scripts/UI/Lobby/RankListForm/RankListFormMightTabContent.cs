using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 战斗力排行榜。
    /// </summary>
    internal class RankListFormMightTabContent : RankListFormBaseTabContent
    {
        protected override RankListType RankType
        {
            get
            {
                return RankListType.TotalMight;
            }
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            GameEntry.Event.Subscribe(EventId.GetRankListData, OnDataObtained);
        }

        protected override void UnsubscribeEvents()
        {
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.GetRankListData, OnDataObtained);
            base.UnsubscribeEvents();
        }

        protected override void OnDataObtained(object sender, GameEventArgs e)
        {
            base.OnDataObtained(sender, e);
            m_Score.gameObject.SetActive(true);
            m_ScoreType.gameObject.SetActive(true);
            m_IntegralIcon.gameObject.SetActive(false);
            m_IntegralText.gameObject.SetActive(false);
        }

        protected override IEnumerator PopulateCo(LCRefreshRankList packet)
        {
            yield return null;

            var enemies = packet.RankInfo;
            var listItems = new List<RankListLocalServerItem>();
            for (int i = 0; i < enemies.Count; i++)
            {
                var go = NGUITools.AddChild(m_ListView.gameObject, m_ItemTemplate);
                var script = go.GetComponent<RankListLocalServerItem>();
                script.Refresh(enemies[i]);
                listItems.Add(script);
            }

            yield return null;

            m_ScrollView.panel.UpdateAnchors();
            m_ListView.Reposition();
            m_ScrollView.ResetPosition();
            for (int i = 0; i < listItems.Count; i++)
            {
                listItems[i].UpdateAnchors();
            }
        }
    }
}
