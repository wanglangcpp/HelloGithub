using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 加载时间轴成功事件。
    /// </summary>
    public class LoadTimeLineGroupSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化加载时间轴成功事件的新实例。
        /// </summary>
        /// <param name="timeLineName">时间轴名称。</param>
        /// <param name="timeLineAssetName">时间轴资源名称。</param>
        /// <param name="timeLineResourceChildName">时间轴子资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadTimeLineGroupSuccessEventArgs(string timeLineName, string timeLineAssetName, object userData)
        {
            TimeLineName = timeLineName;
            TimeLineAssetName = timeLineAssetName;
            UserData = userData;
        }

        /// <summary>
        /// 获取加载时间轴成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.LoadTimeLineGroupSuccess;
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
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
