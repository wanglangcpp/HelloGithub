using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    ///
    /// </summary>
    public class ClaimActivenessChestEventArgs : GameEventArgs
    {
        public ClaimActivenessChestEventArgs(ReceivedGeneralItemsViewData data, int chestId)
        {
            ReceivedItemsView = data;
            ChestId = chestId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ClaimActivenessChest;
            }
        }

        public ReceivedGeneralItemsViewData ReceivedItemsView { get; private set; }

        public int ChestId { get; private set; }
    }
}
