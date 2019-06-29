using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class ChestData : ShedObjectData
    {
        public ChestData(int entityId)
            : base(entityId)
        {

        }

        public new Chest Entity
        {
            get
            {
                return base.Entity as Chest;
            }
        }
    }
}
