namespace Genesis.GameClient
{
    internal class StringReplacementRule_Number : IStringReplacementRule
    {
        public string Key
        {
            get
            {
                return "Number";
            }
        }

        public int MinArgCount
        {
            get
            {
                return 2;
            }
        }

        public string DoAction(string[] args)
        {
            string str = string.Empty;
            str = args[1];
            return str;
        }
    }
}
