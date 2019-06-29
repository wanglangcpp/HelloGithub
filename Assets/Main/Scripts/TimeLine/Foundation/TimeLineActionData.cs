namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴行为的数据基类。
    /// </summary>
    public abstract class TimeLineActionData
    {
        /// <summary>
        /// 时间轴行为的开始时间。
        /// </summary>
        protected float m_StartTime;

        /// <summary>
        /// 时间轴行为的持续时间。
        /// </summary>
        protected float m_Duration;

        /// <summary>
        /// 类型码，宜与具体实现类的名称一致。
        /// </summary>
        public abstract string ActionType
        {
            get;
        }

        /// <summary>
        /// 获取时间轴行为的开始时间。
        /// </summary>
        public float StartTime
        {
            get
            {
                return m_StartTime;
            }
        }

        /// <summary>
        /// 获取时间轴行为的持续时间。
        /// </summary>
        public float Duration
        {
            get
            {
                return m_Duration;
            }
        }

        /// <summary>
        /// 向字符串数组中序列化数据。
        /// </summary>
        /// <returns></returns>
        public abstract string[] SerializeData();

        /// <summary>
        /// 从字符串数组中解析数据。
        /// </summary>
        /// <param name="timeLineActionArgs"></param>
        public abstract void ParseData(string[] timeLineActionArgs);
    }
}
