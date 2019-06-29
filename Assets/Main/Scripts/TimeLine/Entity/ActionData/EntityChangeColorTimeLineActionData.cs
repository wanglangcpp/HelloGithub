using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityChangeColorTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityChangeColorTimeLineAction";
            }
        }

        private int m_ColorChangeId;
        private float m_ColorChangeDuration;

        public void Init(float? startTime, float? duration, int colorChangeId, float colorChangeDuration)
        {
            m_StartTime = startTime ?? 0f;
            m_Duration = duration ?? 0f;
            m_ColorChangeId = colorChangeId;
            m_ColorChangeDuration = colorChangeDuration;
        }

        public int ColorChangeId
        {
            get
            {
                return m_ColorChangeId;
            }
        }

        public float ColorChangeDuration
        {
            get
            {
                return m_ColorChangeDuration;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseFloat(timeLineActionArgs[2]),
                ConverterEx.ParseInt(timeLineActionArgs[3]).Value,
                ConverterEx.ParseFloat(timeLineActionArgs[4]).Value);
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetString(ColorChangeId));
            ret.Add(ConverterEx.GetString(ColorChangeDuration));
            return ret.ToArray();
        }
    }
}
