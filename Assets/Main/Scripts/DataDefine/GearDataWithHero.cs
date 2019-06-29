namespace Genesis.GameClient
{
    public class GearDataWithHero
    {
        public GearData GearData
        {
            get;
            set;
        }

        public int HeroType
        {
            get;
            set;
        }

        public GearDataWithHero(GearData gearData, int heroType = 0)
        {
            GearData = gearData;
            HeroType = heroType;
        }
    }
}
