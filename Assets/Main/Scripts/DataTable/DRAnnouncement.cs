using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 系统通知配置表。
    /// </summary>
    public class DRAnnouncement : IDataRow
    {
        /// <summary>
        /// 系统通知编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 系统通知内容。
        /// </summary>
        public string AnnouncementMessage { get; private set; }

        /// <summary>
        /// 是否主界面推送显示。
        /// </summary>
        public bool NeedScroll { get; private set; }

        /// <summary>
        /// 消息滚动的优先级。
        /// </summary>
        public int Priority { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            AnnouncementMessage = text[index++];
            NeedScroll = bool.Parse(text[index++]);
            Priority = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRAnnouncement>();
        }
    }
}
