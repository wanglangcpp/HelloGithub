using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 实体要求其发出的子弹释放技能的时间轴行为数据。
    /// </summary>
    public class EntityAskBulletToPerformSkillTimeLineActionData : TimeLineActionData
    {
        public override string ActionType
        {
            get
            {
                return "EntityAskBulletToPerformSkillTimeLineAction";
            }
        }

        private int m_SkillId = 0;
        private int m_TargetBulletTypeId = 0;

        public void Init(float? startTime, float? duration, int skillId, int targetBulletTypeId)
        {
            m_StartTime = startTime ?? 0f;
            m_Duration = 0f;
            m_SkillId = skillId;
            m_TargetBulletTypeId = targetBulletTypeId;
        }

        /// <summary>
        /// 技能编号。
        /// </summary>
        public int SkillId
        {
            get
            {
                return m_SkillId;
            }
        }

        /// <summary>
        /// 目标子弹种类编号。
        /// </summary>
        public int TargetBulletTypeId
        {
            get
            {
                return m_TargetBulletTypeId;
            }
        }

        public override void ParseData(string[] timeLineActionArgs)
        {
            Init(
                ConverterEx.ParseFloat(timeLineActionArgs[1]),
                ConverterEx.ParseFloat(timeLineActionArgs[2]),
                ConverterEx.ParseInt(timeLineActionArgs[3]).Value,
                ConverterEx.ParseInt(timeLineActionArgs[4]).Value);
        }

        public override string[] SerializeData()
        {
            List<string> ret = new List<string>();
            ret.Add(ActionType);
            ret.Add(ConverterEx.GetString(StartTime));
            ret.Add(ConverterEx.GetString(Duration));
            ret.Add(ConverterEx.GetString(SkillId));
            ret.Add(ConverterEx.GetString(TargetBulletTypeId));
            return ret.ToArray();
        }
    }
}
