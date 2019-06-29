using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityImpactShakeCameraTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityImpactShakeCameraTimeLineAction";
            }
        }

        private int m_ShakeId;
        private bool m_AffectMe = true;
        private bool m_AffectOthers;

        public void Init(float? startTime, float? duration, int? shakeId, bool? affectMe, bool? affectOthers)
        {
            m_StartTime = startTime ?? 0f;
            m_Duration = duration ?? 0f;
            m_ShakeId = shakeId ?? 0;
            m_AffectMe = affectMe ?? true;
            m_AffectOthers = affectOthers ?? false;
        }

        public int ShakeId
        {
            get
            {
                return m_ShakeId;
            }
        }

        public bool AffectMe
        {
            get
            {
                return m_AffectMe;
            }
        }

        public bool AffectOthers
        {
            get
            {
                return m_AffectOthers;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseFloat(timeLineActionArgs[2]),
                ConverterEx.ParseInt(timeLineActionArgs[3]),
                ConverterEx.ParseBool(timeLineActionArgs[4]),
                ConverterEx.ParseBool(timeLineActionArgs[5]));
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetString(ShakeId));
            ret.Add(ConverterEx.GetString(AffectMe));
            ret.Add(ConverterEx.GetString(AffectOthers));
            return ret.ToArray();
        }
    }
}
