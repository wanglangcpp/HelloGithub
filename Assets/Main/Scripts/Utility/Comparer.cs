using GameFramework;
using GameFramework.DataTable;

namespace Genesis.GameClient
{
    /// <summary>
    /// 比较器。
    /// </summary>
    public static class Comparer
    {
        private static IDataTable<DRItem> s_CachedItemDataTable = null;

        /// <summary>
        /// 装备排序器。
        /// </summary>
        public static int GearDataComparer(GearData a, GearData b)
        {
            if (a.Quality != b.Quality)
            {
                return b.Quality.CompareTo(a.Quality);
            }

            if (a.StrengthenLevel != b.StrengthenLevel)
            {
                return b.StrengthenLevel.CompareTo(a.StrengthenLevel);
            }

            if (a.Level != b.Level)
            {
                return b.Level.CompareTo(a.Level);
            }

            if (a.Type != b.Type)
            {
                return a.Type.CompareTo(b.Type);
            }

            return a.Id.CompareTo(b.Id);
        }

        public static int GearDataWithHeroComparer(GearDataWithHero a, GearDataWithHero b)
        {
            if (a.HeroType > 0 && b.HeroType <= 0)
            {
                return -1;
            }

            if (a.HeroType <= 0 && b.HeroType > 0)
            {
                return 1;
            }

            if (a.HeroType > 0 && b.HeroType > 0)
            {
                return HeroComparer(GameEntry.Data.LobbyHeros.GetData(a.HeroType), GameEntry.Data.LobbyHeros.GetData(b.HeroType));
            }

            return GearDataComparer(a.GearData, b.GearData);
        }

        public static int CompareHeroes(BaseLobbyHeroData a, BaseLobbyHeroData b)
        {
            if (a is LobbyHeroData && !(b is LobbyHeroData))
            {
                return -1;
            }

            if (b is LobbyHeroData && !(a is LobbyHeroData))
            {
                return 1;
            }

            if (!(a is LobbyHeroData) && !(b is LobbyHeroData))
            {
                var unpossessedA = a as UnpossessedLobbyHeroData;
                var unpossessedB = b as UnpossessedLobbyHeroData;
                return UnpossessedHeroComparer(unpossessedA, unpossessedB);
            }

            var lobbyA = a as LobbyHeroData;
            var lobbyB = b as LobbyHeroData;
            return HeroComparer(lobbyA, lobbyB);
        }

        public static int CompareHeroes_Album(BaseLobbyHeroData a, BaseLobbyHeroData b)
        {
            if (a is LobbyHeroData && !(b is LobbyHeroData))
            {
                return -1;
            }

            if (b is LobbyHeroData && !(a is LobbyHeroData))
            {
                return 1;
            }

            if (!(a is LobbyHeroData) && !(b is LobbyHeroData))
            {
                var unpossessedA = a as UnpossessedLobbyHeroData;
                var unpossessedB = b as UnpossessedLobbyHeroData;
                return UnpossessedHeroComparer(unpossessedA, unpossessedB);
            }

            var lobbyA = a as LobbyHeroData;
            var lobbyB = b as LobbyHeroData;
            return HeroComparer_Album(lobbyA, lobbyB);
        }

        /// <summary>
        /// 可升星优先排序
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int CompareHeros_AlbumByUpStarLevel(BaseLobbyHeroData a, BaseLobbyHeroData b) {
            if (a is LobbyHeroData && !(b is LobbyHeroData))
            {
                return -1;
            }
            else if (b is LobbyHeroData && !(a is LobbyHeroData))
            {
                return 1;
            }
            else if (!(a is LobbyHeroData) && !(b is LobbyHeroData))
            {
                var unpossessedA = a as UnpossessedLobbyHeroData;
                var unpossessedB = b as UnpossessedLobbyHeroData;
                return UnpossessedHeroComparer(unpossessedA, unpossessedB);
            }
            else {
                var lobbyA = a as LobbyHeroData;
                var lobbyB = b as LobbyHeroData;
                return HeroComparer_AlbumCanStarLevelUp(lobbyA, lobbyB);
            }
        }

