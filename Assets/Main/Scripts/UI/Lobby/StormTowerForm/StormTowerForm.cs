using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework;
using GameFramework.Event;
using System;

namespace Genesis.GameClient
{
    public partial class StormTowerForm : NGUIForm
    {
        private class Item
        {
            public int ItemIcon;
            public int ItemCount;
        }
        /// <summary>
        /// 层间距
        /// </summary>
        private const int m_Interval = 312;

        [SerializeField]
        private GameObject m_TowerPrent = null;
        /// <summary>
        /// 塔层——左侧
        /// </summary>
        [SerializeField]
        private GameObject m_LayerItemLeft = null;
        /// <summary>
        /// 塔层——右侧
        /// </summary>
        [SerializeField]
        private GameObject m_LayerItemRight = null;
        /// <summary>
        /// 最高记录
        /// </summary>
        [SerializeField]
        private UILabel m_BestRecord = null;
        /// <summary>
        /// 游戏玩法
        /// </summary>
        [SerializeField]
        private UILabel m_GameIntroduction = null;
        /// <summary>
        /// 关卡介绍
        /// </summary>
        [SerializeField]
        private UILabel m_LayerIntroduction = null;
        /// <summary>
        /// 奖励
        /// </summary>
        [SerializeField]
        private GeneralItemView[] m_Rewards = null;
        /// <summary>
        /// 挑战次数
        /// </summary>
        [SerializeField]
        private UILabel m_ChallengeCount = null;
        /// <summary>
        /// 结束扫荡
        /// </summary>
        [SerializeField]
        private GameObject m_BtnOverMoppongUp = null;
        /// <summary>
        /// 扫荡
        /// </summary>
        [SerializeField]
        private GameObject m_BtnMoppingUp = null;
        /// <summary>
        /// 挑战
        /// </summary>
        [SerializeField]
        private GameObject m_BtnFight = null;
        /// <summary>
        /// 领奖窗口
        /// </summary>
        [SerializeField]
        private ClaimReward m_ClaimRewardWindow = null;
        ///// <summary>
        ///// 扫荡窗口
        ///// </summary>
        //[SerializeField]
        //private GameObject m_MoppingUpWindow = null;

        [SerializeField]
        private UIScrollView m_ScrollView = null;
        [SerializeField]
        private UITable m_Table = null;

        //[SerializeField]
        //private UITexture m_MapBg = null;
        /// <summary>
        /// 风暴之塔的配表数据
        /// </summary>
        private DRInstanceForTower[] m_AllDataTable = null;
        private IDataTable<DRInstanceForTower> m_DRDataTable = null;

        private Dictionary<int, TowerLayerItem> m_AllTowerLayerItem = new Dictionary<int, TowerLayerItem>();

        private Dictionary<int, List<Item>> SumCurrentLayerReward = new Dictionary<int, List<Item>>();

        /// <summary>
        /// 当前所在层
        /// </summary>
        private int m_CurrentLay;

        /// <summary>
        /// 当前完成的最高层
        /// </summary>
        private int m_FinishLayer;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_AllDataTable = GameEntry.DataTable.GetDataTable<DRInstanceForTower>().GetAllDataRows();
            m_DRDataTable = GameEntry.DataTable.GetDataTable<DRInstanceForTower>();
            //if (m_MapBg == null)
            //    m_MapBg = m_ScrollView.transform.parent.GetComponent<UITexture>();
            //m_MapBg.depth = 3;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //m_CurrentLay = GameEntry.Data.StromTowerData.StromTowerInfo.CurLayerNum > 0 ? GameEntry.Data.StromTowerData.StromTowerInfo.CurLayerNum : 1;
            m_FinishLayer = GameEntry.Data.StromTowerData.StromTowerInfo.CurLayerNum;
            m_CurrentLay = Mathf.Clamp(m_FinishLayer + 1, m_DRDataTable.MinIdDataRow.Id, m_DRDataTable.MaxIdDataRow.Id);

            m_LayerItemLeft.SetActive(false);
            m_LayerItemRight.SetActive(false);
            m_ClaimRewardWindow.m_SelfWindow.SetActive(false);
            m_GameIntroduction.text = GameEntry.Localization.GetString("UI_TEXT_TOWER_INTRODUCTION");
            UIEventListener.Get(m_BtnOverMoppongUp).onClick = OnClickOverMoppingUp;
            UIEventListener.Get(m_BtnMoppingUp).onClick = OnClickMoppingUp;
            UIEventListener.Get(m_BtnFight).onClick = OnClickFight;
            SubscribeEvents();
            RefreshCurrentLayerData();
            StartCoroutine(RefreshTowerLay());
            StartCoroutine(CumSumLayerReward());
        }

        private void SubscribeEvents()
        {
            GameEntry.Event.Subscribe(EventId.StromTowerDataChange, StromTowerDataChange);
        }
        private void UnsubscribeEvents()
        {
            GameEntry.Event.Unsubscribe(EventId.StromTowerDataChange, StromTowerDataChange);
        }
        private void StromTowerDataChange(object sender, GameEventArgs e)
        {
            StromTowerInfoEventArgs args = e as StromTowerInfoEventArgs;
            foreach (var item in m_AllTowerLayerItem)
            {
                item.Value.SetBoxStatus(item.Key, args.BoxStatus);
            }
            m_AllTowerLayerItem[m_CurrentLay].RefreshLayerData(m_AllDataTable[m_CurrentLay - 1]);
            m_AllTowerLayerItem[1].RefreshLayerData(m_AllDataTable[0]);
            m_FinishLayer = GameEntry.Data.StromTowerData.StromTowerInfo.CurLayerNum;
            m_CurrentLay = Mathf.Clamp(m_FinishLayer + 1, m_DRDataTable.MinIdDataRow.Id, m_DRDataTable.MaxIdDataRow.Id);
            RefreshCurrentLayerData();
        }

