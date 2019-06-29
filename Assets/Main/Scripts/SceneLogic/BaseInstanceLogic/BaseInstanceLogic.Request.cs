using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class BaseInstanceLogic
    {
        protected Requests m_Requests;

        public bool HasRequests
        {
            get
            {
                return m_Requests != null;
            }
        }

        public int RequestCompleteCount
        {
            get
            {
                if (!HasRequests)
                {
                    return 0;
                }

                return m_Requests.CompleteCount;
            }
        }

        public bool IsRequestComplete(int requestId)
        {
            if (!HasRequests)
            {
                return false;
            }

            return m_Requests.IsComplete(requestId);
        }

        public bool SetRequestComplete(int requestId, bool success)
        {
            if (!HasRequests)
            {
                return false;
            }

            m_Requests.SetRequestComplete(requestId, success);
            return true;
        }

        public class Requests
        {
            private readonly HashSet<int> m_RequestComplete = new HashSet<int>();

            public int CompleteCount
            {
                get
                {
                    return m_RequestComplete.Count;
                }
            }

            public bool IsComplete(int requestId)
            {
                if (requestId > Constant.InstanceRequestCount)
                {
                    Log.Warning("Instance request id is invalid.");
                    return false;
                }

                return m_RequestComplete.Contains(requestId);
            }

            public void SetRequestComplete(int requestId, bool success)
            {
                if (requestId > Constant.InstanceRequestCount)
                {
                    Log.Warning("Instance request id is invalid.");
                    return;
                }

                if (success)
                {
                    m_RequestComplete.Add(requestId);
                }
                else
                {
                    m_RequestComplete.Remove(requestId);
                }
            }
        }
    }
}
