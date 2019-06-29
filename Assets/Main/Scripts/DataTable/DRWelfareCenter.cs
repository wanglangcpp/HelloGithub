using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 福利中心配置表
    /// </summary>
    public class DRWelfareCenter : IDataRow
    {
        public int Id { get; private set; }
        /// <summary>
        /// 福利类型
        /// </summary>
        public string Type { get; private set; }
        /// <summary>
        /// 小红点
        /// </summary>
        public bool Recommend { get; private set; }
        /// <summary>
        /// 关联的窗口名
        /// </summary>
        public string RelateForm { get; private set; }
        /// <summary>
        /// 关联的窗口路径
        /// </summary>
        public string FormPath { get; private set; }


        public void ParseDataRow(string dataRowText)
        {
            string[] rowData = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(rowData[index++]);
            index++;
            Type = rowData[index++];
            Recommend = bool.Parse(rowData[index++]);
            RelateForm = rowData[index++];
            FormPath = rowData[index++];
        }


    }
}

