using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        /// <summary>
        /// 背包 -- 使用道具。
        /// </summary>
        /// <param name="typeId">道具类型编号。</param>
        /// <param name="count">数量。</param>
        /// <param name="heroTypeId">英雄类型编号。</param>
        public void InventoryUseHeroExpItem(int typeId, int count, int heroTypeId = 0)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                var request = new CLUseHeroExpItem
                {
                    HeroId = heroTypeId,
                    ItemInfo = new PBItemInfo { Type = typeId, Count = count },
                };

                GameEntry.Network.Send(request);
                return;
            }
        }
    }
}
