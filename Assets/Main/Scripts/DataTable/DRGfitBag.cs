using UnityEngine;
using System.Collections;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class DRGfitBag : IDataRow
    {
        private const int ItemCount = 6;

        /// <summary>
        /// 礼包编号
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 礼包名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 礼包类型
        /// </summary>
        public int Type;
        /// <summary>
        /// 礼包价格
        /// </summary>
        public int Price;
        /// <summary>
        /// 图标编号
        /// </summary>
        public int IconId;
        /// <summary>
        /// 是否广播
        /// </summary>
        public bool Broadcast;
        /// <summary>
        /// 限购次数
        /// </summary>
        public int Count;
        /// <summary>
        /// 奖励内容
        /// </summary>
        public string Desc;
        /// <summary>
        /// 奖励钻石数量
        /// </summary>
        public int Diamond;
        /// <summary>
        /// 礼包所有道具
        /// </summary>
        public List<Item> Items = new List<Item>();
        public struct Item
        {
            public int Icon; public int Count;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Type = int.Parse(text[index++]);
            Price = int.Parse(text[index++]);
            IconId = int.Parse(text[index++]);
            Broadcast = bool.Parse(text[index++]);
            Count = int.Parse(text[index++]);
            Desc = text[index++];
            Diamond = int.Parse(text[index++]);

            for (int i = 0; i < ItemCount; i++)
            {
                if (int.Parse(text[index]) != 0)
                {
                    Item item = new Item();
                    item.Icon = int.Parse(text[index++]);
                    item.Count = int.Parse(text[index++]);
                    Items.Add(item);
                }
            }

        }

    }
}

