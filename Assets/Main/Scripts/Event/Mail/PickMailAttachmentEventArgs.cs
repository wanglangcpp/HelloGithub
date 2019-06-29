using GameFramework.Event;

namespace Genesis.GameClient
{
    public class PickMailAttachmentEventArgs : GameEventArgs
    {
        public PickMailAttachmentEventArgs(RewardCollectionHelper rewards)
        {
            ShowRewards = rewards;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.PickMailAttachment;
            }
        }

        public RewardCollectionHelper ShowRewards
        {
            get;
            private set;
        }
    }
}