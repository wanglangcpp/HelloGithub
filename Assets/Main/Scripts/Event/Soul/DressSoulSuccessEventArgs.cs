using GameFramework.Event;

namespace Genesis.GameClient
{
    public class DressSoulSuccessEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.DressSoulSuccess;
            }
        }
    }
}
