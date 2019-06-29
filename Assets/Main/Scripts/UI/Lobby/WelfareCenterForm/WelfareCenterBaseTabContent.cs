using UnityEngine;
using System.Collections;
using GameFramework.Event;
using GameFramework;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 福利中心内容基类
    /// </summary>
    public abstract class WelfareCenterBaseTabContent : MonoBehaviour
    {
        /// <summary>
        /// Item模板
        /// </summary>
        [SerializeField]
        protected GameObject m_ItemTemplate = null;

        [SerializeField]
        protected UIScrollView m_ScrollView = null;

        [SerializeField]
        protected UIGrid m_Grid = null;

        private UIResourceReleaser m_ResourceReleaser = new UIResourceReleaser();

        public static GameFrameworkAction<object> onPaySuccess = null;

        public static string Itemdata = null;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
            OnOpen(this.gameObject);
        }
        protected virtual void OnOpen(GameObject obj)
        {
            StartCoroutine(RefreshData());
        }
        protected abstract IEnumerator RefreshData();
        #region MonoBehaviour
        private void Start()
        {
            m_ResourceReleaser.CollectWidgets(gameObject);
            SubscribeEvents();
        }
        private void OnEnable()
        {
            ChargeStatusData ne = GameEntry.Data.ChargeStatusData;
            if (ne.StatusData != null)
            {
                UpdataItemStatus(ne.StatusData);
            }
            //SubscribeEvents();
        }
        private void OnDisable()
        {
            //UnsubscribeEvents();
        }
        private void OnDestroy()
        {
            UnsubscribeEvents();
            m_ResourceReleaser.ReleaseResources();
            OnClose();
        }
        protected virtual void OnClose() { }
        #endregion

        protected virtual void SubscribeEvents()
        {
            GameEntry.Event.Subscribe(EventId.GetItemStatus, GetItemSuatus);
            GameEntry.Event.Subscribe(EventId.ReceiveBuyItem, OnReceiveBuyItem);
        }



        protected virtual void UnsubscribeEvents()
        {
            GameEntry.Event.Unsubscribe(EventId.GetItemStatus, GetItemSuatus);
            GameEntry.Event.Unsubscribe(EventId.ReceiveBuyItem, OnReceiveBuyItem);
        }
        protected virtual void OnReceiveBuyItem(object sender, GameEventArgs e)
        {
            ReceivePayItemEventArgs ne = e as ReceivePayItemEventArgs;
            GameEntry.RewardViewer.RequestShowRewards(ne.CompoundItemInfo, false, onPaySuccess, Itemdata);
            for (int i = 0; i < ne.CompoundItemInfo.Items.Count; i++)
            {
                Log.Info("Receive Buy Item:Type{0}:Count{1}", ne.CompoundItemInfo.Items[i].Type, ne.CompoundItemInfo.Items[i].Count);
            }
        }

        public static void OnPaySuccess(object obj)
        {
            Itemdata = null;
            onPaySuccess = null;
            SDKManager.Instance.helper.Record("Pay", obj.ToString());
        }

        /// <summary>
        /// 获取充值物品的状态的回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void GetItemSuatus(object sender, GameEventArgs e) { }

        /// <summary>
        /// 初始化之后做状态更新(礼包类)
        /// </summary>
        /// <param name="ne"></param>
        protected virtual void UpdataItemStatus(ChargeStatus ne) { }
    }
}

