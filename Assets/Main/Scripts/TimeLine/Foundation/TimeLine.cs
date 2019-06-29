using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴。
    /// </summary>
    /// <typeparam name="T">时间轴持有者类型。</typeparam>
    internal class TimeLine<T> : ITimeLine<T> where T : class
    {
        private readonly int m_Id;
        private readonly LinkedList<TimeLineAction<T>> m_Actions;

        /// <summary>
        /// 初始化时间轴的新实例。
        /// </summary>
        /// <param name="timeLineId">时间轴编号。</param>
        public TimeLine(int timeLineId)
        {
            m_Id = timeLineId;
            m_Actions = new LinkedList<TimeLineAction<T>>();
        }

        /// <summary>
        /// 获取时间轴编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取时间轴持续时间。
        /// </summary>
        public float Duration
        {
            get
            {
                float maxEndTime = 0f;
                foreach (TimeLineAction<T> action in m_Actions)
                {
                    if (maxEndTime < action.EndTime)
                    {
                        maxEndTime = action.EndTime;
                    }
                }

                return maxEndTime;
            }
        }

        /// <summary>
        /// 获取时间轴行为。
        /// </summary>
        public LinkedList<TimeLineAction<T>> Actions
        {
            get
            {
                return m_Actions;
            }
        }

        /// <summary>
        /// 获取时间轴行为副本。
        /// </summary>
        /// <returns>时间轴行为副本。</returns>
        public LinkedList<TimeLineAction<T>> CloneActions()
        {
            LinkedList<TimeLineAction<T>> actions = new LinkedList<TimeLineAction<T>>();
            foreach (TimeLineAction<T> action in m_Actions)
            {
                actions.AddLast(action.Clone());
            }

            return actions;
        }

        /// <summary>
        /// 增加时间轴行为。
        /// </summary>
        /// <param name="action">要增加的时间轴行为。</param>
        /// <returns>是否增加时间轴行为成功。</returns>
        public bool AddAction(TimeLineAction<T> action)
        {
            if (action == null)
            {
                return false;
            }

            LinkedListNode<TimeLineAction<T>> current = m_Actions.First;
            while (current != null)
            {
                if (current.Value == action)
                {
                    return false;
                }

                if (current.Value.StartTime > action.StartTime)
                {
                    m_Actions.AddBefore(current, action);
                    return true;
                }

                current = current.Next;
            }

            m_Actions.AddLast(action);
            return true;
        }

        /// <summary>
        /// 移除时间轴行为。
        /// </summary>
        /// <param name="action">要移除的时间轴行为。</param>
        /// <returns>是否移除时间轴行为成功。</returns>
        public bool RemoveAction(TimeLineAction<T> action)
        {
            if (action == null)
            {
                return false;
            }

            foreach (TimeLineAction<T> i in m_Actions)
            {
                if (i == action)
                {
                    m_Actions.Remove(action);
                    return true;
                }
            }

            return false;
        }
    }
}
