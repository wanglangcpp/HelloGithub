using UnityEngine;

namespace Genesis.GameClient
{
    public class ReplaceSkillInfo
    {
        private int m_ReplacedSkillId = 0;
        private int m_ReplacementSkillId = 0;
        private float m_ReplacementSkillWaitTime = 0f;
        private bool m_ReplacementSkillEnabled = false;
        private bool m_ReplacementStateImpacted = false;

        public int ReplacedSkillId
        {
            get
            {
                return m_ReplacedSkillId;
            }
        }

        public int ReplacementSkillId
        {
            get
            {
                return m_ReplacementSkillId;
            }
        }

        public void Reset()
        {
            m_ReplacedSkillId = 0;
            m_ReplacementSkillId = 0;
            m_ReplacementSkillWaitTime = 0f;
            m_ReplacementSkillEnabled = false;
            m_ReplacementStateImpacted = false;
        }

        public void SetReplaceSkill(int replacedSkillId, int replacementSkillId, float replacementWaitTime)
        {
            m_ReplacedSkillId = replacedSkillId;
            m_ReplacementSkillId = replacementSkillId;
            m_ReplacementSkillWaitTime = replacementWaitTime;
            m_ReplacementSkillEnabled = false;
            m_ReplacementStateImpacted = false;
        }

        public bool RecordEnable(int currentSkillId)
        {
            if (m_ReplacedSkillId > 0 && m_ReplacementSkillId > 0 && currentSkillId == m_ReplacementSkillId)
            {
                m_ReplacementSkillEnabled = true;
                return true;
            }

            return false;
        }

        public void RecordStateImpact()
        {
            m_ReplacementStateImpacted = true;
        }

        public bool CanReplaceSkill(int currentSkillId)
        {
            return m_ReplacedSkillId > 0 && m_ReplacementSkillId > 0 && currentSkillId == m_ReplacedSkillId && Time.time <= m_ReplacementSkillWaitTime;
        }

        public bool NeedReplaceSkill()
        {
            if (!m_ReplacementSkillEnabled)
            {
                return false;
            }

            return m_ReplacedSkillId > 0 && m_ReplacementSkillId > 0 && !m_ReplacementStateImpacted;
        }

        public bool NeedRestartSkill()
        {
            if (!m_ReplacementSkillEnabled)
            {
                return false;
            }

            return m_ReplacedSkillId > 0 && m_ReplacementSkillId > 0 && m_ReplacementStateImpacted;
        }
    }
}
