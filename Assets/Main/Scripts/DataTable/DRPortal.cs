using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 传送点配置表。
    /// </summary>
    public class DRPortal : IDataRow
    {
        /// <summary>
        /// 传送点编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
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

        /// <summary>
        /// 半径。
        /// </summary>
        public float Radius
        {
            get;
            private set;
        }

        /// <summary>
        /// 传送入口触发时的特效名称。
        /// </summary>
        public string BeforePortageEffectNameOnTrigger
        {
            get;
            private set;
        }

        /// <summary>
        /// 传送出口触发时的特效名称。
        /// </summary>
        public string AfterPortageEffectNameOnTrigger
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
            Radius = float.Parse(text[index++]);
            BeforePortageEffectNameOnTrigger = text[index++];
            AfterPortageEffectNameOnTrigger = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRPortal>();
        }
    }
}
