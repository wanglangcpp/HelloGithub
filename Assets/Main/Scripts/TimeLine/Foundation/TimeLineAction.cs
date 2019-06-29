namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴行为。
    /// </summary>
    /// <typeparam name="T">时间轴持有者类型。</typeparam>
    public abstract class TimeLineAction<T> where T : class
    {
        private readonly TimeLineActionData m_TimeLineActionData;

        /// <summary>
        /// 初始化时间轴行为的新实例。
        /// </summary>
        /// <param name="timeLineActionData">时间轴行为的数据。</param>
        public TimeLineAction(TimeLineActionData timeLineActionData)
        {
            if (timeLineActionData == null)
            {
                throw new System.ArgumentNullException("timeLineActionData");
            }

            m_TimeLineActionData = timeLineActionData;
        }

        /// <summary>
        /// 获取时间轴行为的开始时间。
        /// </summary>
        public float StartTime
        {
            get
            {
                return m_TimeLineActionData.StartTime;
            }
        }

        /// <summary>
        /// 获取时间轴行为的持续时间。
        /// </summary>
        public float Duration
        {
            get
            {
                return m_TimeLineActionData.Duration;
            }
        }

        /// <summary>
        /// 获取时间轴行为的结束时间。
        /// </summary>
        internal float EndTime
        {
            get
            {
                return StartTime + Duration;
            }
        }

        /// <summary>
        /// 创建时间轴行为的浅表副本。
        /// </summary>
        /// <returns>时间轴行为的浅表副本。</returns>
        internal TimeLineAction<T> Clone()
        {
            return MemberwiseClone() as TimeLineAction<T>;
        }

        /// <summary>
        /// 时间轴行为开始。
        /// </summary>
        /// <param name="timeLineInstance">时间轴实例。</param>
        public virtual void OnStart(ITimeLineInstance<T> timeLineInstance)
        {

        }

        /// <summary>
        /// 时间轴行为结束。
        /// </summary>
        /// <param name="timeLineInstance">时间轴实例。</param>
        public virtual void OnFinish(ITimeLineInstance<T> timeLineInstance)
        {

        }

        /// <summary>
        /// 时间轴行为轮询。
        /// </summary>
        /// <param name="timeLineInstance">时间轴实例。</param>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        public virtual void OnUpdate(ITimeLineInstance<T> timeLineInstance, float elapseSeconds)
        {

        }

        /// <summary>
        /// 时间轴行为中断。
        /// </summary>
        /// <param name="timeLineInstance">时间轴实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnBreak(ITimeLineInstance<T> timeLineInstance, object userData)
        {

        }

        /// <summary>
        /// 时间轴行为事件。
        /// </summary>
        /// <param name="timeLineInstance">时间轴实例。</param>
        /// <param name="sender">事件源。</param>
        /// <param name="eventId">事件编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnEvent(ITimeLineInstance<T> timeLineInstance, object sender, int eventId, object userData)
        {

        }

        /// <summary>
        /// 时间轴行为调试绘制。
        /// </summary>
        /// <param name="timeLineInstance">时间轴实例。</param>
        public virtual void OnDebugDraw(ITimeLineInstance<T> timeLineInstance)
        {

        }

        /// <summary>
        /// 快进至本行为结束。
        /// </summary>
        /// <returns>快进时间量。</returns>
        protected float FastForwardSelf(ITimeLineInstance<T> timeLineInstance)
        {
            float amount = StartTime + Duration - timeLineInstance.CurrentTime;
            if (amount <= 0f)
            {
                return 0f;
            }

            timeLineInstance.FastForward(amount, false);
            return amount;
        }
    }
}
