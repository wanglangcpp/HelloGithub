namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴创建器。
    /// </summary>
    public static class TimeLineCreator
    {
        /// <summary>
        /// 创建时间轴管理器。
        /// </summary>
        /// <typeparam name="T">时间轴持有者类型。</typeparam>
        /// <returns>时间轴管理器。</returns>
        public static ITimeLineManager<T> CreateTimeLineManager<T>() where T : class
        {
            return new TimeLineManager<T>();
        }

        /// <summary>
        /// 创建时间轴。
        /// </summary>
        /// <typeparam name="T">时间轴持有者类型。</typeparam>
        /// <param name="timeLineId">时间轴编号。</param>
        /// <returns>时间轴。</returns>
        public static ITimeLine<T> CreateTimeLine<T>(int timeLineId) where T : class
        {
            return new TimeLine<T>(timeLineId);
        }
    }
}
