namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴实例接口。
    /// </summary>
    /// <typeparam name="T">时间轴持有者类型。</typeparam>
    public interface ITimeLineInstance<T> where T : class
    {
        /// <summary>
        /// 获取源时间轴编号。
        /// </summary>
        int Id
        {
            get;
        }

        /// <summary>
        /// 获取时间轴实例是否有效。
        /// </summary>
        bool IsActive
        {
            get;
        }

        /// <summary>
        /// 获取时间轴示例是否已被打断。
        /// </summary>
        bool IsBroken
        {
            get;
        }

        /// <summary>
        /// 获取时间轴实例持有者。
        /// </summary>
        T Owner
        {
            get;
        }

        /// <summary>
        /// 获取时间轴实例持续时间。
        /// </summary>
        float Duration
        {
            get;
        }

        /// <summary>
        /// 获取时间轴实例当前时间。
        /// </summary>
        float CurrentTime
        {
            get;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        object UserData
        {
            get;
        }

        /// <summary>
        /// 快进时间轴实例时间。
        /// </summary>
        /// <param name="deltaTime">时间增量。</param>
        /// <param name="ignoreMidwayAction">是否忽略跳过的事件，True为忽略，False为不忽略。</param>
        void FastForward(float deltaTime, bool ignoreMidwayAction);

        /// <summary>
        /// 时间轴实例中断。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        void Break(object userData = null);

        /// <summary>
        /// 时间轴实例销毁。
        /// </summary>
        void Destroy();

        /// <summary>
        /// 触发时间轴实例事件。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="eventId">事件编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        void FireEvent(object sender, int eventId, object userData = null);

        /// <summary>
        /// 立即触发时间轴实例事件。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="eventId">事件编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        void FireEventNow(object sender, int eventId, object userData = null);

        /// <summary>
        /// 试图获取用户自定义数据。
        /// </summary>
        /// <typeparam name="TUD">用户自定义数据类型。</typeparam>
        /// <param name="key">用户自定义数据关键字。</param>
        /// <param name="val">用户自定义数据的值。</param>
        /// <returns>是否获取成功。</returns>
        bool TryGetUserData<TUD>(string key, out TUD val);
    }
}
