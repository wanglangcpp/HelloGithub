namespace Genesis.GameClient
{
    public interface ICanHaveTarget : ICampable
    {
        bool HasTarget
        {
            get;
        }

        ITargetable Target
        {
            get;
            set;
        }
    }
}
