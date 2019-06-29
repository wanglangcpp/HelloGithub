using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本配置表。
    /// </summary>
    public class DRInstanceGroupForBoss : IDataRow
    {
        public int Id
        {
            get;
            protected set;
        }


        /// <summary>
        /// 副本名称
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }
        /// <summary>
        /// 副本描述
        /// </summary>
        public string Description
        {
            get;
            protected set;
        }
        /// <summary>
        /// 副本选择界面对应的图片名称
        /// </summary>
        public string ChapterIconName
        {
            get;
            protected set;
        }
        /// <summary>
        /// 1Boss的大头像ID
        /// </summary>
        public int BossTextureId1
        {
            get;
            protected set;
        }
        /// <summary>
        /// 2Boss的大头像ID
        /// </summary>
        public int BossTextureId2
        {
            get;
            protected set;
        }
        /// <summary>
        /// 3Boss的大头像ID
        /// </summary>
        public int BossTextureId3
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱1开启所需星星数量
        /// </summary>
        public int Chest1NeedStar
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱1物品ID1
        /// </summary>
        public int Chest1RewardId1
        { get; protected set; }
        /// <summary>
        /// 宝箱1物品数量1
        /// </summary>
        public int Chest1RewardCount1
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱1物品ID2
        /// </summary>
        public int Chest1RewardId2
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱1物品数量2
        /// </summary>
        public int Chest1RewardCount2
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱1物品ID3
        /// </summary>
        public int Chest1RewardId3
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱1物品数量3
        /// </summary>
        public int Chest1RewardCount3
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱1物品ID4
        /// </summary>
        public int Chest1RewardId4
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱1物品数量4
        /// </summary>
        public int Chest1RewardCount4
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱2开启所需星星数量
        /// </summary>
        public int Chest2NeedStar
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱2物品ID1
        /// </summary>
        public int Chest2RewardId1
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱2物品数量1
        /// </summary>
        public int Chest2RewardCount1
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱2物品ID2
        /// </summary>
        public int Chest2RewardId2
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱2物品数量2
        /// </summary>
        public int Chest2RewardCount2
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱2物品ID3
        /// </summary>
        public int Chest2RewardId3
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱2物品数量3
        /// </summary>
        public int Chest2RewardCount3
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱2物品ID4
        /// </summary>
        public int Chest2RewardId4
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱2物品数量4
        /// </summary>
        public int Chest2RewardCount4
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱3开启所需星星数量
        /// </summary>
        public int Chest3NeedStar
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱3物品ID1
        /// </summary>
        public int Chest3RewardId1
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱3物品数量1
        /// </summary>
        public int Chest3RewardCount1
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱3物品ID2
        /// </summary>
        public int Chest3RewardId2
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱3物品数量2
        /// </summary>
        public int Chest3RewardCount2
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱3物品ID3
        /// </summary>
        public int Chest3RewardId3
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱3物品数量3
        /// </summary>
        public int Chest3RewardCount3
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱3物品ID4
        /// </summary>
        public int Chest3RewardId4
        {
            get;
            protected set;
        }
        /// <summary>
        /// 宝箱3物品数量4
        /// </summary>
        public int Chest3RewardCount4
        {
            get;
            protected set;
        }

        



        public virtual void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            ChapterIconName = text[index++];
            BossTextureId1 = int.Parse(text[index++]);
            BossTextureId2 = int.Parse(text[index++]);
            BossTextureId3 = int.Parse(text[index++]);
            Chest1NeedStar = int.Parse(text[index++]);
            Chest1RewardId1 = int.Parse(text[index++]);
            Chest1RewardCount1 = int.Parse(text[index++]);
            Chest1RewardId2 = int.Parse(text[index++]);
            Chest1RewardCount2 = int.Parse(text[index++]);
            Chest1RewardId3 = int.Parse(text[index++]);
            Chest1RewardCount3 = int.Parse(text[index++]);
            Chest1RewardId4 = int.Parse(text[index++]);
            Chest1RewardCount4 = int.Parse(text[index++]);
            Chest2NeedStar = int.Parse(text[index++]);
            Chest2RewardId1 = int.Parse(text[index++]);
            Chest2RewardCount1 = int.Parse(text[index++]);
            Chest2RewardId2 = int.Parse(text[index++]);
            Chest2RewardCount2 = int.Parse(text[index++]);
            Chest2RewardId3 = int.Parse(text[index++]);
            Chest2RewardCount3 = int.Parse(text[index++]);
            Chest2RewardId4 = int.Parse(text[index++]);
            Chest2RewardCount4 = int.Parse(text[index++]);
            Chest3NeedStar = int.Parse(text[index++]);
            Chest3RewardId1 = int.Parse(text[index++]);
            Chest3RewardCount1 = int.Parse(text[index++]);
            Chest3RewardId2 = int.Parse(text[index++]);
            Chest3RewardCount2 = int.Parse(text[index++]);
            Chest3RewardId3 = int.Parse(text[index++]);
            Chest3RewardCount3 = int.Parse(text[index++]);
            Chest3RewardId4 = int.Parse(text[index++]);
            Chest3RewardCount4 = int.Parse(text[index++]);
        }




        private void AvoidJIT()
        {
            new Dictionary<int, DRInstanceGroupForBoss>();
        }
    }
}
