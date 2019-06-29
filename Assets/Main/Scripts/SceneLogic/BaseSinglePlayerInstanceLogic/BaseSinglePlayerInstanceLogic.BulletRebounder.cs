using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        private readonly IDictionary<int, int> m_BulletRebounderIdToEntityId = new Dictionary<int, int>();

        public bool ShowBulletRebounder(int bulletRebounderId)
        {
            if (m_BulletRebounderIdToEntityId.ContainsKey(bulletRebounderId))
            {
                Log.Warning("There already exists bullet rebounder '{0}'.", bulletRebounderId.ToString());
                return false;
            }

            IDataTable<DRBulletRebounder> dt = GameEntry.DataTable.GetDataTable<DRBulletRebounder>();
            DRBulletRebounder dataRow = dt.GetDataRow(bulletRebounderId);
            if (dataRow == null)
            {
                Log.Warning("Can not find bullet rebounder '{0}'.", bulletRebounderId.ToString());
                return false;
            }

            int entityId = GameEntry.Entity.GetSerialId();
            var data = new BulletRebounderData(entityId);
            data.BulletRebounderId = bulletRebounderId;
            data.Position = new Vector2(dataRow.PositionX, dataRow.PositionY);
            data.Rotation = dataRow.Angle;
            GameEntry.Entity.ShowBulletRebounder(data);
            m_BulletRebounderIdToEntityId.Add(bulletRebounderId, entityId);

            return true;
        }

        public bool HideBulletRebounder(int bulletRebounderId)
        {
            int entityId = 0;
            if (!m_BulletRebounderIdToEntityId.TryGetValue(bulletRebounderId, out entityId))
            {
                Log.Warning("Can not find air wall '{0}'.", bulletRebounderId.ToString());
                return false;
            }

            GameEntry.Entity.HideEntity(entityId);
            m_BulletRebounderIdToEntityId.Remove(bulletRebounderId);

            return true;
        }
    }
}
