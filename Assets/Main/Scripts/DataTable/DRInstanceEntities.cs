using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本内实体配置表。
    /// </summary>
    public abstract class DRInstanceEntities : IDataRow
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

#if UNITY_EDITOR

        /// <summary>
        /// 策划备注。
        /// </summary>
        public string GDRemark
        {
            get;
            protected set;
        }

#endif

        /// <summary>
        /// 类型编号。
        /// </summary>
        public int EntityTypeId
        {
            get;
            protected set;
        }

        /// <summary>
        /// 位置X。
        /// </summary>
        public float PositionX
        {
            get;
            protected set;
        }

        /// <summary>
        /// 位置Y。
        /// </summary>
        public float PositionY
        {
            get;
            protected set;
        }

        /// <summary>
        /// 朝向。
        /// </summary>
        public float Rotation
        {
            get;
            protected set;
        }

        /// <summary>
        /// 是否计入玩家击杀。
        /// </summary>
        public bool CountForPlayerKill
        {
            get;
            protected set;
        }

        public virtual void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
#if UNITY_EDITOR
            GDRemark = text[index++];
#else
            index++;
#endif
            EntityTypeId = int.Parse(text[index++]);
            PositionX = float.Parse(text[index++]);
            PositionY = float.Parse(text[index++]);
            Rotation = float.Parse(text[index++]);
            CountForPlayerKill = bool.Parse(text[index++]);
        }

#if UNITY_EDITOR

        public virtual string[] WriteDataRow()
        {
            List<string> ret = new List<string>();
            ret.Add(string.Empty);
            ret.Add(Id.ToString());
            ret.Add(GDRemark);
            ret.Add(EntityTypeId.ToString());
            ret.Add(PositionX.ToString("F2"));
            ret.Add(PositionY.ToString("F2"));
            ret.Add(Rotation.ToString("F2"));
            ret.Add(CountForPlayerKill.ToString());
            return ret.ToArray();
        }

#endif

        private void AvoidJIT()
        {
            new Dictionary<int, DRInstanceEntities>();
        }
    }
}
