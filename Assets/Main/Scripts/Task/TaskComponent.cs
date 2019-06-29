using UnityEngine;
using System.Collections;
using GameFramework;
using System.Collections.Generic;
using GameFramework.DataTable;
using System;
using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 任务组件
    /// </summary>
    public class TaskComponent : MonoBehaviour
    {
        /// <summary>
        /// 领奖结束回调
        /// </summary>
        public GameFrameworkAction<object> onClaimTaskReward = null;

        private MeHeroCharacter m_MeHero = null;

        /// <summary>
        /// 是否过检查每日任务列表
        /// </summary>
        public bool HasHasCheckTaskList { get { return m_HasCheckTaskList; } }
        private bool m_HasCheckTaskList = false;

        /// <summary>
        /// 主城NpcId
        /// </summary>
        public int LobbyNpcId { get { return m_LobbyNpcId; } }
        private int m_LobbyNpcId;

        /// <summary>
        /// 寻路停下的距离
        /// </summary>
        public float Distance { get { return m_Distance; } }
        private float m_Distance;

        /// <summary>
        /// 寻路目标位置
        /// </summary>
        public Vector3 TargetPos { get { return m_TargetPos; } }
        private Vector3 m_TargetPos = new Vector3();

        public List<DRTask> FinishTaskList { get { return m_FinishTaskList; } }
        private List<DRTask> m_FinishTaskList = new List<DRTask>();

        #region MonoBehaviour
        private void Awake()
        {
            Log.Info("Task component has been initialized.");
        }
        private void Start()
        {
            GameEntry.Event.Subscribe(EventId.ReceiveAndShowItems, ReceiveAndShowItems);
        }
        private void OnDistory()
        {
            GameEntry.Event.Unsubscribe(EventId.ReceiveAndShowItems, ReceiveAndShowItems);
        }
        #endregion
        /// <summary>
        /// 当前关卡击杀的怪物类型及数量
        /// </summary>
        public Dictionary<int, int> MonsterCount = new Dictionary<int, int>();

        public System.DateTime LogInTime;

        /// <summary>
        /// 寻路
        /// </summary>
        /// <param name="lobbyNpcId"></param>
        public void LobbyNavMesh(int lobbyNpcId)
        {
            m_LobbyNpcId = lobbyNpcId;
            m_MeHero = GameEntry.SceneLogic.MeHeroCharacter;
            if (m_MeHero != null)
            {
                m_TargetPos = FindLobbyNpcPostion(lobbyNpcId);
                m_MeHero.StartLoobyMove(m_TargetPos);
            }
        }

        /// <summary>
        /// 查找Npc位置
        /// </summary>
        /// <param name="npcId"></param>
        /// <returns></returns>
        private Vector3 FindLobbyNpcPostion(int npcId)
        {
            m_Distance = UnityEngine.Random.Range(0.5f, 2.5f);
            DRLobbyNpc NpcId = GameEntry.DataTable.GetDataTable<DRLobbyNpc>().GetDataRow(npcId);
            Vector2 npcPos = new Vector2(NpcId.PositionX, NpcId.PositionY);
            Vector3 pos = new Vector3();
            bool isMove = AIUtility.TrySamplePosition(npcPos, false, out pos);
            if (isMove)
                return pos;
            return m_MeHero.transform.position;
        }

        /// <summary>
        /// 检查是否走道NPC附近
        /// </summary>
        public void CheckHeroPostion()
        {
            float distance = Vector3.Distance(m_TargetPos, m_MeHero.transform.position);
            if (distance <= m_Distance)
            {
                m_MeHero.AutoMove = false;
                CheckNpcHasTask(m_LobbyNpcId);
            }
        }

        /// <summary>
        /// 检查Npc有没有对话任务
        /// </summary>
        /// <param name="npcId"></param>
        /// <returns></returns>
        public bool CheckNpcHasTask(int npcId)
        {
            m_LobbyNpcId = npcId;
            if (GameEntry.Data.TaskStepData.TasksItemData == null)
            {
                return false;
            }
            var taskList = GameEntry.Data.TaskStepData.TasksItemData.GetComTypeTask(ComType.Dialogue);

            foreach (var item in taskList)
            {
                DRTask task = GameEntry.DataTable.GetDataTable<DRTask>().GetDataRow(item);
                DRTaskTalk talk = GameEntry.DataTable.GetDataTable<DRTaskTalk>().GetDataRow(task.Conditions[1]);
                if (talk.NpcId == npcId)
                {
                    OpenTaskDialogWnd(task);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 检查是否有新增每日任务
        /// </summary>
        /// <returns></returns>
        public bool CheckTaskListIsSame()
        {
            DRTask[] TaskTable = GameEntry.DataTable.GetDataTable<DRTask>().GetAllDataRows();
            var taskDic = GameEntry.Data.TaskStepData.TasksItemData.CurrentTaskListId;
            List<int> AllCheckTask = new List<int>();
            for (int i = 0; i < TaskTable.Length; i++)
            {
                if (TaskTable[i].TaskType == (int)TaskListType.Daily)
                {
                    if (TaskTable[i].Level <= GameEntry.Data.Player.Level)
                    {
                        if (!taskDic.Contains(TaskTable[i].Id))
                        {
                            AllCheckTask.Add(TaskTable[i].Id);
                        }
                    }
                }
            }
            m_HasCheckTaskList = true;
            if (AllCheckTask.Count != 0)
            {
                CLAddTask helper = new CLAddTask();
                helper.TaskId.AddRange(AllCheckTask);
                GameEntry.Network.Send(helper);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检测任务是否完成
        /// </summary>
        /// <param name="taskId">检测的任务Idk</param>
        /// <returns></returns>
        public bool CheckTaskIsFinish(int taskId)
        {
            return GameEntry.Data.TaskStepData.CurrentFinishTasksId.Contains(taskId);
        }

        /// <summary>
        /// 任务排序（追踪窗口）
        /// </summary>
        public List<TaskStep> SortTaskList()
        {
            List<TaskStep> sortList = new List<TaskStep>();

            if (GameEntry.Data.TaskStepData.TasksItemData == null)
                return null;

            List<TaskStep> currentTask = GameEntry.Data.TaskStepData.TasksItemData.CurrentTaskList;
            TaskStep midTask = new TaskStep();
            for (int i = 0; i < currentTask.Count - 1; i++)
            {
                for (int j = 0; j + 1 <= currentTask.Count - 1 - i; j++)
                {
                    if (currentTask[j].Task.TaskType > currentTask[j + 1].Task.TaskType ||
                        (currentTask[j].Task.TaskType == currentTask[j + 1].Task.TaskType
                        && !currentTask[j].IsFinish && currentTask[j + 1].IsFinish)
                        )
                    {
                        midTask = currentTask[j];
                        currentTask[j] = currentTask[j + 1];
                        currentTask[j + 1] = midTask;
                    }
                }
            }
            return currentTask;
        }


        /// <summary>
        /// 打开对话
        /// </summary>
        /// <param name="task"></param>
        public void OpenTaskDialogWnd(DRTask task)
        {
            if (task.ComType == (int)ComType.Dialogue)
            {
                int dialogId = task.Conditions[1];
                GameEntry.UI.OpenUIForm(UIFormId.DialogueForm, new DialogueFormDisplayData() { DialogId = dialogId, Task = task });
            }
        }

        /// <summary>
        /// 是否可以跳转
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public bool isGoTo(int taskId)
        {
            var task = GameEntry.DataTable.GetDataTable<DRTask>().GetDataRow(taskId);

            ComType comType = (ComType)task.ComType;
            switch (comType)
            {
                case ComType.LogIn:
                case ComType.LogInTime:
                case ComType.CollectionHreo:
                case ComType.CollectionStarHreo:
                case ComType.FriendCount:
                case ComType.LevelUp:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 根据任务解析任务类型，开始任务
        /// </summary>
        /// <param name="type">任务ID</param>
        /// <returns></returns>
        public void GoToStartTask(DRTask task)
        {
            int type = task.ComType;
            int whereToGoId = task.WhereToGet;
            int condition0 = task.Conditions[0];
            int condition1 = task.Conditions[1];
            int condition2 = task.Conditions[2];
            int condition3 = task.Conditions[3];
            ComType comType = (ComType)type;
            switch (comType)
            {
                case ComType.Dialogue:
                    if (GameEntry.UI.GetUIForm(UIFormId.TasksForm) != null)
                    {
                        GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(UIFormId.TasksForm));
                    }
                    var talkTask = GameEntry.DataTable.GetDataTable<DRTaskTalk>().GetDataRow(condition1);
                    if (talkTask == null)
                    {
                        Log.Error("Don't find this talk id {0} in {1} table", condition0, talkTask);
                        return;
                    }
                    LobbyNavMesh(talkTask.NpcId);
                    break;
                case ComType.Fight:
                case ComType.PassCustom:
                    string levelId = GameEntry.DataTable.GetDataTable<DRWhereToGet>().GetDataRow(whereToGoId).Params[1];
                    int m_LevelId = int.Parse(levelId);
                    GameEntry.UI.OpenUIForm(UIFormId.InstanceInfoForm, new InstanceInfoDisplayData { InstanceId = m_LevelId });
                    break;
                case ComType.EquipmentUp:
                case ComType.HeroUpStar:
                case ComType.HeroAdvanced:
                case ComType.WearBadge:
                case ComType.StarMap:
                case ComType.GivePower:
                case ComType.PVP:
                case ComType.JingYanFuBen:
                case ComType.GameDraw:
                case ComType.BuyPower:
                case ComType.BuyGold:
                case ComType.BuyShopItem:
                case ComType.RankMatch:
                case ComType.PassCustomCount:
                case ComType.PvpWin:
                case ComType.AnyFuBen:
                case ComType.CoinFuBen:
                case ComType.BossFuBen:
                case ComType.ToWerFuBen:
                    WhereToGetLogic_Base whereToGetLogic = GameEntry.WhereToGet.GetLogic(whereToGoId);
                    whereToGetLogic.OnClick();
                    break;
            }
        }

        /// <summary>
        /// 当前领取奖励的任务ID
        /// </summary>
        private int ClaimTaskId = 0;
        /// <summary>
        /// 完成任务
        /// </summary>
        public void FinishTask(int taskID)
        {
            onClaimTaskReward = OnClaimedTaskReward;
            ClaimTaskId = taskID;
            CLClaimTaskReward helper = new CLClaimTaskReward();
            helper.TaskId = taskID;
            GameEntry.Network.Send(helper);
        }
        public void FinishTask(int taskID, int talkID)
        {
            onClaimTaskReward = OnClaimedTaskReward;
            ClaimTaskId = taskID;
            CLNPCDialogue helper = new CLNPCDialogue();
            helper.TaskId = taskID;
            helper.NPCId = talkID;
            GameEntry.Network.Send(helper);
        }

        /// <summary>
        /// 领奖弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiveAndShowItems(object sender, GameEventArgs e)
        {
            if (ClaimTaskId != 0)
            {
                GameEntry.Data.TaskStepData.AddFinishedTaskId(ClaimTaskId);
            }
            ShowGetItemsInfoEventArgs ne = e as ShowGetItemsInfoEventArgs;
            GameEntry.RewardViewer.RequestShowRewards(ne.Rewards.ReceiveGoodsData, false, onClaimTaskReward, ClaimTaskId);
        }
        /// <summary>
        /// 任务领奖结束的回调
        /// </summary>
        private void OnClaimedTaskReward(object taskId)
        {
            ClaimTaskId = 0;
            onClaimTaskReward = null;
            GameEntry.Event.Fire(null, new ClaimTaskRewardSuccessEventArgs() { TaskId = Convert.ToInt32(taskId) });
        }
    }
}

