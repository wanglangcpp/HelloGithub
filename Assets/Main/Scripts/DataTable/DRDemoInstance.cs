using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 展示副本配置表。
    /// </summary>
    public class DRDemoInstance : DRInstance
    {
        public int HeroMinHP { get; private set; }

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
            index = ParseAIBehaviors(text, index);
            InstanceNpcs = text[index++];
            InstanceBuildings = text[index++];
            SpawnPointX = float.Parse(text[index++]);
            SpawnPointY = float.Parse(text[index++]);
            SpawnAngle = float.Parse(text[index++]);
            HeroMinHP = int.Parse(text[index++]);
            SceneRegionMaskId = int.Parse(text[index++]);
            GuidePointSetId = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRDemoInstance>();
        }
    }
}
