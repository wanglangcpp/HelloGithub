using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class DREpigraph : IDataRow
    {
        /// <summary>
        /// 铭文编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 铭文名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 铭文描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 铭文类型。
        /// </summary>
        public int Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 铭文品质。
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
        /// 卖店价格。
        /// </summary>
        public int Price
        {
            get;
            private set;
        }

        /// <summary>
        /// 铭文属性编号。
        /// </summary>
        public int AttributeType
        {
            get;
            private set;
        }

        /// <summary>
        /// 战魂属性加成。
        /// </summary>
        public float[] AddValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 升级消耗碎片数量。
        /// </summary>
        public int[] CostPieceCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 铭文碎片编号。
        /// </summary>
        public int PieceId
        {
            get;
            private set;
        }

        /// <summary>
        /// 铭文分解碎片数量。
        /// </summary>
        public int PieceCount
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
            Type = int.Parse(text[index++]);
            Quality = int.Parse(text[index++]);
            IconId = int.Parse(text[index++]);
            MinLevel = int.Parse(text[index++]);
            Price = int.Parse(text[index++]);
            AttributeType = int.Parse(text[index++]);
            AddValue = new float[Constant.MaxEpigraphLevel];
            CostPieceCount = new int[Constant.MaxEpigraphLevel];
            for (int i = 0; i < Constant.MaxEpigraphLevel; i++)
            {
                AddValue[i] = float.Parse(text[index++]);
                CostPieceCount[i] = int.Parse(text[index++]);
            }
            PieceId = int.Parse(text[index++]);
            PieceCount = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DREpigraph>();
        }
    }
}
