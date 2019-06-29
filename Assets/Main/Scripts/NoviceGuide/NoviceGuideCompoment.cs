using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using System.Text;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class NoviceGuideCompoment : MonoBehaviour
    {
        public enum ConditiionType : int
        {
            Level = 1,
            OpenFunction = 2,
            Task = 3,
        }

        private Dictionary<int, List<DRGuideUI>> groupDict;
        private Dictionary<int, int> groupLineDict;
        private Dictionary<string, string> keyLineDict;
        private Dictionary<string, List<DRGuideUI>> partDict;

        //private List<DRGuideUI> waitingGuide;
        //private Transform curForm;
        /// <summary>
        /// 是否有引导中ID
        /// </summary>
        private int curID = -1;
        public bool isShow = false;
        private bool isInit = false;
        private bool isGuiding = false;//是否是引导中
        private bool isSignIn = false;

        #region GM命令

        public bool operation = true;
        public void SwitchNoviceGuide(bool flag)
        {
            operation = flag;
            SetOperation(flag);
            Log.Debug("NoviceGuide:" + flag);
            if (flag)
            {
                curID = -1;
                isShow = false;
            }
            else
            {
                if (isShow)
                {
                    //TODO
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">完成的GroupID</param>
        public void ResetTo(int value)
        {
            SendGroupID(value);
            //SwitchNoviceGuide(true);
            //GameEntry.Data.Player.GuidanceGroupId = GetNextGroupId(value);
            //curID = groupDict[GameEntry.Data.Player.GuidanceGroupId][0].Id;
        }

        public void SetOperation(bool flag)
        {
            GameEntry.Setting.SetBool("NoviceGuideUI" + GameEntry.Data.Player.Id.ToString(), flag);
        }

        public bool GetOperation()
        {
            return GameEntry.Setting.GetBool("NoviceGuideUI" + GameEntry.Data.Player.Id.ToString(), false);
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 前置条件：更新了1003的PlayerData
        /// </summary>
        public void Init()
        {
            operation = GetOperation();
            if (!operation) return;
            if (isInit) return;
            partDict = new Dictionary<string, List<DRGuideUI>>();
            keyLineDict = new Dictionary<string, string>();
            var guideTable = GameEntry.DataTable.GetDataTable<DRGuideUI>().GetAllDataRows();
            System.Array.Sort(guideTable, new GuideIDComparer());//小到大

            InitPartGuide(guideTable);
            groupDict = InitGroupDict(guideTable);
            groupLineDict = InitGuideLine(groupDict);
            Log.Debug("Server groupId:" + GameEntry.Data.Player.GuidanceGroupId);
            GameEntry.Data.Player.GuidanceGroupId = GetNextGroupId(GameEntry.Data.Player.GuidanceGroupId);
            //curServerGroupID = GameEntry.Data.Player.GuidanceGroupId;
            operation = (GameEntry.Data.Player.GuidanceGroupId != -1);
            if (operation)
            {
                curID = groupDict[GameEntry.Data.Player.GuidanceGroupId][0].Id;
                GameEntry.Event.Subscribe(EventId.OpenFunctionAnimationEnd, OnOpenFunctionEnd);
                isInit = true;
            }
        }

        private void InitPartGuide(DRGuideUI[] guideTable)
        {
            string previousKey = CreateOpenKey(guideTable[0]);
            List<DRGuideUI> oneShowList = new List<DRGuideUI>();
            oneShowList.Add(guideTable[0]);
            partDict.Add(previousKey, oneShowList);

            for (int i = 1; i < guideTable.Length; i++)
            {
                DRGuideUI cur = guideTable[i];
                DRGuideUI pre = guideTable[i - 1];
                if (cur.Group != pre.Group || cur.UIFormID != pre.UIFormID)
                {
                    string curKey = CreateOpenKey(cur);
                    keyLineDict.Add(previousKey, curKey);
                    previousKey = curKey;
                    oneShowList = new List<DRGuideUI>();
                    partDict.Add(curKey, oneShowList);
                }
                oneShowList.Add(cur);
            }
        }

        private Dictionary<int, List<DRGuideUI>> InitGroupDict(DRGuideUI[] guideArray)
        {
            Dictionary<int, List<DRGuideUI>> tmpDict = new Dictionary<int, List<DRGuideUI>>();
            for (int i = 0; i < guideArray.Length; i++)
            {
                if (!tmpDict.ContainsKey(guideArray[i].Group))
                {
                    tmpDict.Add(guideArray[i].Group, new List<DRGuideUI>());
                }
                tmpDict[guideArray[i].Group].Add(guideArray[i]);
            }

            //步骤排序
            int[] keyStr = tmpDict.Keys.ToArray();
            for (int i = 0; i < keyStr.Length; i++)
            {
                tmpDict[keyStr[i]] = tmpDict[keyStr[i]].OrderBy(x => x.Id).ToList();
            }
            return tmpDict;
        }

        /// <summary>
        /// 形成引导线
        /// </summary>
        /// <param name="tmpDict"></param>
        /// <returns></returns>
        private Dictionary<int, int> InitGuideLine(Dictionary<int, List<DRGuideUI>> tmpDict)
        {
            int[] keys = tmpDict.Keys.ToArray();
            System.Array.Sort(keys);
            Dictionary<int, int> nextDict = new Dictionary<int, int>();
            nextDict.Add(0, keys[0]);
            for (int i = 1; i < keys.Length; i++)
            {
                if (!nextDict.ContainsKey(keys[i]))
                    nextDict.Add(keys[i - 1], keys[i]);
                else
                    Log.Error("repeat GroupID:" + keys[i]);
            }
            return nextDict;
        }

        /// <summary>
        /// 根据ID排序
        /// </summary>
        public class GuideIDComparer : IComparer<DRGuideUI>
        {
            public int Compare(DRGuideUI x, DRGuideUI y)
            {
                return x.Id.CompareTo(y.Id);
            }
        }

        #endregion

#region 监听

        private  void OnOpenFunctionEnd(object sender, GameEventArgs e) {
            if (!isInit) return;
            if (!operation) return;
            if (isShow) return;
            NGUIForm form = (e as OpenFunctionAnimationEndEventArgs).form;
            CheckHasGuide(form);
        }

        public void OnLevelUp(NGUIForm form) {
            if (!isInit) return;
            if (!operation) return;
            if (isShow) return;
            //CheckHasGuide(form);
        }

        public void OnTaskCompleted(NGUIForm form) {
            if (!isInit) return;
            if (!operation) return;
            if (isShow) return;
            CheckHasGuide(form);
        }

        //部分引导中需要返回，但是界面并不是新打开onOpen而是在onResume
        public void CheckNoviceGuide(NGUIForm form)
        {
            if (!isInit) return;
            if (!operation) return;
            if (isShow) return;
            if (!isSignIn) {
                isSignIn = true;
            }else if (!isGuiding) return;
            CheckHasGuide(form);
        }

        private void CheckHasGuide(NGUIForm form)
        {
            int groupId = GameEntry.Data.Player.GuidanceGroupId;
            int formID = form.UIForm.TypeId;
            Log.Debug("Check Novice Guide:" + (UIFormId)formID + ":" + formID + ",groupId:" + groupId + ",curID:" + curID);
            foreach (var id in GameEntry.Data.TaskStepData.TasksItemData.CurrentTaskListId)
            {
                Log.Debug("CurTask:id:" + id);
            }
            string checkKey = CreateOpenKey(groupId, formID, curID);
            if (partDict.ContainsKey(checkKey))
            {
                bool isAllow = CheckCanOpenGuide(groupId);
                if (isAllow)
                {
                    ShowNoviceGuide(partDict[checkKey], form, groupId);
                }
            }
        }

        #endregion

        private int GetNextGroupId(int curGroupID)
        {
            if (groupLineDict.ContainsKey(curGroupID))
            {
                return groupLineDict[curGroupID];
            }
            return -1;
        }

        public void SetNextGuideID(int lastID)
        {
            List<DRGuideUI> guideList = groupDict[GameEntry.Data.Player.GuidanceGroupId];
            for (int i = 0; i < guideList.Count; i++)
            {
                if (lastID == guideList[i].Id)
                {
                    if (i + 1 < guideList.Count)
                    {
                        curID = guideList[i + 1].Id;
                        break;
                    }
                }
            }
        }

        private void ShowNoviceGuide(List<DRGuideUI> oneGuide, NGUIForm openForm, int groupId)
        {
            isShow = true;
            isGuiding = true;
            int endGuideID = groupDict[groupId][groupDict[groupId].Count - 1].Id;
            CheckInNoviceTime(oneGuide[0]);
            GameEntry.UI.OpenUIForm(UIFormId.NoviceGuideDialog, new NoviceGuideDialogData
            {
                oneGroup = oneGuide,
                mForm = openForm,
                lastGuideId = endGuideID
            });
        }

        /// <summary>
        /// 判断新手期是否要强引导
        /// </summary>
        public void CheckInNoviceTime(DRGuideUI data)
        {
            if (!data.IsShowInGuide && GameEntry.Data.Player.Level > 30)
            {
                int startID = groupDict[data.Group][0].Id;
                if (startID == data.Id)
                {//开始的时候会发送一次
                    SendGroupID(data.Group);
                }
            }
        }

        public void CheckKeyStep(DRGuideUI item)
        {
            if (item.IsStep)
            {
                SendGroupID(item.Group);
            }
        }

        private void SendGroupID(int completeGroupID)
        {
            CLAddGuidanceGroup data = new CLAddGuidanceGroup();
            data.GroupId = completeGroupID;
            //curServerGroupID = completeGroupID;
            GameEntry.Network.Send(data);
        }

        public void SetNextGroupID()
        {
            isGuiding = false;
            int oldGuideID = GameEntry.Data.Player.GuidanceGroupId;
            if (groupLineDict.ContainsKey(oldGuideID))
            {
                GameEntry.Data.Player.GuidanceGroupId = groupLineDict[oldGuideID];
                curID = groupDict[GameEntry.Data.Player.GuidanceGroupId][0].Id;
            }
            else
            {//End
                GameEntry.Event.Unsubscribe(EventId.OpenFunctionAnimationEnd, OnOpenFunctionEnd);
                operation = false;
            }
        }

        /// <summary>
        /// 能否满足表的条件
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        private bool CheckCanOpenGuide(int groupID)
        {
            switch ((ConditiionType)groupDict[groupID][0].OpenType)
            {
                case ConditiionType.Level:
                    return GameEntry.Data.Player.Level >= groupDict[groupID][0].Condition;
                case ConditiionType.OpenFunction:
                    return GameEntry.OpenFunction.CheckFunctionIsOpen(groupDict[groupID][0].Condition);
                case ConditiionType.Task:
                    return GameEntry.TaskComponent.CheckTaskIsFinish(groupDict[groupID][0].Condition);
            }
            return false;
        }

        /// <summary>
        /// Unused 小于30级（策划意思是30级后会触发，但允许跳过（引导时下线后就不再弹出））
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsShowInGuide(DRGuideUI item)
        {
            if (item.IsShowInGuide && GameEntry.Data.Player.Level < 30)
            {
                return true;
            }
            return false;
        }

        private string CreateOpenKey(DRGuideUI item)
        {
            return CreateOpenKey(item.Group, item.UIFormID, item.Id);
        }

        private string CreateOpenKey(int group, int formId, int id)
        {
            return new StringBuilder(group.ToString()).Append(formId).Append(id).ToString();
        }

        #region MonoBehavior

        private void OnDestroy()
        {
            if(null != GameEntry.Event)
                GameEntry.Event.Unsubscribe(EventId.OpenFunctionAnimationEnd, OnOpenFunctionEnd);
        }

        #endregion
    }
}