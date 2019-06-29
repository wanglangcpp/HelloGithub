using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="NewGearQualityUpSuccessForm"/> 的显示数据。
    /// </summary>
    public class NewGearQualityUpDisplayData : UIFormBaseUserData
    {
        public QualityType LastQuality { get; set; }

        public int LastQualityLevel { get; set; }

        public NewGearData BaseNewGearData { get; set; }

        public float[] LastAttribute { get; set; }

        public AttributeType[] LastAttributeType { get; set; }

        public float[] NowAttribute { get; set; }

        public AttributeType[] NowAttributeType { get; set; }

        public GameFrameworkAction<object> OnOpenFinished {get;set;}

        public GameFrameworkAction<object> OnCloseAction { get; set; }
    }
}
