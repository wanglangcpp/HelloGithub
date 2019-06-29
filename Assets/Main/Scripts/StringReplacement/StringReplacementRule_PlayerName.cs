namespace Genesis.GameClient
{
    public class StringReplacementRule_PlayerName : IStringReplacementRule
    {
        public string Key
        {
            get
            {
                return "PlayerName";
            }
        }

        public int MinArgCount
        {
            get
            {
                return 1;
            }
        }

        public string DoAction(string[] args)
        {
            return GameEntry.Data.Player.Name;
        }
    }
}
