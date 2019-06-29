using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{

    public class DRPlotDialogue : IDataRow
    {
        /// <summary>
        /// 剧情对话编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 头像IconID。
        /// </summary>
        public int HeadIconId
        {
            get;
            private set;
        }

        /// <summary>
        /// 剧情内容。
        /// </summary>
        public string Content
        {
            get;
            private set;
        }

        /// <summary>
        /// 角色名字。
        /// </summary>
        public string RoleName
        {
            get;
            private set;
        }

        /// <summary>
        /// 头像是否在左边。
        /// </summary>
        public bool HeadLeft
        {
            get;
            private set;
        }

        /// <summary>
        /// 下一个剧情的id（0为该剧情结束）。
        /// </summary>
        public int NextId
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否是玩家。
        /// </summary>
        public bool IsPlayer
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            HeadIconId = int.Parse(text[index++]);
            Content = text[index++];
            RoleName = text[index++];
            HeadLeft = bool.Parse(text[index++]);
            NextId = int.Parse(text[index++]);
            IsPlayer = bool.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRPlotDialogue>();
        }
    }
}
