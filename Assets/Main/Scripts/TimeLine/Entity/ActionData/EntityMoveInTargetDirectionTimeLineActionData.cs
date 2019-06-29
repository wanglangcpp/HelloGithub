using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityMoveInTargetDirectionTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityMoveInTargetDirectionTimeLineAction";
            }
        }

        private float m_Offset;

        public float Offset
        {
            get
            {
                return m_Offset;
            }
        }

        private float m_Delay;

        public float Delay
        {
            get
            {
                return m_Delay;
            }
        }

        private bool m_FaceTargetPosOnMove;

        public bool FaceTargetPosOnMove
        {
            get
            {
                return m_FaceTargetPosOnMove;
            }
        }

        public void Init(float? startTime, float? duration, float offset, float delay, bool faceTargetPosOnMove)
        {
            m_StartTime = startTime ?? 0f;
            m_Duration = duration ?? 0f;
            m_Offset = offset;
            m_Delay = delay > 0f ? delay : 0f;
            m_FaceTargetPosOnMove = faceTargetPosOnMove;
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseFloat(timeLineActionArgs[2]),
                ConverterEx.ParseFloat(timeLineActionArgs[3]).Value,
                ConverterEx.ParseFloat(timeLineActionArgs[4]).Value,
                bool.Parse(timeLineActionArgs[5]));
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetString(Offset));
            ret.Add(ConverterEx.GetString(Delay));
            ret.Add(ConverterEx.GetString(FaceTargetPosOnMove));
            return ret.ToArray();
        }
    }
}
