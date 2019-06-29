using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        private SkillRushingParams m_SkillRushingParams;

        public Vector3 SkillRushingDirInput { get; private set; }

        public bool SetSkillRushingDirInput(Vector3 value)
        {
            if (!IsDuringSkillRushing || !SkillRushingParams.AcceptDirInput)
            {
                return false;
            }

            SkillRushingDirInput = value;
            return true;
        }

        public bool IsDuringSkillRushing
        {
            get
            {
                return Skilling && m_SkillRushingParams != null;
            }
        }

        public SkillRushingParams SkillRushingParams
        {
            get
            {
                return m_SkillRushingParams;
            }
        }

        public bool StartSkillRushing(SkillRushingParams skillRushingParams)
        {
            if (!Skilling)
            {
                return false;
            }

            m_SkillRushingParams = skillRushingParams;
            return true;
        }

        public void StopSkillRushing()
        {
            if (m_SkillRushingParams == null)
            {
                return;
            }

            m_SkillRushingParams = null;
        }
    }
}
