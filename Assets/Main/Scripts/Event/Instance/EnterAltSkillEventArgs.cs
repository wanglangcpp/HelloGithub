using GameFramework.Event;

namespace Genesis.GameClient
{
    public class EnterAltSkillEventArgs : GameEventArgs
    {
        public EnterAltSkillEventArgs(int altSkillId, int altSkillLevel)
        {
            AltSkillId = altSkillId;
            AltSkillLevel = altSkillLevel;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.EnterAltSkill;
            }
        }

        public int AltSkillId
        {
            get;
            private set;
        }

        public int AltSkillLevel
        {
            get;
            private set;
        }
    }
}
