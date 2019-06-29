using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 玩家对阵其他 AI 副本配置表。
    /// </summary>
    public class DRPvpaiInstance : DRInstance
    {
        /// <summary>
        /// 对方出生点X坐标
        /// </summary>
        public float OppSpawnPointX
        {
            get;
            private set;
        }

        /// <summary>
        /// 对方出生点Y坐标
        /// </summary>
        public float OppSpawnPointY
        {
            get;
            private set;
        }

        /// <summary>
        /// 对方出生朝向
        /// </summary>
        public float OppSpawnAngle
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
            SceneId = int.Parse(text[index++]);
            TimerType = int.Parse(text[index++]);
            TimerDuration = int.Parse(text[index++]);
            TimerAlert = int.Parse(text[index++]);
            index = ParseAIBehaviors(text, index);
            SpawnPointX = float.Parse(text[index++]);
            SpawnPointY = float.Parse(text[index++]);
            SpawnAngle = float.Parse(text[index++]);
            OppSpawnPointX = float.Parse(text[index++]);
            OppSpawnPointY = float.Parse(text[index++]);
            OppSpawnAngle = float.Parse(text[index++]);
            SceneRegionMaskId = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRPvpaiInstance>();
        }
    }
}
