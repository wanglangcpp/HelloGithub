using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class LoginData
    {
        public int AuthorizedCode = 0;
        public string AccountName = string.Empty;
        public string LoginKey = string.Empty;
    }
}
