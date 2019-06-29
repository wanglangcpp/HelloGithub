using GameFramework.ObjectPool;
using UnityEngine;

namespace Genesis.GameClient
{
    public class NameBoardObject : ObjectBase
    {
        public NameBoardObject(object target)
            : base(target)
        {

        }

        protected override void Release()
        {
            var target = Target as NameBoard;
            if (target == null || target.gameObject == null)
            {
                return;
            }

            GameObject.DestroyObject((Target as NameBoard).gameObject);
        }
    }
}
