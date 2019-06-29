using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityReplacementTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityReplacementTimeLineAction";
            }
        }

        private int? m_ReplacedTimeLineId;
        private int? m_ReplacementTimeLineId;
        private float? m_ReplacementWaitTime;

        public void Init(float? startTime, int? replacedTimeLineId, int? replacementTimeLineId, float? replacementWaitTime)
        {
            m_StartTime = startTime ?? 0f;
            m_ReplacedTimeLineId = replacedTimeLineId;
            m_ReplacementTimeLineId = replacementTimeLineId;
            m_ReplacementWaitTime = replacementWaitTime;
        }

        public int? ReplacedTimeLineId
        {
            get
            {
                return m_ReplacedTimeLineId;
            }
        }

        public int? ReplacementTimeLineId
        {
            get
            {
                return m_ReplacementTimeLineId;
            }
        }

        public float? ReplacementWaitTime
        {
            get
            {
                return m_ReplacementWaitTime;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseInt(timeLineActionArgs[1]),
                ConverterEx.ParseInt(timeLineActionArgs[2]),
                ConverterEx.ParseInt(timeLineActionArgs[3]),
                ConverterEx.ParseFloat(timeLineActionArgs[4]));
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(ReplacedTimeLineId));
            ret.Add(ConverterEx.GetString(ReplacementTimeLineId));
            ret.Add(ConverterEx.GetString(ReplacementWaitTime));
            return ret.ToArray();
        }
    }
}
