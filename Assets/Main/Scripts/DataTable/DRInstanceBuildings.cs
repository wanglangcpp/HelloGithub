using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本内建筑物配置表。
    /// </summary>
    public partial class DRInstanceBuildings : DRInstanceEntities
    {
        /// <summary>
        /// 建筑物类型编号（对应 Building 表编号）
        /// </summary>
        public int BuildingId
        {
            get
            {
                return EntityTypeId;
            }
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRInstanceBuildings>();
        }
    }
}
