using GameFramework.Event;

namespace Genesis.GameClient
{
    public class TriggerPortalEventArgs : GameEventArgs
    {
        public TriggerPortalEventArgs(Character target, int fromPortalId, int toPortalId)
        {
            Target = target;
            FromPortalId = fromPortalId;
            ToPortalId = toPortalId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.TriggerPortal;
            }
        }

        public Character Target
        {
            get;
            private set;
        }

        public int FromPortalId
        {
            get;
            private set;
        }

        public int ToPortalId
        {
            get;
            private set;
        }
    }
}
