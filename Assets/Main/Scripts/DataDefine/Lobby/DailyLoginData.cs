using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Genesis.GameClient
{
    public class DailyLoginData
    {
        private int m_LoginDayCount;
        private bool m_IsLoginedToday;

        private int m_CachedMonth;

        private Dictionary<int, DRDailyLogin> m_DailyLoginConfig = new Dictionary<int, DRDailyLogin>();

        public Dictionary<int, DRDailyLogin> DailyLoginConfig
        {
            get
            {
                RefreshConfig();

                return m_DailyLoginConfig;
            }
        }

        private void RefreshConfig()
        {
            if (m_CachedMonth != GameEntry.Time.LobbyServerUtcTime.Month)
            {
                m_CachedMonth = GameEntry.Time.LobbyServerUtcTime.Month;
                m_DailyLoginConfig.Clear();

                var dt = GameEntry.DataTable.GetDataTable<DRDailyLogin>();
                var rows = dt.GetAllDataRows();

                for (int i = 0; i < rows.Length; i++)
                {
                    if (rows[i].Month == m_CachedMonth)
                    {
                        m_DailyLoginConfig[rows[i].Index] = rows[i];
                    }
                }
            }
        }

        public int LoginedDayCount
        {
            get
            {
                return m_LoginDayCount;
            }
        }

        public bool IsLoginedToday
        {
            get
            {
                return m_IsLoginedToday;
            }
        }

        public DRDailyLogin GetDailyLoginRow(int index)
        {
            DRDailyLogin dr = null;
            DailyLoginConfig.TryGetValue(index, out dr);

            return dr;
        }

        public void UpdateData(int loginedCount, bool canClaimToday)
        {
            m_IsLoginedToday = !canClaimToday;
            m_LoginDayCount = loginedCount;
        }
    }
}