using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 物品配置表。
    /// </summary>
    public class DRItem : IDataRow
    {
        /// <summary>
        /// 物品编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 物品名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 物品描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 排序参数。
        /// </summary>
        public int Order
        {
            get;
            private set;
        }

        /// <summary>
        /// 道具分类。
        /// </summary>
        public int Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 道具品质。
        /// </summary>
        public int Quality
        {
            get;
            private set;
        }

        /// <summary>
        /// 图标编号。
        /// </summary>
        public int IconId
        {
            get;
            private set;
        }

        /// <summary>
        /// 使用等级下限。
        /// </summary>
        public int MinLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 堆叠上限。
        /// </summary>
        public int MaxCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 卖店价格。
        /// </summary>
        public int Price
        {
            get;
            private set;
        }

        /// <summary>
        /// 自动使用。
        /// </summary>
        public bool AutoUse
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否广播。
        /// </summary>
        public bool Broadcast
        {
            get;
            private set;
        }

        /// <summary>
        /// 功能编号。
        /// </summary>
        public int FunctionId
        {
            get;
            private set;
        }

        /// <summary>
        /// 功能参数。
        /// </summary>
        public string FunctionParams
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            Order = int.Parse(text[index++]);
            Type = int.Parse(text[index++]);
            Quality = int.Parse(text[index++]);
            IconId = int.Parse(text[index++]);
            MinLevel = int.Parse(text[index++]);
            MaxCount = int.Parse(text[index++]);
            Price = int.Parse(text[index++]);
            AutoUse = bool.Parse(text[index++]);
            Broadcast = bool.Parse(text[index++]);
            FunctionId = int.Parse(text[index++]);
            FunctionParams = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRItem>();
        }
    }
}
