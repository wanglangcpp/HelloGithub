using UnityEngine;
using System.Collections;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class DRSevenDayLogin : IDataRow
    {
        private const int RewardCount = 4;
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 登陆类型（1新手2循环）
        /// </summary>
        public int LoginType { get; private set; }
        /// <summary>
        /// 登陆天数
        /// </summary>
        public int NumberDay { get; private set; }

        private List<PBItemInfo> m_Rewards = new List<PBItemInfo>();
        public List<PBItemInfo> Rewards { get { return m_Rewards; } }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            LoginType = int.Parse(text[index++]);
            NumberDay = int.Parse(text[index++]);
            for (int i = 0; i < RewardCount; i++)
            {
                int type = int.Parse(text[index++]);
                int count = int.Parse(text[index++]);
                if (type > 0)
                {
                    m_Rewards.Add(new PBItemInfo() { Type = type, Count = count });
                }
            }
        }

    }
}

