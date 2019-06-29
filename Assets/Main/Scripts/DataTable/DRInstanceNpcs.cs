using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本内NPC配置表。
    /// </summary>
    public partial class DRInstanceNpcs : DRInstanceEntities
    {
        /// <summary>
        /// NPC类型编号（对应 NPC 表编号）。
        /// </summary>
        public int NpcId
        {
            get
            {
                return EntityTypeId;
            }
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRInstanceNpcs>();
        }
    }
}
