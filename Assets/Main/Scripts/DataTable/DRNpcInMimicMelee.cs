using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 模拟乱斗中的NPC配置表。
    /// </summary>
    public class DRNpcInMimicMelee : IDataRow
    {
        /// <summary>
        /// NPC编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 小地图上的精灵图名称。
        /// </summary>
        public string SpriteNameOnMiniMap { get; private set; }

        /// <summary>
        /// 小地图上的精灵图尺寸。
        /// </summary>
        public Vector2 SpriteSizeOnMiniMap { get; private set; }

        /// <summary>
        /// 显示深度。
        /// </summary> 
        public int DisplayDepthOnMiniMap { get; private set; }

        /// <summary>
        /// 给击杀者的积分。
        /// </summary>
        public int ScoreForKiller { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            SpriteNameOnMiniMap = text[index++];

            string vecStr = text[index++].Trim('"');
            if (!string.IsNullOrEmpty(vecStr))
            {
                SpriteSizeOnMiniMap = ConverterEx.ParseVector2(vecStr).Value;
            }

            DisplayDepthOnMiniMap = int.Parse(text[index++]);
            ScoreForKiller = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRNpcInMimicMelee>();
        }
    }
}
