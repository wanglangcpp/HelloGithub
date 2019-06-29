using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityShowOrHideWeaponTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityShowOrHideWeaponTimeLineAction";
            }
        }

        private int[] m_ShowWeaponIndices;

        public int[] ShowWeaponIndices
        {
            get
            {
                return m_ShowWeaponIndices;
            }
        }

        private int[] m_HideWeaponIndices;

        public int[] HideWeaponIndices
        {
            get
            {
                return m_HideWeaponIndices;
            }
        }

        public void Init(float? startTime, float? duration, int[] showWeaponIndices, int[] hideWeaponIndices)
        {
            m_StartTime = startTime ?? 0f;
            m_Duration = duration ?? 0f;
            m_ShowWeaponIndices = showWeaponIndices;
            m_HideWeaponIndices = hideWeaponIndices;
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseFloat(timeLineActionArgs[2]),
                ConverterEx.ParseIntArray(timeLineActionArgs[3]),
                ConverterEx.ParseIntArray(timeLineActionArgs[4]));
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetStringFromArray(ShowWeaponIndices));
            ret.Add(ConverterEx.GetStringFromArray(HideWeaponIndices));
            return ret.ToArray();
        }
    }
}
