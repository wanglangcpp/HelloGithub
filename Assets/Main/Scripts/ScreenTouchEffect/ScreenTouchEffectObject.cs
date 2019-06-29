using GameFramework.ObjectPool;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ScreenTouchEffectObject : ObjectBase
    {
        public ScreenTouchEffectObject(object target)
            : base(target)
        {

        }

        protected override void Release()
        {
            var target = Target as ScreenTouchEffect;
            if (target == null || target.gameObject == null)
            {
                return;
            }

            GameObject.DestroyObject(target.gameObject);
        }
    }
}
