using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntitySphereColliderTriggerImpactTimeLineActionData : EntityColliderTriggerImpactTimeLineActionDataAbstract
    {
        public override string ActionType
        {
            get
            {
                return "EntitySphereColliderTriggerImpactTimeLineAction";
            }
        }

        private float? m_Radius;

        public float? Radius
        {
            get
            {
                return m_Radius;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Offset = ConverterEx.ParseVector3(timeLineActionArgs[index++]);
            m_Radius = ConverterEx.ParseFloat(timeLineActionArgs[index++]);
            index = ParseData(timeLineActionArgs, index);

        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetStringFromVector3(Offset));
            ret.Add(ConverterEx.GetString(Radius));
            SerializeData(ret);
            return ret.ToArray();
        }
    }
}
