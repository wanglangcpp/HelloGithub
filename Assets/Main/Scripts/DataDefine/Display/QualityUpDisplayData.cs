namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="QualityUpSuccessForm"/> 的显示数据。
    /// </summary>
    public class QualityUpDisplayData : UIFormBaseUserData
    {
        public QualityType LastQuality { get; set; }

        public BaseLobbyHeroData BaseHeroData { get; set; }

        public int LastQualityLevel { get; set; }

        public float LastMaxHp { get; set; }

        public float LastPhysicalAttack { get; set; }

        public float LastPhysicalDefense { get; set; }
    }
}
