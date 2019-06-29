using UnityEngine;
using System.Collections.Generic;
using GameFramework.DataTable;

namespace Genesis.GameClient
{
    public class DROnlineRewards : IDataRow
    {
        private const int RewardCount = 4;

        public int Id { get; private set; }

        public int CumulativeTime { get; private set; }

        private List<PBItemInfo> m_Rewards = new List<PBItemInfo>();
        public List<PBItemInfo> Rewards { get { return m_Rewards; } }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            CumulativeTime = int.Parse(text[index++]);
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

