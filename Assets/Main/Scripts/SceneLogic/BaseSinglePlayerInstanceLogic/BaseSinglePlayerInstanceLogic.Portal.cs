using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        private readonly IDictionary<int, int> m_PortalIdToEntityId = new Dictionary<int, int>();

        public bool ShowPortal(int portalId, PortalParam[] portalParams, int? guidePointGroupToActivateOnExit, int? guidePointGroupToActivateOnShow)
        {
            if (m_PortalIdToEntityId.ContainsKey(portalId))
            {
                Log.Warning("There already exists portal '{0}'.", portalId.ToString());
                return false;
            }

            IDataTable<DRPortal> drPortal = GameEntry.DataTable.GetDataTable<DRPortal>();
            DRPortal dataRow = drPortal.GetDataRow(portalId);
            if (dataRow == null)
            {
                Log.Warning("Can not find portal '{0}'.", portalId.ToString());
                return false;
            }

            int entityId = GameEntry.Entity.GetSerialId();
            PortalData portalData = new PortalData(entityId);
            portalData.PortalId = portalId;
            portalData.Position = new Vector2(dataRow.PositionX, dataRow.PositionY);
            portalData.Rotation = dataRow.Angle;
            portalData.Radius = dataRow.Radius;
            portalData.PortalParams = portalParams;
            portalData.GuidePointGroupToActivateOnExit = guidePointGroupToActivateOnExit;
            portalData.GuidePointGroupToActivateOnShow = guidePointGroupToActivateOnShow;
            GameEntry.Entity.ShowPortal(portalData);
            m_PortalIdToEntityId.Add(portalId, entityId);
            return true;
        }

        public bool HidePortal(int portalId)
        {
            int entityId = 0;
            if (!m_PortalIdToEntityId.TryGetValue(portalId, out entityId))
            {
                Log.Warning("Can not find portal '{0}'.", portalId.ToString());
                return false;
            }

            GameEntry.Entity.HideEntity(entityId);
            m_PortalIdToEntityId.Remove(portalId);

            return true;
        }
    }
}
