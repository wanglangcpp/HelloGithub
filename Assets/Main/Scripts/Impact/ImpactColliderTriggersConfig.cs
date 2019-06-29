using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class ImpactColliderTriggersConfig : ScriptableObject
    {
        [SerializeField]
        private ColliderTrigger[] m_ColliderTriggerTemplate = null;

        public ColliderTrigger this[int index]
        {
            get
            {
                return m_ColliderTriggerTemplate[index];
            }
        }

        public int Length
        {
            get
            {
                return m_ColliderTriggerTemplate.Length;
            }
        }
    }
}
