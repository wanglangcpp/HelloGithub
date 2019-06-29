using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时空裂缝副本配置表。
    /// </summary>
    public class DRInstanceCosmosCrack : DRInstance
    {
        public override void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            index = ParseRequestDescs(text, index);
            SceneId = int.Parse(text[index++]);
            TimerType = int.Parse(text[index++]);
            TimerDuration = int.Parse(text[index++]);
            TimerAlert = int.Parse(text[index++]);
            index = ParseAIBehaviors(text, index);
            InstanceNpcs = text[index++];
            SpawnPointX = float.Parse(text[index++]);
            SpawnPointY = float.Parse(text[index++]);
            SpawnAngle = float.Parse(text[index++]);
            SceneRegionMaskId = int.Parse(text[index++]);
            GuidePointSetId = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRInstanceCosmosCrack>();
        }
    }
}
