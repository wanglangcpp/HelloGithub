using GameFramework;

namespace Genesis.GameClient
{
    public class StringReplacementRule_HeroName : IStringReplacementRule
    {
        public string Key
        {
            get
            {
                return "HeroName";
            }
        }

        public int MinArgCount
        {
            get
            {
                return 2;
            }
        }

        public string DoAction(string[] args)
        {
            string str = string.Empty;
            DRHero heroData = GameEntry.DataTable.GetDataTable<DRHero>().GetDataRow(int.Parse(args[1]));
            if (heroData == null)
            {
                Log.Error("StringReplacementRule_AnnounceHeroName Wrong Hero Data Id {0}");
                return str;
            }

            str = GameEntry.Localization.GetString(heroData.Name);

            return str;
        }
    }
}
