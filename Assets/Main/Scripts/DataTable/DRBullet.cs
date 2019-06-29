using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 子弹配置表。
    /// </summary>
    public class DRBullet : IDataRow
    {
        /// <summary>
        /// 子弹编号。
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
        /// 偏移位置X。
        /// </summary>
        public float OffsetX
        {
            get;
            private set;
        }

        /// <summary>
        /// 偏移位置Y。
        /// </summary>
        public float OffsetY
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
        /// 距离地面高度。
        /// </summary>
        public float Height
        {
            get;
            private set;
        }

        /// <summary>
        /// 速度。
        /// </summary>
        public float Speed
        {
            get;
            private set;
        }

        /// <summary>
        /// 存在时间。
        /// </summary>
        public float KeepTime
        {
            get;
            private set;
        }

        /// <summary>
        /// AI行为。
        /// </summary>
        public string AIBehavior
        {
            get;
            private set;
        }

        /// <summary>
        /// 父节点类型
        /// </summary>
        public TransformParentType TransformParentType
        {
            get;
            private set;
        }

        /// <summary>
        /// 坐标参考类型
        /// </summary>
        public TransformType TransformType
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否可反弹。
        /// </summary>
        public bool IsReboundable
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
            OffsetX = float.Parse(text[index++]);
            OffsetY = float.Parse(text[index++]);
            Angle = float.Parse(text[index++]);
            Height = float.Parse(text[index++]);
            Speed = float.Parse(text[index++]);
            KeepTime = float.Parse(text[index++]);
            AIBehavior = text[index++];
            TransformParentType = (TransformParentType)(int.Parse(text[index++]));
            TransformType = (TransformType)(int.Parse(text[index++]));
            IsReboundable = bool.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRBullet>();
        }
    }
}
