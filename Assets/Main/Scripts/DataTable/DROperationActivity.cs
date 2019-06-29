using GameFramework.DataTable;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 运营活动配置表。
    /// </summary>
    public class DROperationActivity : IDataRow
    {
        /// <summary>
        /// 活动编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 活动名称。
        /// </summary>
        public string ActivityName { get; private set; }

        /// <summary>
        /// 活动描述。
        /// </summary>
        public string ActivityDesc { get; private set; }

        /// <summary>
        /// 活动图标编号。
        /// </summary>
        public int ActivityIconId { get; private set; }

        /// <summary>
        /// 活动界面路径。
        /// </summary>
        public string ActivityUIPath { get; private set; }

        /// <summary>
        /// 每天首次登录是否弹出。
        /// </summary>
        public bool AutoShow { get; private set; }

        /// <summary>
        /// 活动开启时间。
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// 活动结束时间。
        /// </summary>
        public DateTime EndTime { get; private set; }

        /// <summary>
        /// 处理逻辑名称。(仅服务器使用。)
        /// </summary>
        public string ProcessorName { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            ActivityName = text[index++];
            ActivityDesc = text[index++];
            ActivityIconId = int.Parse(text[index++]);
            ActivityUIPath = text[index++];
            AutoShow = bool.Parse(text[index++]);
            StartTime = DateTime.Parse(text[index++]);
            EndTime = DateTime.Parse(text[index++]);
            ProcessorName = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DROperationActivity>();
        }
    }
}
