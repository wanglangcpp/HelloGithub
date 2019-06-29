namespace Genesis.GameClient
{
    public class StringReplacementRule_NewLine : IStringReplacementRule
    {
        public string Key
        {
            get
            {
                return "NL";
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
            return "\n";
        }
    }
}
