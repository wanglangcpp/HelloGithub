using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Genesis.GameClient
{
    public class TasksListForm : NGUIForm
    {
        [Serializable]
        private class Tab
        {
            public UIToggle uIToggle;
            public GameObject objUIForm;
            public UIIntKey uiIntKey;
        }
        [SerializeField]
        private Tab[] m_Tabs = null;

        private Dictionary<int, int> m_TasksData = new Dictionary<int, int>();

        /// <summary>
        /// 任务类型的Key值对应的窗口的对象
        /// </summary>
        public Dictionary<int, GameObject> KeyAndToOpenForm = new Dictionary<int, GameObject>();

        /// <summary>
        /// 当前打开的任务列表窗口
        /// </summary>
        private int currentKey;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RefreshTabList();
            SubscribeEvents();
            TasksFormDisplayData displayData = userData as TasksFormDisplayData;
            if (displayData == null)
            {
                InitTabs(TaskListType.Default);
                return;
            }
            InitTabs(displayData.Scenario);
        }
        /// <summary>
        /// 刷新任务列表（根据功能开放表开放）
        /// </summary>
        private void RefreshTabList()
        {
            List<DROpenFunction> OpenFunction = GameEntry.OpenFunction.ShowSubTabFunction((int)OpenFunctionComponent.Function.Task);

            for (int i = 0; i < m_Tabs.Length; i++)
            {
                int key = m_Tabs[i].uiIntKey.Key;
                GameObject go = m_Tabs[i].objUIForm;
                go.SetActive(false);
                //储存窗口和其对应的键值
                KeyAndToOpenForm.Add(key, go);

                var OpenTaskType = OpenFunction.Find(x => x.Id == m_Tabs[i].uiIntKey.Key);
                if (OpenTaskType != null && OpenTaskType.isOpen)
                {
                    m_Tabs[i].uIToggle.gameObject.SetActive(true);
                }
                else
                {
                    m_Tabs[i].uIToggle.gameObject.SetActive(false);
                }
            }
        }
        private void InitTabs(TaskListType scenario)
        {
            for (int i = 0; i < m_Tabs.Length; i++)
            {

                if ((int)scenario == m_Tabs[i].uiIntKey.Key)
                {
                    if (m_Tabs[i].uIToggle.gameObject.activeSelf)
                    {
                        m_Tabs[i].uIToggle.Set(true);
                        return;
                    }
                }
            }
        }
        public void OnTabChange(int key, bool ischange)
        {
            if (!ischange)
            {
                return;
            }
            if (currentKey != 0)
            {
                KeyAndToOpenForm[currentKey].SetActive(false);
            }
            if (KeyAndToOpenForm.ContainsKey(key))
            {
                KeyAndToOpenForm[key].SetActive(true);
                currentKey = key;
            }
        }

        private void SubscribeEvents()
        {
            //这里有一个 领取任务奖励的监听
            //GameEntry.Event.Subscribe(EventId)
        }
        private void UnsubscribeEvents()
        {

        }
        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            UnsubscribeEvents();
        }
    }

    public enum TaskListType
    {
        /// <summary>
        /// 未指定
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 普通任务
        /// </summary>
        Default = 30010,

        /// <summary>
        /// 每日任务
        /// </summary>
        Daily = 30012,

        /// <summary>
        /// 成就任务
        /// </summary>
        Achievement = 30013,

        /// <summary>
        /// 隐藏任务
        /// </summary>
        Hide,
    }
}

