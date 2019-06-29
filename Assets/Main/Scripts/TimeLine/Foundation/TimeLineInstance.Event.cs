namespace Genesis.GameClient
{
    internal partial class TimeLineInstance<T>
    {
        /// <summary>
        /// 事件结点。
        /// </summary>
        private class Event
        {
            private readonly object m_Sender;
            private readonly int m_EventId;
            private readonly object m_UserData;

            public Event(object sender, int eventId, object userData)
            {
                m_Sender = sender;
                m_EventId = eventId;
                m_UserData = userData;
            }

            public object Sender
            {
                get
                {
                    return m_Sender;
                }
            }

            public int EventId
            {
                get
                {
                    return m_EventId;
                }
            }

            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }
        }
    }
}
