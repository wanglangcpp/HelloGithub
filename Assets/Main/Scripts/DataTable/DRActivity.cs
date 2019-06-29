using GameFramework.DataTable;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 活动配置表。
    /// </summary>
    public class DRActivity : IDataRow
    {
        private const int DaysPerWeek = 7;
        private const int ActivePeriodMaxCount = 3;

        /// <summary>
        /// 活动编号（即类型）。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 活动背景图编号。
        /// </summary>
        public int TextureId
        {
            get;
            private set;
        }

        /// <summary>
        /// 解锁等级。
        /// </summary>
        public int UnlockLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 排序优先级。数值较小者显示靠前。
        /// </summary>
        public int OrderPrioprity
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否显示。用于直接关闭活动入口。
        /// </summary>
        public bool ShouldDisplay
        {
            get;
            private set;
        }

        /// <summary>
        /// 开启时间描述。
        /// </summary>
        public string ActiveTimeDesc
        {
            get;
            private set;
        }

        /// <summary>
        /// 按星期开启标记。
        /// </summary>
        public bool[] ActiveOnWeekDays
        {
            get;
            private set;
        }

        /// <summary>
        /// 各段开启时间。
        /// </summary>
        public TimeSpan[] StartTimes
        {
            get;
            private set;
        }

        /// <summary>
        /// 各段结束时间。
        /// </summary>
        public TimeSpan[] EndTimes
        {
            get;
            private set;
        }

        /// <summary>
        /// 挑战次数用完时是否允许进入功能。
        /// </summary>
        public bool AllowOpenOnNoRemainingPlayCount
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] rowData = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(rowData[index++]);
            index++;
            Name = rowData[index++];
            Description = rowData[index++];
            TextureId = int.Parse(rowData[index++]);
            UnlockLevel = int.Parse(rowData[index++]);
            OrderPrioprity = int.Parse(rowData[index++]);
            ShouldDisplay = bool.Parse(rowData[index++]);
            ActiveTimeDesc = rowData[index++];

            ActiveOnWeekDays = new bool[DaysPerWeek];
            for (int i = 0; i < ActiveOnWeekDays.Length; i++)
            {
                ActiveOnWeekDays[i] = bool.Parse(rowData[index++]);
            }

            var startTimes = new List<TimeSpan>();
            var endTimes = new List<TimeSpan>();

            int lastIndex = index;
            for (int i = 0; i < ActivePeriodMaxCount; i++)
            {
                var startTimeText = rowData[index++];
                if (string.IsNullOrEmpty(startTimeText))
                {
                    break;
                }

                startTimes.Add(TimeSpan.Parse(startTimeText));
                endTimes.Add(TimeSpan.Parse(rowData[index++]));
            }
            index = lastIndex + 2 * ActivePeriodMaxCount;

            StartTimes = startTimes.ToArray();
            EndTimes = endTimes.ToArray();

            AllowOpenOnNoRemainingPlayCount = bool.Parse(rowData[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRActivity>();
        }
    }
}
