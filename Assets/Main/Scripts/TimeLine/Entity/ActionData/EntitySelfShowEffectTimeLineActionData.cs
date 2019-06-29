using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntitySelfShowEffectTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntitySelfShowEffectTimeLineAction";
            }
        }

        private string m_ResourceName;
        private string m_AttachPointPath;
        private bool m_AutoStop;

        public string ResourceName
        {
            get
            {
                return m_ResourceName;
            }
        }

        public string AttachPointPath
        {
            get
            {
                return m_AttachPointPath;
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
            m_AttachPointPath = timeLineActionArgs[index++];
            m_AutoStop = timeLineActionArgs.Length > index ? ConverterEx.ParseBool(timeLineActionArgs[index++]).Value : false;
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ResourceName);
            ret.Add(AttachPointPath);
            ret.Add(ConverterEx.GetString(AutoStop));
            return ret.ToArray();
        }
    }
}
