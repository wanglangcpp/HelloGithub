using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 实体保持连续点击技能的时间轴行为数据。
    /// </summary>
    public class EntityKeepContinualTapSkillTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityKeepContinualTapSkillTimeLineAction";
            }
        }

        private float[] m_InputCheckIntervals = new float[0];

        /// <summary>
        /// 获取检查输入的区间。
        /// </summary>
        /// <returns></returns>
        public IList<float> GetInputCheckIntervals()
        {
            return m_InputCheckIntervals;
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_InputCheckIntervals = ConverterEx.ParseFloatArray(timeLineActionArgs[index++]);
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetStringFromArray(m_InputCheckIntervals));
            return ret.ToArray();
        }
    }
}