        public static int UnpossessedHeroComparer(UnpossessedLobbyHeroData a, UnpossessedLobbyHeroData b)
        {
            if (a.ComposedItemCount != b.ComposedItemCount)
            {
                b.ComposedItemCount.CompareTo(a.ComposedItemCount);
            }

            if (a.StarLevel != b.StarLevel)
            {
                b.StarLevel.CompareTo(a.StarLevel);
            }

            if (a.DefaultTotalQualityLevel != b.DefaultTotalQualityLevel)
            {
                b.DefaultTotalQualityLevel.CompareTo(a.DefaultTotalQualityLevel);
            }

            if (a.DefaultMight != b.DefaultMight)
            {
                b.DefaultMight.CompareTo(a.DefaultMight);
            }

            return a.Type.CompareTo(b.Type);
        }

        private static int HeroComparer_AlbumCanStarLevelUp(LobbyHeroData a, LobbyHeroData b)
        {
            if (a.CanStarLevelUp && !b.CanStarLevelUp)
            {
                return -1;
            }
            else if (!a.CanStarLevelUp && b.CanStarLevelUp)
            {
                return 1;
            }
            return HeroComparer_Album(a, b);
        }

        private static int HeroComparer_Album(LobbyHeroData a, LobbyHeroData b)
        {
            if (b.StarLevel != a.StarLevel)
            {
                return b.StarLevel.CompareTo(a.StarLevel);
            }

            if (b.Quality != a.Quality)
            {
                return b.Quality.CompareTo(a.Quality);
            }

            if (b.Level != a.Level)
            {
                return b.Level.CompareTo(a.Level);
            }

            if (b.Might != a.Might)
            {
                return b.Might.CompareTo(a.Might);
            }

            return a.Type.CompareTo(b.Type);
        }

        public static int HeroComparer(LobbyHeroData a, LobbyHeroData b)
        {
            int aIndex = -1;
            int bIndex = -1;

            var heroTeam = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default);
            for (int i = 0; i < heroTeam.HeroType.Count; i++)
            {
                if (heroTeam.HeroType[i] == a.Type)
                {
                    aIndex = i;
                }

                if (heroTeam.HeroType[i] == b.Type)
                {
                    bIndex = i;
                }
            }

            if (aIndex >= 0 && bIndex < 0)
            {
                return -1;
            }

            if (bIndex >= 0 && aIndex < 0)
            {
                return 1;
            }

            if (aIndex < 0 && bIndex < 0)
            {
                return HeroComparer_Album(a, b);
            }

            return aIndex.CompareTo(bIndex);
        }

        /// <summary>
        /// 战魂排序器。
        /// </summary>
        public static int SoulDataComparer(SoulData a, SoulData b)
        {
            if (a.Quality != b.Quality)
            {
                return b.Quality.CompareTo(a.Quality);
            }

            return a.Type.CompareTo(b.Type);
        }

        public static int SoulDataWithHeroComparer(SoulDataWithHero a, SoulDataWithHero b)
        {
            if (a.HeroType > 0 && b.HeroType <= 0)
            {
                return -1;
            }

            if (a.HeroType <= 0 && b.HeroType > 0)
            {
                return 1;
            }

            if (a.HeroType > 0 && b.HeroType > 0)
            {
                return HeroComparer(GameEntry.Data.LobbyHeros.GetData(a.HeroType), GameEntry.Data.LobbyHeros.GetData(b.HeroType));
            }

            return SoulDataComparer(a.SoulData, b.SoulData);
        }

        /// <summary>
        /// 铭文排序器。
        /// </summary>
        public static int EpigraphDataComparer(EpigraphData a, EpigraphData b)
        {
            if (a.DTQuality != b.DTQuality)
            {
                return b.DTQuality.CompareTo(a.DTQuality);
            }

            return a.Id.CompareTo(b.Id);
        }

        /// <summary>
        /// 道具排序器。
        /// </summary>
        public static int ItemDataComparer(ItemData a, ItemData b)
        {
            if (s_CachedItemDataTable == null)
            {
                s_CachedItemDataTable = GameEntry.DataTable.GetDataTable<DRItem>();
            }

            DRItem itemA = s_CachedItemDataTable.GetDataRow(a.Type);
            if (itemA == null)
            {
                Log.Warning("Can not find item {0}.", a.Type.ToString());
                return 1;
            }

            DRItem itemB = s_CachedItemDataTable.GetDataRow(b.Type);
            if (itemB == null)
            {
                Log.Warning("Can not find item {0}.", b.Type.ToString());
                return -1;
            }

            if (itemA.Order != itemB.Order)
            {
                return itemB.Order.CompareTo(itemA.Order);
            }

            if (itemA.Quality != itemB.Quality)
            {
                return itemB.Quality.CompareTo(itemA.Quality);
            }

            return a.Type.CompareTo(b.Type);
        }
    }
}
