using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 建筑死亡事件。
    /// </summary>
    public class BuildingDeadEventArgs : GameEventArgs
    {
        public BuildingDeadEventArgs(Building deadBuilding, Entity impactSourceEntity)
        {
            Building = deadBuilding;
            BuildingData = deadBuilding.Data;
            ImpactSourceEntity = impactSourceEntity;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.BuildingDead;
            }
        }

        public Building Building
        {
            get;
            private set;
        }

        public BuildingData BuildingData
        {
            get;
            private set;
        }

        public Entity ImpactSourceEntity
        {
            get;
            private set;
        }
    }
}
