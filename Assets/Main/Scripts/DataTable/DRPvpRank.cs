using UnityEngine;
using System.Collections;
using GameFramework.DataTable;

namespace Genesis.GameClient
{
    public class DRPvpRank : IDataRow
    {
        public int Id { get; private set; }
        public int Type { get; private set; }
        public string GradingName { get; private set; }
        public string GradingIcon { get; private set; }
        /// <summary>
        /// 表示一个段位的最低积分
        /// </summary>
        public int MinIntegral { get; private set; }
        /// <summary>
        /// 表示一个段位的最高积分
        /// </summary>
        public int MaxIntegral { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Type = int.Parse(text[index++]);
            GradingName = text[index++];
            GradingIcon = text[index++];
            MinIntegral = int.Parse(text[index++]);
            MaxIntegral = int.Parse(text[index++]);
        }

    }
}

