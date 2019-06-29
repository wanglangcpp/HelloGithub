using UnityEngine;
using System.Collections;
using GameFramework.Event;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 每日签到
    /// </summary>
    public class EveryDaySignInForm : WelfareCenterBaseTabContent
    {
        [SerializeField]
        private GameObject BoxPrent = null;
        [SerializeField]
        private DailyQuestRewardChestItem m_BoxTemplates = null;

        [SerializeField]
        private UIProgressBar m_Progress = null;
        [SerializeField]
        private UISprite m_ProgressArea = null;

        [SerializeField]
        private EveryDaySignInScrollView m_ItemScrollView = null;


        private List<int> boxList = new List<int>();
        private EveryDaySignInItem m_TodayItem = null;
        private Dictionary<int, DailyQuestRewardChestItem> boxItems = new Dictionary<int, DailyQuestRewardChestItem>();
        private Dictionary<int, EveryDaySignInItem> signItems = new Dictionary<int, EveryDaySignInItem>();
        /// <summary>
        /// 领取的奖励所在天的Id
        /// </summary>
        [HideInInspector]
        public static int ClaimDay = 0;
        protected override void OnOpen(GameObject obj)
        {
            //m_ItemScrollView.InitDragEvent();
            boxList = GameEntry.Data.EveryDaySignInData.ListCumulativeBox;
            base.OnOpen(obj);

        }
        protected override void OnClose()
        {
            base.OnClose();
            //m_ItemScrollView.DiposeDragEvent();
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            GameEntry.Event.Subscribe(EventId.DailyLogined, OnLogined);
            GameEntry.Event.Subscribe(EventId.ClaimSignInBoxSuccess, OnClaimBoxSuccess);
            GameEntry.Event.Subscribe(EventId.RetroactiveSuccess, OnRetroactiveSuccess);
            RefreshBoxStatus();
            //m_TodayItem.RefreshIconState();
        }
        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            GameEntry.Event.Unsubscribe(EventId.DailyLogined, OnLogined);
            GameEntry.Event.Unsubscribe(EventId.ClaimSignInBoxSuccess, OnClaimBoxSuccess);
            GameEntry.Event.Unsubscribe(EventId.RetroactiveSuccess, OnRetroactiveSuccess);
        }
        /// <summary>
        /// 补签成功回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRetroactiveSuccess(object sender, GameEventArgs e)
        {
            RefreshBoxStatus();
            if (ClaimDay > 0)
                signItems[ClaimDay].RefreshIconState();
            ClaimDay = 0;
        }

        /// <summary>
        /// 签到成功回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLogined(object sender, GameEventArgs e)
        {
            RefreshBoxStatus();
            if (m_TodayItem != null)
                m_TodayItem.RefreshIconState();

            var showData = e as DailyLoginedEventArgs;
            if (showData == null)
                return;
            GameEntry.RewardViewer.RequestShowRewards(showData.ShowRewards.ReceiveGoodsData, true);
        }
        /// <summary>
        /// 领取宝箱成功回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClaimBoxSuccess(object sender, GameEventArgs e)
        {
            RefreshBoxStatus();
        }
        protected override IEnumerator RefreshData()
        {
            Dictionary<int, DRDailyLogin> tableData = GameEntry.Data.EveryDaySignInData.DicTableData;
            foreach (var item in tableData)
            {
                var script = m_ItemScrollView.GetOrCreateItem(item.Key);
                script.SetItemData(GameEntry.Data.EveryDaySignInData.DicTableData[item.Key]);
                signItems.Add(item.Key, script);
                if (script.IsToday)
                    m_TodayItem = script;
            }
            AddCumulativeBox();
            yield return null;
        }
        /// <summary>
        /// 添加累积签到宝箱
        /// </summary>
        private void AddCumulativeBox()
        {
            for (int i = 0; i < boxList.Count; i++)
            {
                GameObject go = NGUITools.AddChild(BoxPrent, m_BoxTemplates.gameObject);
                go.name = boxList[i].ToString();
                DailyQuestRewardChestItem boxItem = go.GetComponent<DailyQuestRewardChestItem>();
                boxItems.Add(boxList[i], boxItem);
                UIEventListener.Get(go).onClick = OnClickBox;

                //bool IsOpen = (boxStatus & (1 << boxList[i])) != 0;
                //int thisStatus = IsOpen ? boxList[i] <= GameEntry.Data.EveryDaySignInData.ClaimCount ? 1 : 0 : 2;
                //SetBox(boxItem, boxList[boxList.Count - 1], boxList[i], IsOpen, i);
                //UIIntKey key = go.GetComponent<UIIntKey>();
                //if (key == null)
                //    key = go.AddComponent<UIIntKey>();
                //key.Key = thisStatus;
                //go.name = boxList[i].ToString();
                //UIEventListener.Get(go).onClick = OnClickBox;
                //UIParticle uIParticle = go.GetComponent<UIParticle>();
                //if (uIParticle == null)
                //    continue;
                //if (thisStatus == 1)
                //    uIParticle.Play(-1);
                //else
                //    uIParticle.Stop();
            }
            m_BoxTemplates.gameObject.SetActive(false);
            RefreshBoxStatus();
        }
        /// <summary>
        /// 更新宝箱状态
        /// </summary>
        private void RefreshBoxStatus()
        {
            m_Progress.value = (float)GameEntry.Data.EveryDaySignInData.ClaimCount / boxList[boxList.Count - 1];
            var boxStatus = GameEntry.Data.EveryDaySignInData.BoxStatus;
            for (int i = 0; i < boxList.Count; i++)
            {
                if (boxItems.ContainsKey(boxList[i]))
                {
                    var item = boxItems[boxList[i]];
                    bool IsOpen = (boxStatus & (1 << boxList[i])) != 0;
                    int thisStatus = !IsOpen ? boxList[i] <= GameEntry.Data.EveryDaySignInData.ClaimCount ? 1 : 0 : 2;
                    SetBox(item, boxList[boxList.Count - 1], boxList[i], IsOpen, i);
                    UIIntKey key = item.gameObject.GetComponent<UIIntKey>();
                    if (key == null)
                        key = item.gameObject.AddComponent<UIIntKey>();
                    key.Key = thisStatus;

                    UIParticle uIParticle = item.gameObject.GetComponent<UIParticle>();
                    if (uIParticle == null)
                        continue;
                    if (thisStatus == 1)
                    {
                        uIParticle.playOnAwake = true;
                        uIParticle.Play(-1);
                    }
                    else
                    {
                        uIParticle.playOnAwake = false;
                        uIParticle.Stop();
                    }

                }
            }
        }

        private void SetBox(DailyQuestRewardChestItem chestItem, int maxActiveness, int activeness, bool isOpen, int index)
        {
            m_ProgressArea.ResetAndUpdateAnchors();
            chestItem.Refresh(maxActiveness, activeness, isOpen);
            var width = chestItem.GetComponent<UIWidget>().width;
            float progressValue = (float)(index + 1) / boxList.Count;
            chestItem.transform.localPosition = new Vector3(m_ProgressArea.width * progressValue - width / 2, m_BoxTemplates.transform.localPosition.y, m_BoxTemplates.transform.localPosition.z);
        }
        private void OnClickBox(GameObject go)
        {
            int index = 0;
            bool BoxIsOK = int.TryParse(go.name, out index);
            int key = go.GetComponent<UIIntKey>().Key;
            if (key == 1 && BoxIsOK)
            {
                CLClaimDailyLoginGift sender = new CLClaimDailyLoginGift();
                sender.ClaimDay = index;
                GameEntry.Network.Send(sender);
                return;
            }
            List<ChestRewardDisplayData.Reward> rewards = new List<ChestRewardDisplayData.Reward>();
            List<PBItemInfo> listRewards = GameEntry.Data.EveryDaySignInData.DataTable.GetDataRow(index).BoxRewards;
            for (int i = 0; i < listRewards.Count; i++)
                rewards.Add(new ChestRewardDisplayData.Reward(listRewards[i].Type, listRewards[i].Count));
            GameEntry.UI.OpenUIForm(UIFormId.ChestRewardSubForm, new ChestRewardDisplayData(rewards, GameEntry.Localization.GetString("UI_TEXT_PROMPT"), GameEntry.Localization.GetString("UI_TEXT_CUMULATIVE_IN_SIGNIN", index)));
        }
        [Serializable]
        private class EveryDaySignInScrollView : UIScrollViewCache<EveryDaySignInItem>
        {
            //public void InitDragEvent()
            //{
            //    m_ScrollView.onDragStarted = () =>
            //    {
            //        for (int i = 0; i < m_ItemParent.transform.childCount; i++)
            //        {
            //            var child = m_ItemParent.transform.GetChild(i).GetComponent<EveryDaySignInItem>();
            //            child.ShowEffect(false);
            //        }
            //    };

            //    m_ScrollView.onDragFinished = () =>
            //    {
            //        if (m_ItemParent.transform.childCount <= 0)
            //            return;

            //        float yOffset = m_ScrollView.panel.clipOffset.y;

            //        float yButtom = m_ScrollView.panel.baseClipRegion.w;

            //        float perHeight = NGUIMath.CalculateRelativeWidgetBounds(m_ItemParent.transform.GetChild(0).transform).size.y;
            //        int scrollLine = yOffset > 0 ? (int)Math.Floor((yOffset) / perHeight) : (int)Math.Ceiling(Math.Abs(yOffset) / perHeight);
            //        int perLineCount = m_ItemParent.GetComponent<UIGrid>().maxPerLine;
            //        int showingLine = (int)Math.Floor(yButtom / perHeight);

            //        if ((Math.Abs(yOffset) % perHeight) < (perHeight / 2))
            //        {
            //            showingLine--;
            //        }

            //        for (int i = 0; i < m_ItemParent.transform.childCount; i++)
            //        {
            //            var child = m_ItemParent.transform.GetChild(i).GetComponent<EveryDaySignInItem>();
            //            if (i < scrollLine * perLineCount)
            //            {
            //                child.ShowEffect(false);
            //            }
            //            else
            //            {
            //                if (i >= (scrollLine + showingLine) * perLineCount)
            //                {
            //                    child.ShowEffect(false);
            //                }
            //                else
            //                {
            //                    child.RefreshIconState();
            //                }
            //            }
            //        }
            //    };

            //}

            //public void DiposeDragEvent()
            //{
            //    m_ScrollView.onDragStarted = null;
            //    m_ScrollView.onDragFinished = null;
            //}
        }
    }
}

