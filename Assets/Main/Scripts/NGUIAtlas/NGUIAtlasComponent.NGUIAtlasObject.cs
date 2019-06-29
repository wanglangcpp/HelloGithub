using GameFramework;
using GameFramework.ObjectPool;

namespace Genesis.GameClient
{
    public partial class NGUIAtlasComponent
    {
        private class NGUIAtlasObject : ObjectBase
        {
            public NGUIAtlasObject(string name, object target)
                : base(name, target)
            {

            }

            protected override void Release()
            {
                GameEntry.Resource.Recycle(Target);
                Log.Info("Recycle NGUI atlas '{0}'.", Name);
            }
        }
    }
}
