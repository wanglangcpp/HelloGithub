using GameFramework.ObjectPool;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ColliderTriggerObject : ObjectBase
    {
        public ColliderTriggerObject(string name, object target)
            : base(name, target)
        {

        }

        protected override void Release()
        {
            var colliderTrigger = Target as ColliderTrigger;
            if (colliderTrigger == null || colliderTrigger.gameObject == null)
            {
                return;
            }

            GameObject.DestroyObject(colliderTrigger.gameObject);
        }
    }
}
