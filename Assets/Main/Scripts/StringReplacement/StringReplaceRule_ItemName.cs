using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    internal class StringReplaceRule_ItemName : IStringReplacementRule
    {
        public string Key
        {
            get
            {
                return "ItemName";
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
            DRItem itemData = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(int.Parse(args[1]));
            if (itemData == null)
            {
                Log.Error("StringReplaceRule_ItemName Wrong Item Data Id {0}");
                return str;
            }

            Color qualityColor = ColorUtility.GetColorForQuality(itemData.Quality);

            str = ColorUtility.AddColorToString(qualityColor, GameEntry.Localization.GetString(itemData.Name));

            return str;
        }
    }
}
