using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class DRBuff : IDataRow
    {
        public const int BuffParamCount = 10;
        public const int NonExcludingGroup = 0;

        public int Id
        {
            get;
            private set;
        }

        public string NameKey
        {
            get;
            private set;
        }

        public string DescKey
        {
            get;
            private set;
        }

        /// <summary>
        /// 增益(>0)/减益(<0)/未定义(0)
        /// </summary>
        public int GoodOrBad
        {
            get;
            private set;
        }

        public int ExcludingGroup
        {
            get;
            private set;
        }

        public int ExcludingLevel
        {
            get;
            private set;
        }

        public int BuffType
        {
            get;
            private set;
        }

        public float Duration
        {
            get;
            private set;
        }

        /// <summary>
        /// 周期性 Buff 的心跳周期 (秒)，非正值为无效
        /// </summary>
        public float HeartBeat
        {
            get;
            private set;
        }

        public float[] BuffParams
        {
            get;
            private set;
        }

        public int DefaultEffectId
        {
            get;
            private set;
        }

        public int BlendEffectId
        {
            get;
            private set;
        }

        public bool TransferOnHeroSwitch
        {
            get;
            private set;
        }

        public bool KeepOnFakeDeath
        {
            get;
            private set;
        }

        /// <summary>
        /// 变色编号。
        /// </summary>
        public int ColorChangeId
        {
            get;
            private set;
        }

        /// <summary>
        /// 空中处理。
        /// </summary>
        public int GoToAir
        {
            get;
            private set;
        }

        public int ElementId
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
            NameKey = text[index++];
            DescKey = text[index++];
            GoodOrBad = int.Parse(text[index++]);
            ExcludingGroup = int.Parse(text[index++]);
            ExcludingLevel = int.Parse(text[index++]);
            BuffType = int.Parse(text[index++]);
            Duration = float.Parse(text[index++]);
            HeartBeat = float.Parse(text[index++]);
            BuffParams = new float[BuffParamCount];
            for (int i = 0; i < BuffParamCount; i++)
            {
                BuffParams[i] = float.Parse(text[index++]);
            }
            DefaultEffectId = int.Parse(text[index++]);
            BlendEffectId = int.Parse(text[index++]);
            TransferOnHeroSwitch = bool.Parse(text[index++]);
            KeepOnFakeDeath = bool.Parse(text[index++]);
            ColorChangeId = int.Parse(text[index++]);
            GoToAir = int.Parse(text[index++]);
            ElementId = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRBuff>();
        }
    }
}
