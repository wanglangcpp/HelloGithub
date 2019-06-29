using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class EntityBulletTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityBulletTimeLineAction";
            }
        }

        private int? m_BulletId;

        public void Init(float? startTime, int? bulletId)
        {
            m_StartTime = startTime ?? 0f;
            m_BulletId = bulletId;
        }

        public int? BulletId
        {
            get
            {
                return m_BulletId;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseInt(timeLineActionArgs[2]));
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(BulletId));
            return ret.ToArray();
        }
    }
}
