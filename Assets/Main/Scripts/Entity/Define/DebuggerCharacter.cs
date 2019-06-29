namespace Genesis.GameClient
{
    public class DebuggerCharacter : Character
    {
        public override float DeadKeepTime
        {
            get
            {
                return 0f;
            }
        }

        public override bool NeedShowHPBarOnDamage
        {
            get
            {
                return false;
            }
        }

        public override int SteadyBuffId
        {
            get
            {
                return 0;
            }
        }
    }
}
