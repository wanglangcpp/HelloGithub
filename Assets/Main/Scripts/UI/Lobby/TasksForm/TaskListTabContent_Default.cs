using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class TaskListTabContent_Default : TaskListBaseTabContent
    {
        private List<TaskItem_Default> listItem = new List<TaskItem_Default>();

        protected override TaskListType TaskType
        {
            get
            {
                return TaskListType.Default;
            }
        }
        protected override void OnInit()
        {
            base.OnInit();
            listItem.Clear();
        }

        protected override IEnumerator PopulateStart(List<TaskStep> data)
        {
            yield return null;
            var dRTask = GameEntry.DataTable.GetDataTable<DRTask>();
            if (data.Count == 0)
            {
                //没有任务给个提示
            }
            foreach (var item in data)
            {
                GameObject go = NGUITools.AddChild(m_ListView.gameObject, m_ItemTemplate);
                TaskItem_Default script = go.GetComponent<TaskItem_Default>();
                script.RefreshItemData(item);
                listItem.Add(script);
            }
            yield return null;

            m_ScrollView.panel.UpdateAnchors();
            m_ListView.Reposition();
            m_ScrollView.ResetPosition();
            for (int i = 0; i < listItem.Count; i++)
            {
                listItem[i].UpdateAnchors();
            }
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }
        protected override void OnRefreshTaskData(object sender, GameEventArgs e)
        {
            base.OnRefreshTaskData(sender, e);
        }

    }
}

