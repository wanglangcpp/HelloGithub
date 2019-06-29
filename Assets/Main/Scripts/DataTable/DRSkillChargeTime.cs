using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 蓄力技能蓄力时间配置表。
    /// </summary>
    public class DRSkillChargeTime : IDataRow
    {
        private List<float> m_ValidChargeEndTime;

        /// <summary>
        /// 技能编号。
        /// </summary>
        public int Id { get; private set; }

        public List<float> ValidChargeEndTime
        {
            get { return m_ValidChargeEndTime; }
        }

        public float TotalTime
        {
            get
            {
                if (m_ValidChargeEndTime.Count == 0)
                    return 0;

                return m_ValidChargeEndTime[m_ValidChargeEndTime.Count - 1];
            }
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            m_ValidChargeEndTime = new List<float>();

            int index = 1;
            Id = int.Parse(text[index++]);
            index++;
            AddTime(float.Parse(text[index++]));
            AddTime(float.Parse(text[index++]));
            AddTime(float.Parse(text[index++]));
            AddTime(float.Parse(text[index++]));
            AddTime(float.Parse(text[index++]));
            AddTime(float.Parse(text[index++]));
            AddTime(float.Parse(text[index++]));
            AddTime(float.Parse(text[index++]));
        }

        private void AddTime(float time)
        {
            if (time > 0)
                m_ValidChargeEndTime.Add(time);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRSkillChargeTime>();
        }
    }
}
