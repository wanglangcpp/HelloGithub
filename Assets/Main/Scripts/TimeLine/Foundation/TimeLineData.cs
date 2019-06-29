using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴数据类
    /// </summary>
    public sealed class TimeLineData
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取时间轴编号
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        private List<TimeLineActionData> m_ActionDataCollection = new List<TimeLineActionData>();

        /// <summary>
        /// 获取本时间轴下的行为
        /// </summary>
        public IList<TimeLineActionData> Actions
        {
            get
            {
                return m_ActionDataCollection;
            }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="id"></param>
        public TimeLineData(int id)
        {
            m_Id = id;
        }
    }
}
