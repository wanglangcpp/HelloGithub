using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;

namespace Genesis.GameClient
{
    public class TaskStepData : IGenericData<TaskStepData, TaskItemInfoData>
    {
        public int Key
        {
            get { return m_Id; }
        }
        private int m_Id;

        private TaskItemInfoData m_TasksItemData = null;

        public TaskItemInfoData TasksItemData { get { return m_TasksItemData; } }

        public int Activeness { get { return m_Activeness; } }
        public int m_Activeness = 0;

        /// <summary>
        /// 任务关系字典（当前任务，前置任务）
        /// </summary>
        public Dictionary<int, int> taskMap { get; private set; }
        /// <summary>
        /// 当前完成的任务列表
        /// </summary>
        public List<int> CurrentFinishTasksId
        {
            get
            {
                InitData();
                return currentFinishTaskId;
            }
        }
        private List<int> currentFinishTaskId = null;

        private IDataTable<DRTask> dataTable = null;


        private void InitData()
        {
            if (currentFinishTaskId != null)
            {
                return;
            }
            dataTable = GameEntry.DataTable.GetDataTable<DRTask>();
            currentFinishTaskId = new List<int>();
            taskMap = new Dictionary<int, int>();
            if (taskMap.Count == 0)
            {
                for (int i = 0; i < dataTable.GetAllDataRows().Length; i++)
                {
                    int taskID = dataTable.GetAllDataRows()[i].Id;
                    string aferTask = dataTable.GetAllDataRows()[i].AfterTask;
                    if (!aferTask.Equals("null"))
                    {
                        string[] per = aferTask.Split(new string[] { "|" }, StringSplitOptions.None);
                        for (int j = 0; j < per.Length; j++)
                        {
                            int tmpID = int.Parse(per[j]);
                            if (!taskMap.ContainsKey(tmpID)) taskMap.Add(tmpID, taskID);
                            else Log.Error("OpenFunctionsComponent repeatTaskID:" + tmpID);
                        }
                    }
                }
            }
            if (m_TasksItemData != null && m_TasksItemData.CurrentTaskListId.Count > 0)
            {
                for (int i = 0; i < m_TasksItemData.CurrentTaskListId.Count; i++)
                {
                    CheckTask(m_TasksItemData.CurrentTaskListId[i]);
                }
            }
        }
        /// <summary>
        /// 递归添加完成的任务
        /// </summary>
        /// <param name="checkId"></param>
        private void CheckTask(int checkId)
        {
            if (taskMap.ContainsKey(checkId))
            {
                int finishId = taskMap[checkId];
                if (CurrentFinishTasksId.Contains(finishId))
                {
                    return;
                }
                CurrentFinishTasksId.Add(finishId);
                if (finishId == dataTable.MinIdDataRow.Id)
                {
                    return;
                }
                CheckTask(finishId);
            }
        }

        public void AddFinishedTaskId(int taskId)
        {
            if (!CurrentFinishTasksId.Contains(taskId))
            {
                CurrentFinishTasksId.Add(taskId);
            }
        }

        public void UpdateData(TaskItemInfoData data)
        {
            Log.Debug("刷新任务信息......");
            m_TasksItemData = data;
            m_Activeness = data.Activeness;
            InitData();
        }
    }
    public class TaskStep
    {
        public DRTask Task;
        public bool IsFinish;
        public int Step;
        public long ClaimRewardTime;
        public long LastUpdateTime;
    }

    public class TaskItemInfoData
    {
        public int Activeness;
        public List<TaskStep> CurrentTaskList = new List<TaskStep>(0);

        /// <summary>
        /// 所有进行中的任务
        /// </summary>
        public List<int> CurrentTaskListId = new List<int>();

        public List<TaskStep> GetTasksInfo(TaskListType taskType)
        {
            List<TaskStep> taskList = new List<TaskStep>();
            foreach (var item in CurrentTaskList)
            {
                int type = item.Task.TaskType;
                TaskListType currentType = GetFunctionTypeInTask(type);
                if (currentType == taskType)
                {
                    //不是每日任务就直接添加服务器发过来的列表
                    if (taskType != TaskListType.Daily)
                    {
                        if (!taskList.Contains(item))
                        {
                            taskList.Add(item);
                        }
                    }
                    else
                    {
                        //上次领奖时间
                        long claimTime = item.ClaimRewardTime;
                        System.DateTime m_ClaimTime = new System.DateTime(claimTime, System.DateTimeKind.Utc);

                        //上次更新任务时间
                        long lastTime = item.LastUpdateTime;
                        System.DateTime m_UpdataTime = new System.DateTime(lastTime, System.DateTimeKind.Utc);

                        //判断今天是否完成
                        if (IsSameDay(m_ClaimTime))
                        {
                            if (m_ClaimTime.TimeOfDay.TotalHours < 19 && System.DateTime.UtcNow.TimeOfDay.TotalHours < 19)
                                continue;
                        }
                        else
                        {
                            System.TimeSpan spa = System.DateTime.UtcNow - m_ClaimTime;
                            if (spa.TotalDays < 1 &&
                                (m_ClaimTime.TimeOfDay.TotalHours >= 19) && System.DateTime.UtcNow.TimeOfDay.TotalHours < 19)
                                continue;
                        }
                        //判断今天是否已经开始做了
                        if (IsSameDay(m_UpdataTime))
                        {
                            if (m_UpdataTime.TimeOfDay.TotalHours < 19 && System.DateTime.UtcNow.TimeOfDay.TotalHours >= 19)
                                item.Step = 0;
                        }
                        else
                        {
                            if (m_UpdataTime.TimeOfDay.TotalHours < 19 || System.DateTime.UtcNow.TimeOfDay.TotalHours >= 19)
                                item.Step = 0;
                        }
                        item.IsFinish = item.Step >= item.Task.Conditions[0];
                        taskList.Add(item);
                    }
                }
            }
            return taskList;
        }
        private bool IsSameDay(System.DateTime time)
        {
            return time.Year == System.DateTime.UtcNow.Year && time.DayOfYear == System.DateTime.UtcNow.DayOfYear;
        }

        private TaskListType GetFunctionTypeInTask(int taskType)
        {
            TaskListType listType = TaskListType.Unspecified;
            if (taskType == 1 || taskType == 2)
                listType = TaskListType.Default;
            else if (taskType == 3)
                listType = TaskListType.Daily;
            else if (taskType == 4)
                listType = TaskListType.Achievement;
            else { }
            return listType;
        }

        /// <summary>
        /// 获取某种完成类型的任务列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetComTypeTask(ComType comType)
        {
            List<int> taskList = new List<int>();
            foreach (var item in CurrentTaskListId)
            {
                DRTask task = GameEntry.DataTable.GetDataTable<DRTask>().GetDataRow(item);
                if (task.ComType == (int)comType)
                {
                    taskList.Add(item);
                }
            }
            return taskList;
        }
    }
}


