namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="StarUpSuccessForm"/> 的显示数据。
    /// </summary>
    public class StrengthenDisplayData : UIFormBaseUserData
    {
        public BaseLobbyHeroData BaseHeroData { get; set; }

        public float LastMaxHP { get; set; }

        public float LastPhysicalAttack { get; set; }

        public float LastPhysicalDefense { get; set; }
    }
}