        /// <summary>
        /// 刷新当前爬塔数据
        /// </summary>
        private void RefreshCurrentLayerData()
        {
            m_ChallengeCount.text = GameEntry.Localization.GetString("UI_TEXT_TOWER_FIGHT", (2 - GameEntry.Data.StromTowerData.StromTowerInfo.ChallengeNum));
            m_BestRecord.text = GameEntry.Localization.GetString("UI_TEXT_TOWER_RECORD", GameEntry.Data.StromTowerData.StromTowerInfo.MaxLayerNum);
            var currentInstance = m_AllDataTable[m_CurrentLay - 1].CheckPoint;
            var possibleDrops = GameEntry.DataTable.GetDataTable<DRInstance>().GetDataRow(currentInstance).Description;
            m_LayerIntroduction.text = GameEntry.Localization.GetString(possibleDrops);
            m_BtnFight.SetActive(m_FinishLayer < m_DRDataTable.MaxIdDataRow.Id);
            #region 可能获得的物品
            //var currentInstance = m_AllDataTableData[m_CurrentLay - 1].CheckPoint;
            //var possibleDrops = GameEntry.DataTable.GetDataTable<DRInstance>().GetDataRow(currentInstance).PossibleDrops;
            //for (int i = 0; i < m_Rewards.Length; ++i)
            //{
            //    if (i < possibleDrops.Length)
            //    {
            //        m_Rewards[i].gameObject.SetActive(true);
            //        m_Rewards[i].InitItem(possibleDrops[i].ItemId);
            //    }
            //    else
            //    {
            //        m_Rewards[i].gameObject.SetActive(false);
            //    }
            //}
            #endregion
        }
        /// <summary>
        /// 加载所有塔层
        /// </summary>
        private IEnumerator RefreshTowerLay()
        {
            for (int i = 0; i < m_AllDataTable.Length; i++)
            {
                GameObject obj = null;
                if (m_AllDataTable[i].Id % 2 != 0)
                    obj = NGUITools.AddChild(m_TowerPrent, m_LayerItemLeft);
                else
                    obj = NGUITools.AddChild(m_TowerPrent, m_LayerItemRight);
                obj.SetActive(true);
                obj.name = m_AllDataTable[i].Id.ToString();
                TowerLayerItem script = obj.GetComponent<TowerLayerItem>();
                script.RefreshLayerData(m_AllDataTable[i]);
                m_AllTowerLayerItem.Add(m_AllDataTable[i].Id, script);
            }
            m_Table.enabled = true;
            yield return null;
        }
        /// <summary>
        /// 计算在各个塔层结束时的累计奖励
        /// </summary>
        private IEnumerator CumSumLayerReward()
        {
            yield return null;
            yield return null;
            yield return null;
            SumCurrentLayerReward.Add(1, new List<Item>());
            Dictionary<int, int> rewardItems = new Dictionary<int, int>();
            for (int i = 1; i < m_AllDataTable.Length; i++)
            {
                List<Item> itemList = new List<Item>();
                foreach (var item in m_AllDataTable[i].LayItems)
                {
                    if (rewardItems.ContainsKey(item.ItemIcon))
                        rewardItems[item.ItemIcon] += item.ItemCount;
                    else
                        rewardItems.Add(item.ItemIcon, item.ItemCount);
                }
                foreach (var item in rewardItems)
                    itemList.Add(new Item() { ItemIcon = item.Key, ItemCount = item.Value });
                if (SumCurrentLayerReward.ContainsKey(i + 1))
                    SumCurrentLayerReward[i + 1] = itemList;
                else
                    SumCurrentLayerReward.Add(i + 1, itemList);
            }
        }

        /// <summary>
        /// 结束扫荡
        /// </summary>
        /// <param name="go"></param>
        private void OnClickOverMoppingUp(GameObject go)
        {
            if (SumCurrentLayerReward.Count != 0)
            {
                m_ClaimRewardWindow.RefreshReward(SumCurrentLayerReward[m_CurrentLay]);
            }
        }
        private void OnClickMoppingUp(GameObject go)
        {

        }
        /// <summary>
        /// 开始挑战
        /// </summary>
        /// <param name="go"></param>
        private void OnClickFight(GameObject go)
        {
            if (GameEntry.Data.StromTowerData.StromTowerInfo.ChallengeNum >= 2)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_CHALLENGE_RUN_OUT"));
                return;
            }
            CLEnterInstanceForTower response = new CLEnterInstanceForTower();
            response.InstanceId = GameEntry.DataTable.GetDataTable<DRInstanceForTower>().GetDataRow(m_CurrentLay).CheckPoint;
            GameEntry.Network.Send(response);
        }
        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            UnsubscribeEvents();
        }
    }
}

