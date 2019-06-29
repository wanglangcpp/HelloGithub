using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    ///
    /// </summary>
    public class OnSkillBadgeDataChangeEventArgs : GameEventArgs
    {
        public OnSkillBadgeDataChangeEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.OnSkillBadgeDataChanged;
            }
        }
    }
}
