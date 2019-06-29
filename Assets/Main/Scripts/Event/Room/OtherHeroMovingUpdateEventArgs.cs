using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public class OtherHeroMovingUpdateEventArgs : GameEventArgs
    {
        public OtherHeroMovingUpdateEventArgs(int playerId, int entityId, Vector2 position, float rotation)
        {
            PlayerId = playerId;
            EntityId = entityId;
            Position = position;
            Rotation = rotation;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.OtherHeroMovingUpdate;
            }
        }

        public int PlayerId { get; private set; }

        public int EntityId { get; private set; }

        public Vector2 Position { get; private set; }

        public float Rotation { get; private set; }
    }
}
