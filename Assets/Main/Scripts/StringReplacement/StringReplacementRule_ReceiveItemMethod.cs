namespace Genesis.GameClient
{
    public class StringReplacementRule_ReceiveItemMethod : IStringReplacementRule
    {
        public enum ReceiveItemMethodType
        {
            None,
            CoinChance = 1,
            MoneyChance,
            GearFoundry,
            GearCompose,
            HeroPieceCompose,
            TurnOverChess,
            CompleteInstance,
            PvpTokenExchange,
        }

        public string Key
        {
            get
            {
                return "ReceiveItemMethod";
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

            switch ((ReceiveItemMethodType)int.Parse(args[1]))
            {
                case ReceiveItemMethodType.CoinChance:
                    str = GameEntry.Localization.GetString("CHAT_MSG_SYSTEM_COINCHANCE");
                    break;
                case ReceiveItemMethodType.CompleteInstance:
                    str = GameEntry.Localization.GetString("CHAT_MSG_SYSTEM_COMPLETEINSTANCE");
                    break;
                case ReceiveItemMethodType.GearCompose:
                    str = GameEntry.Localization.GetString("CHAT_MSG_SYSTEM_GEARCOMPOSE");
                    break;
                case ReceiveItemMethodType.GearFoundry:
                    str = GameEntry.Localization.GetString("CHAT_MSG_SYSTEM_GEARFOUNDRY");
                    break;
                case ReceiveItemMethodType.HeroPieceCompose:
                    str = GameEntry.Localization.GetString("CHAT_MSG_SYSTEM_HEROPIECECOMPOSE");
                    break;
                case ReceiveItemMethodType.MoneyChance:
                    str = GameEntry.Localization.GetString("CHAT_MSG_SYSTEM_MONEYCHANCE");
                    break;
                case ReceiveItemMethodType.PvpTokenExchange:
                    str = GameEntry.Localization.GetString("CHAT_MSG_SYSTEM_PVPTOKENEXCHANGE");
                    break;
                case ReceiveItemMethodType.TurnOverChess:
                    str = GameEntry.Localization.GetString("CHAT_MSG_SYSTEM_TURNOVERCHESS");
                    break;
                default:
                    break;
            }

            str = ColorUtility.AddColorToString(GameEntry.ClientConfig.ClientColorConfig.ActivityColor, str);

            return str;
        }
    }
}
