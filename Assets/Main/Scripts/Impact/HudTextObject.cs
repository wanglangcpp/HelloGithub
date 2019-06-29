using GameFramework.ObjectPool;
using UnityEngine;

namespace Genesis.GameClient
{
    public class HudTextObject : ObjectBase
    {
        public HudTextObject(object target)
            : base(target)
        {

        }

        protected override void Release()
        {
            var target = Target as HudText;
            if (target == null || target.gameObject == null)
            {
                return;
            }

            GameObject.DestroyObject(target.gameObject);
        }
    }
}
