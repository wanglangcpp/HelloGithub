using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// NearbyPlayer随机点。
    /// </summary>
    public class DRNearbyPlayerRandomPosition : IDataRow
    {
        /// <summary>
        /// 随机点Id。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 随机点X坐标。
        /// </summary>
        public float PositionX { get; private set; }

        /// <summary>
        /// 随机点Y坐标。
        /// </summary>
        public float PositionY { get; private set; }

        /// <summary>
        /// 随机半径。
        /// </summary>
        public float RandomRadius { get; private set; }

        /// <summary>
        /// 权重。
        /// </summary>
        public int Weight { get; private set; }

        /// <summary>
        /// 最小停留时间。
        /// </summary>
        public float MinStayTime { get; private set; }

        /// <summary>
        /// 最大停留时间。
        /// </summary>
        public float MaxStayTime { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            PositionX = float.Parse(text[index++]);
            PositionY = float.Parse(text[index++]);
            RandomRadius = float.Parse(text[index++]);
            Weight = int.Parse(text[index++]);
            MinStayTime = float.Parse(text[index++]);
            MaxStayTime = float.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRNearbyPlayerRandomPosition>();
        }
    }
}
