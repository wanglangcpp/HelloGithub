using UnityEngine;
using System.Collections;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class DRInstanceForTower : DRInstance
    {
        private const int LayItemCount = 4;
        private const int BoxItemCount = 3;

        /// <summary>
        /// 前置副本
        /// </summary>
        public int ForwardInstance { get; protected set; }
        /// <summary>
        /// 开放副本
        /// </summary>
        public int OpenInstance { get; protected set; }

        public class Item
        {
            public int ItemIcon = 0;
            public int ItemCount = 0;
        }
        /// <summary>
        /// 塔层奖励物品
        /// </summary>
        public List<Item> LayItems { get { return m_ItemsIcon; } }
        private List<Item> m_ItemsIcon = new List<Item>();

        /// <summary>
        /// 是否有宝箱
        /// </summary>
        public bool IsHaveBox { get; protected set; }

        /// <summary>
        /// 宝箱奖励物品
        /// </summary>
        public List<Item> BoxItems { get { return m_BoxItems; } }
        private List<Item> m_BoxItems = new List<Item>();
        /// <summary>
        /// 爬塔对应副本ID
        /// </summary>
        public int CheckPoint { get; protected set; }

        public override void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            ForwardInstance = int.Parse(text[index++]);
            OpenInstance = int.Parse(text[index++]);
            for (int i = 0; i < LayItemCount; i++)
            {
                int Icon = int.Parse(text[index++]);
                int Count = int.Parse(text[index++]);
                if (Icon != -1)
                {
                    m_ItemsIcon.Add(new Item() { ItemIcon = Icon, ItemCount = Count });
                }
            }
            IsHaveBox = bool.Parse(text[index++]);
            for (int i = 0; i < BoxItemCount; i++)
            {
                int Icon = int.Parse(text[index++]);
                int Count = int.Parse(text[index++]);
                if (Icon > 0 && Count > 0)
                {
                    m_BoxItems.Add(new Item() { ItemIcon = Icon, ItemCount = Count });
                }
            }
            CheckPoint = int.Parse(text[index++]);
        }
    }
}

