using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备最大品阶配置表。
    /// </summary>
    public class DRNewGearQualityMaxLevel : IDataRow
    {
        /// <summary>
        /// 品质编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 最大品阶。
        /// </summary>
        public int MaxLevel { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            MaxLevel = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRNewGearQualityMaxLevel>();
        }
    }
}
