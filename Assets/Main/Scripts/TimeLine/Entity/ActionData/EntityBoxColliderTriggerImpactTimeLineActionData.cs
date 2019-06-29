using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityBoxColliderTriggerImpactTimeLineActionData : EntityColliderTriggerImpactTimeLineActionDataAbstract
    {
        public override string ActionType
        {
            get
            {
                return "EntityBoxColliderTriggerImpactTimeLineAction";
            }
        }

        private float? m_Direction;
        private Vector3? m_Size;

        public float? Direction
        {
            get
            {
                return m_Direction;
            }
        }

        public Vector3? Size
        {
            get
            {
                return m_Size;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Offset = ConverterEx.ParseVector3(timeLineActionArgs[index++]);
            m_Direction = ConverterEx.ParseFloat(timeLineActionArgs[index++]);
            m_Size = ConverterEx.ParseVector3(timeLineActionArgs[index++]);
            index = ParseData(timeLineActionArgs, index);
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetStringFromVector3(Offset));
            ret.Add(ConverterEx.GetString(Direction));
            ret.Add(ConverterEx.GetStringFromVector3(Size));
            SerializeData(ret);
            return ret.ToArray();
        }
    }
}
