using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntitySelfAddBuffTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntitySelfAddBuffTimeLineAction";
            }
        }

        private int m_BuffId;

        public void Init(float? startTime, int buffId)
        {
            m_StartTime = startTime ?? 0f;
            m_BuffId = buffId;
        }

        public int BuffId
        {
            get
            {
                return m_BuffId;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseInt(timeLineActionArgs[2]).Value);
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(BuffId));
            return ret.ToArray();
        }
    }
}
