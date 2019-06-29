using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntitySoundTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntitySoundTimeLineAction";
            }
        }

        private int m_SoundId;
        private bool m_NeedBroadcast;
        private bool m_StopWhenBreak;
        private bool m_StopWhenFinish;

        public void Init(float? startTime, float? duration, int soundId, bool needBroadcast, bool stopWhenBreak, bool stopWhenFinish)
        {
            m_StartTime = startTime ?? 0f;
            m_Duration = duration ?? 0f;
            m_SoundId = soundId;
            m_NeedBroadcast = needBroadcast;
            m_StopWhenBreak = stopWhenBreak;
            m_StopWhenFinish = stopWhenFinish;
        }

        public int SoundId
        {
            get
            {
                return m_SoundId;
            }
        }

        public bool NeedBroadcast
        {
            get
            {
                return m_NeedBroadcast;
            }
        }

        public bool StopWhenBreak
        {
            get
            {
                return m_StopWhenBreak;
            }
        }

        public bool StopWhenFinish
        {
            get
            {
                return m_StopWhenFinish;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseFloat(timeLineActionArgs[2]),
                int.Parse(timeLineActionArgs[3]),
                bool.Parse(timeLineActionArgs[4]),
                bool.Parse(timeLineActionArgs[5]),
                bool.Parse(timeLineActionArgs[6]));
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetString(SoundId));
            ret.Add(ConverterEx.GetString(NeedBroadcast));
            ret.Add(ConverterEx.GetString(StopWhenBreak));
            ret.Add(ConverterEx.GetString(StopWhenFinish));
            return ret.ToArray();
        }
    }
}
