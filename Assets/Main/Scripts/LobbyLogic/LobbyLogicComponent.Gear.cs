using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        public List<GearData> GetOneTypeOfGear(int gearTypeId)
        {
            List<GearData> allGear = GameEntry.Data.Gears.Data;
            List<GearData> retGear = new List<GearData>();
            for (int i = 0; i < allGear.Count; i++)
            {
                if (allGear[i].Position == gearTypeId)
                {
                    retGear.Add(allGear[i]);
                }
            }
            return retGear;
        }

        public List<GearData> GetRecommendedGear(List<GearData> gearList, int HeroId)
        {
            List<GearData> retGear = new List<GearData>();
            if (gearList.Count < 4)
            {
                return gearList;
            }

            gearList.Sort(Comparer.GearDataComparer);

            for (int i = 0; i < 4; i++)
            {
                if (i + 1 > gearList.Count)
                {
                    break;
                }
                bool hasGear = false;
                for (int j = 0; j < retGear.Count; j++)
                {
                    if (retGear[j].Type == gearList[i].Type)
                    {
                        hasGear = true;
                        break;
                    }
                }
                if (!hasGear)
                {
                    retGear.Add(gearList[i]);
                }
            }
            return retGear;
        }

        public int GetUpgradeCost(int quality, int level, out int nextLevelCost)
        {
            DRGearLevelUp gearUpgradeData = GameEntry.DataTable.GetDataTable<DRGearLevelUp>().GetDataRow(quality);
            if (gearUpgradeData == null)
            {
                Log.Error("Wrong gear quality.");
                nextLevelCost = 0;
                return 0;
            }

            nextLevelCost = gearUpgradeData.LevelUpCostCoin[level - 1];
            int totalCost = 0;
            for (int i = level; i < GameEntry.Data.Player.Level; i++)
            {
                totalCost += gearUpgradeData.LevelUpCostCoin[i - 1];
            }
            return totalCost;
        }

        public GearData[] GetFastComposeGears(GearData gear)
        {
            GearData[] retGears = new GearData[Constant.GearComposeMaterialCount];
            if (gear == null)
            {
                List<GearData>[] groupedGears = new List<GearData>[Constant.GearQualityCount + 1];
                for (int i = 1; i <= Constant.GearQualityCount; i++)
                {
                    groupedGears[i] = new List<GearData>();
                }
                List<GearData> allGears = GameEntry.Data.Gears.Data;
                for (int i = 0; i < allGears.Count; i++)
                {
                    groupedGears[allGears[i].Quality].Add(allGears[i]);
                }
                for (int i = 1; i < Constant.GearQualityCount; i++)
                {
                    if (groupedGears[i].Count >= Constant.GearComposeMaterialCount)
                    {
                        for (int j = 0; j < Constant.GearComposeMaterialCount; j++)
                        {
                            retGears[j] = groupedGears[i][j];
                        }
                        return retGears;
                    }
                }
                return null;
            }
            else
            {
                List<GearData> specifiedGears = new List<GearData>();
                List<GearData> allGears = GameEntry.Data.Gears.Data;
                for (int i = 0; i < allGears.Count; i++)
                {
                    if (allGears[i].Quality == gear.Quality && allGears[i].Id != gear.Id)
                    {
                        specifiedGears.Add(allGears[i]);
                    }
                }
                if (specifiedGears.Count < Constant.GearComposeMaterialCount)
                {
                    return null;
                }
                else
                {
                    retGears[0] = gear;
                    for (int i = 1; i <= Constant.GearComposeMaterialCount; i++)
                    {
                        retGears[i] = specifiedGears[i - 1];
                    }
                }
            }
            return retGears;
        }
    }
}
