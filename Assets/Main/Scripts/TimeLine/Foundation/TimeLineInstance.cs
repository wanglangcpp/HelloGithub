using GameFramework;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴实例。
    /// </summary>
    /// <typeparam name="T">时间轴持有者类型。</typeparam>
    internal partial class TimeLineInstance<T> : ITimeLineInstance<T> where T : class
    {
        private readonly int m_Id;
        private readonly T m_Owner;
        private readonly float m_Duration;
        private readonly LinkedList<TimeLineAction<T>> m_Actions;
        private readonly LinkedList<TimeLineAction<T>> m_RunningActions;
        private readonly Queue<Event> m_Events;
        private readonly object m_UserData;
        private bool m_Active;
        private bool m_Broken;
        private object m_BreakUserData;
        private float m_CurrentTime;
        private bool m_IgnoreMidwayActionOnFastForward;

        private LinkedListNode<TimeLineAction<T>> m_CurrentAction;

        private const int InfiniteLoopCount = 100;
        private const float ActionFaultTolerantTime = 0.1f; // 时间轴上的事件计算截止时间的容错时间
        /// <summary>
        /// 初始化时间轴实例的新实例。
        /// </summary>
        /// <param name="owner">时间轴实例持有者。</param>
        /// <param name="timeLine">源时间轴。</param>
        /// <param name="userData">用户自定义数据。</param>
        public TimeLineInstance(T owner, TimeLine<T> timeLine, object userData)
        {
            if (owner == null)
            {
                throw new System.ArgumentNullException("owner");
            }

            if (timeLine == null)
            {
                throw new System.ArgumentNullException("timeLine");
            }

            m_Id = timeLine.Id;
            m_Owner = owner;
            m_Duration = timeLine.Duration;
            m_Actions = timeLine.CloneActions();
            m_RunningActions = new LinkedList<TimeLineAction<T>>();
            m_Events = new Queue<Event>();
            m_UserData = userData;
            m_Active = true;
            m_Broken = false;
            m_BreakUserData = null;
            m_CurrentTime = 0f;
            m_CurrentAction = m_Actions.First;
            RecordLog("Time line ctor.");
        }

        /// <summary>
        /// 获取源时间轴编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取时间轴实例是否有效。
        /// </summary>
        public bool IsActive
        {
            get
            {
                return m_Active;
            }
        }

        /// <summary>
        /// 获取时间轴示例是否已经被打断。
        /// </summary>
        public bool IsBroken
        {
            get
            {
                return m_Broken;
            }
        }

        /// <summary>
        /// 获取时间轴实例持有者。
        /// </summary>
        public T Owner
        {
            get
            {
                return m_Owner;
            }
        }

        /// <summary>
        /// 获取时间轴实例持续时间。
        /// </summary>
        public float Duration
        {
            get
            {
                return m_Duration;
            }
        }

        /// <summary>
        /// 获取时间轴实例当前时间。
        /// </summary>
        public float CurrentTime
        {
            get
            {
                return m_CurrentTime;
            }
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        /// <summary>
        /// 初始化时间轴实例。
        /// </summary>
        /// <returns>时间轴实例是否已完成。</returns>
        public void Init()
        {
            ProcessInstance(0f);
            RecordLog("Time line inited.");
        }

        /// <summary>
        /// 时间轴实例轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">当前已流逝时间，以秒为单位。</param>
        /// <returns>时间轴实例是否已完成。</returns>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            m_CurrentTime += elapseSeconds;
            ProcessInstance(elapseSeconds);
        }

        /// <summary>
        /// 时间轴实例销毁。
        /// </summary>
        public void Destroy()
        {
            m_Broken = true;
            CallOnBreak(null);
            m_Active = false;
            RecordLog("Time line destroyed.");
        }

        /// <summary>
        /// 时间轴实例中断。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void Break(object userData)
        {
            if (!m_Active)
            {
                throw new System.NotSupportedException("Time line instance is not active.");
            }

            RecordLog("Time line broken.");
            m_Broken = true;
            m_BreakUserData = userData;
        }

        /// <summary>
        /// 触发时间轴实例事件。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="eventId">事件编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void FireEvent(object sender, int eventId, object userData)
        {
            if (!CanFireEvent(sender, eventId, userData))
            {
                return;
            }

            m_Events.Enqueue(new Event(sender, eventId, userData));
        }

        /// <summary>
        /// 立即触发时间轴实例事件。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="eventId">事件编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void FireEventNow(object sender, int eventId, object userData = null)
        {
            if (!CanFireEvent(sender, eventId, userData))
            {
                return;
            }

            ProcessEvent(new Event(sender, eventId, userData));
        }

        /// <summary>
        /// 快进时间轴实例时间。
        /// </summary>
        /// <param name="deltaTime">时间增量。</param>
        /// <param name="ignoreMidwayAction">是否忽略跳过的事件，True为忽略，False为不忽略。忽略的含义是调用Break和Finish</param>
        public void FastForward(float deltaTime, bool ignoreMidwayAction)
        {
            if (deltaTime < 0)
            {
                //In time line system, Fast forward function only allow forward, Backward is invalid.
                return;
            }

            m_IgnoreMidwayActionOnFastForward = ignoreMidwayAction;
            m_CurrentTime += deltaTime;
        }

        /// <summary>
        /// 时间轴实例调试绘制。
        /// </summary>
        public void DebugDraw()
        {
            foreach (TimeLineAction<T> action in m_RunningActions)
            {
                action.OnDebugDraw(this);
            }
        }

        /// <summary>
        /// 处理时间轴实例。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        private void ProcessInstance(float elapseSeconds)
        {
            UpdateRunningActions(elapseSeconds);
            DealWithNewActions();
            ProcessEvents();

            if (!m_Active)
            {
                RecordLog("Oops, time line becomes inactive.");
                return;
            }

            StopActions();
            DeactivateIfNeeded();
        }

        private void DeactivateIfNeeded()
        {
            if (m_Broken || m_CurrentTime >= m_Duration)
            {
                m_Active = false;
                RecordLog("Time line deactivated.");
            }
        }

        private void DealWithNewActions()
        {
            while (m_CurrentAction != null && m_CurrentTime >= m_CurrentAction.Value.StartTime)
            {
                if (m_IgnoreMidwayActionOnFastForward && m_CurrentTime > (m_CurrentAction.Value.EndTime + ActionFaultTolerantTime))
                {
                    // 不做任何事，自然跳过了当前的 Action。
                }
                else
                {
                    m_CurrentAction.Value.OnStart(this);
                    m_RunningActions.AddLast(m_CurrentAction.Value);
                }

                m_CurrentAction = m_CurrentAction.Next;
            }

            m_IgnoreMidwayActionOnFastForward = false;
        }

        private void UpdateRunningActions(float elapseSeconds)
        {
            foreach (TimeLineAction<T> timeLineAction in m_RunningActions)
            {
                timeLineAction.OnUpdate(this, elapseSeconds);
            }
        }

        private void StopActions()
        {
            if (m_Broken)
            {
                CallOnBreak(m_BreakUserData);
            }
            else
            {
                LinkedListNode<TimeLineAction<T>> current = m_RunningActions.First;
                while (current != null)
                {
                    if (m_CurrentTime >= current.Value.EndTime)
                    {
                        LinkedListNode<TimeLineAction<T>> next = current.Next;
                        m_RunningActions.Remove(current);
                        RecordLog("Before call on finish on: {0}", current.Value);
                        current.Value.OnFinish(this);
                        RecordLog("After call on finish on: {0}", current.Value);
                        current = next;
                        continue;
                    }

                    current = current.Next;
                }
            }
        }

        private void ProcessEvents()
        {
            int loopCount = 0;
            bool loggedInfiniteLoop = false;
            while (m_Active && !m_Broken && m_Events.Count > 0)
            {
                Event e = m_Events.Dequeue();
                ProcessEvent(e);

                ++loopCount;
                if (!loggedInfiniteLoop && loopCount >= InfiniteLoopCount)
                {
                    RecordLog("Too many events in time line instance. ");
                    loggedInfiniteLoop = true;
                    var errorMessage = string.Format("Too many events in time line instance. Time line ID is '{0}', Event being processed is '{1}', user data is '{2}.",
                        m_Id.ToString(), e.EventId.ToString(), e.UserData == null ? "<null>" : e.UserData.ToString());

#if UNITY_EDITOR
                    throw new System.Exception(errorMessage);
#else
                    BuglyAgent.ReportException("CustomError", errorMessage, new System.Diagnostics.StackTrace().ToString());
#endif
                }
            }
        }

        private void ProcessEvent(Event e)
        {
            RecordLog("Begin ProcessEvent: {0}", e.EventId.ToString());

            foreach (TimeLineAction<T> action in m_RunningActions)
            {
                if (!m_Active || m_Broken)
                {
                    break;
                }

                action.OnEvent(this, e.Sender, e.EventId, e.UserData);
            }

            RecordLog("End ProcessEvent: {0}", e.EventId.ToString());
        }

        private void CallOnBreak(object userData)
        {
            RecordLog("Begin CallOnBreak");

            for (var node = m_RunningActions.First; node != null; node = node.Next)
            {
                node.Value.OnBreak(this, userData);
            }

            RecordLog("End CallOnBreak");
        }

        private bool CanFireEvent(object sender, int eventId, object userData)
        {
            if (!m_Active)
            {
                StringBuilder logSb = new StringBuilder();
                logSb.AppendFormat("Time line instance has invalid state: m_Active is '{0}' and m_Broken is '{1}'. ", m_Active.ToString(), m_Broken.ToString());
                var characterOwner = sender as Character;
                if (characterOwner != null)
                {
                    var motion = characterOwner.Motion;
                    logSb.AppendFormat("Owner is '{0} ({1}, {2})'. ", characterOwner.GetType().Name, characterOwner.Name, motion == null ? "<null>" : motion.CurrentStateName);
                }
                else
                {
                    logSb.AppendFormat("Owner is '{0}'. ", sender == null ? "<null>" : sender.GetType().Name);
                }

                logSb.AppendFormat("Event ID is '{0}'. ", (EntityTimeLineEvent)eventId);

                Log.Warning(logSb.ToString());
                return false;
            }

            if (m_Broken)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 试图获取用户自定义数据。
        /// </summary>
        /// <typeparam name="TUD">用户自定义数据类型。</typeparam>
        /// <param name="key">用户自定义数据关键字。</param>
        /// <param name="val">用户自定义数据的值。</param>
        /// <returns>是否获取成功。</returns>
        public bool TryGetUserData<TUD>(string key, out TUD val)
        {
            var userDataDict = UserData as Dictionary<string, object>;

            if (userDataDict == null)
            {
                val = default(TUD);
                return false;
            }

            object raw;
            if (!userDataDict.TryGetValue(key, out raw) || !(raw is TUD))
            {
                val = default(TUD);
                return false;
            }

            val = (TUD)raw;
            return true;
        }

        private const string LogFile = "TimeLineLog.txt";

        private static string LogFilePath
        {
            get
            {
                return Utility.Path.GetCombinePath(GameEntry.Resource.ReadWritePath, LogFile);
            }
        }

        private static bool s_HasWrittenLog = false;

        [System.Diagnostics.Conditional("RECORD_TIME_LINE_LOG")]
        private void RecordLog(string logFormat, params object[] args)
        {
            string logMessage = string.Format(logFormat, args);
            logMessage = string.Format("[TimeLine {0} Id={1} HashCode={2}] {3}\n", DateTime.Now.ToString("hh:mm:ss.fff"), m_Id.ToString(), GetHashCode(), logMessage);

            if (s_HasWrittenLog)
            {
                File.AppendAllText(LogFilePath, logMessage);
            }
            else
            {
                s_HasWrittenLog = true;
                File.WriteAllText(LogFilePath, logMessage);
            }
        }
    }
}
