using System.Collections.Generic;
using GameFramework.DataTable;

namespace Genesis.GameClient
{
    /// <summary>
    /// 每日签到数据
    /// </summary>
    public class EveryDaySignInData
    {
        /// <summary>
        /// 签到配置表
        /// </summary>
        public IDataTable<DRDailyLogin> DataTable { get { return m_DataTable; } }
        private IDataTable<DRDailyLogin> m_DataTable;

        /// <summary>
        /// 累积签到宝箱
        /// </summary>
        public List<int> ListCumulativeBox
        {
            get
            {
                InitData();
                return m_ListCumulativeBox;
            }
        }
        private List<int> m_ListCumulativeBox;
        /// <summary>
        /// 每天的签到配表数据
        /// </summary>
        public Dictionary<int, DRDailyLogin> DicTableData
        {
            get
            {
                InitData();
                return m_DicTableData;
            }
        }
        private Dictionary<int, DRDailyLogin> m_DicTableData;

        /// <summary>
        /// 每天的签到状态
        /// </summary>
        public Dictionary<int, int> ClaimStatus
        {
            get
            {
                InitData();
                return m_ClaimStatus;
            }
        }
        private Dictionary<int, int> m_ClaimStatus;

        private int SignInStatus;
        /// <summary>
        /// 签到次数
        /// </summary>
        public int ClaimCount { get; private set; }
        /// <summary>
        /// 今天是否能签到
        /// </summary>
        public bool CanClaim { get; private set; }
        /// <summary>
        /// 补签次数
        /// </summary>
        public int RetroactiveCount { get; set; }
        /// <summary>
        /// 累计宝箱状态
        /// </summary>
        public int BoxStatus { get; private set; }

        public EveryDaySignInData()
        {
            m_ListCumulativeBox = new List<int>();
            m_ClaimStatus = new Dictionary<int, int>();
            m_DicTableData = new Dictionary<int, DRDailyLogin>();
        }
        /// <summary>
        /// 初始化读表数据
        /// </summary>
        private void InitData()
        {
            if (m_DataTable != null)
                return;
            m_DataTable = GameEntry.DataTable.GetDataTable<DRDailyLogin>();
            //int days = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(System.DateTime.Now.Year, System.DateTime.Now.Month);
            int days = System.DateTime.DaysInMonth(GameEntry.Time.LobbyServerUtcTime.Year, GameEntry.Time.LobbyServerUtcTime.Month);

            foreach (var item in m_DataTable.GetAllDataRows())
            {
                if (!m_DicTableData.ContainsKey(item.Id) && m_DicTableData.Count < days)
                {
                    m_DicTableData.Add(item.Id, item);
                }
                if (item.BoxRewards.Count > 0 && !m_ListCumulativeBox.Contains(item.Id))
                {
                    m_ListCumulativeBox.Add(item.Id);
                }
            }
        }
        /// <summary>
        /// 更新签到的状态数据
        /// </summary>
        private void RefreshStatusData()
        {
            InitData();
            foreach (var item in m_DataTable.GetAllDataRows())
            {
                if (!m_ClaimStatus.ContainsKey(item.Id))
                    m_ClaimStatus.Add(item.Id, SetClaimStatus(item.Id));
                else
                    m_ClaimStatus[item.Id] = SetClaimStatus(item.Id);
            }
        }
        /// <summary>
        /// 设置签到状态(0-不可领取，1-可以领取，2-已领取)
        /// <param name="id">日期</param>
        /// <returns></returns>
        private int SetClaimStatus(int id)
        {
            bool isNoClaim = (SignInStatus & (1 << id)) == 0;
            int thisStatus = isNoClaim ? id <= GameEntry.Time.LobbyServerUtcTime.Day ? 1 : 0 : 2;
            return thisStatus;
        }
        public void UpdataData(PBDailyLoginInfo info)
        {
            ClaimCount = info.ClaimCount;
            CanClaim = info.CanClaim;
            SignInStatus = info.ClaimDays;
            BoxStatus = info.ClaimGifts;
            //RetroactiveCount=info
            RefreshStatusData();
        }
        public void UpdataData(int status)
        {
            SignInStatus = status;
            RefreshStatusData();
            ClaimCount++;
        }
        public void UpDataBoxData(int status)
        {
            BoxStatus = status;
        }
    }
}