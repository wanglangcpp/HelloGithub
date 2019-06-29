using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 排行榜标签页内容基类。
    /// </summary>
    internal abstract class RankListFormBaseTabContent : MonoBehaviour
    {
        [SerializeField]
        protected UILabel m_MyRank = null;

        [SerializeField]
        protected GameObject m_ItemTemplate = null;

        [SerializeField]
        protected UIScrollView m_ScrollView = null;

        [SerializeField]
        protected UIGrid m_ListView = null;

        [SerializeField]
        protected GameObject[] m_Titles = null;

        [SerializeField]
        protected UILabel m_Score = null;//分数

        [SerializeField]
        protected UILabel m_ScoreType = null;//积分类型

        [SerializeField]
        protected UILabel m_IntegralText = null;//Pvp积分

        [SerializeField]
        protected UISprite m_IntegralIcon = null;//Pvp积分图标

        public virtual GameObject CurTitle
        {
            get;
            set;
        }

        protected enum TitleType
        {
            MyRank = 0,
            NoOneRank = 1,
            PlayerNotOnRank = 2,
        }

        protected void OnEnable()
        {
            Clear();
            SubscribeEvents();
            OnInit();
        }

        protected void OnDisable()
        {
            Clear();
            UnsubscribeEvents();
        }

        protected virtual void SubscribeEvents()
        {
        }

        protected virtual void UnsubscribeEvents()
        {
        }

        protected void ClearListView()
        {
            var items = m_ListView.GetChildList();
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i] != null)
                {
                    Destroy(items[i].gameObject);
                }
            }
        }

        private void Clear()
        {
            ClearListView();
            m_MyRank.text = string.Empty;
        }

        protected virtual RankListType RankType
        {
            get
            {
                return RankListType.Unspecified;
            }
        }

        public virtual void OnInit()
        {
            GameEntry.LobbyLogic.RefreshRankList(RankType);
        }

        protected virtual void OnDataObtained(object sender, GameEventArgs e)
        {
            GetRankListDataEventArgs msg = e as GetRankListDataEventArgs;
            var packet = msg.RefreshRankList;
            if (packet == null)
            {
                Log.Warning("Packet is invalid.");
                return;
            }
            if (msg.RefreshRankList.RankType != (int)RankType)
            {
                return;
            }

            for (int i = 0; i < m_Titles.Length; i++)
            {
                m_Titles[i].SetActive(false);
            }
            if (!packet.HasMyRank)
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
                m_Score.text = packet.HasMyScore ? packet.MyScore.ToString() : string.Empty;
                CurTitle = m_Titles[(int)TitleType.MyRank];
            }
            ClearListView();
            StartCoroutine(PopulateCo(packet));
        }

        protected abstract IEnumerator PopulateCo(LCRefreshRankList packet);
    }

}
