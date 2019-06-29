using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    ///
    /// </summary>
    public class UpdateAchievementEventArgs : GameEventArgs
    {
        public UpdateAchievementEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.UpdateAchievement;
            }
        }
    }
}
