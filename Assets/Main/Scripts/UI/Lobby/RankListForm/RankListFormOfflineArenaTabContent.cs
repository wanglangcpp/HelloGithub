using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技排行榜。
    /// </summary>
    internal class RankListFormOfflineArenaTabContent : RankListFormBaseTabContent
    {
        protected override RankListType RankType
        {
            get
            {
                return RankListType.OfflineArena;
            }
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            CurTitle = m_Titles[0];
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
            var heroTeam = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Arena).HeroType;
            int might = 0;
            for (int i = 0; i < heroTeam.Count; i++)
            {
                var hero = GameEntry.Data.LobbyHeros.GetData(heroTeam[i]);
                if (hero == null)
                {
                    continue;
                }
                might += hero.Might;
            }
            m_Score.text = might.ToString();
        }

        protected override IEnumerator PopulateCo(LCRefreshRankList packet)
        {
            yield return null;

            var enemies = packet.RankInfo;
            var listItems = new List<RankListFormOfflineArenaItem>();
            for (int i = 0; i < enemies.Count; i++)
            {
                var go = NGUITools.AddChild(m_ListView.gameObject, m_ItemTemplate);
                var script = go.GetComponent<RankListFormOfflineArenaItem>();
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
