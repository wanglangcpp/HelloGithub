using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 加载预加载资源失败事件。
    /// </summary>
    public class LoadPreloadResourceFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化加载预加载资源失败事件的新实例。
        /// </summary>
        /// <param name="name">预加载资源的名称。</param>
        /// <param name="assetName">预加载资源的名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadPreloadResourceFailureEventArgs(string name, string assetName, string errorMessage, object userData)
        {
            Name = name;
            AssetName = assetName;
            ErrorMessage = errorMessage;
            UserData = userData;
        }

        /// <summary>
        /// 获取加载预加载资源失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.LoadPreloadResourceFailure;
            }
        }

        /// <summary>
        /// 获取预加载资源的名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取预加载资源的名称。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
