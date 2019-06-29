using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityWeaponAnimationTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityWeaponAnimationTimeLineAction";
            }
        }

        private int m_WeaponIndex;

        public int WeaponIndex
        {
            get
            {
                return m_WeaponIndex;
            }
        }

        private string m_AnimationName = string.Empty;

        public string AnimationName
        {
            get
            {
                return m_AnimationName;
            }
        }

        private float? m_Speed;

        public float? Speed
        {
            get
            {
                return m_Speed;
            }
        }

        private float? m_Time;

        public float? Time
        {
            get
            {
                return m_Time;
            }
        }

        private float? m_FadeLength;

        public float? FadeLength
        {
            get
            {
                return m_FadeLength;
            }
        }

        public void Init(float? startTime, float? duration, int weaponIndex, string animationName, float? speed, float? time, float? fadeLength)
        {
            m_StartTime = startTime ?? 0f;
            m_Duration = duration ?? 0f;
            m_WeaponIndex = weaponIndex;
            m_AnimationName = animationName;
            m_Speed = speed;
            m_Time = time;
            m_FadeLength = fadeLength;
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseFloat(timeLineActionArgs[2]),
                int.Parse(timeLineActionArgs[3]),
                timeLineActionArgs[4],
                ConverterEx.ParseFloat(timeLineActionArgs[5]),
                ConverterEx.ParseFloat(timeLineActionArgs[6]),
                ConverterEx.ParseFloat(timeLineActionArgs[7]));
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetString(WeaponIndex));
            ret.Add(AnimationName);
            ret.Add(ConverterEx.GetString(Speed));
            ret.Add(ConverterEx.GetString(Time));
            ret.Add(ConverterEx.GetString(FadeLength));
            return ret.ToArray();
        }
    }
}
