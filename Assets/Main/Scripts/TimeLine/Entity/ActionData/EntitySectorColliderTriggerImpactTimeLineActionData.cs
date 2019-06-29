using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntitySectorColliderTriggerImpactTimeLineActionData : EntityColliderTriggerImpactTimeLineActionDataAbstract
    {
        public override string ActionType
        {
            get
            {
                return "EntitySectorColliderTriggerImpactTimeLineAction";
            }
        }

        private float? m_Direction;
        private int? m_Angle;
        private float? m_Radius;
        private float? m_Height;

        public float? Direction
        {
            get
            {
                return m_Direction;
            }
        }

        public int? Angle
        {
            get
            {
                return m_Angle;
            }
        }

        public float? Radius
        {
            get
            {
                return m_Radius;
            }
        }

        public float? Height
        {
            get
            {
                return m_Height;
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
            m_Angle = ConverterEx.ParseInt(timeLineActionArgs[index++]);
            m_Radius = ConverterEx.ParseFloat(timeLineActionArgs[index++]);
            m_Height = ConverterEx.ParseFloat(timeLineActionArgs[index++]);
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
            ret.Add(ConverterEx.GetString(Angle));
            ret.Add(ConverterEx.GetString(Radius));
            ret.Add(ConverterEx.GetString(Height));
            SerializeData(ret);
            return ret.ToArray();
        }
    }
}
