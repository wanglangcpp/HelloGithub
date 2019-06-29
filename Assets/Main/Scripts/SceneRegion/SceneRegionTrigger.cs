using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class SceneRegionTrigger : MonoBehaviour
    {
        private Collider m_CachedCollider = null;

        public event GameFrameworkAction<Collider, Collider> SceneRegionTriggerEnter = null;

        public event GameFrameworkAction<Collider, Collider> SceneRegionTriggerExit = null;

        public event GameFrameworkAction<Collider, Collider> SceneRegionTriggerStay = null;

        private void Awake()
        {
            m_CachedCollider = GetComponent<Collider>();
            m_CachedCollider.isTrigger = true;
            Rigidbody rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            gameObject.layer = Constant.Layer.RegionTriggerLayerId;
        }

        private void OnDisable()
        {
            SceneRegionTriggerEnter = null;
            SceneRegionTriggerExit = null;
            SceneRegionTriggerStay = null;
        }

        private void OnTriggerEnter(Collider target)
        {
            if (SceneRegionTriggerEnter == null)
            {
                return;
            }

            SceneRegionTriggerEnter(m_CachedCollider, target);
        }

        private void OnTriggerExit(Collider target)
        {
            if (SceneRegionTriggerExit == null)
            {
                return;
            }

            SceneRegionTriggerExit(m_CachedCollider, target);
        }

        private void OnTriggerStay(Collider target)
        {
            if (SceneRegionTriggerStay == null)
            {
                return;
            }

            SceneRegionTriggerStay(m_CachedCollider, target);
        }
    }
}
