using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public class PortagingEffectEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.PortagingEffect;
            }
        }

        public PortagingEffectEventArgs(int portalId, Vector3 effectPosition, float effectRotation, bool isPrepareToPortage)
        {
            m_PortalId = portalId;
            m_EffectPosition = effectPosition;
            m_EffectRotation = effectRotation;
            m_IsPrepareToPortage = isPrepareToPortage;
        }

        private int m_PortalId;
        private Vector3 m_EffectPosition;
        private float m_EffectRotation;
        private bool m_IsPrepareToPortage;

        public int PortalId
        {
            get
            {
                return m_PortalId;
            }
        }

        public Vector3 EffectPosition
        {
            get
            {
                return m_EffectPosition;
            }
        }

        public float EffectRotation
        {
            get
            {
                return m_EffectRotation;
            }
        }

        public bool IsPrepareToPortage
        {
            get
            {
                return m_IsPrepareToPortage;
            }
        }
    }
}
