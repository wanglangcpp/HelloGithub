using UnityEngine;
using System.Collections;
using GameFramework.DataTable;

namespace Genesis.GameClient
{
    public class DREveryDayGift : IDataRow
    {
        [SerializeField]
        public int Id { get; private set; }
        [SerializeField]
        public string Name { get; private set; }
        /// <summary>
        /// 图标名字
        /// </summary>
        [SerializeField]
        public string Icon { get; private set; }
        /// <summary>
        /// 价格
        /// </summary>
        [SerializeField]
        public int Price { get; private set; }
        /// <summary>
        /// 奖励描述
        /// </summary>
        [SerializeField]
        public string Description { get; private set; }
        /// <summary>
        /// 是否广播
        /// </summary>
        [SerializeField]
        public bool Broadcast { get; private set; }

        [SerializeField]
        public int Count { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] rowData = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(rowData[index++]);
            index++;
            Name= rowData[index++];
            Icon = rowData[index++];
            Price = int.Parse(rowData[index++]);
            Description = rowData[index++];
            Broadcast = bool.Parse(rowData[index++]);
            Count = int.Parse(rowData[index++]);
        }
    }
}

