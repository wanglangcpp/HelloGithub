using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityEnterAltSkillTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityEnterAltSkillTimeLineAction";
            }
        }

        private int m_AltSkillId;
        private int m_AltSkillLevel;
        private float? m_KeepTime;

        public void Init(float? startTime, int altSkillId, int altSkillLevel, float? keepTime)
        {
            m_StartTime = startTime ?? 0f;
            m_AltSkillId = altSkillId;
            m_AltSkillLevel = altSkillLevel;
            m_KeepTime = keepTime;
        }

        public int AltSkillId
        {
            get
            {
                return m_AltSkillId;
            }
        }

        public int AltSkillLevel
        {
            get
            {
                return m_AltSkillLevel;
            }
        }

        public float? KeepTime
        {
            get
            {
                return m_KeepTime;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                int.Parse(timeLineActionArgs[2]),
                int.Parse(timeLineActionArgs[3]),
                ConverterEx.ParseFloat(timeLineActionArgs[4]));
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(AltSkillId));
            ret.Add(ConverterEx.GetString(AltSkillLevel));
            ret.Add(ConverterEx.GetString(KeepTime));
            return ret.ToArray();
        }
    }
}
