using UnityEngine;

namespace Genesis.GameClient
{
    public abstract class EntityMotion : MonoBehaviour
    {
        protected Entity m_Owner = null;

        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }

        protected virtual void Awake()
        {
            m_Owner = GetComponent<Entity>();
        }
    }
}
