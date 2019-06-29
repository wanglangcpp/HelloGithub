using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本配置表。
    /// </summary>
    public class DRSinglePvpInstance : DRInstance
    {
        /// <summary>
        /// 出生点X坐标2。
        /// </summary>
        public float SpawnPointX2
        {
            get;
            protected set;
        }

        /// <summary>
        /// 出生点Y坐标2。
        /// </summary>
        public float SpawnPointY2
        {
            get;
            protected set;
        }

        /// <summary>
        /// 出生朝向2。
        /// </summary>
        public float SpawnAngle2
        {
            get;
            protected set;
        }

        public override void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            SceneId = int.Parse(text[index++]);
            TimerType = int.Parse(text[index++]);
            TimerDuration = int.Parse(text[index++]);
            TimerAlert = int.Parse(text[index++]);
            SpawnPointX = float.Parse(text[index++]);
            SpawnPointY = float.Parse(text[index++]);
            SpawnAngle = float.Parse(text[index++]);
            SpawnPointX2 = float.Parse(text[index++]);
            SpawnPointY2 = float.Parse(text[index++]);
            SpawnAngle2 = float.Parse(text[index++]);
            SceneRegionMaskId = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRSinglePvpInstance>();
        }
    }
}
