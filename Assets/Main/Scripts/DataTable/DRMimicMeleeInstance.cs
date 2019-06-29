using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 模拟乱斗副本配置表。
    /// </summary>
    public class DRMimicMeleeInstance : DRInstance
    {
        /// <summary>
        /// 复活点个数上限。
        /// </summary>
        private const int RevivePointMaxCount = 10;

        /// <summary>
        /// 等级范围。
        /// </summary>
        public int LevelRangeId
        {
            get;
            private set;
        }

        /// <summary>
        /// 复活点。
        /// </summary>
        public Vector2[] RevivePoints
        {
            get;
            private set;
        }

        /// <summary>
        /// 经验分配主要系数。
        /// </summary>
        public float ExpDistrMainCoef
        {
            get;
            private set;
        }

        /// <summary>
        /// 经验分配次要系数。
        /// </summary>
        public float ExpDistrSecondaryCoef
        {
            get;
            private set;
        }

        /// <summary>
        /// 复活等待时间。
        /// </summary>
        public float ReviveWaitTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 小地图纹理编号。
        /// </summary>
        public int MiniMapTextureId
        {
            get;
            private set;
        }

        /// <summary>
        /// 小地图缩放比例。
        /// </summary>
        public float MiniMapScale
        {
            get;
            private set;
        }

        /// <summary>
        /// 小地图偏移量。
        /// </summary>
        public Vector2 MiniMapOffset
        {
            get;
            private set;
        }

        public override void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            LevelRangeId = int.Parse(text[index++]);
            SceneId = int.Parse(text[index++]);
            TimerType = int.Parse(text[index++]);
            TimerDuration = int.Parse(text[index++]);
            TimerAlert = int.Parse(text[index++]);
            index = ParseAIBehaviors(text, index);
            InstanceNpcs = text[index++];
            InstanceBuildings = text[index++];
            SpawnPointX = float.Parse(text[index++]);
            SpawnPointY = float.Parse(text[index++]);
            SpawnAngle = float.Parse(text[index++]);
            SceneRegionMaskId = int.Parse(text[index++]);

            var revivePoints = new List<Vector2>();
            for (int i = 0; i < RevivePointMaxCount; i++)
            {
                string vecStr = text[index++].Trim('"');
                if (string.IsNullOrEmpty(vecStr))
                {
                    continue;
                }

                revivePoints.Add(ConverterEx.ParseVector2(vecStr).Value);
            }
            RevivePoints = revivePoints.ToArray();

            ExpDistrMainCoef = float.Parse(text[index++]);
            ExpDistrSecondaryCoef = float.Parse(text[index++]);
            ReviveWaitTime = float.Parse(text[index++]);
            MiniMapTextureId = int.Parse(text[index++]);
            MiniMapScale = float.Parse(text[index++]);
            MiniMapOffset = ConverterEx.ParseVector2(text[index++].Trim('"')).Value;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRMimicMeleeInstance>();
        }
    }
}
