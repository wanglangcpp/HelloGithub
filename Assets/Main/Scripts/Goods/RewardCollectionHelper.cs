using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 领取奖励的辅助类。
    /// </summary>
    /// <remarks>
    /// 构造 <see cref="Genesis.GameClient.ReceiveGoodsData"/> 对象以便显示，并抛出相应的事件。每个方法应至多调用一次。
    /// </remarks>
    public class RewardCollectionHelper
    {
        private ReceivedGeneralItemsViewData m_ReceiveGoodsData = new ReceivedGeneralItemsViewData();

        /// <summary>
        /// 获取用于显示的奖品数据。
        /// </summary>
        public ReceivedGeneralItemsViewData ReceiveGoodsData
        {
            get
            {
                return m_ReceiveGoodsData;
            }
        }

        /// <summary>
        /// 每种道具添加后的事件。
        /// </summary>
        public event GameFrameworkAction<int, int> OnPerItemAdded;

        /// <summary>
        /// 每个装备添加后的事件。
        /// </summary>
        public event GameFrameworkAction<int> OnPerGearAdded;

        /// <summary>
        /// 每个战魂添加后的事件。
        /// </summary>
        public event GameFrameworkAction<int> OnPerSoulAdded;

        /// <summary>
        /// 每个铭文添加后的事件。
        /// </summary>
        public event GameFrameworkAction<int> OnPerEpigraphAdded;

        /// <summary>
        /// 设置获取货币的数量。
        /// </summary>
        ///  <param name="typeId">货币类型。</param>
        /// <param name="count">数量。</param>
        public void SetCurrency(CurrencyType typeId, int count)
        {
            m_ReceiveGoodsData.AddCurrency(typeId, count);
        }

        /// <summary>
        /// 添加显示道具信息（英雄货币等信息可能为假）。
        /// </summary>
        /// <param name="itemsData">显示道具信息。</param>
        public void AddFakeGeneralItems(IList<PBItemInfo> items, IList<PBItemInfo> heroPieceItems)
        {
            if (items == null)
            {
                return;
            }
            m_ReceiveGoodsData.AddFakeGeneralItems(items, heroPieceItems);
        }

        /// <summary>
        /// 添加英雄信息。
        /// </summary>
        /// <param name="heros">英雄信息。</param>
        public void AddHeros(IList<PBLobbyHeroInfo> heros)
        {
            if (heros == null)
            {
                return;
            }

            for (int i = 0; i < heros.Count; ++i)
            {
                GameEntry.Data.LobbyHeros.AddData(heros[i]);
                m_ReceiveGoodsData.AddHero(heros[i]);
            }
            GameEntry.Event.Fire(this, new LobbyHeroDataChangedEventArgs());
        }

        /// <summary>
        /// 添加道具信息。
        /// </summary>
        /// <param name="itemsData">道具信息。</param>
        public void AddItems(IList<PBItemInfo> items)
        {
            if (items == null)
            {
                return;
            }

            for (int i = 0; i < items.Count; ++i)
            {
                var item = items[i];
                if (item == null)
                {
                    continue;
                }

                int oldCount = GeneralItemUtility.GetGeneralItemCount(item.Type);
                GeneralItemUtility.UpdateItemsData(items[i]);
                int deltaCount = item.Count - oldCount;
                if (deltaCount <= 0)
                {
                    continue;
                }

                items[i].Count = deltaCount;

                if (OnPerItemAdded != null)
                {
                    OnPerItemAdded(item.Type, deltaCount);
                }
            }

            m_ReceiveGoodsData.AddFakeGeneralItems(items, null);
            GameEntry.Event.Fire(this, new ItemDataChangedEventArgs());
            GameEntry.Event.Fire(this, new HeroQualityItemDataChangeEventArgs());
        }

        /// <summary>
        /// 添加PBCompoundItemInfo道具信息。
        /// </summary>
        /// <param name="itemsData">道具信息。</param>
        public void AddCompoundItems(IList<PBCompoundItemInfo> compoundItems)
        {
            if (compoundItems == null)
            {
                return;
            }

            for (int i = 0; i < compoundItems.Count; i++)
            {
                if (compoundItems[i].ItemInfo != null)
                {
                    AddItems(new List<PBItemInfo>() { compoundItems[i].ItemInfo });
                }
                if (compoundItems[i].AutoUseItemInfo != null)
                {
                    if (compoundItems[i].AutoUseItemInfo.PlayerInfo != null)
                    {
                        GameEntry.Data.Player.UpdateData(compoundItems[i].AutoUseItemInfo.PlayerInfo);
                    }

                    if (compoundItems[i].AutoUseItemInfo.AutoUseItemInfo != null)
                    {
                        AddFakeGeneralItems(new List<PBItemInfo>() { compoundItems[i].AutoUseItemInfo.AutoUseItemInfo }, compoundItems[i].AutoUseItemInfo.ItemInfo);
                    }

                    if (compoundItems[i].AutoUseItemInfo.LobbyHeroInfo.Count > 0)
                    {
                        var heros = compoundItems[i].AutoUseItemInfo.LobbyHeroInfo;
                        for (int j = 0; j < heros.Count; ++j)
                        {
                            GameEntry.Data.LobbyHeros.AddData(heros[j]);
                        }
                    }

                    if (compoundItems[i].AutoUseItemInfo.ItemInfo.Count > 0)
                    {
                        GeneralItemUtility.UpdateItemsData(compoundItems[i].AutoUseItemInfo.ItemInfo);
                    }
                }
            }
        }
    }
}
