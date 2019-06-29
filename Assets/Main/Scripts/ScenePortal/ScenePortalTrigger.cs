using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ScenePortalTrigger : MonoBehaviour
    {
        private Collider m_CachedCollider = null;

        public event GameFrameworkAction<Collider, Collider> ScenePortalTriggerEnter = null;

        public event GameFrameworkAction<Collider, Collider> ScenePortalTriggerExit = null;

        public event GameFrameworkAction<Collider, Collider> ScenePortalTriggerStay = null;

        private void Awake()
        {
            m_CachedCollider = GetComponent<Collider>();
            m_CachedCollider.isTrigger = true;
            Rigidbody rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            gameObject.layer = Constant.Layer.PortalTriggerLayerId;
        }

        private void OnDisable()
        {
            ScenePortalTriggerEnter = null;
            ScenePortalTriggerExit = null;
            ScenePortalTriggerStay = null;
        }

        private void OnTriggerEnter(Collider target)
        {
            if (ScenePortalTriggerEnter == null)
            {
                return;
            }

            ScenePortalTriggerEnter(m_CachedCollider, target);
        }

        private void OnTriggerExit(Collider target)
        {
            if (ScenePortalTriggerExit == null)
            {
                return;
            }

            ScenePortalTriggerExit(m_CachedCollider, target);
        }

        private void OnTriggerStay(Collider target)
        {
            if (ScenePortalTriggerStay == null)
            {
                return;
            }

            ScenePortalTriggerStay(m_CachedCollider, target);
        }
    }
}
