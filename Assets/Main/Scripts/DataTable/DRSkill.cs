using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 技能配置表。
    /// </summary>
    public class DRSkill : IDataRow
    {
        private const int ParametersCount = 2;

        /// <summary>
        /// 技能编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 技能名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 技能描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 技能冷却时间。
        /// </summary>
        public float CoolDownTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 技能图标编号。
        /// </summary>
        public int IconId
        {
            get;
            private set;
        }

        /// <summary>
        /// 参数个数。
        /// </summary>
        public int ParameterCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前技能等级属性值。
        /// </summary>
        public float[] AttrBaseValues
        {
            get;
            private set;
        }

        /// <summary>
        /// 属性逐级增量。
        /// </summary>
        public float[] AttrDeltasPerLevel
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
            CoolDownTime = float.Parse(text[index++]);
            IconId = int.Parse(text[index++]);
            ParameterCount = int.Parse(text[index++]);
            AttrBaseValues = new float[ParametersCount];
            AttrDeltasPerLevel = new float[ParametersCount];
            for (int i = 0; i < ParametersCount; i++)
            {
                AttrBaseValues[i] = float.Parse(text[index++]);
                AttrDeltasPerLevel[i] = float.Parse(text[index++]);
            }
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRSkill>();
        }
    }
}
