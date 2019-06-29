namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴管理器接口。
    /// </summary>
    /// <typeparam name="T">时间轴持有者类型。</typeparam>
    public interface ITimeLineManager<T> where T : class
    {
        /// <summary>
        /// 获取时间轴数量。
        /// </summary>
        int TimeLineCount
        {
            get;
        }

        /// <summary>
        /// 获取时间轴实例数量。
        /// </summary>
        int TimeLineInstanceCount
        {
            get;
        }

        /// <summary>
        /// 时间轴管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">当前已流逝时间，以秒为单位。</param>
        void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 时间轴管理器调试绘制。
        /// </summary>
        void DebugDraw();

        /// <summary>
        /// 关闭并清理时间轴管理器。
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 清理所有时间轴实例。
        /// </summary>
        void ClearAllTimeInstances();

        /// <summary>
        /// 创建时间轴实例。
        /// </summary>
        /// <param name="owner">时间轴实例持有者。</param>
        /// <param name="timeLineId">源时间轴编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的时间轴实例。</returns>
        ITimeLineInstance<T> CreateTimeLineInstance(T owner, int timeLineId, object userData = null);

        /// <summary>
        /// 创建时间轴实例。
        /// </summary>
        /// <param name="owner">时间轴实例持有者。</param>
        /// <param name="timeLine">源时间轴。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的时间轴实例。</returns>
        ITimeLineInstance<T> CreateTimeLineInstance(T owner, ITimeLine<T> timeLine, object userData = null);

        /// <summary>
        /// 中断时间轴实例。
        /// </summary>
        /// <param name="timeLineInstance">时间轴实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        void BreakTimeLineInstance(ITimeLineInstance<T> timeLineInstance, object userData = null);

        /// <summary>
        /// 中断某个持有者的所有时间轴实例。
        /// </summary>
        /// <param name="owner">时间轴实例持有者。</param>
        /// <param name="userData">用户自定义数据。</param>
        void BreakTimeLineInstances(T owner, object userData = null);

        /// <summary>
        /// 销毁时间轴实例。
        /// </summary>
        /// <param name="timeLineInstance">时间轴示例。</param>
        void DestroyTimeLineInstance(ITimeLineInstance<T> timeLineInstance);

        /// <summary>
        /// 销毁某个持有者的所有时间轴实例。
        /// </summary>
        /// <param name="owner">时间轴实例持有者。</param>
        void DestroyTimeLineInstances(T owner);

        /// <summary>
        /// 向所有时间轴实例的所有行为发送事件。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="eventId">事件编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        void FireEvent(object sender, int eventId, object userData = null);

        /// <summary>
        /// 获取时间轴。
        /// </summary>
        /// <param name="timeLineId">要获取的时间轴的编号。</param>
        /// <returns>要获取的时间轴。</returns>
        ITimeLine<T> GetTimeLine(int timeLineId);

        /// <summary>
        /// 增加时间轴。
        /// </summary>
        /// <param name="timeLine">要增加的时间轴。</param>
        /// <returns>是否增加时间轴成功。</returns>
        bool AddTimeLine(ITimeLine<T> timeLine);

        /// <summary>
        /// 移除时间轴。
        /// </summary>
        /// <param name="timeLineId">要移除的时间轴的编号。</param>
        /// <returns>是否移除时间轴成功。</returns>
        bool RemoveTimeLine(int timeLineId);

        /// <summary>
        /// 移除时间轴。
        /// </summary>
        /// <param name="timeLine">要移除的时间轴。</param>
        /// <returns>是否移除时间轴成功。</returns>
        bool RemoveTimeLine(ITimeLine<T> timeLine);
    }
}
