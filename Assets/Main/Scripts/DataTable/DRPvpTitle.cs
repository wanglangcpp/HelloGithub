using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class DRPvpTitle : IDataRow
    {
        public int Id
        {
            get;
            protected set;
        }

        public string TitleName
        {
            get;
            set;
        }

        public int TitleTextureId
        {
            get;
            set;
        }

        public int TitleMinScore
        {
            get;
            set;
        }

        public int TitleMaxScore
        {
            get;
            set;
        }

        public int[] RewardId
        {
            get;
            set;
        }

        public int[] RewardCount
        {
            get;
            set;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            TitleName = text[index++];

            TitleTextureId = int.Parse(text[index++]);
            TitleMinScore = int.Parse(text[index++]);
            TitleMaxScore = int.Parse(text[index++]);
            RewardId = new int[Constant.PvpMaxRewardNum];
            RewardCount = new int[Constant.PvpMaxRewardNum];
            for (int i = 0; i < Constant.PvpMaxRewardNum; i++)
            {
                RewardId[i] = int.Parse(text[index++]);
                RewardCount[i] = int.Parse(text[index++]);
            }
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRPvpTitle>();
        }
    }
}
