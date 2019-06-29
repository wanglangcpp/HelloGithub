using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class StringReplacementRule_SoulName : IStringReplacementRule
    {
        public string Key
        {
            get
            {
                return "SoulName";
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
            DRSoul soulData = GameEntry.DataTable.GetDataTable<DRSoul>().GetDataRow(int.Parse(args[1]));
            if (soulData == null)
            {
                Log.Error("StringReplacement_SoulName Wrong Gear Data Id {0}");
                return str;
            }

            Color qualityColor = ColorUtility.GetColorForQuality(soulData.Quality);

            str = ColorUtility.AddColorToString(qualityColor, GameEntry.Localization.GetString(soulData.Name));

            return str;
        }
    }
}
