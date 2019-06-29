using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="HeroInfoForm"/> 显示数据
    /// </summary>
    public class HeroInfoDisplayData : UIFormBaseUserData
    {
        public HeroInfoScenario Scenario { get; set; }

        public int IndexInAllHeroes { get; set; }

        public List<BaseLobbyHeroData> AllHeroes { get; set; }
    }
}
