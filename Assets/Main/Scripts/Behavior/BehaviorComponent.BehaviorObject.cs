using GameFramework;
using GameFramework.ObjectPool;

namespace Genesis.GameClient
{

    public partial class BehaviorComponent
    {
        private class BehaviorObject : ObjectBase
        {
            public BehaviorObject(string name, object target)
                : base(name, target)
            {

            }

            protected override void Release()
            {
                GameEntry.Resource.Recycle(Target);
                Log.Info("Recycle behavior '{0}'.", Name);
            }
        }
    }
}
