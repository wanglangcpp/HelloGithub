using UnityEngine;
using System;
using GameFramework.Event;
using System.Collections.Generic;
using System.Collections;

namespace Genesis.GameClient
{
    public class SweepResult : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_SweptCountLabel = null;

        [SerializeField]
        private GeneralItemView m_WannaGetItem = null;

        [SerializeField]
        private GameObject m_WannaGetRoot = null;

        [SerializeField]
        private UILabel m_WannaGetItemInfoLabel = null;

        [SerializeField]
        private ResultScrollView m_ResultScrollView = null;

        [SerializeField]
        private UIToggle m_AutoSweepToggle = null;

        [SerializeField]
        private UIButton m_SweepButton = null;

        [SerializeField]
        private GameObject m_MaskObject = null;

        [SerializeField]
        private UILabel m_ButtonTextLabel = null;

        [SerializeField]
        private UILabel m_AutoSweepTipsLabel = null;

        [SerializeField]
        private float m_ItemDisplayTime;

        [SerializeField]
        private float m_ItemScrollStrength;

        [SerializeField]
        private GameObject m_SweepFinishedAnimationObject = null;

        [SerializeField]
        private Animation m_SweepFinishedAnimation = null;

        private SweepDisplayData m_DisplayData = null;

        /// <summary>
        /// Show的时候会将RemainCount置为m_DisplayData.MaxSweepCount
        /// 每收到一次扫荡的数据包，RemainCount--;
        /// </summary>
        private int m_RemainSweepCount = 0;

        private int m_RealSweepCount = 0;

        /// <summary>
        /// 已经获得的指定物品的数量，当获得的数量大于想要获得的数量的时候，停止扫荡
        /// </summary>
        private int m_GotAppointedItemCount = 0;

        /// <summary>
        /// 扫荡成功后，当前条目停留显示的计时器
        /// </summary>
        private float m_CurrentDisplayTime = 0f;

        private const float CacheDefaultDisplayTime = 0.6f;

        private bool m_EnableSweep = false;
        private bool m_IsFinished = false;
        private bool m_EnableNextRequest = true;

        /// <summary>
        /// 当玩家获得的物品足够的时候，还想继续扫荡，那么就继续扫，此时这个值才会为True
        /// </summary>
        private bool m_IsForceSweep = false; 

        public Action OnHideAction = null;

        public void Initialize()
        {
            gameObject.SetActive(false);
            m_SweepFinishedAnimationObject.gameObject.SetActive(false);
        }

        public void Show(SweepDisplayData displayData, bool isAutoSweep)
        {
            GameEntry.Event.Subscribe(EventId.CleanOutInstance, OnSweepSuccess);
            gameObject.SetActive(true);

            m_DisplayData = displayData;
            m_AutoSweepToggle.Set(isAutoSweep);

            m_ButtonTextLabel.text = GameEntry.Localization.GetString("UI_BUTTON_INSTANCE_START_SETTLEMENT");

            m_ResultScrollView.SetScrollViewAutoSpringStrength(m_ItemScrollStrength);
            m_ResultScrollView.Prepare();

            m_GotAppointedItemCount = 0;

            UIEventListener.Get(m_MaskObject).onClick = OnMaskClick;

            ResetSweepStatus();
        }

        public void Hide()
        {
            GameEntry.Event.Unsubscribe(EventId.CleanOutInstance, OnSweepSuccess);

            m_ResultScrollView.RecycleAllItems();
            m_ResultScrollView.ResetGridPosition();

            gameObject.SetActive(false);

            if (OnHideAction != null)
                OnHideAction.Invoke();

        }

        public void OnContinueSweepClick()
        {
            if (CheckLevelEnable())
                ResetSweepStatus();
        }

