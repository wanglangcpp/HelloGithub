using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using GameFramework;

namespace Genesis.GameClient
{
    public class TaskItem_Default : MonoBehaviour
    {
        private Color m_Color = new Color((float)255 / 255, (float)172 / 255, 0 / 255);

        [Serializable]
        private struct Item
        {
            public UISprite ItemIcon;
            public UILabel ItemCount;
        }
        [SerializeField]
        private UILabel m_TaskName = null;

        [SerializeField]
        private UILabel m_TaskStep = null;

        [SerializeField]
        private UISprite m_TaskIcon = null;

        [SerializeField]
        private Item[] m_Rewards = null;

        [SerializeField]
        private UIButton m_BtnEnd = null;

        [SerializeField]
        private UIButton m_BtnStatr = null;

        private DRTask taskData = null;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
        }

        public void RefreshItemData(TaskStep data)
        {
            taskData = data.Task;

            if (taskData.TaskType == 1)
            {
                m_TaskName.color = m_Color;
            }
            m_TaskName.text = GameEntry.Localization.GetString(taskData.Desc);

            //if (taskData.Conditions[0] == 1 || taskData.Conditions[0] == 0 || taskData.Conditions[0] == -1)
            //{
            //    m_TaskStep.text = string.Empty;
            //}
            //else
            //{
            //    m_TaskStep.text = GameEntry.Localization.GetString("UI_TEXT_TASK_COMPLETION", data.Step, taskData.Conditions[0]);
            //}

            if (taskData.IconId != -1)
            {
                m_TaskIcon.LoadAsync(taskData.IconId);
            }

            for (int i = 0; i < m_Rewards.Length; i++)
            {
                if (i < taskData.Rewards.Count)
                {
                    m_Rewards[i].ItemIcon.LoadAsync(GeneralItemUtility.GetGeneralItemIconId(taskData.Rewards[i].IconId));
                    m_Rewards[i].ItemCount.text = taskData.Rewards[i].Count == 0 ? string.Empty : taskData.Rewards[i].Count.ToString();
                }
                else
                {
                    m_Rewards[i].ItemIcon.gameObject.SetActive(false);
                }
            }

            int step = data.Step;
            if (taskData.ComType == (int)ComType.LogInTime)
            {
                m_BtnStatr.gameObject.SetActive(false);
                m_BtnEnd.gameObject.SetActive(TaskIsFinish(ref step));
            }
            else
            {
                m_BtnEnd.gameObject.SetActive(data.IsFinish);
                if (GameEntry.TaskComponent.isGoTo(taskData.Id))
                    m_BtnStatr.gameObject.SetActive(!data.IsFinish);
                else
                    m_BtnStatr.gameObject.SetActive(false);
            }
            if (taskData.Conditions[0] <= 1)
                m_TaskStep.text = string.Empty;
            else
                m_TaskStep.text = GameEntry.Localization.GetString("UI_TEXT_TASK_COMPLETION", step, taskData.Conditions[0]);

        }
        /// <summary>
        /// 判断任务是否完成
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        private bool TaskIsFinish(ref int step)
        {
            TimeSpan gapTime = GameEntry.Time.LobbyServerUtcTime - GameEntry.TaskComponent.LogInTime;
            step += gapTime.Minutes;
            if (step < taskData.Conditions[0])
                return false;
            return true;
        }

        public void UpdateAnchors()
        {
            var rects = GetComponentsInChildren<UIRect>();
            for (int i = 0; i < rects.Length; i++)
            {
                rects[i].UpdateAnchors();
            }
        }

        /// <summary>
        /// 前往任务
        /// </summary>
        public void OnClickStartBtn()
        {
            GameEntry.TaskComponent.GoToStartTask(taskData);
        }
        /// <summary>
        /// 结束任务(领取任务奖励)
        /// </summary>
        public void OnClickEndBtn()
        {
            int taskId = taskData.Id;
            GameEntry.TaskComponent.FinishTask(taskId);
        }
    }
}

