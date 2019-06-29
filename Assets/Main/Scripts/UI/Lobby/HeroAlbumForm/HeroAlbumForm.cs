using GameFramework;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class HeroAlbumForm : NGUIForm
    {
        [SerializeField]
        private UIScrollView m_ScrollView = null;

        [SerializeField]
        private UITable m_HeroAlbumTable = null;

        [SerializeField]
        private ScrollViewCache m_ScrollViewCache = null;

        [SerializeField]
        private GameObject m_TagObj = null;

        private Dictionary<int, HeroAlbumCard> m_AllHeroCards = new Dictionary<int, HeroAlbumCard>();

        private bool m_JustOpened = true;

        /// <summary>
        /// 是否可以点击打开英雄展示界面（限制同时点击多个英雄）
        /// </summary>
        public static bool IsCanClickOpenHeroInfoFormBtn = false;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_JustOpened = true;
            GameEntry.Event.Subscribe(EventId.ComposeHeroComplete, OnComposeHeroComplete);
        }

        protected override void OnResume()
        {
            base.OnResume();
            RefreshHeroCards(m_JustOpened);
            m_JustOpened = false;
            IsCanClickOpenHeroInfoFormBtn = true;
        }

        protected override void OnClose(object userData)
        {
            ClearCardTable();
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.ComposeHeroComplete, OnComposeHeroComplete);
            m_JustOpened = false;
            base.OnClose(userData);
        }

        private void RefreshHeroCards(bool resetPosition)
        {
            List<BaseLobbyHeroData> heroList = new List<BaseLobbyHeroData>();

            var lobbyHeroes = GameEntry.Data.LobbyHeros.Data;
            var possessedHeroDict = new Dictionary<int, BaseLobbyHeroData>();
            for (int i = 0; i < lobbyHeroes.Count; ++i)
            {
                possessedHeroDict[lobbyHeroes[i].Type] = lobbyHeroes[i];
            }
            var dataTable = GameEntry.DataTable.GetDataTable<DRHero>();
            var rows = dataTable.GetAllDataRows();
            for (int i = 0; i < rows.Length; i++)
            {
                if (possessedHeroDict.ContainsKey(rows[i].Id))
                {
                    heroList.Add(possessedHeroDict[rows[i].Id]);
                }
                else
                {
                    heroList.Add(new UnpossessedLobbyHeroData(rows[i].Id));
                }
            }

            foreach (BaseLobbyHeroData data in heroList) {
                if (data is LobbyHeroData) {
                    DRHero heroDataRow = GameEntry.DataTable.GetDataTable<DRHero>().GetDataRow(data.Type);
                    var item = GameEntry.Data.Items.GetData(heroDataRow.StarLevelUpItemId);
                    int count = item == null ? 0 : item.Count;
                    int starLevelUpItemCount = heroDataRow.StarLevelUpItemCounts[data.StarLevel - 1];
                    ((LobbyHeroData)data).CanStarLevelUp = count >= starLevelUpItemCount && data.StarLevel < Constant.HeroStarLevelCount;
                }
            }

            heroList.Sort(Comparer.CompareHeros_AlbumByUpStarLevel);
            StartCoroutine(ClearAndShowCards(heroList, resetPosition));
        }

        private IEnumerator ClearAndShowCards(List<BaseLobbyHeroData> heroList, bool resetPosition)
        {
            m_AllHeroCards.Clear();

            yield return null;

            int lastUnpossessedIndex = 0;
            for (int i = 0; i < heroList.Count; i++)
            {
                var script = m_ScrollViewCache.GetOrCreateItem(i);
                script.RefreshData(heroList[i]);
                m_AllHeroCards[script.HeroId] = script;
                if (!script.UnpossessedHero)
                {
                    lastUnpossessedIndex = i;
                    SetObjName(script.gameObject, i);
                }
                else
                {
                    if (lastUnpossessedIndex == i - 1)
                    {
                        SetObjName(m_TagObj, i);
                    }
                    SetObjName(script.gameObject, i + 1);
                }
            }
            m_TagObj.SetActive(lastUnpossessedIndex < heroList.Count - 1);
            m_ScrollViewCache.RecycleItemsAtAndAfter(heroList.Count);
            m_HeroAlbumTable.Reposition();

            if (resetPosition)
            {
                m_ScrollView.ResetPosition();
                //GameEntry.NoviceGuide.CheckNoviceGuide(transform, UIFormId.HeroAlbumForm);
            }
        }

        private void SetObjName(GameObject obj, int index)
        {
            string name = string.Empty;
            if (index < 10)
            {
                name = "HeroAlbumCard0" + index.ToString();
            }
            else
            {
                name = "HeroAlbumCard" + index.ToString();
            }
            obj.name = name;
        }

        private void OnComposeHeroComplete(object sender, GameEventArgs e)
        {
            var ne = e as ComposeHeroCompleteEventArgs;

            ReceivedGeneralItemsViewData receiveGoodsData = new ReceivedGeneralItemsViewData();

            PBLobbyHeroInfo heroinfo = new PBLobbyHeroInfo();
            heroinfo.Type = ne.HeroType;
            receiveGoodsData.AddHero(heroinfo);
            receiveGoodsData.GetShowHeroData();
            GameEntry.RewardViewer.RequestShowRewards(receiveGoodsData, true, AfterReceiveItems, ne.HeroType);

            RefreshHeroCards(false);
        }

        private void AfterReceiveItems(object userData = null)
        {
        }

        private void ClearCardTable()
        {
            m_AllHeroCards.Clear();
            m_ScrollViewCache.RecycleAllItems();
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<HeroAlbumCard>
        {

        }
    }
}
