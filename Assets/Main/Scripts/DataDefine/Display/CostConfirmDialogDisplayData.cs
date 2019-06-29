using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="CostConfirmDialog"/> 显示数据。
    /// </summary>
    public class CostConfirmDialogDisplayData : UIFormBaseUserData
    {
        /// <summary>
        /// 模式。
        /// </summary>
        public CostConfirmDialogType Mode = CostConfirmDialogType.Undefine;

        /// <summary>
        /// 标题。非金币、体力、竞技使用。
        /// </summary>
        public string Title = string.Empty;

        /// <summary>
        /// 上方消息内容。非金币、体力、竞技使用。
        /// </summary>
        public string PreMessage = string.Empty;

        /// <summary>
        /// 下方消息内容。非金币、体力、竞技使用。
        /// </summary>
        public string PostMessage = string.Empty;

        /// <summary>
        /// 货币类型。非金币、体力、竞技使用。
        /// </summary>
        public CurrencyType UseCurrencyType = CurrencyType.Coin;

        /// <summary>
        /// 显示。非金币、体力、竞技使用。
        /// </summary>
        public int CurrencyCount = 0;

        /// <summary>
        /// 用户自定义数据。
        /// </summary>
        public object UserData = null;

        /// <summary>
        /// 按钮回调。
        /// </summary>
        public GameFrameworkAction<object> OnClickCancel = null;

        /// <summary>
        /// 按钮回调。
        /// </summary>
        public GameFrameworkAction<object> OnClickConfirm = null;
    }
}
