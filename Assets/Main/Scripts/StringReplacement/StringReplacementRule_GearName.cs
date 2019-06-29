using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class StringReplacementRule_GearName : IStringReplacementRule
    {
        public string Key
        {
            get
            {
                return "GearName";
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
            DRGear gearData = GameEntry.DataTable.GetDataTable<DRGear>().GetDataRow(int.Parse(args[1]));
            if (gearData == null)
            {
                Log.Error("StringReplacementRule_AnnounceGearName Wrong Gear Data Id.");
                return str;
            }

            Color qualityColor = ColorUtility.GetColorForQuality(gearData.Quality);

            str = ColorUtility.AddColorToString(qualityColor, GameEntry.Localization.GetString(gearData.Name));

            return str;
        }
    }
}
