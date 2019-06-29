namespace Genesis.GameClient
{
    public interface IGenericData<T, PBT> where T : class
    {
        int Key
        {
            get;
        }

        void UpdateData(PBT data);
    }
}
