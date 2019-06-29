using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityFaceToTargetTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityFaceToTargetTimeLineAction";
            }
        }

        private float m_AngularSpeed;

        /// <summary>
        /// 角速度的绝对值。小于等于零时认为不限制角速度。
        /// </summary>
        public float AngularSpeed
        {
            get
            {
                return m_AngularSpeed;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            int index = 0;
            index++;
            m_StartTime = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_Duration = ConverterEx.ParseFloat(timeLineActionArgs[index++]) ?? 0f;
            m_AngularSpeed = ConverterEx.ParseFloat(timeLineActionArgs[index++]).Value;
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetString(AngularSpeed));
            return ret.ToArray();
        }
    }
}
