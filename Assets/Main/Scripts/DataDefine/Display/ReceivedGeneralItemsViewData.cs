using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 获得显示的物品数据。
    /// </summary>
    public class ReceivedGeneralItemsViewData
    {
        public class FakeHeroInfo
        {
            public int Count { get; set; }
            public int Id { get; set; }
            public bool HasPieces { get; set; }
        }

        private List<KeyValuePair<int, int>> m_FakeItems = new List<KeyValuePair<int, int>>();

        private List<FakeHeroInfo> m_FakeHeroInfos = new List<FakeHeroInfo>();

        private List<PBItemInfo> m_Items = new List<PBItemInfo>();

        public List<PBItemInfo> Items
        {
            get { return m_Items; }
        }
        /// <summary>
        /// 获取物品条数。
        /// </summary>
        public int Count
        {
            get
            {
                return m_FakeHeroInfos.Count + m_FakeItems.Count;
            }
        }

        /// <summary>
        /// 用户数据。
        /// </summary>
        public object UserData { get; set; }

        public void AddFakeGeneralItems(IList<PBItemInfo> items, IList<PBItemInfo> heroPieceItems)
        {
            int heroId = 0;
            for (int i = 0, j = 0; i < items.Count; i++)
            {
                m_Items.Add(items[i]);
                if (IsGeneralItemsHero(items[i].Type, out heroId))
                {
                    FakeHeroInfo heroInfo = new FakeHeroInfo();
                    heroInfo.Id = heroId;
                    heroInfo.HasPieces = heroPieceItems != null && j < heroPieceItems.Count;
                    if (heroInfo.HasPieces)
                    {
                        var item = GameEntry.Data.Items.GetData(heroPieceItems[j].Type);
                        heroInfo.Count = heroPieceItems[j].Count - ((item == null) ? 0 : item.Count);
                        j++;
                    }
                    else
                    {
                        heroInfo.Count = 1;
                    }
                    m_FakeHeroInfos.Add(heroInfo);
                }
                else
                {
                    m_FakeItems.Add(new KeyValuePair<int, int>(items[i].Type, items[i].Count));
                }
            }
        }

        /// <summary>
        /// 添加英雄数据。
        /// </summary>
        /// <param name="heroInfo">英雄信息。</param>
        public void AddHero(PBLobbyHeroInfo heroInfo)
        {
            FakeHeroInfo hero = new FakeHeroInfo();
            hero.Id = heroInfo.Type;
            hero.HasPieces = false;
            hero.Count = 1;
            m_FakeHeroInfos.Add(hero);
        }

        /// <summary>
        /// 添加道具数据。
        /// </summary>
        /// <param name="typeId">种类编号。</param>
        /// <param name="count">数量。</param>
        public void AddItem(int typeId, int count)
        {
            if (count == 0)
            {
                return;
            }
            m_FakeItems.Add(new KeyValuePair<int, int>(typeId, count));
        }

        /// <summary>
        /// 添加货币数据。
        /// </summary>
        /// <param name="typeId">种类编号。</param>
        /// <param name="count">数量。</param>
        public void AddCurrency(CurrencyType typeId, int count)
        {
            if (count == 0)
            {
                return;
            }
            m_FakeItems.Add(new KeyValuePair<int, int>((int)typeId, count));
        }

        /// <summary>
        /// 其他非道具数据。
        /// </summary>
        /// <param name="typeId">种类。</param>
        public void AddOtherGoods(int typeId)
        {
            m_FakeItems.Add(new KeyValuePair<int, int>(typeId, 1));
        }

        /// <summary>
        /// 获取物品数据。
        /// </summary>
        /// <returns>物品数据的可修改拷贝。</returns>
        public List<KeyValuePair<int, int>> GetItems()
        {
            return new List<KeyValuePair<int, int>>(m_FakeItems);
        }

        /// <summary>
        /// 获显示的英雄数据。
        /// </summary>
        /// <returns>英雄数据的可修改拷贝。</returns>
        public List<FakeHeroInfo> GetHerosForView()
        {
            return new List<FakeHeroInfo>(m_FakeHeroInfos);
        }

        public ReceiveData GetOneHeroDataById(int heroId)
        {
            ReceiveData data = new ReceiveData();
            for (int i = 0; i < m_FakeHeroInfos.Count; i++)
            {
                if (m_FakeHeroInfos[i].Id == heroId)
                {
                    data.AddData(m_FakeHeroInfos[i].Id, m_FakeHeroInfos[i].Count);
                    break;
                }
            }
            return data;
        }

        public ReceiveData GetShowHeroData()
        {
            //m_FakeHeroInfos[i].Count为碎片的数量，若没有碎片，则为1
            ReceiveData data = new ReceiveData();
            for (int i = 0; i < m_FakeHeroInfos.Count; i++)
            {
                data.AddData(m_FakeHeroInfos[i].Id, m_FakeHeroInfos[i].Count);
            }
            return data;
        }

        public ReceiveData GetShowItemData()
        {
            ReceiveData data = new ReceiveData();
            List<KeyValuePair<int, int>> items = null;

            items = m_FakeItems;

            if (items == null)
            {
                return data;
            }
            for (int i = 0; i < items.Count; i++)
            {
                var itemType = GeneralItemUtility.GetGeneralItemType(items[i].Key);
                if (itemType == GeneralItemType.Item || itemType == GeneralItemType.QualityItem || itemType == GeneralItemType.SkillBadge)
                {
                    data.AddData(items[i].Key, items[i].Value);
                }
            }
            return data;
        }

        public bool IsGeneralItemsHero(int itemId, out int heroId)
        {
            bool hasChip = true;
            IDataTable<DRItem> dtItem = GameEntry.DataTable.GetDataTable<DRItem>();
            DRItem drItem = dtItem.GetDataRow(itemId);
            if (drItem != null && drItem.Type == (int)ItemType.HeroDummyItem)
            {
                heroId = int.Parse(drItem.FunctionParams);
            }
            else
            {
                hasChip = false;
                heroId = 0;
            }
            return hasChip;
        }
    }
}
