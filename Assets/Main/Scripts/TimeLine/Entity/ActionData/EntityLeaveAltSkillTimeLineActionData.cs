using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityLeaveAltSkillTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityLeaveAltSkillTimeLineAction";
            }
        }

        public void Init(float? startTime)
        {
            m_StartTime = startTime ?? 0f;
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(ConverterEx.ParseFloat(timeLineActionArgs[1]));
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            return ret.ToArray();
        }
    }
}
