using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 任务类型基类
    /// </summary>
    public abstract class TaskListBaseTabContent : MonoBehaviour
    {
        [SerializeField]
        protected GameObject m_ItemTemplate = null;

        [SerializeField]
        protected UIScrollView m_ScrollView = null;

        [SerializeField]
        protected UIGrid m_ListView = null;

        /// <summary>
        /// 缓存当前进行中任务(任务ID和完成进度)
        /// </summary>
        protected List<TaskStep> m_CurrentTasks = new List<TaskStep>();

        protected virtual TaskListType TaskType
        {
            get
            {
                return TaskListType.Unspecified;
            }
        }
        protected void OnEnable()
        {
            Clear();
            SubscribeEvents();
            OnInit();
        }
        /// <summary>
        /// 根据打开界面不同，缓存不同的任务列表
        /// </summary>
        protected virtual void OnInit()
        {
            #region 获取服务器发来的任务列表并分类处理
            TaskItemInfoData taskList = GameEntry.Data.TaskStepData.TasksItemData;

            if (taskList == null)
            {
                return;
            }
            if (TaskType == TaskListType.Unspecified)
            {
                m_CurrentTasks = taskList.CurrentTaskList;
            }
            else
            {
                if (TaskType == TaskListType.Daily &&
                    !GameEntry.TaskComponent.HasHasCheckTaskList &&
                    !GameEntry.TaskComponent.CheckTaskListIsSame())
                    return;
                m_CurrentTasks = taskList.GetTasksInfo(TaskType);
            }
            TaskStep midTask = new TaskStep();
            for (int i = 0; i < m_CurrentTasks.Count - 1; i++)
            {
                for (int j = 0; j + 1 <= m_CurrentTasks.Count - 1 - i; j++)
                {
                    if (m_CurrentTasks[j].Task.TaskType > m_CurrentTasks[j + 1].Task.TaskType ||
                        (m_CurrentTasks[j].Task.TaskType == m_CurrentTasks[j + 1].Task.TaskType
                        && !m_CurrentTasks[j].IsFinish && m_CurrentTasks[j + 1].IsFinish)
                        )
                    {
                        midTask = m_CurrentTasks[j];
                        m_CurrentTasks[j] = m_CurrentTasks[j + 1];
                        m_CurrentTasks[j + 1] = midTask;
                    }
                }
            }
            StartCoroutine(PopulateStart(m_CurrentTasks));
            return;

            //List<TaskStep> mainList_t = m_CurrentTasks.FindAll(x => x.Task.TaskType == 1 && x.IsFinish);
            //List<TaskStep> mainList_f = m_CurrentTasks.FindAll(x => x.Task.TaskType == 1 && !x.IsFinish);
            //List<TaskStep> otherList_t = m_CurrentTasks.FindAll(x => x.Task.TaskType != 1 && x.IsFinish);
            //List<TaskStep> otherList_f = m_CurrentTasks.FindAll(x => x.Task.TaskType != 1 && !x.IsFinish);
            //mainList_t.AddRange(mainList_f.ToArray());
            //mainList_t.AddRange(otherList_t.ToArray());
            //mainList_t.AddRange(otherList_f.ToArray());
            //m_CurrentTasks = mainList_t;
            //StartCoroutine(PopulateStart(m_CurrentTasks));
            #endregion

        }
        /// <summary>
        /// 开始加载任务列表
        /// </summary>
        /// <param name="taskList"></param>
        /// <returns></returns>
        protected abstract IEnumerator PopulateStart(List<TaskStep> taskList);

        /// <summary>
        /// 清除当前显示的任务列表
        /// </summary>
        private void Clear()
        {
            //m_CurrentTasks.Clear();
            var items = m_ListView.GetChildList();
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i] != null)
                {
                    Destroy(items[i].gameObject);
                }
            }
        }
        protected void OnDisable()
        {
            Clear();
            UnsubscribeEvents();
        }

        protected virtual void SubscribeEvents()
        {
            GameEntry.Event.Subscribe(EventId.TaskListChanged, OnRefreshTaskData);
        }
        protected virtual void UnsubscribeEvents()
        {
            GameEntry.Event.Unsubscribe(EventId.TaskListChanged, OnRefreshTaskData);
        }
        /// <summary>
        /// 任务刷新回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnRefreshTaskData(object sender, GameEventArgs e)
        {
            Clear();
            OnInit();
        }
    }
}

