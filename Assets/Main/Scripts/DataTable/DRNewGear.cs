using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备配置表。
    /// </summary>
    public class DRNewGear : IDataRow
    {
        /// <summary>
        /// 装备编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 适用英雄编号。
        /// </summary>
        public int HeroId { get; private set; }

        /// <summary>
        /// 图标编号。
        /// </summary>
        public int IconId { get; private set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 装备类型。
        /// </summary>
        public NewGearIndex Index { get; private set; }

        /// <summary>
        /// 默认等级。
        /// </summary>
        public int DefaultLevel { get; private set; }

        /// <summary>
        /// 默认总品阶。
        /// </summary>
        public int DefaultTotalQualityLevel { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            HeroId = int.Parse(text[index++]);
            IconId = int.Parse(text[index++]);
            Name = text[index++];
            Index = (NewGearIndex)int.Parse(text[index++]);
            DefaultLevel = int.Parse(text[index++]);
            DefaultTotalQualityLevel = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRNewGear>();
        }
    }
}
