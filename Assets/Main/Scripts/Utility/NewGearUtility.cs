using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备工具类。
    /// </summary>
    public static class NewGearUtility
    {
        /// <summary>
        /// 获取装备图标编号。
        /// </summary>
        /// <param name="gearId">装备编号。</param>
        /// <returns></returns>
        public static int GetIconId(int gearId)
        {
            var dt = GameEntry.DataTable.GetDataTable<DRNewGear>();
            DRNewGear dr = dt.GetDataRow(gearId);
            if (dr == null)
            {
                Log.Warning("Gear '{0}' not found.", gearId.ToString());
                return 0;
            }

            return dr.IconId;
        }
    }
}
