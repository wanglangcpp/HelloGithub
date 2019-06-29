using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="Dialog"/> 显示数据。
    /// </summary>
    public class DialogDisplayData : UIFormBaseUserData
    {
        /// <summary>
        /// 模式，即按钮数量。取值 1、2、3。
        /// </summary>
        public int Mode = 1;

        /// <summary>
        /// 标题。仅显示于 Native 对话框的情况，在 NGUI 对话框中失效。
        /// </summary>
        public string Title = string.Empty;

        /// <summary>
        /// 消息内容。
        /// </summary>
        public string Message = string.Empty;

        /// <summary>
        /// 确认按钮文本。
        /// </summary>
        public string ConfirmText = string.Empty;

        /// <summary>
        /// 取消按钮文本。
        /// </summary>
        public string CancelText = string.Empty;

        /// <summary>
        /// 中立按钮文本。
        /// </summary>
        public string OtherText = string.Empty;

        /// <summary>
        /// 是否暂停游戏。
        /// </summary>
        public bool PauseGame = false;

        /// <summary>
        /// 确定按钮回调。
        /// </summary>
        public GameFrameworkAction<object> OnClickConfirm = null;

        /// <summary>
        /// 取消按钮回调。
        /// </summary>
        public GameFrameworkAction<object> OnClickCancel = null;

        /// <summary>
        /// 中立按钮回调。
        /// </summary>
        public GameFrameworkAction<object> OnClickOther = null;

        /// <summary>
        /// 消息宽度。在 Native 对话框中失效。
        /// </summary>
        public int Width = 0;

        /// <summary>
        /// 用户自定义数据。
        /// </summary>
        public object UserData = null;
    }
}
