using GameFramework;
using GameFramework.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ImpactComponent
    {
        [Serializable]
        private class ColliderTriggers
        {
            [SerializeField]
            private Transform m_ColliderTriggerInstanceRoot = null;

            [SerializeField]
            private int m_InstancePoolCapacity = 16;

            [SerializeField]
            private ImpactColliderTriggersConfig m_Config = null;

            private IDictionary<string, ColliderTrigger> m_ColliderTriggerTemplates;
            private IObjectPool<ColliderTriggerObject> m_ColliderTriggerObjects;

            public void Init()
            {
                if (m_ColliderTriggerInstanceRoot == null)
                {
                    Log.Error("You must set collider trigger instance root first.");
                    return;
                }

                m_ColliderTriggerTemplates = new Dictionary<string, ColliderTrigger>();
                for (int i = 0; i < m_Config.Length; i++)
                {
                    string colliderTriggerName = m_Config[i].name;
                    colliderTriggerName = colliderTriggerName.Substring(0, colliderTriggerName.IndexOf("ColliderTrigger"));
                    m_ColliderTriggerTemplates.Add(colliderTriggerName, m_Config[i]);
                }

                m_ColliderTriggerObjects = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<ColliderTriggerObject>("ColliderTrigger", m_InstancePoolCapacity);
            }

            public void Shutdown()
            {
                Clear();
            }

            public void Update()
            {

            }

            public void Clear()
            {

            }

            public ColliderTrigger CreateColliderTrigger(string colliderTriggerName)
            {
                ColliderTrigger colliderTrigger = null;
                ColliderTriggerObject colliderTriggerObject = m_ColliderTriggerObjects.Spawn(colliderTriggerName);
                if (colliderTriggerObject != null)
                {
                    colliderTrigger = colliderTriggerObject.Target as ColliderTrigger;
                    colliderTrigger.gameObject.SetActive(true);
                    return colliderTrigger;
                }

                ColliderTrigger colliderTriggerTemplate = null;
                if (!m_ColliderTriggerTemplates.TryGetValue(colliderTriggerName, out colliderTriggerTemplate))
                {
                    Log.Warning("Can not find collider trigger template '{0}'.", colliderTriggerName);
                    return null;
                }

                colliderTrigger = Instantiate(colliderTriggerTemplate);
                colliderTrigger.CachedTransform.SetParent(m_ColliderTriggerInstanceRoot);
                m_ColliderTriggerObjects.Register(new ColliderTriggerObject(colliderTriggerName, colliderTrigger), true);
                return colliderTrigger;
            }

            public void DestroyColliderTrigger(ColliderTrigger colliderTrigger)
            {
                if (!GameEntry.IsAvailable)
                {
                    return;
                }

                if (colliderTrigger == null || m_ColliderTriggerInstanceRoot == null)
                {
                    return;
                }

                colliderTrigger.CachedTransform.SetParent(m_ColliderTriggerInstanceRoot);
                colliderTrigger.gameObject.SetActive(false);
                m_ColliderTriggerObjects.Unspawn(colliderTrigger);
            }
        }
    }
}
