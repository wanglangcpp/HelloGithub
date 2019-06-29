#if UNITY_EDITOR

using GameFramework;

namespace Genesis.GameClient
{
    public class ExceptionNativeCaller
    {
        public void Trigger()
        {
            Log.Warning("Not supported on editor.");
        }
    }
}

#endif
