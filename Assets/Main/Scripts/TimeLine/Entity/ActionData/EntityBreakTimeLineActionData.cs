using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityBreakTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityBreakTimeLineAction";
            }
        }

        private bool? m_CanBreakByMove;
        private bool? m_CanBreakBySkill;
        private bool? m_CanBreakByImpact;

        public void Init(float? startTime, float? duration, bool? canBreakByMove, bool? canBreakBySkill, bool? canBreakByImpact)
        {
            m_StartTime = startTime ?? 0f;
            m_Duration = duration ?? 0f;
            m_CanBreakByMove = canBreakByMove;
            m_CanBreakBySkill = canBreakBySkill;
            m_CanBreakByImpact = canBreakByImpact;
        }

        public bool? CanBreakByMove
        {
            get
            {
                return m_CanBreakByMove;
            }
        }

        public bool? CanBreakBySkill
        {
            get
            {
                return m_CanBreakBySkill;
            }
        }

        public bool? CanBreakByImpact
        {
            get
            {
                return m_CanBreakByImpact;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseFloat(timeLineActionArgs[2]),
                ConverterEx.ParseBool(timeLineActionArgs[3]),
                ConverterEx.ParseBool(timeLineActionArgs[4]),
                ConverterEx.ParseBool(timeLineActionArgs[5]));
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetString(CanBreakByMove));
            ret.Add(ConverterEx.GetString(CanBreakBySkill));
            ret.Add(ConverterEx.GetString(CanBreakByImpact));
            return ret.ToArray();
        }
    }
}
