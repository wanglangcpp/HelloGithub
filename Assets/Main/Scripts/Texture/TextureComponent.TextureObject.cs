using GameFramework;
using GameFramework.ObjectPool;

namespace Genesis.GameClient
{

    public partial class TextureComponent
    {
        private class TextureObject : ObjectBase
        {
            public TextureObject(string name, object target)
                : base(name, target)
            {

            }

            protected override void Release()
            {
                GameEntry.Resource.Recycle(Target);
                Log.Info("Recycle texture '{0}'.", Name);
            }
        }
    }
}
