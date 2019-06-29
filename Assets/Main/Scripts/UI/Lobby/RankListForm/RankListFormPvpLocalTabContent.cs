using GameFramework;
using GameFramework.Event;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace Genesis.GameClient
{
    /// <summary>
    /// Pvp本地服务器排行榜。
    /// </summary>
    internal class RankListFormPvpLocalTabContent : RankListFormBaseTabContent
    {
        protected override RankListType RankType
        {
            get
            {
                return RankListType.PvpLocalServer;
            }
        }
        public override void OnInit()
        {
            if (RankType == RankListType.Unspecified)
            {
                return;
            }
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLGetSinlgePVPRanks msg = new CLGetSinlgePVPRanks();
                msg.Type = (int)RankType;
                GameEntry.Network.Send(msg);
            }
        }
        protected override void SubscribeEvents()
        {
            GameEntry.Event.Subscribe(EventId.GetSinglePvpRank, OnDataObtained);
        }

        protected override void UnsubscribeEvents()
        {
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.GetSinglePvpRank, OnDataObtained);
        }

        protected override void OnDataObtained(object sender, GameEventArgs e)
        {
            GetSinglePvpRanksEventArgs msg = e as GetSinglePvpRanksEventArgs;
            var packet = msg.Packet;
            if (packet == null)
            {
                Log.Warning("Packet is invalid.");
                return;
            }
            if (packet.Type != (int)RankType)
            {
                return;
            }

            for (int i = 0; i < m_Titles.Length; i++)
            {
                m_Titles[i].SetActive(false);
            }
            if (packet.MyRank == 0)
            {
                if (packet.RankInfo.Count > 0)
                {
                    m_Titles[(int)TitleType.PlayerNotOnRank].SetActive(true);
                    CurTitle = m_Titles[(int)TitleType.PlayerNotOnRank];
                }
                else
                {
                    m_Titles[(int)TitleType.NoOneRank].SetActive(true);
                    CurTitle = m_Titles[(int)TitleType.NoOneRank];
                }
            }
            else
            {
                m_Titles[(int)TitleType.MyRank].SetActive(true);
                m_MyRank.text = packet.MyRank.ToString();
                m_Score.gameObject.SetActive(false);
                m_ScoreType.gameObject.SetActive(false);
                m_IntegralIcon.gameObject.SetActive(true);
                m_IntegralText.gameObject.SetActive(true);
                m_IntegralText.text = packet.HasMyScore ? packet.MyScore.ToString() : string.Empty;
                CurTitle = m_Titles[(int)TitleType.MyRank];
            }
            ClearListView();
            StartCoroutine(PopulateRanks(packet));
        }

        private IEnumerator PopulateRanks(LCGetSinlgePVPRanks packet)
        {
            yield return null;
            #region 原有逻辑
            // 
            //             m_MyRank.text = packet.MyRank.ToString();
            // 
            //             var enemies = packet.PlayerInfo;
            //             for (int i = 0; i < enemies.Count; ++i)
            //             {
            //                 var go = NGUITools.AddChild(m_ListView.gameObject, m_ItemTemplate);
            //                 var script = go.GetComponent<RankListLocalServerItem>();
            //                 //script.Refresh(enemies[i], packet.Score[i], i + 1);
            //                 script.gameObject.SetActive(false);
            //                 script.gameObject.SetActive(true);
            //             }
            // 
            //             m_ListView.Reposition();
            //             m_ScrollView.ResetPosition();
            #endregion
            var enemies = packet.RankInfo;

            var listItems = new List<RankListFormPvpAllServerItem>();
            for (int i = 0; i < enemies.Count; ++i)
            {
                var go = NGUITools.AddChild(m_ListView.gameObject, m_ItemTemplate);
                var script = go.GetComponent<RankListFormPvpAllServerItem>();
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

        protected override IEnumerator PopulateCo(LCRefreshRankList packet)
        {
            throw new System.NotImplementedException();
        }
    }
}
