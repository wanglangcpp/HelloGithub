using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using GameFramework;
using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 功能解锁组件
    /// </summary>
    public class OpenFunctionComponent : MonoBehaviour
    {
        public enum OpenType : int
        {
            Level = 1,
            Task = 2,
            None = -1,
        }
        /// <summary>
        /// 功能号
        /// </summary>
        public enum Function : int
        {
            /// <summary>
            /// 任务
            /// </summary>
            Task = 30002,
            /// <summary>
            /// 福利中心
            /// </summary>
            WelfareCenter = 10002,
            /// <summary>
            /// 根任务节点
            /// </summary>
            First = 10001,
            /// <summary>
            /// 首冲
            /// </summary>
            FirstCharge = 10005,
            /// <summary>
            /// 升星
            /// </summary>
            StarLevel = 30006,
            /// <summary>
            /// 强化
            /// </summary>
            Strengthen = 30007,
            /// <summary>
            /// 装备
            /// </summary>
            NewGear = 30008,
            /// <summary>
            /// 技能
            /// </summary>
            Skill = 30009,
        }
        private bool isTaskListInit = false;
        private bool isInit = false;
        private bool isServerOpenListInit = false;
        private List<int> serverOpenList;
        private List<int> defaultTaskList;

        private List<DROpenFunction> orgFunctionList;
        private Dictionary<int, List<DROpenFunction>> openGroupDict;
        private Dictionary<HeroInfoForm_Possessed.TabType, DROpenFunction> heroDict;
        private List<DROpenFunction> openedFunctionList;
        private List<DROpenFunction> closedFunctionList;
        private bool isFirstCharge;
        public bool isPlayingAnimation = false;
        private NGUIForm mainForm;
        private bool operation;

        //根据服务器开放所需功能

        #region 初始化
        private void Init()
        {
            if (isInit) return;
            isInit = true;
            operation = GetOperation();
            Log.Debug("OpenFunctionComponent:GetOperation:" + operation);
            if (!operation)
            {
                SetOperation(false);
                orgFunctionList = InitAllFunctions();
            }
            else
            {
                orgFunctionList = InitFunctions();
            }
            InitOpenFunctions();
            openGroupDict = SortFunctionGroupsLogic(orgFunctionList);
            heroDict = InitHeroDict();
            GameEntry.Event.Subscribe(EventId.ClaimTaskRewardSuccess, ClaimTaskRewardSuccess);
            GameEntry.Event.Subscribe(EventId.PlayerLevelUpAnimationCallBack, OnLevelUpAnimationCallback);
        }



        public void InitTaskList(List<int> curTaskList = null)
        {
            if (isTaskListInit) return;
            isTaskListInit = true;
            this.defaultTaskList = curTaskList;
            Log.Debug("OpenFunctionComponent.InitTaskList:" + curTaskList.Count);
            if (isServerOpenListInit)
                Init();
        }

        public void InitServerOpenList(List<int> serverOpenList)
        {
            if (isServerOpenListInit) return;
            if (serverOpenList.Count <= 0)
            {
                Log.Error("serverOpenFunctionList Count<=0");
            }
            isServerOpenListInit = true;
            this.serverOpenList = serverOpenList;
            Log.Debug("OpenFunctionComponent.InitServerOpenList:" + serverOpenList.Count);
            if (isTaskListInit)
                Init();
        }

        private List<DROpenFunction> InitFunctions()
        {
            DROpenFunction[] functionArrays = GameEntry.DataTable.GetDataTable<DROpenFunction>().GetAllDataRows();
            CheckTableIsCorrect(functionArrays);

            Dictionary<int, DROpenFunction> tmpDict = new Dictionary<int, DROpenFunction>();
            List<DROpenFunction> orgFunctionList = new List<DROpenFunction>();
            for (int i = 0; i < functionArrays.Length; i++)
            {
                tmpDict.Add(functionArrays[i].Id, functionArrays[i]);
            }
            //根据服务器开放的功能做显示
            for (int i = 0; i < serverOpenList.Count; i++)
            {
                if (tmpDict.ContainsKey(serverOpenList[i]))
                {
                    DROpenFunction per = tmpDict[serverOpenList[i]];
                    if ((Function)per.Id == Function.FirstCharge)
                    {
                        isFirstCharge = per.IsDefaultOpen;
                    }
                    if (per.IsDefaultOpen)
                    {
                        per.isOpen = true;
                        orgFunctionList.Add(per);
                    }
                    else
                    {
                        if ((OpenType)per.OpenType != OpenType.None && per.Contidtion != -1)
                        {
                            orgFunctionList.Add(per);
                        }
                    }
                }
                else
                {
                    Log.Error("serverOpenList error:Id=" + serverOpenList[i]);
                }
            }
            return orgFunctionList;
        }

        private void InitOpenFunctions()
        {
            for (int i = 0; i < orgFunctionList.Count; i++)
            {
                if (orgFunctionList[i].isOpen) continue;
                switch ((OpenType)orgFunctionList[i].OpenType)
                {
                    case OpenType.Level://等级
                        orgFunctionList[i].isOpen = (GameEntry.Data.Player.Level - orgFunctionList[i].Contidtion) >= 0;
                        break;
                    case OpenType.Task://任务ID
                        orgFunctionList[i].isOpen = GameEntry.TaskComponent.CheckTaskIsFinish(orgFunctionList[i].Contidtion);
                        break;
                }
            }

            openedFunctionList = new List<DROpenFunction>();
            closedFunctionList = new List<DROpenFunction>();
            for (int i = 0; i < orgFunctionList.Count; i++)
            {
                if (orgFunctionList[i].isOpen)
                {
                    openedFunctionList.Add(orgFunctionList[i]);
                }
                else
                {
                    closedFunctionList.Add(orgFunctionList[i]);
                }
            }
        }

        //校验表是否有问题（单条逻辑检测）
        private bool CheckTableIsCorrect(DROpenFunction[] functionArrays)
        {
            List<int> errorIdList = new List<int>();
            for (int i = 0; i < functionArrays.Length; i++)
            {
                DROpenFunction item = functionArrays[i];
                if ((OpenType)item.OpenType == OpenType.None && item.Contidtion == -1 && !item.IsDefaultOpen)
                {
                    errorIdList.Add(item.Id);
                }
                if (item.FunctionGroup == -1 && item.ParentId == -1)
                {
                    errorIdList.Add(item.Id);
                }
            }
            for (int i = 0; i < errorIdList.Count; i++)
            {
                Log.Debug("Invalid openFunctionId:" + errorIdList[i]);
            }
            return errorIdList.Count <= 0;
        }

        /// <summary>
        /// 将功能开放的表数据进行排序,UI使用
        /// </summary>
        private Dictionary<int, List<DROpenFunction>> SortFunctionGroupsLogic(List<DROpenFunction> orgFunctionList)
        {
            var tmpGroupDict = new Dictionary<int, List<DROpenFunction>>();
            //首先判断是否有ItemGroup,根据值拿对应的Grid,然后排序
            for (int i = 0; i < orgFunctionList.Count; i++)
            {
                DROpenFunction item = orgFunctionList[i];
                if (item.FunctionGroup == -1) continue;
                if (!tmpGroupDict.ContainsKey(item.FunctionGroup))
                {
                    tmpGroupDict.Add(item.FunctionGroup, new List<DROpenFunction>());
                }
                tmpGroupDict[item.FunctionGroup].Add(item);
            }
            //根据GroupIndex排序：小到大
            int[] keyStr = tmpGroupDict.Keys.ToArray();
            for (int i = 0; i < keyStr.Length; i++)
            {
                tmpGroupDict[keyStr[i]] = tmpGroupDict[keyStr[i]].OrderBy(x => x.GroupIndex).ToList();
            }
            return tmpGroupDict;
        }

        //绑定英雄面板功能
        private Dictionary<HeroInfoForm_Possessed.TabType, DROpenFunction> InitHeroDict()
        {
            Dictionary<HeroInfoForm_Possessed.TabType, DROpenFunction> tmpHeroDict = new Dictionary<HeroInfoForm_Possessed.TabType, DROpenFunction>();
            for (int i = 0; i < orgFunctionList.Count; i++)
            {
                if (tmpHeroDict.Count >= 4) break;
                switch ((Function)orgFunctionList[i].Id)
                {
                    case Function.StarLevel:
                        tmpHeroDict.Add(HeroInfoForm_Possessed.TabType.StarLevel, orgFunctionList[i]);
                        break;
                    case Function.Strengthen:
                        tmpHeroDict.Add(HeroInfoForm_Possessed.TabType.Strengthen, orgFunctionList[i]);
                        break;
                    case Function.NewGear:
                        tmpHeroDict.Add(HeroInfoForm_Possessed.TabType.NewGear, orgFunctionList[i]);
                        break;
                    case Function.Skill:
                        tmpHeroDict.Add(HeroInfoForm_Possessed.TabType.Skill, orgFunctionList[i]);
                        break;
                }
            }
            return tmpHeroDict;
        }


        #endregion


        #region 逻辑入口监听

        private void OnLevelUpAnimationCallback(object sender, GameEventArgs e)
        {
            if (!operation) return;
            int playerLv = GameEntry.Data.Player.Level;
            Log.Debug("OpenFunctionComponent.CheckLv=" + playerLv);
            var newOpenFunctionList = new List<DROpenFunction>();
            for (int i = 0; i < closedFunctionList.Count; i++)
            {
                if ((OpenType)closedFunctionList[i].OpenType == OpenType.Level && playerLv >= closedFunctionList[i].Contidtion)
                {
                    closedFunctionList[i].isOpen = true;
                    openedFunctionList.Add(closedFunctionList[i]);
                    if (closedFunctionList[i].IsPlayAnimation) {
                        newOpenFunctionList.Add(closedFunctionList[i]);
                    }
                    closedFunctionList.Remove(closedFunctionList[i]);
                }
            }
            if (newOpenFunctionList.Count > 0)
            {
                ShowAnimation(newOpenFunctionList);
            }
            else
            {
                GameEntry.NoviceGuide.OnLevelUp(mainForm);
            }
        }

        private void ClaimTaskRewardSuccess(object sender, GameEventArgs e)
        {
            if (!operation) return;
            Log.Debug("OpenFunction Receive OnClaimTaskReward");
            ClaimTaskRewardSuccessEventArgs data = e as ClaimTaskRewardSuccessEventArgs;
            int completedTaskId = data.TaskId;
            //判断完成的任务ID是否解锁新功能
            var newOpenFunctionList = new List<DROpenFunction>();
            for (int i = 0; i < closedFunctionList.Count; i++)
            {
                if ((OpenType)closedFunctionList[i].OpenType == OpenType.Task && completedTaskId == closedFunctionList[i].Contidtion)
                {
                    closedFunctionList[i].isOpen = true;
                    openedFunctionList.Add(closedFunctionList[i]);
                    if (closedFunctionList[i].IsPlayAnimation)
                    {
                        newOpenFunctionList.Add(closedFunctionList[i]);
                    }
                    closedFunctionList.Remove(closedFunctionList[i]);
                }
            }
            if (newOpenFunctionList.Count > 0)
            {
                ShowAnimation(newOpenFunctionList);
            }
            else
            {
                GameEntry.NoviceGuide.OnTaskCompleted(mainForm);
            }
        }

        private void ShowAnimation(List<DROpenFunction> showList)
        {
            if (!isPlayingAnimation)
            {
                isPlayingAnimation = true;
                OpenFunctionsData data = new OpenFunctionsData();
                data.lobby = mainForm;
                data.itemList = new List<DROpenFunction>(showList);
                showList.Clear();
                GameEntry.UI.OpenUIForm(UIFormId.OpenFunctionDialog, data);
            }
        }

        /// <summary>
        /// 主界面显示用
        /// </summary>
        public void ShowLobbyOpenFunction(NGUIForm form, Dictionary<int, List<DROpenFunction>> showGroupList)
        {
            if (mainForm == null)
            {
                mainForm = form;
            }
            int[] keys = showGroupList.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                StartCoroutine(RefreshOpenFunction(showGroupList[keys[i]]));
            }
        }

        private IEnumerator RefreshOpenFunction(List<DROpenFunction> groupList)
        {
            string functionPath = groupList[0].FunctionPath;
            Transform grid = mainForm.transform.Find(functionPath.Substring(0, functionPath.LastIndexOf("/")));
            for (int i = 0; i < grid.childCount; i++)
            {
                Transform gridChild = grid.GetChild(i);
                //根据表，找到对应的GameObject，将比较组件赋予
                for (int j = 0; j < groupList.Count; j++)
                {
                    Transform dataTranform = mainForm.transform.Find(groupList[j].FunctionPath);
                    if (dataTranform == gridChild)
                    {
                        OpenFunctionData perData = gridChild.gameObject.GetComponent<OpenFunctionData>();
                        if (perData == null)
                        {
                            perData = gridChild.gameObject.AddComponent<OpenFunctionData>();
                        }
                        perData.data = groupList[j];
                        gridChild.gameObject.SetActive(perData.data.isOpen);
                        break;
                    }
                    else
                    {   //现在把prefab多余的GameObject设成false
                        if (j == groupList.Count - 1) gridChild.gameObject.SetActive(false);
                    }
                }
            }
            UIGrid gridComponent = grid.GetComponent<UIGrid>();
            //Log.Debug("grid:" + grid.name);
            gridComponent.enabled = true;
            gridComponent.sorting = UIGrid.Sorting.Custom;
            gridComponent.onCustomSort = CustomSort;
            gridComponent.repositionNow = true;
            gridComponent.Reposition();
            yield return 0;//下一帧
        }

        private int CustomSort(Transform orgTransform, Transform compareTransform)
        {
            int pos1 = orgTransform.GetComponent<OpenFunctionData>().data.GroupIndex;
            int pos2 = compareTransform.GetComponent<OpenFunctionData>().data.GroupIndex;
            if (orgTransform.GetComponent<OpenFunctionData>().data.FunctionGroup == 1)
            {
                return pos1 - pos2;
            }
            else
            {
                return pos2 - pos1;
            }
        }

        #endregion

        #region 对外API
        /// <summary>
        /// 判断功能是否开放
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public bool CheckFunctionIsOpen(int functionID)
        {
            for (int i = 0; i < orgFunctionList.Count; i++)
            {
                if (orgFunctionList[i].Id == functionID)
                {
                    return orgFunctionList[i].isOpen;
                }
            }
            Log.Error("CheckFunctionIsOpen error functionID:" + functionID);
            return false;
        }

        public bool CheckHeroAlbumCard()
        {
            if (!operation) return true;
            DROpenFunction[] values = heroDict.Values.ToArray();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].isOpen) return true;
            }
            GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FUNCTION_OPEN_TASK") });
            return false;
        }

        public bool CheckHeroToggle(HeroInfoForm_Possessed.TabType type)
        {
            //检测当前任务组，判断是否开启
            if (!heroDict[type].isOpen)
            {
                return false;
            }
            else return true;
        }

        public void SetIsFirstCharge(bool flag)
        {
            if (isFirstCharge == flag) return;
            isFirstCharge = flag;
            for (int i = 0; i < orgFunctionList.Count; i++)
            {//首冲礼包ID
                if ((Function)orgFunctionList[i].Id == Function.FirstCharge)
                {
                    orgFunctionList[i].isOpen = flag;
                    break;
                }
            }
        }

        public bool IsFirstCharge()
        {
            return isFirstCharge;
        }

        /// <summary>
        /// 显示已开放的子页签
        /// </summary>
        /// <param name="parentId">主功能号ID</param>
        /// <returns>已排序已开放的功能列表</returns>
        public List<DROpenFunction> ShowSubTabFunction(int parentId)
        {
            List<DROpenFunction> list = new List<DROpenFunction>();
            for (int i = 0; i < orgFunctionList.Count; i++)
            {
                if (orgFunctionList[i].ParentId == parentId && orgFunctionList[i].isOpen)
                {
                    list.Add(orgFunctionList[i]);
                }
            }
            return list;
        }

        public Dictionary<int, List<DROpenFunction>> GetLobbyOpenFunction()
        {
            return openGroupDict;
        }

        #endregion


        #region GM命令

        public void SwtichOpenFunction(bool flag)
        {
            operation = flag;
            Log.Debug("SwtichOpenFunction " + flag);
            SetOperation(flag);
            if (flag)
            {
                ShowLobbyOpenFunction(mainForm, openGroupDict);
            }
            else
            {
                Dictionary<int, List<DROpenFunction>> gmGroupDict = InitAllDict();
                ShowLobbyOpenFunction(mainForm, gmGroupDict);
            }
        }

        private Dictionary<int, List<DROpenFunction>> InitAllDict()
        {
            DROpenFunction[] funArr = GameEntry.DataTable.GetDataTable<DROpenFunction>().GetAllDataRows();
            Dictionary<int, DROpenFunction> tmpDict = new Dictionary<int, DROpenFunction>();
            List<DROpenFunction> tmpList = new List<DROpenFunction>();
            for (int i = 0; i < funArr.Length; i++) tmpDict.Add(funArr[i].Id, funArr[i]);
            for (int i = 0; i < serverOpenList.Count; i++)
            {
                //验证服务器数据合法性
                if (tmpDict.ContainsKey(serverOpenList[i]))
                {
                    DROpenFunction per = tmpDict[serverOpenList[i]];
                    per.isOpen = true;
                    tmpList.Add(per);
                }
            }
            Dictionary<int, List<DROpenFunction>> gmGroupDict = new Dictionary<int, List<DROpenFunction>>();
            //首先判断是否有FunctionGroup,根据值拿对应的Grid,然后排序
            for (int i = 0; i < tmpList.Count; i++)
            {
                DROpenFunction item = tmpList[i];
                if (item.FunctionGroup == -1) continue;
                if (!gmGroupDict.ContainsKey(item.FunctionGroup))
                {
                    gmGroupDict.Add(item.FunctionGroup, new List<DROpenFunction>());
                }
                gmGroupDict[item.FunctionGroup].Add(item);
            }
            //根据GroupIndex排序：小到大
            int[] keyStr = gmGroupDict.Keys.ToArray();
            for (int i = 0; i < keyStr.Length; i++)
            {
                gmGroupDict[keyStr[i]] = gmGroupDict[keyStr[i]].OrderBy(x => x.GroupIndex).ToList();
            }
            return gmGroupDict;
        }

        private void SetOperation(bool flag)
        {
            GameEntry.Setting.SetBool("OpenFunctionUI" + GameEntry.Data.Player.Id.ToString(), flag);
        }

        public bool GetOperation()
        {
            return GameEntry.Setting.GetBool("OpenFunctionUI" + GameEntry.Data.Player.Id.ToString(), true);
        }

        /// <summary>
        /// 提高开发测试体验
        /// </summary>
        private List<DROpenFunction> InitAllFunctions()
        {
            DROpenFunction[] functionArrays = GameEntry.DataTable.GetDataTable<DROpenFunction>().GetAllDataRows();
            Dictionary<int, DROpenFunction> tmpDict = new Dictionary<int, DROpenFunction>();
            List<DROpenFunction> orgFunctionList = new List<DROpenFunction>();
            for (int i = 0; i < functionArrays.Length; i++)
            {
                tmpDict.Add(functionArrays[i].Id, functionArrays[i]);
            }
            for (int i = 0; i < functionArrays.Length; i++)
            {
                //验证服务器数据合法性
                if (tmpDict.ContainsKey(functionArrays[i].Id))
                {
                    DROpenFunction per = tmpDict[functionArrays[i].Id];
                    per.isOpen = true;
                    orgFunctionList.Add(per);
                }
            }
            return orgFunctionList;
        }

        #endregion
    }

}

