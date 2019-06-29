using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    ///
    /// </summary>
    public class ShouldShowHudValueChangedEventArgs : GameEventArgs
    {
        public ShouldShowHudValueChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ShouldShowHudValueChanged;
            }
        }
    }
}
