namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="HeroTeamForm"/> 显示数据。
    /// </summary>
    public class HeroTeamDisplayData : UIFormBaseUserData
    {
        public LobbyHeroesData Heroes { get; set; }

        public HeroTeamDisplayScenario Scenario { get; set; }

        public InstanceLogicType InstanceLogicType { get; set; }

        public int InstanceId { get; set; }

        public HeroTeamDisplayData()
        {
            InstanceId = -1;
        }
    }
}
