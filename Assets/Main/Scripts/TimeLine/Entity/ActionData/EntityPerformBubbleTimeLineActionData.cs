using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityPerformBubbleTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityPerformBubbleTimeLineAction";
            }
        }

        private string m_BubbleContent = null;

        public string BubbleContent
        {
            get
            {
                return m_BubbleContent;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_BubbleContent = timeLineActionArgs[index++];
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(BubbleContent);
            return ret.ToArray();
        }
    }
}
