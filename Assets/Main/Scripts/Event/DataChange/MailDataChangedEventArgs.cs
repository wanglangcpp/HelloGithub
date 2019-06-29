using GameFramework.Event;

namespace Genesis.GameClient
{
    public class MailDataChangedEventArgs : GameEventArgs
    {
        public MailDataChangedEventArgs()
        {

        }
        public MailDataChangedEventArgs(bool needReposition)
        {
            NeedRepositionList = needReposition;
        }

        public bool NeedRepositionList { get; private set; }
        public override int Id
        {
            get
            {
                return (int)EventId.MailDataChanged;
            }
        }
    }
}
