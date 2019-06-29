using UnityEngine;
using System.Text;
using GameFramework.DataTable;
using GameFramework;

namespace Genesis.GameClient
{
    public class DRGuideUI : IDataRow
    {
        /// <summary>
        /// 新手引导编号
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 新手引导组ID
        /// </summary>
        public int Group { get; private set; }
        /// <summary>
        /// 触发类型（1等级2功能开放3任务）
        /// </summary>
        public int OpenType { get; private set; }
        /// <summary>
        /// 触发条件的值（等级，任务ID，功能ID）
        /// </summary>
        public int Condition { get; private set; }
        /// <summary>
        /// 引导描述(中间框)
        /// </summary>
        public string Desc { get; private set; }
        /// <summary>
        /// 触发条件类型（1等级2任务ID）
        /// </summary>
        public int ShowType { get; private set; }
        /// <summary>
        /// 打开界面
        /// </summary>
        public int UIFormID { get; private set; }
        /// <summary>
        /// 按钮对象名字
        /// </summary>
        public string ClickObject { get; private set; }
        /// <summary>
        /// 遮罩（1有-1没有）
        /// </summary>
        public bool IsShade { get; private set; }
        /// <summary>
        /// 关键步骤
        /// </summary>
        public bool IsStep { get; private set; }
        /// <summary>
        /// 箭头方向（1左上，2上，3右上，4右，5右下，6下，7左下，8左）
        /// </summary>
        public int Direction { get; private set; }
        /// <summary>
        /// 新手期(小于等于30级)是否总显示（1是-1不是）
        /// </summary>
        public bool IsShowInGuide { get; private set; }
        /// <summary>
        /// 延迟显示时间
        /// </summary>
        public float DelayTime { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Group = int.Parse(text[index++]);
            OpenType = int.Parse(text[index++]);
            Condition = int.Parse(text[index++]);
            Desc = ToStringText(text[index++]);
            //if (Desc != null) Desc = GameEntry.Localization.GetString(Desc);
            ShowType = int.Parse(text[index++]);
            UIFormID = int.Parse(text[index++]);
            ClickObject = ToStringText(text[index++]);
            IsShade = bool.Parse(text[index++]);
            IsStep = bool.Parse(text[index++]);
            Direction = int.Parse(text[index++])-1;
            IsShowInGuide = bool.Parse(text[index++]);
            DelayTime = float.Parse(text[index++]);
            //Log.Debug(toString());
        }

        private string ToStringText(string text)
        {
            if (text.Equals("null")) return null;
            else return text;
        }

        public string toString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ID:" + Id);
            sb.Append(",Group:" + Group);
            sb.Append(",OpenType:" + OpenType);
            sb.Append(",Condition:" + Condition);
            sb.Append(",Desc:" + Desc);
            sb.Append(",ShowType:" + ShowType);
            sb.Append(",UIFormID:" + UIFormID);
            sb.Append(",ClickObject:" + ClickObject);
            sb.Append(",IsShade:" + IsShade);
            sb.Append(",IsStep:" + IsStep);
            sb.Append(",Direction:" + Direction);
            sb.Append(",IsShowInGuide:" + IsShowInGuide);
            sb.Append(",DelayTime:" + DelayTime);
            return sb.ToString();
        }

    }
}
