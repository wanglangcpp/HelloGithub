using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityAliasedAnimationTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityAliasedAnimationTimeLineAction";
            }
        }

        private string m_AnimClipAlias = string.Empty;

        public string AnimClipAlias
        {
            get
            {
                return m_AnimClipAlias;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_AnimClipAlias = timeLineActionArgs[index++];
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(AnimClipAlias);
            return ret.ToArray();
        }
    }
}
