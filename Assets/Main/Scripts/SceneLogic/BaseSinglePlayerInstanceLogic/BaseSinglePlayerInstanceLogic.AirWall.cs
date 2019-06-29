using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        private readonly IDictionary<int, int> m_AirWallIdToEntityId = new Dictionary<int, int>();

        public bool ShowAirWall(int airWallId)
        {
            if (m_AirWallIdToEntityId.ContainsKey(airWallId))
            {
                Log.Warning("There already exists air wall '{0}'.", airWallId.ToString());
                return false;
            }

            IDataTable<DRAirWall> dtAirWall = GameEntry.DataTable.GetDataTable<DRAirWall>();
            DRAirWall dataRow = dtAirWall.GetDataRow(airWallId);
            if (dataRow == null)
            {
                Log.Warning("Can not find air wall '{0}'.", airWallId.ToString());
                return false;
            }

            int entityId = GameEntry.Entity.GetSerialId();
            AirWallData airWallData = new AirWallData(entityId);
            airWallData.AirWallId = airWallId;
            airWallData.Position = new Vector2(dataRow.PositionX, dataRow.PositionY);
            airWallData.Rotation = dataRow.Angle;
            GameEntry.Entity.ShowAirWall(airWallData);
            m_AirWallIdToEntityId.Add(airWallId, entityId);

            return true;
        }

        public bool HideAirWall(int airWallId)
        {
            int entityId = 0;
            if (!m_AirWallIdToEntityId.TryGetValue(airWallId, out entityId))
            {
                Log.Warning("Can not find air wall '{0}'.", airWallId.ToString());
                return false;
            }

            GameEntry.Entity.HideEntity(entityId);
            m_AirWallIdToEntityId.Remove(airWallId);

            return true;
        }

        public bool AirWallEnableObstacle(int airWallId, bool enabled)
        {
            int entityId = 0;
            if (!m_AirWallIdToEntityId.TryGetValue(airWallId, out entityId))
            {
                Log.Warning("Can not find air wall '{0}'.", airWallId.ToString());
                return false;
            }

            var rawEntity = GameEntry.Entity.GetEntity(entityId);
            if (rawEntity == null)
            {
                return false;
            }

            var airWall = rawEntity.Logic as AirWall;
            if (airWall == null)
            {
                return false;
            }

            return airWall.EnableObstacle(enabled);
        }

        public bool AirWallPlayAnimation(int airWallId, string animationName)
        {
            int entityId = 0;
            if (!m_AirWallIdToEntityId.TryGetValue(airWallId, out entityId))
            {
                Log.Warning("Can not find air wall '{0}'.", airWallId.ToString());
                return false;
            }

            var rawEntity = GameEntry.Entity.GetEntity(entityId);
            if (rawEntity == null)
            {
                return false;
            }

            var airWall = rawEntity.Logic as AirWall;
            if (airWall == null)
            {
                return false;
            }

            airWall.PlayAnimation(animationName);
            return true;
        }
    }
}
