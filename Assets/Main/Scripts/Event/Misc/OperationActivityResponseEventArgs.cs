using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 收到运营活动信息事件。
    /// </summary>
    public class OperationActivityResponseEventArgs : GameEventArgs
    {
        public OperationActivityResponseEventArgs(IDictionary<string, string> responseData)
        {
            m_ResponseData = responseData;
        }

        private IDictionary<string, string> m_ResponseData;

        public IDictionary<string, string> GetResponseData()
        {
            return m_ResponseData;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.OperationActivityResponse;
            }
        }
    }
}
