using GameFramework;
using GameFramework.DataTable;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class OfflineBuffPool : BaseBuffPool
    {
        public OfflineBuffPool(IBuffTargetData target, IDataTable<DRBuff> buffDataTable, IDataTable<DRBuffReplace> replaceDataTable, int capacity,
            Predicate<BuffData> customRuleForAdding, GameFrameworkFunc<BuffData, IList<BuffData>> customRuleForRemoving)
            : base(target, buffDataTable, replaceDataTable, capacity, customRuleForAdding, customRuleForRemoving)
        {

        }
    }
}
