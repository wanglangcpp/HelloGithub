using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 加载时间轴失败事件。
    /// </summary>
    public class LoadTimeLineGroupFailureEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化加载时间轴失败事件的新实例。
        /// </summary>
        /// <param name="timeLineName">时间轴名称。</param>
        /// <param name="timeLineAssetName">时间轴资源名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadTimeLineGroupFailureEventArgs(string timeLineName, string timeLineAssetName, string errorMessage, object userData)
        {
            TimeLineName = timeLineName;
            TimeLineAssetName = timeLineAssetName;
            ErrorMessage = errorMessage;
            UserData = userData;
        }

        /// <summary>
        /// 获取加载时间轴失败事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.LoadTimeLineGroupFailure;
            }
        }

        /// <summary>
        /// 获取时间轴名称。
        /// </summary>
        public string TimeLineName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取时间轴资源名称。
        /// </summary>
        public string TimeLineAssetName
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
