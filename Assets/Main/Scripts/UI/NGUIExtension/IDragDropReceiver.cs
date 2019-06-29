namespace Genesis.GameClient
{
    public interface IDragDropReceiver
    {
        void OnItemDragDropStart(UIDragDropItem item);

        void OnItemDragDropMove(UIDragDropItem item);

        void OnItemDragDropEnd(UIDragDropItem item);
    }
}
