using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public abstract class EntityData
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private Vector2 m_Position = Vector2.zero;

        [SerializeField]
        private float m_Rotation = 0f;

        [SerializeField]
        private float m_Scale = 1f;

        [SerializeField]
        private int m_OwnerId = 0;

        public EntityData(int entityId)
        {
            m_Id = entityId;
            GameEntry.Data.RegisterEntityData(this);
        }

        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        public Entity Entity
        {
            get
            {
                return GameEntry.Entity.GetGameEntity(m_Id);
            }
        }

        public Vector2 Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        public float Rotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                m_Rotation = value;
            }
        }

        /// <summary>
        /// 模型缩放比例。
        /// </summary>
        public float Scale
        {
            get
            {
                return m_Scale;
            }
            set
            {
                m_Scale = value;
            }
        }

        /// <summary>
        /// 是否拥有所有者。
        /// </summary>
        public bool HasOwner
        {
            get
            {
                return m_OwnerId != 0;
            }
        }

        /// <summary>
        /// 所有者实体编号。
        /// </summary>
        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
            set
            {
                m_OwnerId = value;
            }
        }

        /// <summary>
        /// 用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            set;
        }

        public void UpdateTransform(PBTransformInfo pb)
        {
            Position = new Vector2(pb.PositionX, pb.PositionY);
            Rotation = pb.Rotation;
        }
    }
}
