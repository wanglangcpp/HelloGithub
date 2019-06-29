using GameFramework.ObjectPool;
using UnityEngine;

namespace Genesis.GameClient
{
    public class BubbleObject : ObjectBase
    {
        public BubbleObject(object target)
            : base(target)
        {

        }

        protected override void Release()
        {
            var target = Target as Bubble;
            if (target == null || target.gameObject == null)
            {
                return;
            }

            GameObject.DestroyObject((Target as Bubble).gameObject);
        }
    }
}
