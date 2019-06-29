using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityReplaceBulletTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityReplaceBulletTimeLineAction";
            }
        }

        private int m_ReplaceBulletId = 0;

        public void Init(float? startTime, float? duration, int replaceBulletId)
        {
            m_StartTime = startTime ?? 0f;
            m_Duration = 0f;
            m_ReplaceBulletId = replaceBulletId;
        }

        public int ReplaceBulletId
        {
            get
            {
                return m_ReplaceBulletId;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseFloat(timeLineActionArgs[2]),
                ConverterEx.ParseInt(timeLineActionArgs[3]).Value);
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetString(ReplaceBulletId));
            return ret.ToArray();
        }
    }
}
