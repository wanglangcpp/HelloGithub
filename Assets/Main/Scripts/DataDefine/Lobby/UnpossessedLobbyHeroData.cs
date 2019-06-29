using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class UnpossessedLobbyHeroData : BaseLobbyHeroData
    {
        public int ComposedItemCount
        {
            get;
            set;
        }

        public int ComposeItemNeed
        {
            get;
            set;
        }

        public UnpossessedLobbyHeroData(int type) : base()
        {
            Type = type;
            Level = 1;
            StarLevel = DefaultStarLevel;
            TotalQualityLevel = DefaultTotalQualityLevel;
            SkillLevels.AddRange(Constant.DefaultHeroSkillLevels);
            var items = GameEntry.Data.Items;
            var item = items.GetData(StarLevelUpItemId);
            ComposedItemCount = (item == null ? 0 : item.Count);
            ComposeItemNeed = PiecesPerHero;
        }
    }
}
