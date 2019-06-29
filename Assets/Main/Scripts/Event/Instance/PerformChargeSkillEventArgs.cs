using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class PerformChargeSkillEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.PerformChargeSkill;
            }
        }

        public PerformChargeSkillEventArgs(OperateType type, int skillId)
        {
            m_OperateType = type;
            m_SkillId = skillId;
        }

        private int m_SkillId;
        public int SkillId
        {
            get
            {
                return m_SkillId;
            }
        }

        private OperateType m_OperateType;
        public OperateType HandleType
        {
            get
            {
                return m_OperateType;
            }
        }

        public enum OperateType
        {
            Show,
            Hide,
            EventHide
        }
    }
}