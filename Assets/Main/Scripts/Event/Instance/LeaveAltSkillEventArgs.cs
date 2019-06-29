using GameFramework.Event;

namespace Genesis.GameClient
{
    public class LeaveAltSkillEventArgs : GameEventArgs
    {
        public LeaveAltSkillEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.LeaveAltSkill;
            }
        }
    }
}
