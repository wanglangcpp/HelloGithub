using UnityEngine;
using System.Collections;
using GameFramework.DataTable;

namespace Genesis.GameClient
{
    public class DRTaskTalk : IDataRow
    {
        /// <summary>
        /// 对话编号
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// NpcId
        /// </summary>
        public int NpcId { get; private set; }
        /// <summary>
        /// 下次对话编号
        /// </summary>
        public int NextTalkId { get; private set; }
        /// <summary>
        /// 对话内容
        /// </summary>
        public string DialogueContent { get; private set; }
        
        /// <summary>
        /// 是否是玩家说的话（决定头像位置）
        /// </summary>
        public bool IsMeTalk { get; private set; }
        /// <summary>
        /// 对话头像图标
        /// </summary>
        public int HeadIcon { get; private set; }

        /// <summary>
        /// Npc名字
        /// </summary>
        public string NpcName { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            NpcId = int.Parse(text[index++]);
            NextTalkId = int.Parse(text[index++]);
            DialogueContent = text[index++];
            IsMeTalk = bool.Parse(text[index++]);
            HeadIcon = int.Parse(text[index++]);
            NpcName = text[index++];
        }

    }
}

