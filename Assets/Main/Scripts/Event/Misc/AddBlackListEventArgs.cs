using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    ///
    /// </summary>
    public class AddBlackListEventArgs : GameEventArgs
    {
        public AddBlackListEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.AddBlackList;
            }
        }
    }
}
