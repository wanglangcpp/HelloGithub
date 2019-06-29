using UnityEngine;
using System.Collections;
using System.Text;

namespace Genesis.GameClient
{
    public class OnLineRewardItem : MonoBehaviour
    {
        /// <summary>
        /// 完成条件
        /// </summary>
        [SerializeField]
        private UILabel m_LabelCondition = null;
        [SerializeField]
        private GeneralItemView[] m_RewardItemView = null;
        [SerializeField]
        private GameObject m_BtnClaim = null;
        [SerializeField]
        private GameObject m_IconClaimed = null;
        [SerializeField]
        private GameObject m_LabelWait = null;
        [SerializeField]
        private UILabel m_LabelCountdown = null;

        private DROnlineRewards dROnlineRewards;
        private bool isStart = false;
        private bool IsFirstRefresh = true;
        private int SumTime;//奖励所需总时长(s)
        private int cruuentOnlineTime; //当前在线总时长
        //System.Timers.Timer timer = new System.Timers.Timer(1);

        public void RefreshItem(DROnlineRewards data)
        {
            dROnlineRewards = data;
            if (IsFirstRefresh)
            {
                IsFirstRefresh = false;
                UIUtility.ReplaceDictionaryTextForLabels(gameObject);
                m_LabelCondition.text = GameEntry.Localization.GetString("UI_TEXT_WAITTIME_AFTER_CLAIM", data.CumulativeTime / 60f);
                for (int i = 0; i < m_RewardItemView.Length; i++)
                {
                    if (i >= data.Rewards.Count)
                    {
                        m_RewardItemView[i].gameObject.SetActive(false);
                        continue;
                    }
                    m_RewardItemView[i].InitItem(data.Rewards[i].Type, data.Rewards[i].Count);
                }
                UIEventListener.Get(m_BtnClaim).onClick = OnClickClaimBtn;
                SumTime = data.CumulativeTime;
                if (data.Id > 1)
                {
                    int lastTime = GameEntry.DataTable.GetDataTable<DROnlineRewards>().GetDataRow(data.Id - 1).CumulativeTime;
                    SumTime -= lastTime;
                }
            }
            SetItemStatus();
        }
        /// <summary>
        /// 设置奖励的状态（已领取，可领取，倒计时，未开始）
        /// </summary>
        public void SetItemStatus()
        {
            m_BtnClaim.SetActive(false);
            m_IconClaimed.SetActive(false);
            m_LabelWait.SetActive(false);
            m_LabelCountdown.gameObject.SetActive(false);

            if (GameEntry.Data.OnlineRewardsData.CountdownRewardsId <= 0)
            {
                m_IconClaimed.SetActive(true);
                return;
            }

            //已领取
            if (GameEntry.Data.OnlineRewardsData.CountdownRewardsId > dROnlineRewards.Id)
            {
                isStart = false;
                m_IconClaimed.SetActive(true);
            }

            else if (GameEntry.Data.OnlineRewardsData.CountdownRewardsId == dROnlineRewards.Id)
            {
                Timing(out cruuentOnlineTime);
                //可以领取
                if (cruuentOnlineTime >= SumTime)
                {
                    isStart = false;
                    m_BtnClaim.SetActive(true);
                }
                //倒计时
                else
                {
                    isStart = true;
                    //startCountDown(SumTime - cruuentOnlineTime);
                    m_LabelCountdown.gameObject.SetActive(true);
                    m_LabelCountdown.text = GameEntry.Localization.GetString("UI_TEXT_WAITTIME_AFTER_CLAIM", Timer(SumTime - cruuentOnlineTime));
                }
            }
            //未开始
            else
            {
                isStart = false;
                m_LabelWait.SetActive(true);
            }
        }

        //private void onTimerHandler(object source, System.Timers.ElapsedEventArgs args)
        //{
        //    Timing(out cruuentOnlineTime);
        //    if (cruuentOnlineTime >= SumTime)
        //    {
        //        isStart = false;
        //        timer.Close();
        //    }
        //    m_LabelCountdown.text = GameEntry.Localization.GetString("UI_TEXT_WAITTIME_AFTER_CLAIM", Timer(SumTime - cruuentOnlineTime));
        //}

        private void Timing(out int time)
        {
            time = 0; //当前在线总时长
            if (GameEntry.Data.OnlineRewardsData.LastClaimRewardsTime > GameEntry.TaskComponent.LogInTime.Ticks)
            {
                //最后一次领奖时间>登录时间（从上次领奖时间开始计时）
                long ticks = GameEntry.Time.LobbyServerUtcTime.Ticks - GameEntry.Data.OnlineRewardsData.LastClaimRewardsTime;
                System.DateTime dateTime = new System.DateTime(ticks, System.DateTimeKind.Utc);
                time = dateTime.Second + dateTime.Minute * 60 + dateTime.Hour * 3600;
            }
            else
            {
                //最后一次领奖时间<登录时间(从登录时间开始计时+累计时间)
                System.TimeSpan onlineTime = GameEntry.Time.LobbyServerUtcTime - GameEntry.TaskComponent.LogInTime;
                time = onlineTime.Seconds + onlineTime.Minutes * 60 + onlineTime.Hours * 3600 + GameEntry.Data.OnlineRewardsData.CumulativeOnlineTime;
            }
        }

        private void OnClickClaimBtn(GameObject go)
        {
            int id = dROnlineRewards.Id;
            CLClaimOnlineReward sender = new CLClaimOnlineReward();
            sender.RewardId = id;
            GameEntry.Network.Send(sender);
        }

        //public IEnumerator startCountDown(int sumTime)
        //{
        //    while (sumTime >= 0)
        //    {
        //        //每隔一秒调用一次
        //        yield return new WaitForSeconds(1);
        //        sumTime--;
        //        m_LabelCountdown.text = GameEntry.Localization.GetString("UI_TEXT_WAITTIME_AFTER_CLAIM", Timer(sumTime));
        //        GameFramework.Log.Debug(Timer(sumTime));
        //        if (sumTime <= 0)
        //        {
        //            break;
        //        }
        //    }
        //    yield return null;
        //}

        /// <summary>
        /// 转换时间格式
        /// </summary>
        /// <param name="curtime"></param>
        private string Timer(int curtime)
        {
            StringBuilder m_Min = null;
            if (curtime >= 60)
            {
                int min = curtime / 60;

                if (min < 10)
                {
                    m_Min = new StringBuilder("0").Append(min.ToString()).Append(":");
                }
                else
                {
                    m_Min = new StringBuilder(min.ToString()).Append(":");
                }
                if (curtime % 60 < 10)
                {
                    m_Min.Append("0");
                }
                return m_Min.Append((curtime % 60).ToString()).ToString();
            }
            else
            {
                if (curtime < 10)
                {
                    m_Min = new StringBuilder("00:0");
                }
                else
                {
                    m_Min = new StringBuilder("00:");
                }
                return m_Min.Append(curtime.ToString()).ToString();
            }
        }

        private float totalTime = 0;
        private void LateUpdate()
        {
            if (isStart)
            {
                totalTime += Time.deltaTime;
                if (totalTime >= 1)//每过1秒执行一次
                {
                    totalTime = 0;
                    SetItemStatus();
                    //m_LabelCountdown.text = GameEntry.Localization.GetString("UI_TEXT_WAITTIME_AFTER_CLAIM", Timer(SumTime - cruuentOnlineTime));
                }
            }
        }
    }
}

