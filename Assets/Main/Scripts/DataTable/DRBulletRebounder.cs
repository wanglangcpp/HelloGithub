using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 子弹反弹器配置表。
    /// </summary>
    public class DRBulletRebounder : IDataRow
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 资源名称。
        /// </summary>
        public string ResourceName
        {
            get;
            private set;
        }

        /// <summary>
        /// 坐标位置X。
        /// </summary>
        public float PositionX
        {
            get;
            private set;
        }

        /// <summary>
        /// 坐标位置Y。
        /// </summary>
        public float PositionY
        {
            get;
            private set;
        }

        /// <summary>
        /// 朝向。
        /// </summary>
        public float Angle
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
            ResourceName = text[index++];
            PositionX = float.Parse(text[index++]);
            PositionY = float.Parse(text[index++]);
            Angle = float.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRBulletRebounder>();
        }
    }
}
