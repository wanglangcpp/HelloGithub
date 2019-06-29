using GameFramework.ObjectPool;
using UnityEngine;

namespace Genesis.GameClient
{
    public class MonsterPositionArrowObject : ObjectBase
    {
        public MonsterPositionArrowObject(object target)
            : base(target)
        {

        }

        protected override void Release()
        {
            var target = Target as MonsterPositionArrow;
            if (target == null || target.gameObject == null)
            {
                return;
            }

            GameObject.DestroyObject((Target as MonsterPositionArrow).gameObject);
        }
    }
}
