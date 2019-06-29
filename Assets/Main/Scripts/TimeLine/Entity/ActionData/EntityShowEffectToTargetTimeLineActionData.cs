using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityShowEffectToTargetTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityShowEffectToTargetTimeLineAction";
            }
        }

        private string m_ResourceName;
        private bool m_AutoStop = true;

        public string ResourceName
        {
            get
            {
                return m_ResourceName;
            }
        }

        public bool AutoStop
        {
            get
            {
                return m_AutoStop;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_ResourceName = timeLineActionArgs[index++];
            m_AutoStop = ConverterEx.ParseBool(timeLineActionArgs[index++]).Value;
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ResourceName);
            ret.Add(ConverterEx.GetString(AutoStop));
            return ret.ToArray();
        }
    }
}
