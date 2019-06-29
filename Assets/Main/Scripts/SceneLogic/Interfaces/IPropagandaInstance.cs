namespace Genesis.GameClient
{
    /// <summary>
    /// Instance logics that takes use of NPC propaganda should implement this interface.
    /// </summary>
    public interface IPropagandaInstance
    {
        InstancePropagandaData CurrentPropagandaData { get; }

        void AddPropaganda(InstancePropagandaData data);

        int GetNpcIdFromIndex(int index);
    }
}
