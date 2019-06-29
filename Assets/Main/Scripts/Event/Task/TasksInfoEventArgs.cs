using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class TasksListChangedEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.TaskListChanged;
            }
        }

        public TasksListChangedEventArgs(List<PBTaskInfo> list)
        {
            m_TaskInfo = list;
        }

        private List<PBTaskInfo> m_TaskInfo = new List<PBTaskInfo>();
        public List<PBTaskInfo> TaskInfo { get { return m_TaskInfo; } }
    }
}

