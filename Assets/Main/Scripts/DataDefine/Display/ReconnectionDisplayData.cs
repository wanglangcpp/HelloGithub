using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="ReconnectionForm"/> 显示数据。
    /// </summary>
    class ReconnectionDisplayData : UIFormBaseUserData
    {
        /// <summary>
        /// 消息内容。
        /// </summary>
        public string Message = string.Empty;

        /// <summary>
        /// 消息内容。
        /// </summary>
        public string ButtonMessage = string.Empty;

        /// <summary>
        /// 确定按钮回调。
        /// </summary>
        public GameFrameworkAction<object> OnClickConfirm = null;

        /// <summary>
        /// 用户自定义数据。
        /// </summary>
        public object UserData = null;
    }
}
