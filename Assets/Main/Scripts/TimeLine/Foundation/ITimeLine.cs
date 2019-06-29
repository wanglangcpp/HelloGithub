namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴接口。
    /// </summary>
    /// <typeparam name="T">时间轴持有者类型。</typeparam>
    public interface ITimeLine<T> where T : class
    {
        /// <summary>
        /// 获取时间轴编号。
        /// </summary>
        int Id
        {
            get;
        }

        /// <summary>
        /// 获取时间轴持续时间。
        /// </summary>
        float Duration
        {
            get;
        }

        /// <summary>
        /// 增加时间轴行为。
        /// </summary>
        /// <param name="action">要增加的时间轴行为。</param>
        /// <returns>是否增加时间轴行为成功。</returns>
        bool AddAction(TimeLineAction<T> action);

        /// <summary>
        /// 移除时间轴行为。
        /// </summary>
        /// <param name="action">要移除的时间轴行为。</param>
        /// <returns>是否移除时间轴行为成功。</returns>
        bool RemoveAction(TimeLineAction<T> action);
    }
}
