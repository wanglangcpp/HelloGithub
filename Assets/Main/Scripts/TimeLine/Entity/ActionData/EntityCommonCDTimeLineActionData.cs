using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityCommonCDTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityCommonCDTimeLineAction";
            }
        }

        private float m_CoolDownTime;

        public float CoolDownTime
        {
            get
            {
                return Mathf.Max(0f, m_CoolDownTime);
            }
        }

        public void Init(float? startTime, float coolDownTime)
        {
            m_StartTime = startTime ?? 0f;
            m_CoolDownTime = coolDownTime;
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseFloat(timeLineActionArgs[2]).Value);
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(CoolDownTime));
            return ret.ToArray();
        }
    }
}
