using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 等级范围配置表。
    /// </summary>
    public class DRLevelRange : IDataRow
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 最小等级。
        /// </summary>
        public int MinLevel { get; private set; }

        /// <summary>
        /// 最大等级。
        /// </summary>
        public int MaxLevel { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            MinLevel = int.Parse(text[index++]);
            MaxLevel = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRLevelRange>();
        }
    }
}
