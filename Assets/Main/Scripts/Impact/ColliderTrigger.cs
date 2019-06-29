using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ColliderTrigger : MonoBehaviour
    {
        private Transform m_CachedTransform = null;
        private Collider m_CachedCollider = null;

        public event GameFrameworkAction<ITimeLineInstance<Entity>, Collider> TriggerEnter = null;

        public event GameFrameworkAction<ITimeLineInstance<Entity>, Collider> TriggerExit = null;

        public event GameFrameworkAction<ITimeLineInstance<Entity>, Collider> TriggerStay = null;

        public Transform CachedTransform
        {
            get
            {
                return m_CachedTransform;
            }
        }

        public ITimeLineInstance<Entity> TimeLineInstance
        {
            get;
            set;
        }

        private void Awake()
        {
            m_CachedTransform = GetComponent<Transform>();
            m_CachedCollider = GetComponent<Collider>();
            m_CachedCollider.isTrigger = true;
            Rigidbody rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            gameObject.layer = Constant.Layer.ColliderTriggerLayerId;
        }

        private void OnDisable()
        {
            TriggerEnter = null;
            TriggerExit = null;
            TriggerStay = null;
            TimeLineInstance = null;
        }

        private void OnTriggerEnter(Collider target)
        {
            if (TriggerEnter == null)
            {
                return;
            }

            TriggerEnter(TimeLineInstance, target);
        }

        private void OnTriggerExit(Collider target)
        {
            if (TriggerExit == null)
            {
                return;
            }

            TriggerExit(TimeLineInstance, target);
        }

        private void OnTriggerStay(Collider target)
        {
            if (TriggerStay == null)
            {
                return;
            }

            TriggerStay(TimeLineInstance, target);
        }
    }
}
