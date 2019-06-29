using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴管理器。
    /// </summary>
    /// <typeparam name="T">时间轴持有者类型。</typeparam>
    internal class TimeLineManager<T> : ITimeLineManager<T> where T : class
    {
        private IList<TimeLine<T>> m_TimeLines;
        private LinkedList<TimeLineInstance<T>> m_TimeLineInstances;

        /// <summary>
        /// 初始化时间轴管理器的新实例。
        /// </summary>
        public TimeLineManager()
        {
            m_TimeLines = new List<TimeLine<T>>();
            m_TimeLineInstances = new LinkedList<TimeLineInstance<T>>();
        }

        /// <summary>
        /// 获取时间轴数量。
        /// </summary>
        public int TimeLineCount
        {
            get
            {
                return m_TimeLines.Count;
            }
        }

        /// <summary>
        /// 获取时间轴实例数量。
        /// </summary>
        public int TimeLineInstanceCount
        {
            get
            {
                return m_TimeLineInstances.Count;
            }
        }

        /// <summary>
        /// 时间轴管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">当前已流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            LinkedListNode<TimeLineInstance<T>> current = m_TimeLineInstances.First;
            while (current != null)
            {
                if (!current.Value.IsActive)
                {
                    LinkedListNode<TimeLineInstance<T>> next = current.Next;
                    m_TimeLineInstances.Remove(current);
                    current = next;
                    continue;
                }

                current.Value.Update(elapseSeconds, realElapseSeconds);
                current = current.Next;
            }
        }

        /// <summary>
        /// 时间轴管理器调试绘制。
        /// </summary>
        public void DebugDraw()
        {
            foreach (TimeLineInstance<T> timeLineInstance in m_TimeLineInstances)
            {
                timeLineInstance.DebugDraw();
            }
        }

        /// <summary>
        /// 关闭并清理时间轴管理器。
        /// </summary>
        public void Shutdown()
        {
            ClearAllTimeInstances();
            m_TimeLines.Clear();
        }

        /// <summary>
        /// 清理所有时间轴实例。
        /// </summary>
        public void ClearAllTimeInstances()
        {
            foreach (TimeLineInstance<T> timeLineInstance in m_TimeLineInstances)
            {
                if (!timeLineInstance.IsBroken && timeLineInstance.IsActive)
                {
                    timeLineInstance.Destroy();
                }
            }

            m_TimeLineInstances.Clear();
        }

        /// <summary>
        /// 创建时间轴实例。
        /// </summary>
        /// <param name="owner">时间轴实例持有者。</param>
        /// <param name="timeLineId">源时间轴编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的时间轴实例。</returns>
        public ITimeLineInstance<T> CreateTimeLineInstance(T owner, int timeLineId, object userData)
        {
            ITimeLine<T> timeLine = GetTimeLine(timeLineId);
            if (timeLine == null)
            {
                throw new System.ArgumentException(string.Format("Can not create time line instance while time line id '{0}' is invalid.", timeLineId.ToString()));
            }

            return CreateTimeLineInstance(owner, timeLine, userData);
        }

        /// <summary>
        /// 创建时间轴实例。
        /// </summary>
        /// <param name="owner">时间轴实例持有者。</param>
        /// <param name="timeLine">源时间轴。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的时间轴实例。</returns>
        public ITimeLineInstance<T> CreateTimeLineInstance(T owner, ITimeLine<T> timeLine, object userData)
        {
            if (owner == null)
            {
                throw new System.ArgumentNullException("owner.");
            }

            TimeLine<T> timeLineImplement = timeLine as TimeLine<T>;
            if (timeLine == null)
            {
                throw new System.ArgumentException("Time line is invalid.");
            }

            TimeLineInstance<T> timeLineInstance = new TimeLineInstance<T>(owner, timeLineImplement, userData);
            if (timeLineInstance == null)
            {
                throw new System.ArgumentException("Can not create time line instance.");
            }

            timeLineInstance.Init();
            if (timeLineInstance.IsActive)
            {
                m_TimeLineInstances.AddLast(timeLineInstance);
            }

            return timeLineInstance;
        }

        /// <summary>
        /// 中断时间轴实例。
        /// </summary>
        /// <param name="timeLineInstance">时间轴实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void BreakTimeLineInstance(ITimeLineInstance<T> timeLineInstance, object userData = null)
        {
            if (timeLineInstance == null)
            {
                throw new System.ArgumentNullException("timeLineInstance");
            }

            timeLineInstance.Break(userData);
        }

        /// <summary>
        /// 中断某个持有者的所有时间轴实例。
        /// </summary>
        /// <param name="owner">时间轴实例持有者。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void BreakTimeLineInstances(T owner, object userData = null)
        {
            foreach (TimeLineInstance<T> timeLineInstance in m_TimeLineInstances)
            {
                if (timeLineInstance.Owner == owner && !timeLineInstance.IsBroken && timeLineInstance.IsActive)
                {
                    timeLineInstance.Break(userData);
                }
            }
        }

        /// <summary>
        /// 销毁时间轴实例。
        /// </summary>
        /// <param name="timeLineInstance">时间轴示例。</param>
        public void DestroyTimeLineInstance(ITimeLineInstance<T> timeLineInstance)
        {
            if (timeLineInstance == null)
            {
                throw new System.ArgumentNullException("timeLineInstance");
            }

            timeLineInstance.Destroy();
        }

        /// <summary>
        /// 销毁某个持有者的所有时间轴实例。
        /// </summary>
        /// <param name="owner">时间轴实例持有者。</param>
        public void DestroyTimeLineInstances(T owner)
        {
            foreach (TimeLineInstance<T> timeLineInstance in m_TimeLineInstances)
            {
                if (timeLineInstance.Owner == owner && !timeLineInstance.IsBroken && timeLineInstance.IsActive)
                {
                    timeLineInstance.Destroy();
                }
            }
        }

        /// <summary>
        /// 向所有时间轴实例的所有行为发送事件。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="eventId">事件编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void FireEvent(object sender, int eventId, object userData)
        {
            foreach (TimeLineInstance<T> timeLineInstance in m_TimeLineInstances)
            {
                timeLineInstance.FireEvent(sender, eventId, userData);
            }
        }

        /// <summary>
        /// 获取时间轴。
        /// </summary>
        /// <param name="timeLineId">要获取的时间轴的编号。</param>
        /// <returns>要获取的时间轴。</returns>
        public ITimeLine<T> GetTimeLine(int timeLineId)
        {
            foreach (TimeLine<T> timeLine in m_TimeLines)
            {
                if (timeLine.Id == timeLineId)
                {
                    return timeLine;
                }
            }

            return null;
        }

        /// <summary>
        /// 增加时间轴。
        /// </summary>
        /// <param name="timeLine">要增加的时间轴。</param>
        /// <returns>是否增加时间轴成功。</returns>
        public bool AddTimeLine(ITimeLine<T> timeLine)
        {
            TimeLine<T> timeLineImplement = timeLine as TimeLine<T>;
            if (timeLineImplement == null)
            {
                return false;
            }

            if (GetTimeLine(timeLineImplement.Id) != null)
            {
                return false;
            }

            m_TimeLines.Add(timeLineImplement);

            return true;
        }

        /// <summary>
        /// 移除时间轴。
        /// </summary>
        /// <param name="timeLineId">要移除的时间轴的编号。</param>
        /// <returns>是否移除时间轴成功。</returns>
        public bool RemoveTimeLine(int timeLineId)
        {
            return RemoveTimeLine(GetTimeLine(timeLineId));
        }

        /// <summary>
        /// 移除时间轴。
        /// </summary>
        /// <param name="timeLine">要移除的时间轴。</param>
        /// <returns>是否移除时间轴成功。</returns>
        public bool RemoveTimeLine(ITimeLine<T> timeLine)
        {
            TimeLine<T> timeLineImplement = timeLine as TimeLine<T>;
            if (timeLineImplement == null)
            {
                return false;
            }

            return m_TimeLines.Remove(timeLineImplement);
        }
    }
}
