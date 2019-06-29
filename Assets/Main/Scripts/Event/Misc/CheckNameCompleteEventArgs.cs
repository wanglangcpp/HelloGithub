using GameFramework.Event;

namespace Genesis.GameClient
{
    internal class CheckNameCompleteEventArgs : GameEventArgs
    {
        public CheckNameCompleteEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.CheckNameComplete;
            }
        }
    }
}
