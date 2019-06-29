namespace Genesis.GameClient
{
    public abstract partial class BasePvpaiInstanceLogic
    {
        protected abstract class PvpaiInstanceSuccess : AbstractInstanceSuccess
        {

        }

        protected abstract class PvpaiInstanceFailure : AbstractInstanceFailure
        {
            public PvpaiInstanceFailure(bool shouldOpenUI) : base(shouldOpenUI)
            {

            }
        }
    }
}
