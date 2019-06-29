using UnityEngine;
using System.Collections;
using GameFramework.DataTable;
using System.Text;
using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// 功能开放的数据体
    /// </summary>
    public class DROpenFunction : IDataRow
    {
        /// <summary>
        /// 功能编号
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 关联的主功能号，例如每日任务的主功能号是任务，无则是-1
        /// </summary>
        public int ParentId { get; private set; }
        /// <summary>
        /// 功能图标ID
        /// </summary>
        public int IconId { get; private set; }
        /// <summary>
        /// 是否默认开放
        /// </summary>
        public bool IsDefaultOpen { get; private set; }
        /// <summary>
        /// GameObject的名称
        /// </summary>
        public string FunctionPath { get; private set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 功能描述
        /// </summary>
        public string Desc { get; private set; }
        /// <summary>
        /// 触发条件类型（1等级2任务ID）
        /// </summary>
        public int OpenType { get; private set; }
        /// <summary>
        /// 触发条件的值
        /// </summary>
        public int Contidtion { get; private set; }
        /// <summary>
        /// 版块（1上面，2右边，3右下）
        /// </summary>
        public int FunctionGroup { get; private set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int GroupIndex { get; private set; }
        /// <summary>
        /// 限时显示时间
        /// </summary>
        public int LimitTime { get; private set; }
        /// <summary>
        /// 子页签是否显示
        /// </summary>
        public bool IsSubpageSign { get; private set; }
        /// <summary>
        /// 是否播放动画
        /// </summary>
        public bool IsPlayAnimation { get; private set; }
        /// <summary>
        /// 目前是否打开
        /// </summary>
        public bool isOpen = false;

        public bool IsSubpage()
        {
            return FunctionGroup == -1;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            IsDefaultOpen = bool.Parse(text[index++]);
            ParentId = int.Parse(text[index++]);
            FunctionPath = text[index++];
            IconId = int.Parse(text[index++]);
            Name = text[index++];
            Desc = text[index++];
            OpenType = int.Parse(text[index++]);
            Contidtion = int.Parse(text[index++]);
            FunctionGroup = int.Parse(text[index++]);
            GroupIndex = int.Parse(text[index++]);
            LimitTime = int.Parse(text[index++]);
            IsSubpageSign = bool.Parse(text[index++]);
            IsPlayAnimation = bool.Parse(text[index++]);
        }

        public string toString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("OpenFunction:" + Id);
            sb.Append(",IsDefaultOpen:" + IsDefaultOpen);
            sb.Append(",ParentId:" + ParentId);
            sb.Append(",FunctionPath:" + FunctionPath);
            sb.Append(",IconId:" + IconId);
            sb.Append(",Name:" + Name);
            sb.Append(",Desc:" + Desc);
            sb.Append(",OpenType:" + OpenType);
            sb.Append(",Contidtion:" + Contidtion);
            sb.Append(",FunctionGroup:" + FunctionGroup);
            sb.Append(",GroupIndex:" + GroupIndex);
            sb.Append(",LimitTime:" + LimitTime);
            sb.Append(",IsSubpageSign:" + IsSubpageSign);
            sb.Append(",IsPlayAnimation:" + IsPlayAnimation);
            return sb.ToString();
        }

    }
}
