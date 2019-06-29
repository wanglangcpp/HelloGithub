using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    ///
    /// </summary>
    public class OnMimicMeleeChangedEventArgs : GameEventArgs
    {
        public OnMimicMeleeChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.OnMimicMeleeChanged;
            }
        }

        public string KillerName { get; set; }

        public string VictimName { get; set; }

        public bool VictimIsMe { get; set; }
    }
}
