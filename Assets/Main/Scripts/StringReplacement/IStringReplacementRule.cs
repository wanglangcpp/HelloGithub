namespace Genesis.GameClient
{
    /// <summary>
    /// 字符串替换规则列表。
    /// </summary>
    public interface IStringReplacementRule
    {
        string Key
        {
            get;
        }

        int MinArgCount
        {
            get;
        }

        string DoAction(string[] args);
    }
}
