using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 建筑物配置表。
    /// </summary>
    public class DRBuildingModel : IDataRow
    {
        /// <summary>
        /// 建筑物编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 资源名称。
        /// </summary>
        public string ResourceName { get; private set; }

        /// <summary>
        /// 伤害触发中心点X。
        /// </summary>
        public float ImpactCenterX { get; private set; }

        /// <summary>
        /// 伤害触发中心点Y。
        /// </summary>
        public float ImpactCenterY { get; private set; }

        /// <summary>
        /// 伤害触发中心点Z。
        /// </summary>
        public float ImpactCenterZ { get; private set; }

        /// <summary>
        /// 伤害触发半径。
        /// </summary>
        public float ImpactRadius { get; private set; }

        /// <summary>
        /// 伤害触发高度。
        /// </summary>
        public float ImpactHeight { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            ResourceName = text[index++];
            ImpactCenterX = float.Parse(text[index++]);
            ImpactCenterY = float.Parse(text[index++]);
            ImpactCenterZ = float.Parse(text[index++]);
            ImpactRadius = float.Parse(text[index++]);
            ImpactHeight = float.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRBuildingModel>();
        }
    }
}
