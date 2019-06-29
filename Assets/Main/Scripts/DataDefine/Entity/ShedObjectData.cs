using System;

namespace Genesis.GameClient
{
    [Serializable]
    public abstract class ShedObjectData : EntityData
    {
        public ShedObjectData(int entityId)
            : base(entityId)
        {

        }

        public new ShedObject Entity
        {
            get
            {
                return base.Entity as AirWall;
            }
        }
    }
}
