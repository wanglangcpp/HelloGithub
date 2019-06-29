using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 玩家英雄移动更新事件。
    /// </summary>
    public class MyHeroMovingUpdateEventArgs : GameEventArgs
    {
        public MyHeroMovingUpdateEventArgs(int entityId, Vector3 position, float rotation, bool forceSendPacket)
        {
            EntityId = entityId;
            Position = position;
            Rotation = rotation;
            ForceSendPacket = forceSendPacket;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.MyHeroMovingUpdate;
            }
        }

        public int EntityId { get; private set; }

        public Vector3 Position { get; private set; }

        public float Rotation { get; private set; }

        public bool ForceSendPacket { get; private set; }
    }
}
