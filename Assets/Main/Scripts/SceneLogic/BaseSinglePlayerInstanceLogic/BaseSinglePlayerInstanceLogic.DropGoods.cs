using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        private readonly IList<KeyValuePair<int, int>> m_DropList = new List<KeyValuePair<int, int>>();

        public int DropGoodsCount
        {
            get;
            set;
        }

        protected void InitDropGoodsList()
        {
            var dataItem = GameEntry.Data.InstanceGoods.InstanceItemData;
            var dataSoul = GameEntry.Data.InstanceGoods.InstanceSoulData;
            for (int i = 0; i < dataItem.Count; i++)
            {
                m_DropList.Add(new KeyValuePair<int, int>(dataItem[i].Key, dataItem[i].Count));
            }

            for (int i = 0; i < dataSoul.Count; i++)
            {
                m_DropList.Add(new KeyValuePair<int, int>(dataSoul[i].Key, 1));
            }
        }

        private void CheckDropGoods(NpcCharacter npc)
        {
            int count = GameEntry.Data.InstanceGoods.GoodsCount;
            if (DropGoodsCount >= count)
            {
                return;
            }

            if (npc.Data.Category == NpcCategory.Boss)
            {
                DropGoods(npc, count - DropGoodsCount);
                return;
            }

            int dropCount = 0;
            var dt = GameEntry.DataTable.GetDataTable<DRDropProbability>();
            DRDropProbability dr = dt.GetDataRow(InstanceId);
            if (dr == null)
            {
                Log.Error("Can not find InstanceId '{0}' in drop probability table.", InstanceId.ToString());
                return;
            }

            for (int i = 0; i < m_DropList.Count && (DropGoodsCount + dropCount) < (count - 1); i++)
            {
                if (m_DropList[i].Value <= 0)
                {
                    continue;
                }

                if (npc.Data.Category == NpcCategory.Normal && dr.NormalRate > 0f && Random.value < dr.NormalRate)
                {
                    dropCount++;
                    continue;
                }

                if (npc.Data.Category == NpcCategory.Elite && dr.EliteRate > 0f && Random.value < dr.EliteRate)
                {
                    dropCount++;
                    continue;
                }
            }

            if (dropCount > 0)
            {
                DropGoods(npc, dropCount);
            }
        }

        private void DropGoods(NpcCharacter npc, int count)
        {
            DropGoodsCount += count;
            for (int i = 0; i < count; i++)
            {
                ChestData chestData = new ChestData(GameEntry.Entity.GetSerialId());
                chestData.Position = npc.CachedTransform.localPosition.ToVector2();
                chestData.Rotation = npc.CachedTransform.localRotation.eulerAngles.y;
                GameEntry.Entity.ShowChest(chestData);
            }
        }
    }
}
