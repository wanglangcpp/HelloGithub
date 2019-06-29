using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class NetworkHelper
    {
        /// <summary>
        /// 处理服务器错误的方式。
        /// </summary>
        private enum ServerErrorHandlingType
        {
            /// <summary>
            /// 弹出飘窗。
            /// </summary>
            Toast,

            /// <summary>
            /// 弹出只有确定按钮的对话框。
            /// </summary>
            DialogWithOkayButton,

            /// <summary>
            /// 弹出只有重启按钮的对话框。
            /// </summary>
            DialogWithRestartButton,

            /// <summary>
            /// 弹出有重启和取消按钮的对话框。
            /// </summary>
            DialogWithRestartAndCancelButton,

            /// <summary>
            /// 发送错误码消息。
            /// </summary>
            SendServerErrorEventMessage,
        }

        /// <summary>
        /// 错误码对应的处理方式。默认使用 <see cref="ServerErrorHandlingType.Toast"/>。
        /// </summary>
        private readonly static Dictionary<ServerErrorCode, ServerErrorHandlingType> s_ErrorHandlingTypes = new Dictionary<ServerErrorCode, ServerErrorHandlingType>
        {
            { ServerErrorCode.RoomStatusError, ServerErrorHandlingType.SendServerErrorEventMessage },
        };
    }
}
