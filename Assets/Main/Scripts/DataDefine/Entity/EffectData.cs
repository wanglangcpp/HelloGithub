using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class EffectData : EntityData
    {
        [SerializeField]
        protected string m_TargetParentPath;

        [SerializeField]
        protected string m_ResourceName;

        [SerializeField]
        protected bool m_CanAttachToDeadOwner;

        [SerializeField]
        protected bool m_NeedSamplePosition = true;

        public new Effect Entity
        {
            get
            {
                return base.Entity as Effect;
            }
        }

        public string TargetParentPath
        {
            get
            {
                return m_TargetParentPath;
            }
        }

        public string ResourceName
        {
            get
            {
                return m_ResourceName;
            }
        }

        public bool CanAttachToDeadOwner
        {
            get
            {
                return m_CanAttachToDeadOwner;
            }

            set
            {
                m_CanAttachToDeadOwner = value;
            }
        }

        public bool NeedSamplePosition
        {
            get
            {
                return m_NeedSamplePosition;
            }

            set
            {
                m_NeedSamplePosition = value;
            }
        }

        public EffectData(int entityId, string resourceName, Vector3 position, float rotation)
            : base(entityId)
        {
            m_ResourceName = resourceName;
            Position = position.ToVector2();
            Rotation = rotation;
        }

        public EffectData(int entityId, string targetParentPath, string resourceName, int ownerId)
            : base(entityId)
        {
            m_TargetParentPath = targetParentPath;
            m_ResourceName = resourceName;
            OwnerId = ownerId;
        }
    }
}