        protected void Update()
        {
            if (!m_EnableSweep)
                return;

            // 扫荡次数达到了最大
            if (m_RemainSweepCount <= 0)
            {
                OnSweepFinished();
                return;
            }

            // 已经获得到了想要Item的数量
            if (m_DisplayData.SweepEntranceType == SweepDisplayData.ShowType.WhereToGet
                && m_GotAppointedItemCount >= m_DisplayData.WannaGetItemCount && m_IsForceSweep == false)
            {
                OnSweepFinished();
                return;
            }

            // 显示时间没有达到设置的显示时间，则返回
            if (m_CurrentDisplayTime <= m_ItemDisplayTime)
            {
                m_CurrentDisplayTime += Time.deltaTime;
                return;
            }

            if (m_EnableNextRequest)
            {
                m_EnableNextRequest = false;
                GameEntry.LobbyLogic.SweepLevel(m_DisplayData.LevelId);
            }
        }

        private bool CheckLevelEnable()
        {
            var levelData = GameEntry.Data.InstanceGroups.GetLevelById(m_DisplayData.LevelId);

            // 副本得到的星数小于3则提示是否打该副本
            if (levelData.StarCount < 3)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_STAR_INSUFFICIENT"),
                    OnClickConfirm = (o) =>
                    {
                        GameEntry.UI.OpenUIForm(UIFormId.InstanceInfoForm, new InstanceInfoDisplayData { InstanceId = m_DisplayData.LevelId });
                        Hide();
                    },
                    OnClickCancel = (o) => { Hide(); },
                });

                return false;
            }

            // 体力是否满足条件
            int sweepOnceCostEnergy = 6; // GameEntry.ServerConfig.GetInt(Constant.ServerConfig.EnergyPerInstance, 6);
            int needEnergy = m_AutoSweepToggle.value ? sweepOnceCostEnergy * m_DisplayData.MaxSweepCount : sweepOnceCostEnergy;

            if (!UIUtility.CheckEnergy(needEnergy))
                return false;

            return true;
        }

        private void ResetSweepStatus()
        {
            m_AutoSweepToggle.GetComponent<BoxCollider>().enabled = false;
            m_SweepButton.isEnabled = false;

            if (m_DisplayData.SweepEntranceType == SweepDisplayData.ShowType.InstanceAuto)
            {
                m_WannaGetRoot.SetActive(false);
            }
            else if (m_DisplayData.SweepEntranceType == SweepDisplayData.ShowType.WhereToGet)
            {
                int needCount = m_DisplayData.WannaGetItemCount - m_GotAppointedItemCount;
                //var itemInBagInfo = GameEntry.Data.HeroQualityItems.GetData(m_DisplayData.WannaGetItemId);
                //if (itemInBagInfo == null)
                //    needCount = m_DisplayData.WannaGetItemCount;
                //else
                //    needCount = m_DisplayData.WannaGetItemCount - itemInBagInfo.Count;

                if (needCount > 0)    // 当玩家的物品数量充足的时候，默认扫荡10次，并且不显示还差几个
                {
                    m_WannaGetItem.InitGeneralItem(m_DisplayData.WannaGetItemId);

                    var item = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>().GetDataRow(m_DisplayData.WannaGetItemId);
                    string name = ColorUtility.AddColorToString(ColorUtility.GetColorForQuality(item.Quality), GameEntry.Localization.GetString(item.Name));
                    string count = ColorUtility.AddColorToString(Constant.Quality.Red, needCount.ToString());
                    m_WannaGetItemInfoLabel.text = GameEntry.Localization.GetString("UI_TEXT_NEED_ITEM", name, m_GotAppointedItemCount, count);

                    m_IsForceSweep = false;
                    m_WannaGetRoot.SetActive(true);
                }
                else
                {
                    m_IsForceSweep = true;
                    m_WannaGetRoot.SetActive(false);
                }
            }

            // 保证第一次直接发送请求
            m_CurrentDisplayTime = m_ItemDisplayTime;

            m_ResultScrollView.RecycleAllItems();
            m_ResultScrollView.ResetGridPosition();

            m_AutoSweepTipsLabel.text = GameEntry.Localization.GetString("UI_TITLE_NAME_CLEANOUTTIMES", ColorUtility.AddColorToString(Constant.Quality.Orange, m_DisplayData.MaxSweepCount.ToString()));
            m_SweptCountLabel.text = GameEntry.Localization.GetString("UI_TEXT_INSTANCE_SETTLEMENT_NUMBER_OF_TIMES", 1/* 开始显示1， m_DisplayData.MaxSweepCount - m_RemainSweepCount*/);

            m_RemainSweepCount = m_AutoSweepToggle.value ? m_DisplayData.MaxSweepCount : 1;
            m_RealSweepCount = m_RemainSweepCount;
            //m_GotAppointedItemCount = 0;

            m_IsFinished = false;
            m_EnableSweep = true;
        }

        private void OnSweepFinished()
        {
            m_EnableSweep = false;
            m_SweepButton.isEnabled = true;
            m_AutoSweepToggle.GetComponent<BoxCollider>().enabled = true;
            m_ButtonTextLabel.text = GameEntry.Localization.GetString("UI_BUTTON_SETTLEMENT_CONTINUE");

            m_ResultScrollView.EnableScroll();

            m_ItemDisplayTime = CacheDefaultDisplayTime;
            m_IsFinished = true;

            StartCoroutine(PlayFinishAnimation());
        }

        private IEnumerator PlayFinishAnimation()
        {
            m_SweepFinishedAnimationObject.GetComponent<UIPanel>().depth = m_ResultScrollView.Depth + 1;
            m_SweepFinishedAnimationObject.gameObject.SetActive(true);
            var animation = m_SweepFinishedAnimation["CleanOutResultIcon"];

            animation.speed = 1;
            animation.normalizedTime = 0;
            m_SweepFinishedAnimation.Play("CleanOutResultIcon");
            while (m_SweepFinishedAnimation.isPlaying)
                yield return null;

            yield return new WaitForSeconds(0.8f);

            animation.speed = -1;
            animation.normalizedTime = 1;
            m_SweepFinishedAnimation.Play("CleanOutResultIcon");

            while (m_SweepFinishedAnimation.isPlaying)
                yield return null;

            m_SweepFinishedAnimationObject.gameObject.SetActive(false);
        }

        /// <summary>
        /// 扫荡结束的时候关闭界面，扫荡未结束的时候加速扫荡（加速显示的时间）
        /// </summary>
        /// <param name="go"></param>
        private void OnMaskClick(GameObject go)
        {
            if (m_IsFinished)
                Hide();
            else
            {
                m_CurrentDisplayTime += m_ItemDisplayTime;
                m_ItemDisplayTime = 0.06f;
            }
        }

        private void OnSweepSuccess(object sender, GameEventArgs e)
        {
            var rewardData = e as CleanOutInstanceEventArgs;
            if (rewardData == null)
                return;

            AddResult(rewardData.Rewards, rewardData.ObtainedCoinCount, rewardData.ObtainedExperienceCount);
            m_SweptCountLabel.text = GameEntry.Localization.GetString("UI_TEXT_INSTANCE_SETTLEMENT_NUMBER_OF_TIMES", m_RealSweepCount - m_RemainSweepCount);

            m_EnableNextRequest = true;
            m_CurrentDisplayTime = 0f;
        }

        private void AddResult(List<PBItemInfo> rewards, int obtainedCoin, int obtainedExperience)
        {
            if (m_DisplayData.SweepEntranceType == SweepDisplayData.ShowType.WhereToGet)
            {
                for (int i = 0; i < rewards.Count; i++)
                {
                    if (rewards[i].Type == m_DisplayData.WannaGetItemId)
                    {
                        m_GotAppointedItemCount += rewards[i].Count;

                        if (m_DisplayData.WannaGetItemCount - m_GotAppointedItemCount > 0)
                        {
                            var item = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>().GetDataRow(m_DisplayData.WannaGetItemId);
                            string name = ColorUtility.AddColorToString(ColorUtility.GetColorForQuality(item.Quality), GameEntry.Localization.GetString(item.Name));
                            string count = ColorUtility.AddColorToString(Constant.Quality.Red, (m_DisplayData.WannaGetItemCount - m_GotAppointedItemCount).ToString());
                            m_WannaGetItemInfoLabel.text = GameEntry.Localization.GetString("UI_TEXT_NEED_ITEM", name, m_GotAppointedItemCount, count);
                        }
                        else
                        {
                            m_WannaGetItemInfoLabel.text = GameEntry.Localization.GetString("UI_TEXT_NEED_ITEM_ENOUGH");
                        }
                        break;
                    }
                }
            }

            var newItem = m_ResultScrollView.AddItemAtLast(m_RealSweepCount - m_RemainSweepCount);
            newItem.SetItemData(rewards, m_RealSweepCount - m_RemainSweepCount + 1, obtainedCoin, obtainedExperience, m_AutoSweepToggle.value);

            m_RemainSweepCount--;

        }

        /// <summary>
        /// 简单扩展的子类，如果有必要再抽象放到基类里
        /// </summary>
        [Serializable]
        private class ResultScrollView : UIScrollViewCache<SweepResultItem>
        {
            private float m_Strength = 10f;
            private LinkedList<SweepResultItem> m_CachedResultItems = new LinkedList<SweepResultItem>();

            private Vector3 m_GridStartPosition;
            private Vector3 m_ScrollViewStartPosition;

            public int Depth
            {
                get
                {
                    return m_ScrollView.panel.depth;
                }
            }

            public void Prepare()
            {
                m_GridStartPosition = m_ItemParent.transform.localPosition;
                m_ScrollViewStartPosition = m_ItemParent.transform.parent.localPosition;
            }

            public void ResetGridPosition()
            {
                m_ItemParent.transform.localPosition = m_GridStartPosition;

                var scrollView = m_ItemParent.transform.parent;
                scrollView.GetComponent<UIPanel>().clipOffset = Vector2.zero;
                scrollView.localPosition = m_ScrollViewStartPosition;
            }

            /// <summary>
            /// 添加一个Item到最后，并且滚动显示出来
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public SweepResultItem AddItemAtLast(int index)
            {
                var newItem = GetOrCreateItem(index);

                newItem.GetComponent<UIDragScrollView>().enabled = false;
                newItem.GetComponent<BoxCollider>().enabled = false;

                if (m_CachedResultItems.Last != null)
                {
                    var lastItem = m_CachedResultItems.Last.Value;

                    var size = NGUIMath.CalculateRelativeWidgetBounds(lastItem.transform, true).size;
                    newItem.transform.localPosition = new Vector3(lastItem.transform.localPosition.x,
                        lastItem.transform.localPosition.y - size.y,
                        lastItem.transform.localPosition.z);

                    SpringPosition.Begin(m_ItemParent, new Vector3(0, m_CachedResultItems.Count * size.y/* + m_GridStartPosition.y / 2*/, 0), m_Strength);
                }
                else
                {
                    Reposition();
                }
                m_CachedResultItems.AddLast(newItem);

                return newItem;
            }

            public void EnableScroll()
            {
                var current = m_CachedResultItems.First;
                while (current != null)
                {
                    current.Value.GetComponent<UIDragScrollView>().enabled = true;
                    current.Value.GetComponent<BoxCollider>().enabled = true;
                    current = current.Next;
                }
            }

            public new void RecycleAllItems()
            {
                base.RecycleAllItems();
                m_CachedResultItems.Clear();
            }

            public void SetScrollViewAutoSpringStrength(float strength)
            {
                m_Strength = strength;
            }
        }
    }
}