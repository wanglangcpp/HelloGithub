using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 伤害配置表。
    /// </summary>
    public class DRImpact : IDataRow
    {
        public const int ImpactParamCount = 22;

        /// <summary>
        /// 伤害编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 伤害类型。
        /// </summary>
        public int ImpactType
        {
            get;
            private set;
        }

        /// <summary>
        /// 覆盖普通霸体。
        /// </summary>
        public bool IgnoreSteady
        {
            get;
            private set;
        }

        /// <summary>
        /// 伤害参数。
        /// </summary>
        public float[] ImpactParams
        {
            get;
            private set;
        }

        /// <summary>
        /// 伤害参数。
        /// </summary>
        /// <param name="index">伤害参数索引。</param>
        /// <returns>技能组编号。</returns>
        public float GetImpactParam(int index)
        {
            return ImpactParams[index];
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            ImpactType = int.Parse(text[index++]);
            IgnoreSteady = bool.Parse(text[index++]);
            ImpactParams = new float[ImpactParamCount];
            for (int i = 0; i < ImpactParamCount; i++)
            {
                ImpactParams[i] = float.Parse(text[index++]);
            }
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRImpact>();
        }
    }
}
