using UnityEngine;
using System.Collections;
using GameFramework.DataTable;

namespace Genesis.GameClient
{
    public class DRCharge : IDataRow
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 商品简介
        /// </summary>
        public string Abstract { get; private set; }

        /// <summary>
        /// 图标编号
        /// </summary>
        public int IconId { get; private set; }

        /// <summary>
        /// 价格
        /// </summary>
        public int Price { get; private set; }

        /// <summary>
        /// 是否广播
        /// </summary>
        public bool Broadcast { get; private set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool Recommend { get; private set; }

        /// <summary>
        /// 是否为月卡
        /// </summary>
        public bool MonthCard { get; private set; }

        /// <summary>
        /// 获得数量
        /// </summary>
        public int GainCount { get; private set; }

        /// <summary>
        /// 额外奖励
        /// </summary>
        public float Extra { get; private set; }

        /// <summary>
        /// 奖励道具Id
        /// </summary>
        public int ItemId { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            Abstract = text[index++];
            IconId = int.Parse(text[index++]);
            Price = int.Parse(text[index++]);
            Broadcast = bool.Parse(text[index++]);
            Recommend = bool.Parse(text[index++]);
            MonthCard = bool.Parse(text[index++]);
            GainCount = int.Parse(text[index++]);
            Extra = float.Parse(text[index++]);
            ItemId= int.Parse(text[index++]);
        }


    }
}

