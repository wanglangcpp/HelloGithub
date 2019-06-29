using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时间轴群组数据类，掌管一组逻辑上有所关联的时间轴
    /// </summary>
    public class TimeLineGroupData
    {
        private const char ColumnSplitter = '\t';
        private List<TimeLineData> m_TimeLineDataCollection = new List<TimeLineData>();

        private GameFrameworkFunc<string, TimeLineActionData> m_ActionDataCreator;

        private static StringBuilder s_SharedSb = new StringBuilder();

        private string m_GroupName;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="groupName">时间轴群组的名称</param>
        /// <param name="actionDataCreator">构造时间轴行为数据的工厂方法</param>
        public TimeLineGroupData(string groupName, GameFrameworkFunc<string, TimeLineActionData> actionDataCreator)
        {
            m_GroupName = groupName;
            m_ActionDataCreator = actionDataCreator;
        }

        /// <summary>
        /// 获取群组中的时间轴数据
        /// </summary>
        public IList<TimeLineData> TimeLines
        {
            get
            {
                return m_TimeLineDataCollection;
            }
        }

        /// <summary>
        /// 清除数据。
        /// </summary>
        public void ClearData()
        {
            m_TimeLineDataCollection.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rawText"></param>
        /// <returns>Error message if any. Otherwise an empty string.</returns>
        public string ParseData(string rawText)
        {
            int timeLineId = 0;
            int timeLineActionCount = 0;
            IList<string[]> timeLineActions = new List<string[]>();

            string[] rowTexts = Utility.Text.SplitToLines(rawText);
            bool readHedaer = true;

            try
            {
                //每一行数据
                for (int row = 0; row < rowTexts.Length; row++)
                {
                    string rowText = rowTexts[row];
                    if (rowText.Length <= 0 || rowText[0] == '#')
                    {
                        continue;
                    }
                    //被制表符分开的字段
                    string[] splitLine = rowText.Split(ColumnSplitter);

                    if (readHedaer)
                    {
                        if (splitLine.Length < 2)
                        {
                            return string.Format("Can not load time line '{0}' ('{1}') at row '{2}'.", m_GroupName, row.ToString(), rowText);
                        }

                        timeLineId = int.Parse(splitLine[0]);
                        timeLineActionCount = int.Parse(splitLine[1]);
                        timeLineActions.Clear();

                        if (timeLineActionCount <= 0)
                        {
                            return string.Format("Can not load time line '{0}' ('{1}') at line '{2}' which action is empty.", m_GroupName, row.ToString(), rowText);
                        }

                        readHedaer = false;
                    }
                    else
                    {
                        if (splitLine.Length < 1)
                        {
                            return string.Format("Can not load time line '{0}' ('{1}') at row '{2}'.", m_GroupName, row.ToString(), rowText);
                        }

                        timeLineActions.Add(splitLine);
                        if (timeLineActions.Count >= timeLineActionCount)
                        {
                            if (!BuildTimeLine(m_GroupName, timeLineId, timeLineActions))
                            {
                                return string.Format("Can not build time line '{0}' when time line id '{1}', maybe duplicated.", m_GroupName, timeLineId.ToString());
                            }

                            readHedaer = true;
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception exception)
            {
                return string.Format("Can not load time line '{0}' at time line id '{1}', action count '{2}' with exception '{3}'.", m_GroupName, timeLineId.ToString(), timeLineActionCount.ToString(), exception.Message);
            }
        }

        /// <summary>
        /// 序列化数据。
        /// </summary>
        /// <returns></returns>
        public string[] SerializeData()
        {
            List<string> lines = new List<string>();
            foreach (var timeLineData in m_TimeLineDataCollection)
            {
                if (timeLineData.Actions.Count <= 0)
                {
                    continue;
                }

                s_SharedSb.Length = 0;
                s_SharedSb.Append(timeLineData.Id);
                s_SharedSb.Append(ColumnSplitter);
                s_SharedSb.Append(timeLineData.Actions.Count);
                lines.Add(s_SharedSb.ToString());

                foreach (var actionData in timeLineData.Actions)
                {
                    lines.Add(string.Join(ColumnSplitter.ToString(), actionData.SerializeData()));
                }
            }

            return lines.ToArray();
        }

        private bool BuildTimeLine(string timeLineName, int timeLineId, IList<string[]> timeLineActionArgsArray)
        {
            TimeLineData timeLine = new TimeLineData(timeLineId);
            for (int i = 0; i < timeLineActionArgsArray.Count; i++)
            {
                try
                {
                    var actionData = m_ActionDataCreator(timeLineActionArgsArray[i][0]);
                    actionData.ParseData(timeLineActionArgsArray[i]);
                    timeLine.Actions.Add(actionData);
                }
                catch (Exception exception)
                {
                    throw new Exception(string.Format("Can not add action data at line '{0}' with exception '{1}\n{2}'.", i.ToString(), exception.Message, exception.StackTrace), exception);
                }
            }

            m_TimeLineDataCollection.Add(timeLine);
            return true;
        }
    }
}
