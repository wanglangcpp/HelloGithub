namespace Genesis.GameClient
{
    public class SoulDataWithHero
    {
        public SoulData SoulData
        {
            get;
            set;
        }

        public int HeroType
        {
            get;
            set;
        }

        public SoulDataWithHero(SoulData soulData, int heroType = 0)
        {
            SoulData = soulData;
            HeroType = heroType;
        }
    }
}
